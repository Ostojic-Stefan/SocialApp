import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { postService } from "../api/postService";
import { ApiError } from "../api/models";
import { PostResponse, PostsForUserRequest, PostsForUserResponse } from "../api/dtos/post";

interface StateType {
    allPosts: PostResponse[];
    postsForUser: PostsForUserResponse[];
    postsFromFriends: PostResponse[];
    postsLoading: boolean;
    allPostsError: string | undefined;
}

const initialState: StateType = {
    allPosts: [],
    postsFromFriends: [],
    postsForUser: [],
    postsLoading: false,
    allPostsError: undefined,
};

export const getAllPosts = createAsyncThunk<PostResponse[], void, { rejectValue: ApiError }>
    ('post/getAll', async function (_arg, { rejectWithValue }) {
        const response = await postService.getAllPosts();
        if (response.hasError) {
            return rejectWithValue(response.error);
        }
        return response.value;
    });

export const uploadPost = createAsyncThunk<boolean, { formData: FormData, contents: string, title: string }, { rejectValue: ApiError }>(
    'post/upload', async function ({ formData, contents, title }, { rejectWithValue }) {
        const imgResult = await postService.uploadPostImage(formData);
        if (imgResult.hasError) {
            return rejectWithValue(imgResult.error);
        }

        const postResult = await postService.uploadPost({
            contents,
            title,
            imageName: imgResult.value.imageName,
        });

        if (postResult.hasError) {
            return rejectWithValue(postResult.error);
        }
        return postResult.value;
    }
);

export const getPostsForUser = createAsyncThunk<PostsForUserResponse, PostsForUserRequest, { rejectValue: ApiError }>(
    'posts/getForUser', async function (request, { rejectWithValue }) {
        const res = await postService.getPostsForUser(request);
        if (res.hasError) {
            return rejectWithValue(res.error);
        }
        return res.value;
    }
);

export const getPostsFromFriends = createAsyncThunk<PostResponse[], void, { rejectValue: ApiError }>(
    "post/postsFromFriends", async function (_, { rejectWithValue }) {
        const response = await postService.getUserFriendsPost();
        if (response.hasError) {
            return rejectWithValue(response.error);
        }
        return response.value;
    }
)

const postSlice = createSlice({
    name: 'post',
    initialState,
    reducers: {
        addLikeToPost(state, action) {
            console.log(action.payload);

            const thePost = state.allPosts.find(post => post.id === action.payload.postId);
            if (!thePost) {
                console.log("could not find the post");
                return;
            }
            thePost.numLikes += 1;
            thePost.likeInfo = {
                likedByCurrentUser: true,
                likeId: action.payload.likeId
            };
        },
        removeLikeFromPost(state, action) {
            console.log(action.payload);

            const thePost = state.allPosts.find(post => post.id === action.payload.postId);
            if (!thePost) {
                console.log("could not find the post");
                return;
            }
            thePost.numLikes -= 1;
            // TODO: figure out why is like info nullable and if there is a better way to handle this situation
            thePost.likeInfo.likedByCurrentUser = false;
            // thePost.likeInfo = undefined;
        },
        // addPostForUser(state, action) {
        //     const postsForUser = state.postsForUser.find(user => user.userInfo.userProfileId === action.payload.userId);
        //     postsForUser?.posts.unshift(action.payload.postData);
        // }
    },
    extraReducers: function (builder) {
        builder.addCase(getAllPosts.pending, (state, _action) => {
            state.postsLoading = true;
        }).addCase(getAllPosts.fulfilled, (state, action) => {
            state.allPosts = action.payload;
            state.postsLoading = false;
        }).addCase(getAllPosts.rejected, (state, action) => {
            console.log('Failed to get posts');
            console.log(action.payload);
            state.postsLoading = false;
            state.allPostsError = action.payload?.errorMessages.join(", ");
        });

        builder.addCase(uploadPost.fulfilled, (state, action) => {
            // const pathname = window.location.pathname
            // const regex = new RegExp(/\/profile\/.*/);
            // if (regex.test(pathname)) {
            //     const postsForUser = state.postsForUser.find(x => x.userInfo.userProfileId === action.payload.userInfo.userProfileId);
            //     if (!postsForUser) return;
            //     postsForUser.posts.unshift(action.payload);
            // } else {
            //     state.allPosts.unshift(action.payload);
            // }
        }).addCase(uploadPost.rejected, (_state, action) => {
            console.log('Failed upload post');
            console.log(action.payload);
        });

        builder.addCase(getPostsForUser.fulfilled, (state, action) => {
            state.postsForUser.push(action.payload);
        }).addCase(getPostsForUser.rejected, (_state, action) => {
            console.log('Failed to get posts for user');
            console.log(action.payload);
        });

        builder.addCase(getPostsFromFriends.fulfilled, (state, action) => {
            state.postsFromFriends = action.payload;
        }).addCase(getPostsFromFriends.rejected, (state, action) => {
            console.log('Failed to get friend`s posts');
            console.log(action.payload);

        });
    }
})

export const { addLikeToPost, removeLikeFromPost } = postSlice.actions;

export default postSlice.reducer;