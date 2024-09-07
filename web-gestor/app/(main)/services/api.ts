import axios from 'axios';
import { parseCookies } from 'nookies';

export const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_URL_API
});

api.interceptors.request.use((request) => {
  const { ['Universitario.token']: token } = parseCookies()

  if (token) {
    request.headers.Authorization = `Bearer ${token}`;
  }

  return request;
});

api.interceptors.response.use(
  response => response,
  error => {
    if (error.response.status === 401) {
      window.location.href = '/login';
    }
    if (error.response.status !== 204)
      throw error;
  });
