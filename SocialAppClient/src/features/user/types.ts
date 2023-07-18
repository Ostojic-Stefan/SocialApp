export interface UserLoginRequest {
    email: string;
    password: string;
}

export interface UserLoginResponse {
    accessToken: string;
}

export interface UserInfomation {
    username: string;
    biography: string;
    avatarUrl: string;
}