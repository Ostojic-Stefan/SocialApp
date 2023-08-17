import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { AddLikeToPostRequest, DeleteLikeRequest, DeleteLikeResponse, GetAllLikesForPostRequest, LikeForAPostResponse, likeService } from "../../api/likeService";
import { ApiError } from "../../api/models";

interface StateType {
    likes: LikeForAPostResponse[]
}

const initialState: StateType = {
    likes: []
}

export const likePost = createAsyncThunk<LikeForAPostResponse, AddLikeToPostRequest, { rejectValue: ApiError }>(
    "like/likePost", async function (data, { rejectWithValue }) {
        const response = await likeService.addLikeToPost(data);
        if (response.hasError) {
            return rejectWithValue(response.error);
        }
        return response.value;
    }
);

export const getLikesForPost = createAsyncThunk<LikeForAPostResponse, GetAllLikesForPostRequest, { rejectValue: ApiError }>(
    "like/getLikesForPost", async function (data, { rejectWithValue }) {
        const response = await likeService.getAllLikesForPost(data);
        if (response.hasError) {
            return rejectWithValue(response.error);
        }
        return response.value;
    }
);

export const deleteLike = createAsyncThunk<DeleteLikeResponse, DeleteLikeRequest, { rejectValue: ApiError }>(
    "like/deleteLike", async function (data, { rejectWithValue }) {
        const response = await likeService.deleteLike(data);
        if (response.hasError) {
            return rejectWithValue(response.error);
        }
        return response.value;
    }
);

const likeSlice = createSlice({
    name: 'like',
    initialState,
    reducers: {
        
    },
    extraReducers: function (builder) {
        builder.addCase(getLikesForPost.fulfilled, (state, action) => {
            const likes = state.likes.find(l => l.postId === action.payload.postId);
            if (likes) {
                likes.likeInfo = action.payload.likeInfo;
            } else {
                state.likes.push(action.payload);
            }
        }).addCase(getLikesForPost.rejected, (_state, action) => {
            console.log(action.error);
        });
    }
});

export default likeSlice.reducer;