export interface IUser {
    id: string;
    userName: string;
    email: string;
}

export interface ILogin {
    email: string;
    password: string;
}

export type UserContextType = {
    user: Iuser;
    login: (login: ILogin) => void;
    logout: () => void;
};


export const userContextDefaultValue: UserContextType = {
    user: {id:"", userName:"", email:""},
    login: (login: ILogin) => {},
    logout: () => {},
};

