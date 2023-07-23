import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { PostsForUserResponse, UserProfileInformation } from "./types";
import { apiHandler } from "../../api/apiHandler";
import { failureToast } from "../../utils/toastDefinitions";

export const uploadProfileImage = createAsyncThunk<string, FormData>(
  "user/uploadProfileImage", async function(data, { rejectWithValue }) {
    try {
      const imageUrl = await apiHandler.user.uploadProfileImage(data);
      return imageUrl;
    } catch (error: any) {
      return rejectWithValue(error);
    }
  }
);

export const setUserProfileImage = createAsyncThunk<boolean, string>(
  'user/setUserProfileImage', async function(data, { rejectWithValue }) {
    try {
      const response = await apiHandler.user.setUserProfileImage(data);
      return response;
    } catch (error: any) {
      return rejectWithValue(error);
    }
  }
)

export const getPostsForUser = createAsyncThunk<PostsForUserResponse[], {username: string}>(
  'user/getPostsForUser', async function(data, { rejectWithValue }) {
    try {
      const posts = await apiHandler.post.getPostsForUser(data.username);
      return posts;
    } catch (error: any) {
      return rejectWithValue(error);
    }
  }
);

export const getUserProfileInformation = createAsyncThunk<UserProfileInformation, string>(
  'user/getUserProfileInformation', async function(data, { rejectWithValue }) {
    try {
      const info = await apiHandler.user.getUserProfileInformation(data);
      return info;
    } catch (error: any) {
      return rejectWithValue(error);
    }
  }
);

interface StateType {
    userInfo?: UserProfileInformation,
    posts: PostsForUserResponse[]
}

const initialState: StateType = {
  userInfo: undefined,
  posts: []
}

const userProfile = createSlice({
    name: 'user',
    initialState,
    reducers: {
      clearState(state, _action) {
        state.posts = [];
        state.userInfo = undefined;
      }
    },
    extraReducers(builder) {
      builder.addCase(getPostsForUser.fulfilled, (state, action) => {
        state.posts = action.payload;
      });

      builder.addCase(getPostsForUser.rejected, (_state, action) => {
         // @ts-ignore
         action.payload.messages.forEach((m: string) => {
          failureToast(m);
        });
      });

      builder.addCase(getUserProfileInformation.fulfilled, (state, action) => {
        state.userInfo = action.payload;
      });

      builder.addCase(getUserProfileInformation.rejected, (_state, action) => {
        // @ts-ignore
        action.payload.messages.forEach((m: string) => {
          failureToast(m);
        });
      });
    }
});

export const { clearState } = userProfile.actions;

export default userProfile.reducer;