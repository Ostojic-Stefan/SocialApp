import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { PostResponse, postService } from "../api/postService";
import { ApiError } from "../api/models";

interface StateType {
    posts: PostResponse[];
    postsLoading: boolean;
    allPostsError: string | undefined;
}

const initialState: StateType = {
    posts: [],
    postsLoading: false,
    allPostsError: undefined
};

export const getAllPosts = createAsyncThunk<PostResponse[], void, { rejectValue: ApiError }>
    ('post/getAll', async function (_arg, { rejectWithValue }) {
        const response = await postService.getAllPosts();
        if (response.hasError) {
            return rejectWithValue(response.error);
        }
        return response.value;
    });


export const uploadPost = createAsyncThunk<PostResponse, { formData: FormData, contents: string }, { rejectValue: ApiError }>(
    'post/upload',
    async function ({ formData, contents }, { rejectWithValue }) {
        const imgResult = await postService.uploadPostImage(formData);
        if (imgResult.hasError) {
            return rejectWithValue(imgResult.error);
        }
        const postResult = await postService.uploadPost({
            contents,
            imageUrl: imgResult.value.imagePath,
        });
        if (postResult.hasError) {
            return rejectWithValue(postResult.error);
        }
        return postResult.value;
    }
);

const postSlice = createSlice({
    name: 'post',
    initialState,
    reducers: {
        addPost(state, action) {
            state.posts.unshift(action.payload);
        },
        addLikeToPost(state, action) {
            console.log(action.payload);

            const thePost = state.posts.find(post => post.id === action.payload.postId);
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

            const thePost = state.posts.find(post => post.id === action.payload.postId);
            if (!thePost) {
                console.log("could not find the post");
                return;
            }
            thePost.numLikes -= 1;
            // TODO: figure out why is like info nullable and if there is a better way to handle this situation
            thePost.likeInfo = undefined;
        }
    },
    extraReducers: function (builder) {
        builder.addCase(getAllPosts.pending, (state, _action) => {
            state.postsLoading = true;
        }).addCase(getAllPosts.fulfilled, (state, action) => {
            state.posts = action.payload;
            state.postsLoading = false;
        }).addCase(getAllPosts.rejected, (state, action) => {
            console.log('Failed to get posts');
            console.log(action.payload);
            state.postsLoading = false;
            state.allPostsError = action.payload?.errorMessages.join(", ");
        });

        builder.addCase(uploadPost.fulfilled, (state, action) => {
            state.posts.unshift(action.payload);
        })
        builder.addCase(uploadPost.rejected, (_state, action) => {
            console.log('Failed upload post');
            console.log(action.payload);
        });
    }
})

export const { addPost, addLikeToPost, removeLikeFromPost } = postSlice.actions;

export default postSlice.reducer;