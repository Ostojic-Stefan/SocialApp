import { axiosInstance, executeApiCall } from "./apiConfig";
import { ApiError, Result } from "./models";

export type SetProfileImageRequest = {
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
};

export type FriendResponse = {
  userProfileId: string;
  avatarUrl: string;
  username: string;
};

// prettier-ignore
interface IUserService {
  uploadProfileImage: (request: FormData) => Promise<Result<string, ApiError>>;
  setProfileImage: (request: SetProfileImageRequest) => Promise<Result<boolean, ApiError>>;
  getUserProfileInformation: (request: GetUserProfileInformationRequest) => Promise<Result<UserProfileInformation, ApiError>>;
  getFriends: () => Promise<Result<FriendResponse[], ApiError>>;
}

export const userService: IUserService = {
  uploadProfileImage: async function (
    request: FormData
  ): Promise<Result<string, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<string>(
        "userprofiles/uploadImage",
        request,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      return response.data;
    });
  },
  setProfileImage: async function (
    request: SetProfileImageRequest
  ): Promise<Result<boolean, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.post<boolean>(
        "userprofiles/setImage",
        request
      );
      return response.data;
    });
  },
  getUserProfileInformation: async function (
    request: GetUserProfileInformationRequest
  ): Promise<Result<UserProfileInformation, ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<UserProfileInformation>(
        `users/${request.username}`
      );
      return response.data;
    });
  },
  getFriends: async function (): Promise<Result<FriendResponse[], ApiError>> {
    return await executeApiCall(async function () {
      const response = await axiosInstance.get<FriendResponse[]>(
        "userprofiles/friends"
      );
      return response.data;
    });
  },
};
