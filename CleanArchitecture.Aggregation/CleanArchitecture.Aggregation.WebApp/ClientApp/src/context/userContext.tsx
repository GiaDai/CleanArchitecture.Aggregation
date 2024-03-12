import * as React from 'react';
import { faker } from '@faker-js/faker';
import { IUser, UserContextType, ILogin } from '../@types/user';

export const UserContext = React.createContext<UserContextType | null>(null);

const UserProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = React.useState<IUser | null>(null);

  const login = (loginData : ILogin) => {
    fetch('/api/v1/user/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(loginData),
    })
        .then((response) => response.json())
        .then((json) => {
            console.log(json.data);
        })
        .catch((error) => {
            console.error('Error login', error);
        });
    setUser({id: faker.string.uuid() , userName: faker.person.bio() , email:faker.internet.email() });
  };

    const logout = () => {
        setUser(null);
    };

    return (
        <UserContext.Provider value={{ user, login, logout }}>
            {children}
        </UserContext.Provider>
    );
}

export default UserProvider;