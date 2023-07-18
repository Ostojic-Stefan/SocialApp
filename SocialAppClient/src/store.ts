import { configureStore } from '@reduxjs/toolkit'
import postReducer from './features/posts/postSlice';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';


const store = configureStore({
  reducer: {
    post: postReducer
  }
});

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch

export const useAppDispatch: () => AppDispatch = useDispatch;
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

export default store