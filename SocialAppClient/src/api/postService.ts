import { axiosInstance, executeApiCall } from "./apiConfig";
import { ApiError, Result } from "./models";

export type GetPostsForUserRequest = {
    username: string;
}

export type UserInfo = {
    userProfileId: string;
    username: string;
    avatarUrl: string;
}

export type PostResponse = {
    id: string;
    imageUrl: string;
    contents: string;
    createdAt: Date;
    updatedAt: Date;
    userInfo: UserInfo;
    numLikes: number;
    numComments: number;
}

export type GetAllPostsResponse = {
    items: PostResponse[];
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
}

export type UploadPostRequest = {
    contents: string;
    imageUrl: string;
}


interface IPostService {
    getAllPosts: () => Promise<Result<GetAllPostsResponse, ApiError>>;
    uploadPostImage: (request: FormData) => Promise<Result<string, ApiError>>;
    uploadPost: (request: UploadPostRequest) => Promise<Result<PostResponse, ApiError>>;
    getPostsForUser: (request: GetPostsForUserRequest) => Promise<Result<PostResponse[], ApiError>>;
}

export const postService: IPostService = {
    getAllPosts: async function (): Promise<Result<GetAllPostsResponse, ApiError>> {
        return await executeApiCall(async function () {
            const response = await axiosInstance.get<GetAllPostsResponse>('posts');
            return response.data;
        });
    },
    uploadPostImage: async function (request: FormData): Promise<Result<string, ApiError>> {
        return await executeApiCall(async function () {
            const response = await axiosInstance.post<string>('posts/upload', request, {
                headers: {
                    "Content-Type": "multipart/form-data",
                },
            });
            return response.data;
        });
    },
    uploadPost: async function (request: UploadPostRequest): Promise<Result<PostResponse, ApiError>> {
        return await executeApiCall(async function () {
            const response = await axiosInstance.post<PostResponse>('posts', request);
            return response.data;
        });
    },
    getPostsForUser: async function (request: GetPostsForUserRequest): Promise<Result<PostResponse[], ApiError>> {
        return await executeApiCall(async function () {
            const response = await axiosInstance.get<PostResponse[]>(`posts/user/${request.username}`);
            return response.data;
        });
    }
}