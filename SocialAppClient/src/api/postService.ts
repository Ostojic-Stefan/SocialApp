import { axiosInstance, executeApiCall } from './apiConfig';
import { UploadImageResponse } from './dtos/image';
import { CreatePostRequest, PostDetailsResponse, PostResponse, PostsForUserRequest, PostsForUserResponse } from './dtos/post';
import { ApiError, Result } from './models';

export interface IPostService {
  getAllPosts: () => Promise<Result<PostResponse[], ApiError>>;
  getPostById: (postId: string) => Promise<Result<PostResponse, ApiError>>;
  uploadPostImage: (request: FormData) => Promise<Result<UploadImageResponse, ApiError>>;
  uploadPost: (request: CreatePostRequest) => Promise<Result<boolean, ApiError>>;
  getPostsForUser: (request: PostsForUserRequest) => Promise<Result<PostsForUserResponse, ApiError>>;
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
  uploadPostImage: async function (request: FormData): Promise<Result<UploadImageResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<UploadImageResponse>('posts/images', request, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      return response.data;
    });
  },
  uploadPost: async function (request: CreatePostRequest): Promise<Result<boolean, ApiError>> {
    return await executeApiCall(async function () {
      console.log(request);
      const response = await axiosInstance.post<boolean>('posts', request);
      return response.data;
    });
  },
  getPostsForUser: async function (request: PostsForUserRequest): Promise<Result<PostsForUserResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<PostsForUserResponse>(`users/${request.userProfileId}/posts`);
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
