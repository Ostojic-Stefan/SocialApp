import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apiHandler } from "../../api/apiHandler";
import { failureToast, successToast } from "../../utils/toastDefinitions";
import { AddCommentRequest, AddCommentResponse, CommentResponse, CommentsFromPostResponse } from "./types";

interface StateType {
    data: {postId: string, comments: CommentResponse[]}[]
}

const initialState: StateType = {
    data: []
}

export const getCommentsByPostId = createAsyncThunk<CommentsFromPostResponse, string>(
    'comment/getCommentsById', async function(postId, { rejectWithValue }) {
        try {
            const response = await apiHandler.comment.getCommentsOnAPost(postId);
            return response;
        } catch (error: any) {
            return rejectWithValue(error);
        }
    }
)

export const addCommentToAPost = createAsyncThunk<AddCommentResponse, AddCommentRequest>(
    'comment/addCommentToAPost', async function(data, { dispatch, rejectWithValue }) {
        try {
            const response = await apiHandler.comment.addCommentToAPost(data);
            dispatch(addComment({postId: data.postId, comment: response}))
            return response;
        } catch (error: any) {
            return rejectWithValue(error);
        }
    }
)

const commentsSlice = createSlice({
    name: 'comment',
    initialState,
    reducers: {
        addComment(state, action) {
            state.data.find(d => d.postId === action.payload.postId)
            ?.comments.push(action.payload.comment);
        }
    },
    extraReducers(builder) {
        builder.addCase(getCommentsByPostId.fulfilled, (state, action) => {
            const postId = action.payload.postId;
            const foundData = state.data.find(d => d.postId === postId)
            if (foundData) {
                foundData.comments = action.payload.comments;
            } else {
                state.data.push({ postId: postId, comments: action.payload.comments})
            }
        });
        builder.addCase(getCommentsByPostId.rejected, (_state, action) => {
            // @ts-ignore
            action.payload.messages.forEach((m: string) => {
                failureToast(m);
            });
        });

        builder.addCase(addCommentToAPost.fulfilled, (_state, _action) => {
            successToast("Successfully created a comment!");
        });
        builder.addCase(addCommentToAPost.rejected, (_state, action) => {
            //@ts-ignore
            action.payload.messages.forEach((m: string) => {
                failureToast(m);
            });
        })
    }
})

export const { addComment } = commentsSlice.actions;

export default commentsSlice.reducer;