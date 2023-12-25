import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { failureToast, successToast } from '../../utils/toastDefinitions';
import { uploadProfileImage } from '../userProfile/userProfileSlice';
import {
  LoggedInUserInfomation,
  UserLoginRequest,
  UserRegisterRequest,
  identityService,
} from '../../api/identityService';
import { ApiError, Result } from '../../api/models';

interface StateType {
  userInfo?: LoggedInUserInfomation; // why is this needed?
  authenticated: boolean;
}

const initialState: StateType = {
  userInfo: undefined,
  authenticated: false
};

export const login = createAsyncThunk<void, UserLoginRequest, { rejectValue: ApiError }>(
  'user/login',
  async function (data, { rejectWithValue }) {
    const response = await identityService.login(data);
    if (response.hasError) {
      return rejectWithValue(response.error);
    }
    return response.value;
  }
);

export const register = createAsyncThunk<Result<void, ApiError>, UserRegisterRequest, { rejectValue: ApiError }>(
  'user/register',
  async function (data, { rejectWithValue, dispatch }) {
    const response = await identityService.register(data);
    if (response.hasError) {
      return rejectWithValue(response.error);
    }
    if (data.imageData) {
      await dispatch(uploadProfileImage(data.imageData));
      // const res = (await dispatch(uploadProfileImage(data.imageData))).payload as UploadProfileImageResponse;
      // await dispatch(setUserProfileImage(res));
    }
    return response;
  }
);

export const getUserInformation = createAsyncThunk<LoggedInUserInfomation, void, { rejectValue: ApiError }>(
  'user/getInfo',
  async function (_data, { rejectWithValue }) {
    const response = await identityService.getCurrentUserInfo();

    if (response.hasError) {
      return rejectWithValue(response.error);
    }
    return response.value;
  }
);

const identitySlice = createSlice({
  name: 'user',
  initialState,
  reducers: {},
  extraReducers(builder) {
    builder.addCase(getUserInformation.fulfilled, (state, action) => {
      console.log(action.payload);
      state.userInfo = action.payload;
    });

    builder.addCase(getUserInformation.rejected, (_state, action) => {
      if (action.payload) {
        action.payload?.errorMessages.forEach((m: string) => {
          failureToast(m);
        });
      }
    });

    builder.addCase(register.fulfilled, (_state, _action) => {
      successToast('Successfuly Registered');
    });

    builder.addCase(register.rejected, (_state, action) => {
      if (action.payload) {
        action.payload?.errorMessages.forEach((m: string) => {
          failureToast(m);
        });
      } else {
        failureToast('Failed to register');
      }
    });

    builder.addCase(login.fulfilled, (state, _action) => {
      state.authenticated = true;
      successToast('Successfuly Logged In');
    });

    builder.addCase(login.rejected, (_state, action) => {
      if (action.payload) {
        action.payload?.errorMessages.forEach((m: string) => {
          failureToast(m);
        });
      } else {
        failureToast('Failed to login');
      }
    });
  },
});

export default identitySlice.reducer;
