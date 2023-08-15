import { axiosInstance, executeApiCall } from "./apiConfig";
import { ApiError, Result } from "./models";
import { UserInfo } from "./postService";

export enum LikeReaction {
    Like, Heart, Happy, Sad, TearsOfJoy
}

export type AddLikeToPostRequest = {
    postId: string;
    reaction: LikeReaction;
}

export type GetAllLikesForPostRequest = {
    postId: string;
}

export type LikeForAPostResponse = {
    postId: string;
    likeInfo: {
        likeReaction: LikeReaction;
        id: string;
        likeUserInformation: UserInfo
    }[];
    likedByUser: boolean;
}

export type DeleteLikeRequest = {
    likeId: string;
}

export type DeleteLikeResponse = {
    postId: string;
}

interface ILikeService {
    addLikeToPost: (request: AddLikeToPostRequest) => Promise<Result<LikeForAPostResponse, ApiError>>;
    getAllLikesForPost: (request: GetAllLikesForPostRequest) => Promise<Result<LikeForAPostResponse, ApiError>>;
    deleteLike: (request: DeleteLikeRequest) => Promise<Result<DeleteLikeResponse, ApiError>>;
}

export const likeService: ILikeService = {
    addLikeToPost: async function (request: AddLikeToPostRequest): Promise<Result<LikeForAPostResponse, ApiError>> {
        return await executeApiCall(async function () {
            const response = await axiosInstance
                .post<LikeForAPostResponse>(`likes/posts/${request.postId}`, { likeReaction: request.reaction });
            return response.data;
        });
    },
    getAllLikesForPost: async function (request: GetAllLikesForPostRequest): Promise<Result<LikeForAPostResponse, ApiError>> {
        return await executeApiCall(async function () {
            const response = await axiosInstance.get<LikeForAPostResponse>(`likes/posts/${request.postId}`);
            return response.data;
        });
    },
    deleteLike: async function (request: DeleteLikeRequest): Promise<Result<DeleteLikeResponse, ApiError>> {
        return await executeApiCall(async function () {
            const response = await axiosInstance.delete<DeleteLikeResponse>(`likes/${request.likeId}`);
            return response.data;
        });
    }
}