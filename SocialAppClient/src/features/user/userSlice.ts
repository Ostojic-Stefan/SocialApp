import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { UserInfomation, UserLoginResponse } from "./types";
import axios from "axios";

export const login = createAsyncThunk<string, {email: string, password: string}>(
    "user/login", async function (data, { rejectWithValue }) {
      try {
        const response = await 
            axios.post<UserLoginResponse>("https://localhost:7113/api/Identity/login", data);
        return response.data.accessToken;
      } catch (error: any) {
        return rejectWithValue(error.message);
      }
    }
);

export const getUserInformation = createAsyncThunk<UserInfomation>(
  'user/getInfo', async function (_data, { rejectWithValue }) {
    try {
      const response = await axios.get<UserInfomation>('https://localhost:7113/api/Identity/me');
      return response.data;
    } catch (error: any) {
      return rejectWithValue(error.message);
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
        localStorage.removeItem('accessToken');
      }
    },
    extraReducers(builder) {
      builder.addCase(login.pending, (_state, _action) => {
        console.log('user pending');
      })
      builder.addCase(login.fulfilled, (_state, action) => {
        localStorage.setItem('accessToken', action.payload);
      })
      builder.addCase(login.rejected, (_state, action) => {
        console.log(action.payload);
      })
      builder.addCase(getUserInformation.fulfilled, (state, action) => {
        state.userInfo = action.payload;
        console.log(action.payload);
      });
      builder.addCase(getUserInformation.rejected, (state, action) => {
        console.log(action.payload);
      });
    }
});

export const { logout } = userSlice.actions;
export default userSlice.reducer;