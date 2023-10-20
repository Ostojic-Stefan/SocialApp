import { createAsyncThunk, createSelector, createSlice } from '@reduxjs/toolkit';
import { failureToast, successToast } from '../../utils/toastDefinitions';
import { GetPostsForUserRequest, postService } from '../../api/postService';
import { ApiError } from '../../api/models';
import { Post, PostsForUser, mapPostFromPostForUser, mapPostsForUser } from './types';
import { RootState } from '../../store';

interface StateType {
  posts: Post[];
  isLoading: boolean;
  postsForUser: PostsForUser[];
}

const initialState: StateType = {
  posts: [],
  isLoading: false,
  postsForUser: [],
};

export const getPosts = createAsyncThunk<Post[], void, { rejectValue: ApiError }>(
  'post/getAll',
  async function (_arg, { rejectWithValue }) {
    const response = await postService.getAllPosts();
    if (response.hasError) {
      return rejectWithValue(response.error);
    }
    return response.value;
  }
);

export const getPostById = createAsyncThunk<Post, string, { rejectValue: ApiError }>(
  'post/getById',
  async function (data, { rejectWithValue }) {
    const response = await postService.getPostById(data);
    if (response.hasError) {
      return rejectWithValue(response.error);
    }
    return response.value;
  }
);

export const uploadPost = createAsyncThunk<void, { formData: FormData; contents: string }, { rejectValue: ApiError }>(
  'post/upload',
  async function ({ formData, contents }, { dispatch, rejectWithValue }) {
    const imgResult = await postService.uploadPostImage(formData);

    if (imgResult.hasError) {
      return rejectWithValue(imgResult.error);
    }

    const postResult = await postService.uploadPost({
      contents,
      imageUrl: imgResult.value.imagePath,
    });

    if (postResult.hasError) {
      return rejectWithValue(postResult.error);
    }
    // TODO: refactor
    dispatch(addPost(postResult.value));
  }
);

// export const getPostsForUser = createAsyncThunk<
//   PostsForUserResponse,
//   GetPostsForUserRequest,
//   { rejectValue: ApiError }
// >('user/getPostsForUser', async function (data, { rejectWithValue }) {
//   const result = await postService.getPostsForUser(data);
//   if (result.hasError) {
//     return rejectWithValue(result.error);
//   }
//   return result.value;
// });

export const getPostsForUser = createAsyncThunk<PostsForUser, GetPostsForUserRequest, { rejectValue: ApiError }>(
  'user/getPostsForUser',
  async function (data, { rejectWithValue }) {
    const result = await postService.getPostsForUser(data);
    if (result.hasError) {
      return rejectWithValue(result.error);
    }
    return mapPostsForUser(result.value);
  }
);

const postSlice = createSlice({
  name: 'post',
  initialState,
  reducers: {
    addPost(state, action) {
      state.posts.unshift(action.payload);
    },
    increaseCommentCount(state, action) {
      const post = state.posts.find((p) => p.id === action.payload.postId);
      if (post) {
        post.numComments += 1;
      }
    },
  },
  extraReducers: function (builder) {
    builder
      .addCase(getPosts.pending, (state, _action) => {
        state.isLoading = true;
      })
      .addCase(getPosts.fulfilled, (state, action) => {
        state.posts = action.payload;
        state.isLoading = false;
      })
      .addCase(getPosts.rejected, (state, _action) => {
        state.isLoading = false;
      });

    builder.addCase(getPostById.fulfilled, (state, action) => {
      const post = state.posts.find((p) => p.id === action.payload.id);
      if (post) {
        Object.assign(post, action.payload);
      } else {
        state.posts.push(action.payload);
      }
    });

    builder
      .addCase(uploadPost.fulfilled, (_state, _action) => {
        successToast('Successfully created a post!');
      })
      .addCase(uploadPost.rejected, (_state, action) => {
        if (action.payload) {
          action.payload.errorMessages.forEach((m: string) => {
            failureToast(m);
          });
        } else {
          failureToast('Failed to create a post');
        }
      });

    builder.addCase(getPostsForUser.fulfilled, (state, action) => {
      const found = state.postsForUser.find((p) => p.userInfo.userProfileId === action.payload.userInfo.userProfileId);
      if (found) {
        Object.assign(found, action.payload);
      } else {
        state.postsForUser.push(action.payload);
      }
    });

    builder.addCase(getPostsForUser.rejected, (_state, action) => {
      if (action.payload) {
        action.payload.errorMessages.forEach((m: string) => {
          failureToast(m);
        });
      }
    });
  },
});

export const { addPost, increaseCommentCount } = postSlice.actions;

const selectPostsForUser = (store: RootState) => store.post.postsForUser;
const selectUsername = (_store: RootState, username: string) => username;

export const postsForUserSelector = createSelector(
  [selectPostsForUser, selectUsername],
  (posts: PostsForUser[], username: string) => {
    console.log('selecting...');
    const postsForUser = posts.find((p) => p.userInfo.username === username);
    return postsForUser ? mapPostFromPostForUser(postsForUser) : undefined;
  }
);

export default postSlice.reducer;
