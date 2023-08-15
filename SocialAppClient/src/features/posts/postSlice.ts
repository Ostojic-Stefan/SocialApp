import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { failureToast, successToast } from "../../utils/toastDefinitions";
import { PostResponse, postService } from "../../api/postService";
import { ApiError } from "../../api/models";

interface StateType {
  posts: PostResponse[],
  userPosts: { username: string, posts: PostResponse[] }
  isLoading: boolean;
}

const initialState: StateType = {
  posts: [],
  isLoading: false,
  userPosts: {
    username: "",
    posts: []
  }
}

export const getPosts = createAsyncThunk<PostResponse[], void, { rejectValue: ApiError }>(
  "post/getAll", async function (_arg, { rejectWithValue }) {
    const response = await postService.getAllPosts();
    if (response.hasError) {
      return rejectWithValue(response.error);
    }
    return response.value;
  }
);

export const uploadPost = createAsyncThunk<void, { formData: FormData, contents: string }, { rejectValue: ApiError }>(
  'post/upload', async function ({ formData, contents }, { dispatch, rejectWithValue }) {
    const imgResult = await postService.uploadPostImage(formData);
    if (imgResult.hasError) {
      return rejectWithValue(imgResult.error);
    }
    const postResult = await postService.uploadPost({ contents, imageUrl: imgResult.value });
    if (postResult.hasError) {
      return rejectWithValue(postResult.error);
    }
    // TODO: refactor
    dispatch(addPost(postResult.value));
  }
);



const postSlice = createSlice({
  name: 'post',
  initialState,
  reducers: {
    addPost(state, action) {
      state.posts.unshift(action.payload);
    },
    increaseCommentCount(state, action) {
      const post = state.posts.find(p => p.id === action.payload.postId);
      if (post) {
        post.numComments += 1;
      }
    }
  },
  extraReducers(builder) {
    builder.addCase(getPosts.pending, (state, _action) => {
      state.isLoading = true;
    });

    builder.addCase(getPosts.fulfilled, (state, action) => {
      state.posts = action.payload;
      state.isLoading = false;
    });

    builder.addCase(getPosts.rejected, (state, _action) => {
      state.isLoading = false;
    });

    builder.addCase(uploadPost.fulfilled, (_state, _action) => {
      successToast("Successfully created a post!")
    });

    builder.addCase(uploadPost.rejected, (_state, action) => {
      if (action.payload) {
        action.payload.errorMessages.forEach((m: string) => {
          failureToast(m);
        });
      } else {
        failureToast("Failed to create a post");
      }
    });

    // builder.addCase(likePost.fulfilled, (state, action) => {
    //   const idx = state.posts.findIndex(p => p.id === action.payload.postId);
    //   if (idx !== -1) {
    //     state.posts[idx].likeInfo = { likedByCurrentUser: true, likeId: action.payload.likeId };
    //     state.posts[idx].numLikes += 1;
    //     successToast("Successfully liked post");
    //   }
    // });

    // builder.addCase(likePost.rejected, (_state, action) => {
    //   if (action.payload) {
    //     action.payload.errorMessages.forEach((m: string) => {
    //       failureToast(m);
    //     });
    //   }
    // });

    // builder.addCase(deleteLike.fulfilled, (state, action) => {
    //   const post = state.posts.find(p => p.id === action.payload.postId);
    //   if (post) {
    //     post.likeInfo = undefined;
    //     post.numLikes -= 1;
    //   }
    // });

  }
});

export const { addPost, increaseCommentCount } = postSlice.actions;

export default postSlice.reducer;