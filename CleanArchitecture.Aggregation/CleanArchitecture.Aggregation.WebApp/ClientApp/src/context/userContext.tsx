import * as React from 'react';
import { faker } from '@faker-js/faker';
import { IUserResponse, UserContextType, ILogin } from '../@types/user';

export const UserContext = React.createContext<UserContextType | null>(null);

const UserProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [userContext, setUserContext] = React.useState<IUserResponse | null>(null);

  const loginContext = (loginData : IUserResponse) => {
    setUserContext(loginData);
  };

    const logoutContext = () => {
        setUserContext(null);
    };

    return (
        <UserContext.Provider value={{ userContext, loginContext, logoutContext }}>
            {children}
        </UserContext.Provider>
    );
}

export default UserProvider;