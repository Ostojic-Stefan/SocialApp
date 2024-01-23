import { UserInfoResponse } from "./user";

export enum LikeReaction {
    Like, Heart, Happy, Sad, TearsOfJoy
}

export type LikeResponse = {
    id: string;
    reaction: LikeReaction;
    userInfo: UserInfoResponse;
}

export type LikesForPostResponse = {
    postId: string;
    likes: LikeResponse[];
}

export type LikeAddResponse = {
    likeId: string;
    postId: string;
}

export type DeleteLikeResponse = {
    postId: string;
}

// TODO: get likes for user

export type LikePostRequest = {
    postId: string;
    likeReaction: LikeReaction;
}

export type LikesForPostRequest = {
    postId: string;
}

export type DeleteLikeRequest = {
    postId: string;
    likeId: string;
}