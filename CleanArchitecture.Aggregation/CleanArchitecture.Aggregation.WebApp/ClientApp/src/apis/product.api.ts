import http from "../utils/http";
import { IProductPaging, IProduct } from '../@types/product';

// export const getProducts has parameter PageNumber & PageSize and return Promise of AxiosResponse
export const getProducts = (PageNumber: number, PageSize: number) => {
  return http.get<IProductPaging>(`/v1/product`, {
   params: {
        pageNumber: PageNumber,
        pageSize: PageSize
    } 
  });
};

// export const postAddProduct has parameter IProduct and return Promise of AxiosResponse
export const postAddProduct = (product: Omit<IProduct,'id'>) => {
  return http.post<IProduct>('/v1/product', product);
};