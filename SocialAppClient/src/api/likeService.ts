import { axiosInstance, executeApiCall } from "./apiConfig";
import { ApiError, Result } from "./models";
import { UserInfoResponse } from "./postService";

export enum LikeReaction {
  Like,
  Heart,
  Happy,
  Sad,
  TearsOfJoy,
}

export type AddLikeToPostRequest = {
  postId: string;
  reaction: LikeReaction;
};

export type GetAllLikesForPostRequest = {
  postId: string;
};

export type LikeForAPostResponse = {
  postId: string;
  likeId: string;
  // likeInfo: {
  //   likeReaction: LikeReaction;
  //   id: string;
  //   userInformation: UserInfoResponse;
  // }[];
  // likedByUser: boolean;
};

export type AllLikesForPostResponse = {
  postId: string;
  likeInfo: {
    likeReaction: LikeReaction;
    id: string;
    userInformation: UserInfoResponse;
  }[];
  likedByUser: boolean;
};

export type DeleteLikeRequest = {
  postId: string;
  likeId: string;
};

export type DeleteLikeResponse = {
  postId: string;
};

export interface ILikeService {
  addLikeToPost: (
    request: AddLikeToPostRequest
  ) => Promise<Result<LikeForAPostResponse, ApiError>>;
  getAllLikesForPost: (
    request: GetAllLikesForPostRequest
  ) => Promise<Result<AllLikesForPostResponse, ApiError>>;
  deleteLike: (
    request: DeleteLikeRequest
  ) => Promise<Result<DeleteLikeResponse, ApiError>>;
}

export const likeService: ILikeService = {
  addLikeToPost: async function (
    request: AddLikeToPostRequest
  ): Promise<Result<LikeForAPostResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<LikeForAPostResponse>(
        `posts/${request.postId}/likes`,
        { likeReaction: request.reaction }
      );
      return response.data;
    });
  },
  getAllLikesForPost: async function (
    request: GetAllLikesForPostRequest
  ): Promise<Result<AllLikesForPostResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<AllLikesForPostResponse>(
        `posts/${request.postId}/likes`
      );
      return response.data;
    });
  },
  deleteLike: async function (
    request: DeleteLikeRequest
  ): Promise<Result<DeleteLikeResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.delete<DeleteLikeResponse>(
        `posts/${request.postId}/likes/${request.likeId}`
      );
      return response.data;
    });
  },
};
