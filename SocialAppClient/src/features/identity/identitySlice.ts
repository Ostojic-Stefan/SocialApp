import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { failureToast, successToast } from "../../utils/toastDefinitions";
import { setUserProfileImage, uploadProfileImage } from "../userProfile/userProfileSlice";
import { LoggedInUserInfomation, UserLoginRequest, UserRegisterRequest, identityService } from "../../api/identityService";
import { ApiError } from "../../api/models";

interface StateType {
  userInfo?: LoggedInUserInfomation,
}

const initialState: StateType = {
  userInfo: undefined
}

export const login = createAsyncThunk<void, UserLoginRequest, { rejectValue: ApiError }>(
    "user/login", async function (data, { rejectWithValue }) {
      const response = await identityService.login(data);
      if (response.hasError) {
        return rejectWithValue(response.error);
      }
      return response.value;
    }
);

export const register = createAsyncThunk<void, UserRegisterRequest, { rejectValue: ApiError }>(
    "user/register", async function(data, { rejectWithValue, dispatch }) {
      const response = await identityService.register(data);
      if (response.hasError) {
        return rejectWithValue(response.error);
      }
      if (data.imageData) {
        const avatarUrl = (await dispatch(uploadProfileImage(data.imageData))).payload as string;
        await dispatch(setUserProfileImage(avatarUrl));
      }
    }
);

export const getUserInformation = createAsyncThunk<LoggedInUserInfomation, void, { rejectValue: ApiError }>(
    'user/getInfo', async function (_data, { rejectWithValue }) {
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
        successToast('Successfuly Registered')
      });

      builder.addCase(register.rejected, (_state, action) => {
        if (action.payload) {
          action.payload?.errorMessages.forEach((m: string) => {
              failureToast(m);
          });
        } else {
            failureToast("Failed to register");
        }
      });

      builder.addCase(login.fulfilled, (_state, _action) => {
        successToast('Successfuly Logged In')
      });

      builder.addCase(login.rejected, (_state, action) => {
        if (action.payload) {
          action.payload?.errorMessages.forEach((m: string) => {
              failureToast(m);
          });
        } else {
            failureToast("Failed to login");
        }
      });
    }
});

export default identitySlice.reducer;
