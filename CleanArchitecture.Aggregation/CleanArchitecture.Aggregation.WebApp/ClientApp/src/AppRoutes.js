import * as React from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { ProductList, ProductDetail, ProductAdd } from "./components/products"
import { UserContext } from './context/userContext';
import { Login } from './components/users';
const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: '/products',
    element: <ProductList />,
    children: [
      {
        path: 'add',
        element: <ProductAdd />
      },
      {
        path: ':id',
        element: <ProductDetail />
      }
    ]
  }
];

export default AppRoutes;

export const RenderRoutes = () => {
  return (
    <Routes>
      {AppRoutes.map((route, index) => {
        const { element, ...rest } = route;
        return <Route key={index} {...rest} element={element} />;
      })}
    </Routes>
  );
}

export const Routing = () => {
  return (
    <Routes>
      <Route exact path='/' element={<Home/>} />
      <Route path='/counter' element={<Counter/>} />
      <Route path='/login' element={<Login/>} />
      <Route path='/fetch-data' element={
        <ProtectedRoutes>
        <FetchData/>
        </ProtectedRoutes>
      } />
      <Route path='/products'>
        <Route index={true} element={<ProductList/>} />
        <Route path='add' element={<ProductAdd/>} />
        <Route path=':id' element={<ProductDetail/>} />
      </Route>
    </Routes>
  );
}

const ProtectedRoutes = ({children}) => {
  const { user } = React.useContext(UserContext);
  if (!user) {
    return <Navigate to="/login" />;
  }
  return children;
}