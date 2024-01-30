import { axiosInstance, executeApiCall } from "./apiConfig";
import { CurrentUserResponse, LoginRequest, RegisterRequest } from "./dtos/identity";
import { ApiError, Result } from "./models";


export interface IIdentityService {
    login: (request: LoginRequest) => Promise<Result<void, ApiError>>;
    register: (request: RegisterRequest) => Promise<Result<boolean, ApiError>>;
    logout: () => Promise<Result<void, ApiError>>;
    getCurrentUserInfo: () => Promise<Result<CurrentUserResponse, ApiError>>;
}

export const identityService: IIdentityService = {
    login: async function (request: LoginRequest): Promise<Result<void, ApiError>> {
        return await executeApiCall(async function () {
            return (await axiosInstance.post('identity/login', request)).data;
        });
    },
    register: async function (request: RegisterRequest): Promise<Result<boolean, ApiError>> {
        return await executeApiCall(async function () {
            return (await axiosInstance.post('identity/register', request)).data;
        });
    },
    getCurrentUserInfo: async function (): Promise<Result<CurrentUserResponse, ApiError>> {
        return await executeApiCall(async function () {
            const response = await axiosInstance.get<CurrentUserResponse>('identity/me');
            return response.data;
        });
    },
    logout: async function (): Promise<Result<void, ApiError>> {
        return await executeApiCall(async function () {
            await axiosInstance.post<void>('identity/logout');
        });
    }
}