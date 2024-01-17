import { axiosInstance, executeApiCall } from './apiConfig';
import { ApiError, Result } from './models';

export type SetProfileImageRequest = {
  avatarUrl: string;
};

export type UploadProfileImageResponse = {
  avatarUrl: string;
};

export type GetUserProfileInformationRequest = {
  username: string;
};

export type UserProfileInformation = {
  userProfileId: string;
  username: string;
  biography: string;
  avatarUrl: string;
  isFriend: boolean;
};

export type FriendResponse = {
  userProfileId: string;
  avatarUrl: string;
  username: string;
};

export type SendFriendRequest = {
  userId: string;
}

// prettier-ignore
interface IUserService {
  // uploads the profile image
  uploadProfileImage: (request: FormData) => Promise<Result<UploadProfileImageResponse, ApiError>>;

  // sets the profile image for a currently logged in user
  setProfileImage: (request: SetProfileImageRequest) => Promise<Result<string, ApiError>>;

  // gets the user profile information for a given username
  getUserProfileInformation: (request: GetUserProfileInformationRequest) => Promise<Result<UserProfileInformation, ApiError>>;

  // gets all the friends for a given user id
  getFriends: (userId: string) => Promise<Result<FriendResponse[], ApiError>>;

  // sends a friend request to the user with userId
  sendFriendRequest: (request: SendFriendRequest) => Promise<Result<boolean, ApiError>>;
}

export const userService: IUserService = {
  uploadProfileImage: async function (request: FormData): Promise<Result<UploadProfileImageResponse, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<UploadProfileImageResponse>('users/images', request, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      return response.data;
    });
  },
  setProfileImage: async function (request: SetProfileImageRequest): Promise<Result<string, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<string>('users/setImage', request);
      return response.data;
    });
  },
  getUserProfileInformation: async function (
    request: GetUserProfileInformationRequest
  ): Promise<Result<UserProfileInformation, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<UserProfileInformation>(`users/${request.username}`);
      return response.data;
    });
  },
  getFriends: async function (userId: string): Promise<Result<FriendResponse[], ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<FriendResponse[]>(`users/${userId}/friends`);
      return response.data;
    });
  },
  sendFriendRequest: async function (request: SendFriendRequest): Promise<Result<boolean, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<boolean>(`users/${request.userId}/friendRequests`);
      return response.data;
    });
  }
};
