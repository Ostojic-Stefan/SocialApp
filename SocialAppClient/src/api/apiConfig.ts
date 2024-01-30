import axios, { AxiosError } from "axios";
import { ApiError, Result } from "./models";

export const axiosInstance = axios.create({
  baseURL: 'http://localhost:5000/api',
  withCredentials: true
});

axiosInstance.interceptors.response.use(null, (error: AxiosError) => {
  if (error.response?.data) {
    const responseData = error.response?.data!;
    // @ts-ignore
    const apiError = new ApiError(responseData.statusCode, responseData.title, responseData.errorMessages);
    return Promise.reject(apiError);
  }
  return Promise.reject(error);
});

// axiosInstance.interceptors.request.use(function (config) {
//   const authToken = JSON.parse(window.localStorage.getItem("auth")!);
//   config.headers.set("Authorization", `Bearer ${authToken}`);
//   return config;
// }, function (error) {
//   return Promise.reject(error);
// });

export async function executeApiCall<TResult>(func: () => Promise<TResult>): Promise<Result<TResult, ApiError>> {
  try {
    return { hasError: false, value: await func() }
  } catch (error) {
    if (error instanceof ApiError) {
      return { hasError: true, error };
    }
    return { hasError: true, error: new ApiError(500, "Unknown Error", []) }
  }
}
