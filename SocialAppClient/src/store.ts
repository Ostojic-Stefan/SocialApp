import { configureStore } from '@reduxjs/toolkit'
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';
import postReducer from './features/posts/postSlice';
import commentReducer from './features/comments/commentsSlice';
import identityReducer from './features/identity/identitySlice';
import userReducer from './features/userProfile/userProfileSlice';

const store = configureStore({
  reducer: {
    identity: identityReducer,
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