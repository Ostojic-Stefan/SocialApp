import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { failureToast, successToast } from "../../utils/toastDefinitions";
import { AddCommentRequest, AddCommentResponse, CommentResponse, CommentsFromPostResponse, commentService } from "../../api/commentService";
import { ApiError } from "../../api/models";

interface StateType {
    data: { postId: string, comments: CommentResponse[] }[]
}

const initialState: StateType = {
    data: []
}

export const getCommentsByPostId = createAsyncThunk<CommentsFromPostResponse, string, { rejectValue: ApiError }>(
    'comment/getCommentsById', async function(postId, { rejectWithValue }) {
        const response = await commentService.getCommentsOnAPost({ postId });
        if (response.hasError) {
            return rejectWithValue(response.error);
        }
        return response.value;
    }
)

export const addCommentToAPost = createAsyncThunk<AddCommentResponse, AddCommentRequest, { rejectValue: ApiError }>(
    'comment/addCommentToAPost', async function(data, { dispatch, rejectWithValue }) {
        const response = await commentService.addCommentToAPost(data);
        if (response.hasError) {
            return rejectWithValue(response.error);
        }
        dispatch(addComment({ postId: data.postId, comment: response.value }))
        return response.value;
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
            if (action.payload) {
                action.payload?.errorMessages.forEach((m: string) => {
                    failureToast(m);
                });
            } else {
                failureToast("Failed to get comments");
            }
        });

        builder.addCase(addCommentToAPost.fulfilled, (_state, _action) => {
            successToast("Successfully created a comment!");
        });

        builder.addCase(addCommentToAPost.rejected, (_state, action) => {
            if (action.payload) {
                action.payload?.errorMessages.forEach((m: string) => {
                    failureToast(m);
                });
            } else {
                failureToast("Failed to add a comment");
            }
        })
    }
})

export const { addComment } = commentsSlice.actions;

export default commentsSlice.reducer;