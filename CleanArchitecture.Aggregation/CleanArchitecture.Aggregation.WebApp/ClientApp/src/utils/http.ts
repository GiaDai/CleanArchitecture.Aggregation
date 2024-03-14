// create class Http has contructor to instance AxiosInstance with base url and headers

import axios, { AxiosInstance } from 'axios';

class Http {
  public instance: AxiosInstance;

  constructor() {
    this.instance = axios.create({
      baseURL: '/api',
      timeout: 10000,
      headers: {
        'Content-Type': 'application/json',
      },
    });
  }
}

const http = new Http().instance;

// generate instance of AxiosInstance has hearder Authorization with token from local storage if it exists
// export default instance of AxiosInstance
class HttpAuthen extends Http {
  constructor() {
    super();
    this.instance.interceptors.request.use((config) => {
      const token = localStorage.getItem('jwToken');
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    });
  }
}

const httpAuthen = new HttpAuthen().instance;

export { httpAuthen };

export default http;