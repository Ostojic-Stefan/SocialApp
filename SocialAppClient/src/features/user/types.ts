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
    avatarUrl?: string;
}

export interface UserInfomation {
    username: string;
    biography: string;
    avatarUrl: string;
}

export interface CommentResponse {
    id: string;
    contents: string;
    postId: string;
    userProfileId: string;
    createdAt: Date;
    updatedAt: Date;
}