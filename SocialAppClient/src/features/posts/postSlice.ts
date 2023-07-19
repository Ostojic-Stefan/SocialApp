import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { Post } from "./types";
import { apiHandler } from "../../api/apiHandler";

interface StateType {
    posts: Post[],
    isLoading: boolean;
}

const initialState: StateType = {
    posts: [],
    isLoading: false
}

export const getPosts = createAsyncThunk<Post[]>(
    "post/getAll", async function (_arg, { rejectWithValue }) {
      try {
        const response = await apiHandler.post.getAll();
        return response.items;
      } catch (error: any) {
        return rejectWithValue(error.message);
      }
    }
);

export const uploadPost = createAsyncThunk<void, {formData: FormData, contents: string}>(
  'post/upload', async function({formData, contents}, thunkAPI) {
    try {
      const imageUrl = await apiHandler.post.uploadImage(formData);
      const response = await apiHandler.post.uploadPost({ imageUrl, contents});
      thunkAPI.dispatch(addPost(response));
    } catch (error: any) {
      console.log(error);
      thunkAPI.rejectWithValue(error);
    }
  }
)

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
      })
    }
});

export const { addPost } = postSlice.actions;

export default postSlice.reducer;