import { Route, Routes } from 'react-router-dom';
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { ProductList, ProductDetail, ProductAdd } from "./components/products"
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
      <Route path='/fetch-data' element={<FetchData/>} />
      <Route path='products' element={<ProductList/>}>
        <Route path='add' element={<ProductAdd/>} />
        {/* <Route path=':id' element={<ProductDetail/>} /> */}
      </Route>
    </Routes>
  );
}