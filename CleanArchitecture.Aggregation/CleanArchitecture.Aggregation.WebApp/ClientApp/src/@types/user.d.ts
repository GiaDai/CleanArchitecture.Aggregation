import { BaseResponse } from "./response";

export interface IUser {
    id: string;
    userName: string;
    email: string;
}

export interface ILogin {
    email: string;
    password: string;
}

export interface IUserResponse {
    id: string;
    userName: string;
    email: string;
    isVerified: boolean;
    roles: string[];
    jwToken: string;
}

export interface ILoginResponse extends BaseResponse {
    data: IUserResponse;
}

export type UserContextType = {
    userContext: Iuser;
    loginContext: (login: IUserResponse) => void;
    logoutContext: () => void;
};


export const userContextDefaultValue: UserContextType = {
    userContext: {id:"", userName:"", email:""},
    loginContext: (login: ILogin) => {},
    logoutContext: () => {},
};

