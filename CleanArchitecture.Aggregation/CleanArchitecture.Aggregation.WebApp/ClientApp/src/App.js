import React, { Component } from 'react';
import { Routing } from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import TodoProvider from './context/todoContext';
import ProductProvider from './context/productContext';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <ProductProvider>
        <TodoProvider>
          <Layout>
            <Routing />
          </Layout>
        </TodoProvider>
      </ProductProvider>
    );
  }
}
