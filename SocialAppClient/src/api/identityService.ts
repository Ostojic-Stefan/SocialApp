import { axiosInstance, executeApiCall } from "./apiConfig";
import { ApiError, Result } from "./models";

export type UserLoginRequest = {
    email: string;
    password: string;
}

export type UserRegisterRequest = {
    username: string;
    email: string;
    password: string;
    biography?: string;
    imageData?: FormData;
}

export type UserInformation = {
    userId: string;
    createdAt: Date;
    updatedAt: Date;
    username: string;
    biography: string;
    avatarUrl: string;
}

export type CommentsOnPost = {
    commentId: string;
    postId: string;
    commenterAvatarUrl: string;
    commenterUsername: string;
    contentsReduced: string;
}

export type LikesOnPost = {
    likeId: string;
    postId: string;
    likerAvatarUrl: string;
    likerUsername: string;
    likerReaction: number; // make enum
}

export type Notifications = {
    commentsOnPost: CommentsOnPost[];
    likesOnPost: LikesOnPost[];
}

export type FriendRequests = {
    requesterId: string;
    requeserAvatarUrl: string;
    requesterUsername: string;
    requestTimeSent: Date;
}

export type CurrentUserInfo = {
    userInformation: UserInformation;
    // notifications: Notifications;
    // friendRequests: FriendRequests[];
}

export interface IIdentityService {
    login: (request: UserLoginRequest) => Promise<Result<{ data: string }, ApiError>>;
    register: (request: UserRegisterRequest) => Promise<Result<{ data: string }, ApiError>>;
    getCurrentUserInfo: () => Promise<Result<CurrentUserInfo, ApiError>>;
}

export const identityService: IIdentityService = {
    login: async function (request: UserLoginRequest): Promise<Result<{ data: string }, ApiError>> {
        return await executeApiCall(async function () {
            return await axiosInstance.post('identity/login', request);
        });
    },
    register: async function (request: UserRegisterRequest): Promise<Result<{ data: string }, ApiError>> {
        return await executeApiCall(async function () {
            return await axiosInstance.post('identity/register', request);
        });
    },
    getCurrentUserInfo: async function (): Promise<Result<CurrentUserInfo, ApiError>> {
        return await executeApiCall(async function () {
            const response = await axiosInstance.get<CurrentUserInfo>('identity/me');
            return response.data;
        })
    }
}