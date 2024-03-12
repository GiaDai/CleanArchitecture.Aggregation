import http from "../utils/http";
import { IProductPaging } from '../@types/product';

// export const getProducts has parameter PageNumber & PageSize and return Promise of AxiosResponse
export const getProducts = (PageNumber: number, PageSize: number) => {
  return http.get<IProductPaging>(`/v1/product`, {
   params: {
        pageNumber: PageNumber,
        pageSize: PageSize
    } 
  });
};