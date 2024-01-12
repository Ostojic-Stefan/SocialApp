import { axiosInstance, executeApiCall } from './apiConfig';
import { ApiError, Result } from './models';

export type GetCommentOnAPostRequest = {
  postId: string;
};

export type AddCommentRequest = {
  postId: string;
  contents: string;
};

// TODO: why not just be the same as CommentResponse?
export type AddCommentResponse = {
  id: string;
  contents: string;
  postId: string;
  userProfileId: string;
  createdAt: Date;
  updatedAt: Date;
};

// TODO: make include UserInfo
export type CommentResponse = {
  id: string;
  contents: string;
  // postId: string;
  userProfileId: string;
  avatarUrl: string;
  username: string;
  createdAt: Date;
  updatedAt: Date;
};

export type CommentsFromPostResponse = {
  postId: string;
  comments: CommentResponse[];
};

export type CommentsForUserRequest = {
  username: string;
}

export type CommentsForUserResponse = {
  id: string;
  contents: string;
  imageUrl: string;
  userProfileId: string;
  avatarUrl: string;
  username: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface ICommentService {
  getCommentsOnAPost: (request: GetCommentOnAPostRequest) => Promise<Result<CommentsFromPostResponse, ApiError>>;
  addCommentToAPost: (request: AddCommentRequest) => Promise<Result<AddCommentResponse, ApiError>>;
  getCommentsForUser: (request: CommentsForUserRequest) => Promise<Result<CommentsForUserResponse, ApiError>>;
}

export const commentService: ICommentService = {
  getCommentsOnAPost: async function (
    request: GetCommentOnAPostRequest
  ): Promise<Result<CommentsFromPostResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<CommentsFromPostResponse>(`posts/${request.postId}/comments`);
      return response.data;
    });
  },
  addCommentToAPost: async function (request: AddCommentRequest): Promise<Result<AddCommentResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<AddCommentResponse>(`posts/${request.postId}/comments`, {
        contents: request.contents,
      });
      return response.data;
    });
  },
  getCommentsForUser: async function (request: CommentsForUserRequest): Promise<Result<CommentsForUserResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<CommentsForUserResponse>(`users/${request.username}/comments`);
      return response.data;
    });
  }
};
