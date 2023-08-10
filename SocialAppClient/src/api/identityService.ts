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

export type LoggedInUserInfomation = {
    userInformation: UserInformation;
    notifications: Notifications;
    friendRequests: FriendRequests[];
}


interface IIdentityService {
    login: (request: UserLoginRequest) => Promise<Result<void, ApiError>>;
    register: (request: UserRegisterRequest) =>Promise<Result<void, ApiError>>;
    getCurrentUserInfo: () =>  Promise<Result<LoggedInUserInfomation, ApiError>>;
}

export const identityService: IIdentityService = {
    login: async function (request: UserLoginRequest): Promise<Result<void, ApiError>> {
        return await executeApiCall(async function() {
            await axiosInstance.post('identity/login', request);
        });
    },
    register: async function (request: UserRegisterRequest): Promise<Result<void, ApiError>> {
        return await executeApiCall(async function() {
            await axiosInstance.post('identity/register', request);
        });
    },
    getCurrentUserInfo: async function (): Promise<Result<LoggedInUserInfomation, ApiError>> {
        return await executeApiCall(async function() {
            const response = await axiosInstance.get<LoggedInUserInfomation>('identity/me');
            return response.data;
        })
    }
}