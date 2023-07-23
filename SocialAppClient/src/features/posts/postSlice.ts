import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { Post } from "./types";
import { apiHandler } from "../../api/apiHandler";
import { failureToast, successToast } from "../../utils/toastDefinitions";

interface StateType {
    posts: Post[],
    userPosts: {username: string, posts: Post[]}
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

export const getPosts = createAsyncThunk<Post[]>(
    "post/getAll", async function(_arg, { rejectWithValue }) {
      try {
        const response = await apiHandler.post.getAll();
        return response.items;
      } catch (error: any) {
        return rejectWithValue(error.message);
      }
    }
);

export const uploadPost = createAsyncThunk<void, {formData: FormData, contents: string}>(
  'post/upload', async function({formData, contents}, { dispatch, rejectWithValue }) {
    try {
      const imageUrl = await apiHandler.post.uploadImage(formData);
      const response = await apiHandler.post.uploadPost({ imageUrl, contents});
      dispatch(addPost(response));
    } catch (error: any) {
      return rejectWithValue(error);
    }
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
        state.posts = action.payload;
        state.isLoading = false;
      })
      builder.addCase(getPosts.rejected, (state, action) => {
        console.log(action.payload);
        state.isLoading = false;
      });

      builder.addCase(uploadPost.fulfilled, (_state, _action) => {
        successToast("Successfully created a post!")
      });

      builder.addCase(uploadPost.rejected, (_state, action) => {
         // @ts-ignore
         action.payload.messages.forEach((m: string) => {
          failureToast(m);
        });
      });
    }
});

export const { addPost } = postSlice.actions;

export default postSlice.reducer;