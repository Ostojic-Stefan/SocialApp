import { axiosInstance, executeApiCall } from "./apiConfig";
import { CurrentUserResponse, IdentityResponse, LoginRequest, RegisterRequest } from "./dtos/identity";
import { ApiError, Result } from "./models";


export interface IIdentityService {
    login: (request: LoginRequest) => Promise<Result<IdentityResponse, ApiError>>;
    register: (request: RegisterRequest) => Promise<Result<IdentityResponse, ApiError>>;
    getCurrentUserInfo: () => Promise<Result<CurrentUserResponse, ApiError>>;
}

export const identityService: IIdentityService = {
    login: async function (request: LoginRequest): Promise<Result<IdentityResponse, ApiError>> {
        return await executeApiCall(async function () {
            return (await axiosInstance.post('identity/login', request)).data;
        });
    },
    register: async function (request: RegisterRequest): Promise<Result<IdentityResponse, ApiError>> {
        return await executeApiCall(async function () {
            return (await axiosInstance.post('identity/register', request)).data;
        });
    },
    getCurrentUserInfo: async function (): Promise<Result<CurrentUserResponse, ApiError>> {
        return await executeApiCall(async function () {
            const response = await axiosInstance.get<CurrentUserResponse>('identity/me');
            return response.data;
        })
    }
}