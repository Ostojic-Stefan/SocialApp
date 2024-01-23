import { axiosInstance, executeApiCall } from './apiConfig';
import { ApiError, Result } from './models';
import { CommentResponse, CommentsForPostRequest, CommentsOnPostResponse, CreateCommentRequest, GetCommentsForUserRequest } from './dtos/comment';

export interface ICommentService {
  getCommentsOnAPost: (request: CommentsForPostRequest) => Promise<Result<CommentsOnPostResponse, ApiError>>;
  addCommentToAPost: (request: CreateCommentRequest) => Promise<Result<CommentResponse, ApiError>>;
  getCommentsForUser: (request: GetCommentsForUserRequest) => Promise<Result<CommentResponse, ApiError>>;
}

export const commentService: ICommentService = {
  getCommentsOnAPost: async function (request: CommentsForPostRequest): Promise<Result<CommentsOnPostResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<CommentsOnPostResponse>(`posts/${request.postId}/comments`);
      return response.data;
    });
  },
  addCommentToAPost: async function (request: CreateCommentRequest): Promise<Result<CommentResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<CommentResponse>(`posts/${request.postId}/comments`, {
        contents: request.contents,
      });
      return response.data;
    });
  },
  getCommentsForUser: async function (request: GetCommentsForUserRequest): Promise<Result<CommentResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<CommentResponse>(`users/${request.username}/comments`);
      return response.data;
    });
  }
};
