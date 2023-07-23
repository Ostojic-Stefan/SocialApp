import { configureStore } from '@reduxjs/toolkit'
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';
import postReducer from './features/posts/postSlice';
import userReducer from './features/user/userSlice';
import commentReducer from './features/comments/commentsSlice';

const store = configureStore({
  reducer: {
    post: postReducer,
    user: userReducer,
    comment: commentReducer
  }
});

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch

export const useAppDispatch: () => AppDispatch = useDispatch;
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

export default store