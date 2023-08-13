import { axiosInstance, executeApiCall } from "./apiConfig";
import { ApiError, Result } from "./models";

export enum LikeReaction {
    Like, Heart, Happy, Sad, TearsOfJoy
}

export type AddLikeToPostRequest = {
    postId: string;
    reaction: LikeReaction;
}

// export type AddLikeToPostResponse = {
//     likeReaction: LikeReaction;
//     userProfileId: string;
//     userInformation: UserInfo
// }

export type GetAllLikesForPostRequest = {
    postId: string;
}

export type LikeUserInfo = {
    id: string;
    username: string;
    avatarUrl: string;
}

export type GetLikesForAPostResponse = {
    id: string;
    postId: string;
    likeReaction: LikeReaction;
    userInformation: LikeUserInfo
}

export type DeleteLikeRequest = {
    likeId: string;
}

export type DeleteLikeResponse = {
    postId: string;
}

export type PostLikeAddResponse = {
    postId: string;
    likeId: string;
}

interface ILikeService {
    addLikeToPost: (request: AddLikeToPostRequest) => Promise<Result<PostLikeAddResponse, ApiError>>;
    getAllLikesForPost: (request: GetAllLikesForPostRequest) => Promise<Result<GetLikesForAPostResponse, ApiError>>;
    deleteLike: (request: DeleteLikeRequest) => Promise<Result<DeleteLikeResponse, ApiError>>;
}

export const likeService: ILikeService = {
    addLikeToPost: async function (request: AddLikeToPostRequest): Promise<Result<PostLikeAddResponse, ApiError>> {
        return await executeApiCall(async function () {
            const response = await axiosInstance
                .post<PostLikeAddResponse>(`likes/posts/${request.postId}`, { likeReaction: request.reaction });
            return response.data;
        });
    },
    getAllLikesForPost: async function (request: GetAllLikesForPostRequest): Promise<Result<GetLikesForAPostResponse, ApiError>> {
        return await executeApiCall(async function () {
            const response = await axiosInstance.get<GetLikesForAPostResponse>(`likes/posts/${request.postId}`);
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