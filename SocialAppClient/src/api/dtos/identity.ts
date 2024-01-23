import { CommentResponse } from "./comment";
import { FriendRequestResponse } from "./friend";
import { LikeResponse } from "./like";
import { UserInfoResponse } from "./user";

export type IdentityResponse = {
    accessToken: string;
}

export type NotificationResponse = | {
    notificationType: "Like";
    postId: string;
    like: LikeResponse;
} | {
    notificationType: "Comment";
    postId: string;
    comment: CommentResponse;
};


export type CurrentUserResponse = {
    userInfo: UserInfoResponse;
    notifications: NotificationResponse[];
    friendRequests: FriendRequestResponse[];
}

export type LoginRequest = {
    email: string;
    password: string;
}

export type RegisterRequest = {
    username: string;
    email: string;
    password: string;
}