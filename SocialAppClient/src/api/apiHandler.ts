import axios, { AxiosError, AxiosResponse } from "axios";
import { GetAllPostsResponse, Post, UploadPost } from "../features/posts/types";
import { CommentResponse, UserInfomation, UserLoginRequest, UserLoginResponse, UserRegisterRequest } from "../features/user/types";

axios.defaults.baseURL = '/api';

axios.defaults.withCredentials = true;

axios.interceptors.request.use((config) => {
  const token = localStorage.getItem("accessToken");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

axios.interceptors.response.use(null, (error: AxiosError) => {
  console.log("INTERCEPTOR:", error);
  if (!error.response?.status && !error.response?.data) return;
  const { status, data }: { status: number; data: any } = error.response;
  switch (status) {
    case 400:
      if (data.errors) {
        const modelStateErrors: string[] = [];
        for (const key in data.errors) {
          if (data.errors[key]) {
            modelStateErrors.push(data.errors[key]);
          }
        }
        throw modelStateErrors.flat();
      }
      break;
    case 401:
      break;
    case 403:
      break;
    case 500:
      break;
    default:
      break;
  }
});

const responseBody = <TResponse>(res: AxiosResponse<TResponse>) => res.data;

const user = {
  getInformation: () => axios.get('identity/me').then(responseBody<UserInfomation>),
  login: (loginRequest: UserLoginRequest) => axios.post('identity/login', loginRequest)
      .then(responseBody<UserLoginResponse>),
  register: (registerRequest: UserRegisterRequest) => axios.post('identity/register', registerRequest)
    .then(responseBody<void>)
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