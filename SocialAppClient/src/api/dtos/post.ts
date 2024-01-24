import { CommentResponse } from "./comment";
import { ImageResponse } from "./image";
import { UserInfoResponse } from "./user";

type PostLikeInfo = {
    likeId: string;
    likedByCurrentUser: boolean;
}

export type PostResponse = {
    id: string;
    images: ImageResponse[];
    title: string;
    contents: string;
    userInfo: UserInfoResponse;
    likeInfo: PostLikeInfo;
    numLikes: number;
    numComments: number;
    createdAt: Date;
    updatedAt: Date;
}

export type PostDetailsResponse = {
    id: string;
    images: ImageResponse[];
    contents: string;
    userInfo: UserInfoResponse;
    likeInfo: PostLikeInfo;
    numLikes: number;
    numComments: number;
    createdAt: Date;
    updatedAt: Date;
    comments: CommentResponse[];
}

export type PostsForUserResponse = {
    userInfo: UserInfoResponse;
    posts: PostResponse[];
}

// requests

export type CreatePostRequest = {
    imageName: string;
    contents: string;
    title: string;
}

export type PostsForUserRequest = {
    userProfileId: string;
}
