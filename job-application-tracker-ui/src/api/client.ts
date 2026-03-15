import axios from 'axios';
import type { AxiosInstance, AxiosError } from 'axios';
import type { ApiErrorResponse } from '@/types/api.types';

const apiClient: AxiosInstance = axios.create({
  baseURL: "http://localhost:5146",
  timeout: 10000, // 10 seconds
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor
apiClient.interceptors.request.use(
  (config) => {
    // Add authorization header if token exists (uncomment and implement as needed)
    // const token = localStorage.getItem('authToken');
    // if (token) {
    //   config.headers.Authorization = `Bearer ${token}`;
    // }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor
apiClient.interceptors.response.use(
  (response) => response,
  (error: AxiosError<ApiErrorResponse>) => {
    const responseData = error.response?.data;

    const apiError: ApiErrorResponse = {
      message: responseData?.message ?? error.message ?? 'An unexpected error occurred.',
      errors: responseData?.errors,
    };

    return Promise.reject(apiError);
  }
);


export default apiClient;