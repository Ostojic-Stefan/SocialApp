import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { failureToast } from '../../utils/toastDefinitions';
import {
  FriendResponse,
  GetUserProfileInformationRequest,
  SetProfileImageRequest,
  UserProfileInformation,
  userService,
} from '../../api/userService';
import { ApiError } from '../../api/models';

export const uploadProfileImage = createAsyncThunk<string, FormData, { rejectValue: ApiError }>(
  'user/uploadProfileImage',
  async function (data, { rejectWithValue }) {
    const result = await userService.uploadProfileImage(data);
    if (result.hasError) {
      return rejectWithValue(result.error);
    }
    return result.value;
  }
);

export const setUserProfileImage = createAsyncThunk<boolean, SetProfileImageRequest, { rejectValue: ApiError }>(
  'user/setUserProfileImage',
  async function (data, { rejectWithValue }) {
    const result = await userService.setProfileImage(data);
    if (result.hasError) {
      return rejectWithValue(result.error);
    }
    return result.value;
  }
);

export const getUserProfileInformation = createAsyncThunk<
  UserProfileInformation,
  GetUserProfileInformationRequest,
  { rejectValue: ApiError }
>('user/getUserProfileInformation', async function (data, { rejectWithValue }) {
  const result = await userService.getUserProfileInformation(data);
  if (result.hasError) {
    return rejectWithValue(result.error);
  }
  return result.value;
});

export const getFriends = createAsyncThunk<FriendResponse[], void, { rejectValue: ApiError }>(
  'user/getFriends',
  async function (_data, { rejectWithValue }) {
    const result = await userService.getFriends();
    if (result.hasError) {
      return rejectWithValue(result.error);
    }
    return result.value;
  }
);

interface StateType {
  userInfo?: UserProfileInformation;
  friends: FriendResponse[];
}

const initialState: StateType = {
  userInfo: undefined,
  friends: [],
};

const userProfile = createSlice({
  name: 'user',
  initialState,
  reducers: {
    clearState(state, _action) {
      state.userInfo = undefined;
    },
  },
  extraReducers(builder) {
    builder.addCase(getUserProfileInformation.fulfilled, (state, action) => {
      state.userInfo = action.payload;
    });

    builder.addCase(getUserProfileInformation.rejected, (_state, action) => {
      if (action.payload) {
        action.payload.errorMessages.forEach((m: string) => {
          failureToast(m);
        });
      }
    });

    builder.addCase(getFriends.fulfilled, (state, action) => {
      state.friends = action.payload;
    });

    builder.addCase(getFriends.rejected, (_state, action) => {
      console.log('Failed to Get Friends');
      if (action.payload) {
        action.payload.errorMessages.forEach((m: string) => {
          failureToast(m);
        });
      }
    });
  },
});

export const { clearState } = userProfile.actions;

export default userProfile.reducer;
