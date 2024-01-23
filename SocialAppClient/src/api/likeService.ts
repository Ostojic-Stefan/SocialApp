import { axiosInstance, executeApiCall } from "./apiConfig";
import { DeleteLikeRequest, DeleteLikeResponse, LikeAddResponse, LikePostRequest, LikesForPostRequest, LikesForPostResponse } from "./dtos/like";
import { ApiError, Result } from "./models";

export interface ILikeService {
  addLikeToPost: (request: LikePostRequest) => Promise<Result<LikeAddResponse, ApiError>>;
  getAllLikesForPost: (request: LikesForPostRequest) => Promise<Result<LikesForPostResponse, ApiError>>;
  deleteLike: (request: DeleteLikeRequest) => Promise<Result<DeleteLikeResponse, ApiError>>;
}

export const likeService: ILikeService = {
  addLikeToPost: async function (request: LikePostRequest): Promise<Result<LikeAddResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<LikeAddResponse>(
        `posts/${request.postId}/likes`, request
      );
      return response.data;
    });
  },
  getAllLikesForPost: async function (request: LikesForPostRequest): Promise<Result<LikesForPostResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<LikesForPostResponse>(
        `posts/${request.postId}/likes`
      );
      return response.data;
    });
  },
  deleteLike: async function (request: DeleteLikeRequest): Promise<Result<DeleteLikeResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.delete<DeleteLikeResponse>(
        `posts/${request.postId}/likes/${request.likeId}`
      );
      return response.data;
    });
  },
};
