import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { UserInfomation, UserRegisterRequest } from "./types";
import { apiHandler } from "../../api/apiHandler";


// TODO: fix this
export const login = createAsyncThunk<string, {email: string, password: string}>(
    "user/login", async function (data, { rejectWithValue }) {
      try {
        const response = await apiHandler.user.login(data)
        return response.accessToken;
      } catch (error: any) {
        return rejectWithValue(error.response.data);
      }
    }
);

export const getUserInformation = createAsyncThunk<UserInfomation>(
  'user/getInfo', async function (_data, { rejectWithValue }) {
    try {
      const response = await apiHandler.user.getInformation();
      return response;
    } catch (error: any) {
      return rejectWithValue(error.response.data);
    }
  }
);

export const register = createAsyncThunk<void, UserRegisterRequest>(
  "user/register", async function(data, { rejectWithValue }) {
    try {
      await apiHandler.user.register(data);
      console.log('successfuly registered!');
    } catch (error: any) {
      return rejectWithValue(error.response.data);
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
      builder.addCase(login.pending, (_state, _action) => {
      })
      builder.addCase(login.fulfilled, (_state, action) => {
        console.log(action);
      })
      builder.addCase(login.rejected, (_state, action) => {
      })
      builder.addCase(getUserInformation.fulfilled, (state, action) => {
        state.userInfo = action.payload;
      });
      builder.addCase(getUserInformation.rejected, (state, action) => {
      });
      builder.addCase(register.rejected, (_state, _action) => {
        console.log('Failed to register!', _action.payload);
      })
    }
});

export const { logout } = userSlice.actions;
export default userSlice.reducer;