import { axiosInstance, executeApiCall } from './apiConfig';
import { CommentResponse } from './commentService';
import { ApiError, Result } from './models';

export type GetPostsForUserRequest = {
  username: string;
};

export type UserInfoResponse = {
  userProfileId: string;
  username: string;
  avatarUrl: string;
};

export type LikeInfoResponse = {
  likeId: string;
  likedByCurrentUser: boolean;
};

export type PostResponse = {
  id: string;
  imageUrl: string;
  contents: string;
  createdAt: Date;
  updatedAt: Date;
  numLikes: number;
  userInfo: UserInfoResponse;
  numComments: number;
  likeInfo?: LikeInfoResponse;
};

export type PostDetailsResponse = {
  id: string;
  imageUrl: string;
  contents: string;
  createdAt: Date;
  updatedAt: Date;
  numLikes: number;
  userInfo: UserInfoResponse;
  numComments: number;
  likeInfo?: LikeInfoResponse;
  comments: CommentResponse[];
};

export type UploadPostRequest = {
  contents: string;
  imageUrl: string;
};

export type UploadPostImageResponse = {
  imagePath: string;
};

export type PostDataResponse = {
  id: string;
  imageUrl: string;
  contents: string;
  createdAt: Date;
  updatedAt: Date;
  numLikes: number;
  numComments: number;
  likeInfo?: LikeInfoResponse;
};

export type PostsForUserResponse = {
  userInfo: UserInfoResponse;
  posts: PostDataResponse[];
};

// prettier-ignore
export interface IPostService {
  getAllPosts: () => Promise<Result<PostResponse[], ApiError>>;
  getPostById: (postId: string) => Promise<Result<PostResponse, ApiError>>;
  uploadPostImage: (request: FormData) => Promise<Result<UploadPostImageResponse, ApiError>>;
  uploadPost: (request: UploadPostRequest) => Promise<Result<PostResponse, ApiError>>;
  getPostsForUser: (request: GetPostsForUserRequest) => Promise<Result<PostsForUserResponse, ApiError>>;
  getPostDetails: (postId: string) => Promise<Result<PostDetailsResponse, ApiError>>;
}

export const postService: IPostService = {
  getAllPosts: async function (): Promise<Result<PostResponse[], ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<PostResponse[]>('posts');
      return response.data;
    });
  },
  getPostById: async function (postId: string): Promise<Result<PostResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<PostResponse>(`posts/${postId}`);
      return response.data;
    });
  },
  uploadPostImage: async function (request: FormData): Promise<Result<UploadPostImageResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<UploadPostImageResponse>('posts/images', request, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      return response.data;
    });
  },
  uploadPost: async function (request: UploadPostRequest): Promise<Result<PostResponse, ApiError>> {
    return await executeApiCall(async function () {
      console.log(request);
      const response = await axiosInstance.post<PostResponse>('posts', request);
      return response.data;
    });
  },
  getPostsForUser: async function (request: GetPostsForUserRequest): Promise<Result<PostsForUserResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<PostsForUserResponse>(`users/${request.username}/posts`);
      return response.data;
    });
  },
  getPostDetails: async function (postId: string): Promise<Result<PostDetailsResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<PostDetailsResponse>(`posts/details/${postId}`);
      return response.data;
    });
  },
};
