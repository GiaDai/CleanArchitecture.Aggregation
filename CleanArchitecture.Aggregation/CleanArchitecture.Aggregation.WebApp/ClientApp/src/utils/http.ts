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

export default http;