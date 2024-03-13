import http from "../utils/http";
import { ILogin, ILoginResponse } from "../@types/user";

// export const postLogin has parameter ILogin and return Promise of AxiosResponse
export const postLogin = (login: ILogin) => {
  return http.post<ILoginResponse>('/v1/user/login', login);
};