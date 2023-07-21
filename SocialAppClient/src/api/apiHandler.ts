import axios, { AxiosError, AxiosResponse } from "axios";
import { GetAllPostsResponse, Post, UploadPost } from "../features/posts/types";
import { CommentResponse, UserInfomation, UserLoginRequest, UserLoginResponse, UserRegisterRequest } from "../features/user/types";

axios.defaults.baseURL = '/api';

axios.defaults.withCredentials = true;

interface CustomError {
  messages: string[];
  code: number;
}

axios.interceptors.response.use(null, (error: AxiosError) => {
  console.log('Axios Interceptor Error => ', error)
  const errorResponse: CustomError = {
  // @ts-ignore
    messages: error.response.data.errorMessages,
  // @ts-ignore
    code: error.response.data.statusCode
  }
  throw errorResponse;
});

const responseBody = <TResponse>(res: AxiosResponse<TResponse>) => res.data;

const user = {
  getInformation: () => axios.get('identity/me').then(responseBody<UserInfomation>),
  login: (loginRequest: UserLoginRequest) => axios.post('identity/login', loginRequest)
    .then(responseBody<void>),
  register: (registerRequest: UserRegisterRequest) => axios.post('identity/register', registerRequest)
    .then(responseBody<void>),
  uploadProfileImage: (data: FormData) => axios.post<string>('identity/uploadImage', data, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  }).then(responseBody<string>),
  setUserProfileImage: (avatarUrl: string) => axios.post<boolean>('identity/setImage', { avatarUrl })
    .then(responseBody<boolean>)
}

const post = {
  getAll: () => axios.get('posts').then(responseBody<GetAllPostsResponse>),
  uploadImage: (data: FormData) => axios.post('posts/upload', data, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    }).then(responseBody<string>),
  uploadPost: (data: UploadPost) => axios.post('posts', data)
    .then(responseBody<Post>)
}

const comment = {
  getCommentsOnAPost: (postId: string) => axios.get(`comments/posts/postId?postId=${postId}`)
    .then(responseBody<CommentResponse>)
}

export const apiHandler = {
    post,
    user,
    comment
}