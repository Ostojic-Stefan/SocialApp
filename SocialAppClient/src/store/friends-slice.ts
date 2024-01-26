import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { UserInfoResponse } from "../api/dtos/user";
import { ApiError } from "../api/models";
import { userService } from "../api/userService";

interface StateType {
    userProfileFriends: UserInfoResponse[];
    currentUserFriends: UserInfoResponse[];
}

const initialState: StateType = {
    userProfileFriends: [],
    currentUserFriends: []
}

export const getFriendsForUser = createAsyncThunk<{ friends: UserInfoResponse[], isCurrentUser: boolean },
    { userId: string, isCurrentUser: boolean }, { rejectValue: ApiError }>(
        "friends/getForUser", async function ({ userId, isCurrentUser }, { rejectWithValue }) {
            const response = await userService.getFriends(userId);
            if (response.hasError) {
                return rejectWithValue(response.error);
            }
            return { friends: response.value, isCurrentUser };
        }
    );

const friendsSlice = createSlice({
    name: "friends",
    initialState,
    reducers: {},
    extraReducers: function (builder) {
        builder.addCase(getFriendsForUser.fulfilled, (state, action) => {
            if (action.payload.isCurrentUser) {
                state.currentUserFriends = action.payload.friends;
                return;
            }
            state.userProfileFriends = action.payload.friends;
        }).addCase(getFriendsForUser.rejected, (_state, action) => {
            console.log('Failed to get Friends');
            console.log(action.payload);
        });
    }
});

export default friendsSlice.reducer;