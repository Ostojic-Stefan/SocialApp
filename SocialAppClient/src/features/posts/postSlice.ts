import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { GetAllPostsResponse, Post } from "./types";
import axios from "axios";

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
        const response = await axios.get<GetAllPostsResponse>("https://localhost:7113/api/Posts");
        console.log(response.data);
        return response.data.items;
      } catch (error: any) {
        return rejectWithValue(error.message);
      }
    }
);

const postSlice = createSlice({
    name: 'post',
    initialState,
    reducers: {},
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

export default postSlice.reducer;