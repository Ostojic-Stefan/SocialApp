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

export type UserDetailsResponse = {
    userInfo: UserInfoResponse;
    isFriend: boolean;
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