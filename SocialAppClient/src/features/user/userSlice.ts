import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { UserInfomation, UserRegisterRequest } from "./types";
import { apiHandler } from "../../api/apiHandler";
import { failureToast, successToast } from "../../utils/toastDefinitions";


// TODO: fix this
export const login = createAsyncThunk<void, {email: string, password: string}>(
    "user/login", async function (data, { rejectWithValue }) {
      try {
        await apiHandler.user.login(data)
      } catch (error: any) {
        return rejectWithValue(error);
      }
    }
);

export const getUserInformation = createAsyncThunk<UserInfomation>(
  'user/getInfo', async function (_data, { rejectWithValue }) {
    try {
      const response = await apiHandler.user.getInformation();
      return response;
    } catch (error: any) {
      return rejectWithValue(error);
    }
  }
);

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

export const register = createAsyncThunk<void, UserRegisterRequest>(
  "user/register", async function(data, { rejectWithValue, dispatch }) {
    try {
      await apiHandler.user.register(data);
      if (data.imageData) {
        const avatarUrl = (await dispatch(uploadProfileImage(data.imageData))).payload as string;
        await dispatch(setUserProfileImage(avatarUrl));
      }
    } catch (error: any) {
      return rejectWithValue(error);
    }
  }
);

interface StateType {
    userInfo?: UserInfomation,
}

const initialState: StateType = {
    userInfo: undefined
}

const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
      logout(state, _action) {
        state.userInfo = undefined;
      }
    },
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
      })

      builder.addCase(login.rejected, (_state, action) => {
        // @ts-ignore
        action.payload.messages.forEach((m: string) => {
          failureToast(m);
        });
      })
    }
});

export const { logout } = userSlice.actions;
export default userSlice.reducer;