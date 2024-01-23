import { axiosInstance, executeApiCall } from './apiConfig';
import { ImageResponse, UploadImageResponse } from './dtos/image';
import { AddUserImageRequest, GetUserImagesRequest, SetProfileImageRequest, UserDetailsResponse, UserProfileInformationRequest } from './dtos/user';
import { ApiError, Result } from './models';

interface IUserService {
  uploadImage: (request: FormData) => Promise<Result<UploadImageResponse, ApiError>>;
  addUserImage: (request: AddUserImageRequest) => Promise<Result<boolean, ApiError>>;
  setProfileImage: (request: SetProfileImageRequest) => Promise<Result<string, ApiError>>;
  getUserProfileInformation: (request: UserProfileInformationRequest) => Promise<Result<UserDetailsResponse, ApiError>>;
  getUserImages: (request: GetUserImagesRequest) => Promise<Result<ImageResponse[], ApiError>>;


  // gets all the friends for a given user id
  // getFriends: (userId: string) => Promise<Result<FriendResponse[], ApiError>>;

  // sends a friend request to the user with userId
  // sendFriendRequest: (request: SendFriendRequest) => Promise<Result<boolean, ApiError>>;
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
  setProfileImage: async function (request: SetProfileImageRequest): Promise<Result<string, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<string>('users/images/set', request);
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
  }
};
