import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { apiHandler } from "../../api/apiHandler";
import { failureToast, successToast } from "../../utils/toastDefinitions";
import { LoggedInUserInfomation, UserRegisterRequest } from "./types";
import { setUserProfileImage, uploadProfileImage } from "../userProfile/userProfileSlice";

interface StateType {
    userInfo?: LoggedInUserInfomation,
}

const initialState: StateType = {
  userInfo: undefined
}

// TODO: fix this
export const login = createAsyncThunk<void, {email: string, password: string}>(
    "user/login", async function (data, { rejectWithValue }) {
      try {
        await apiHandler.identity.login(data)
      } catch (error: any) {
        return rejectWithValue(error);
      }
    }
);

export const register = createAsyncThunk<void, UserRegisterRequest>(
    "user/register", async function(data, { rejectWithValue, dispatch }) {
      try {
        await apiHandler.identity.register(data);
        if (data.imageData) {
          const avatarUrl = (await dispatch(uploadProfileImage(data.imageData))).payload as string;
          await dispatch(setUserProfileImage(avatarUrl));
        }
      } catch (error: any) {
        return rejectWithValue(error);
      }
    }
  );

export const getUserInformation = createAsyncThunk<LoggedInUserInfomation>(
    'user/getInfo', async function (_data, { rejectWithValue }) {
      try {
        const response = await apiHandler.identity.getLoggedInUserInformation();
        return response;
      } catch (error: any) {
        return rejectWithValue(error);
      }
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

      builder.addCase(register.fulfilled, (_state, _action) => {
        successToast('Successfuly Registered')
      });

      builder.addCase(register.rejected, (_state, action) => {
        // @ts-ignore
        action.payload.messages.forEach((m: string) => {
          failureToast(m);
        });
      });

      builder.addCase(login.fulfilled, (_state, _action) => {
        successToast('Successfuly Logged In')
      });

      builder.addCase(login.rejected, (_state, action) => {
        // @ts-ignore
        action.payload.messages.forEach((m: string) => {
          failureToast(m);
        });
      });
    }
});

export default identitySlice.reducer;
