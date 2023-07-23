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

export interface LoggedInUserInfomation {
    username: string;
    biography: string;
    avatarUrl: string;
}
