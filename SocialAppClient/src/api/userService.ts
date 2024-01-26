import { axiosInstance, executeApiCall } from './apiConfig';
import { UpdateFriendStatusRequest } from './dtos/friend';
import { ImageResponse, UploadImageResponse } from './dtos/image';
import { AddUserImageRequest, GetUserImagesRequest, SendFriendRequest, SetProfileImageRequest, UpdateUserProfileRequest, UserDetailsResponse, UserInfoResponse, UserProfileInformationRequest } from './dtos/user';
import { ApiError, Result } from './models';

interface IUserService {
  uploadImage: (request: FormData) => Promise<Result<UploadImageResponse, ApiError>>;
  addUserImage: (request: AddUserImageRequest) => Promise<Result<boolean, ApiError>>;
  setProfileImage: (request: SetProfileImageRequest) => Promise<Result<boolean, ApiError>>;
  getUserProfileInformation: (request: UserProfileInformationRequest) => Promise<Result<UserDetailsResponse, ApiError>>;
  getUserImages: (request: GetUserImagesRequest) => Promise<Result<ImageResponse[], ApiError>>;
  updateUserProfile: (request: UpdateUserProfileRequest) => Promise<Result<boolean, ApiError>>;
  sendFriendRequest: (request: SendFriendRequest) => Promise<Result<boolean, ApiError>>;
  updateFriendRequestStatus: (request: UpdateFriendStatusRequest) => Promise<Result<boolean, ApiError>>;
  getFriends: (userId: string) => Promise<Result<UserInfoResponse[], ApiError>>;
}

export const userService: IUserService = {
  uploadImage: async function (request: FormData): Promise<Result<UploadImageResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<UploadImageResponse>('users/images', request, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      return response.data;
    });
  },
  setProfileImage: async function (request: SetProfileImageRequest): Promise<Result<boolean, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<boolean>('users/images/set', request);
      return response.data;
    });
  },
  getUserProfileInformation: async function (request: UserProfileInformationRequest): Promise<Result<UserDetailsResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<UserDetailsResponse>(`users/${request.username}`);
      return response.data;
    });
  },
  getUserImages: async function (request: GetUserImagesRequest): Promise<Result<ImageResponse[], ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<ImageResponse[]>(`users/${request.userProfileId}/images`);
      return response.data;
    });
  },
  addUserImage: async function (request: AddUserImageRequest): Promise<Result<boolean, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<boolean>(`users/images/add`, request);
      return response.data;
    });
  },
  updateUserProfile: async function (request: UpdateUserProfileRequest): Promise<Result<boolean, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.put<boolean>('users', request);
      return response.data;
    });
  },
  sendFriendRequest: async function (request: SendFriendRequest): Promise<Result<boolean, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<boolean>(`users/${request.userId}/friendRequests`);
      return response.data;
    });
  },
  updateFriendRequestStatus: async function (request: UpdateFriendStatusRequest): Promise<Result<boolean, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.put<boolean>(`users/${request.userId}/friendRequests`, { status: request.status });
      return response.data;
    });
  },
  getFriends: async function (userId: string): Promise<Result<UserInfoResponse[], ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<UserInfoResponse[]>(`users/${userId}/friends`);
      return response.data;
    });
  }
};
