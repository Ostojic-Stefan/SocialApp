import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { failureToast, successToast } from "../../utils/toastDefinitions";
import { GetAllPostsResponse, Post, postService } from "../../api/postService";
import { ApiError } from "../../api/models";

interface StateType {
  posts: Post[],
  userPosts: { username: string, posts: Post[] }
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

export const getPosts = createAsyncThunk<GetAllPostsResponse, void, { rejectValue: ApiError }>(
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
    }
  },
  extraReducers(builder) {
    builder.addCase(getPosts.pending, (state, _action) => {
      state.isLoading = true;
    })
    builder.addCase(getPosts.fulfilled, (state, action) => {
      state.posts = action.payload.items;
      state.isLoading = false;
    })
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
  }
});

export const { addPost } = postSlice.actions;

export default postSlice.reducer;