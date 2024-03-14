import * as React from 'react';
import { IUserResponse, UserContextType, ILogin } from '../@types/user';

export const UserContext = React.createContext<UserContextType | null>(null);

const UserProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [userContext, setUserContext] = React.useState<IUserResponse | null>(null);

  const loginContext = (loginData : IUserResponse) => {
    setUserContext(loginData);
    localStorage.setItem('jwToken', loginData.jwToken);
    localStorage.setItem('email', loginData.email);
    localStorage.setItem('roles', loginData.roles.join(',')); // join array to string    
  };

    const logoutContext = () => {
        setUserContext(null);
        localStorage.removeItem('jwToken');
        localStorage.removeItem('email');
        localStorage.removeItem('roles');
    };

    return (
        <UserContext.Provider value={{ userContext, loginContext, logoutContext }}>
            {children}
        </UserContext.Provider>
    );
}

export default UserProvider;