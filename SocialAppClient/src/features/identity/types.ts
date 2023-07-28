export interface UserLoginRequest {
    email: string;
    password: string;
}

export interface UserLoginResponse {
    accessToken: string;
}

export interface UserRegisterRequest {
    username: string;
    email: string;
    password: string;
    biography?: string;
    imageData?: FormData;
}

interface UserInformation {
    userId: string;
    createdAt: Date;
    updatedAt: Date;
    username: string;
    biography: string;
    avatarUrl: string;
}

interface CommentsOnPost {
    commentId: string;
    postId: string;
    commenterAvatarUrl: string;
    commenterUsername: string;
    contentsReduced: string;
}

interface LikesOnPost {
    likeId: string;
    postId: string;
    likerAvatarUrl: string;
    likerUsername: string;
    likerReaction: number; // make enum
}

interface Notifications {
    commentsOnPost: CommentsOnPost[];
    likesOnPost: LikesOnPost[];
}

interface FriendRequests {
    requesterId: string;
    requeserAvatarUrl: string;
    requesterUsername: string;
    requestTimeSent: Date;
}

export interface LoggedInUserInfomation {
    userInformation: UserInformation;
    notifications: Notifications;
    friendRequests: FriendRequests[];
}
