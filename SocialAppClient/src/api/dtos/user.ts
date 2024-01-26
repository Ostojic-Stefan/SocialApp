import { ImageResponse } from "./image";

export type UserInfoResponse = {
    userProfileId: string;
    username: string;
    profileImage: ImageResponse;
    biography: string;
}

export type UserImagesResponse = {
    images: ImageResponse[];
}

export enum FriendStatus {
    Friend, NotFriend, WaitingAcceptance, WaitingApproval
}

export type UserDetailsResponse = {
    userInfo: UserInfoResponse;
    friendStatus: FriendStatus;
    createdAt: Date;
    updatedAt: Date;
    numPosts: number;
    numFriends: number;
    numLikes: number;
}

// requests

export type SetProfileImageRequest = {
    imageId: string;
}

export type UserProfileInformationRequest = {
    username: string;
}

export type GetUserImagesRequest = {
    userProfileId: string;
}

export type AddUserImageRequest = {
    avatarUrl: string;
}

export type UpdateUserProfileRequest = {
    username: string;
    biography: string;
}

export type SendFriendRequest = {
    userId: string;
}