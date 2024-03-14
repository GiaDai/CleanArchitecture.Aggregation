import { httpAuthen } from "../utils/http";
import { IProductPaging, IProduct } from '../@types/product';

// export const getProducts has parameter PageNumber & PageSize and return Promise of AxiosResponse
export const getProducts = (PageNumber: number, PageSize: number) => {
  return httpAuthen.get<IProductPaging>(`/v1/product`, {
   params: {
        pageNumber: PageNumber,
        pageSize: PageSize
    } 
  });
};

// export const postAddProduct has parameter IProduct and return Promise of AxiosResponse
export const postAddProduct = (product: Omit<IProduct,'id'>) => {
  return httpAuthen.post<IProduct>('/v1/product', product);
};

// export const putUpdateProduct has parameter IProduct and return Promise of AxiosResponse
export const putUpdateProduct = (product: IProduct) => {
  return httpAuthen.put<IProduct>(`/v1/product/${product.id}`, product);
};

// export const deleteProduct has parameter id and return Promise of AxiosResponse
export const deleteProduct = (id: number) => {
  return httpAuthen.delete<IProduct>(`/v1/product/${id}`);
};

// export const getProduct has parameter id and return Promise of AxiosResponse
export const getProduct = (id: number) => {
  return httpAuthen.get<IProduct>(`/v1/product/${id}`);
};

// export const getSearchProduct has parameter name and return Promise of AxiosResponse
export const getSearchProduct = (name: string) => {
  return httpAuthen.get<IProduct[]>(`/v1/product/search?name=${name}`);
};
