import axios, { AxiosError } from "axios";
import { useSearchParams } from "react-router-dom";

export const useQueryString = () => {
    const [searchParams] = useSearchParams();
    const searchParamsObject = Object.fromEntries([...searchParams]);
    return searchParamsObject;
}

// export isAxiosError function add generic type any and return boolean
export const isAxiosError = <T>(error: any): error is AxiosError<T> => {
    return axios.isAxiosError(error);
}