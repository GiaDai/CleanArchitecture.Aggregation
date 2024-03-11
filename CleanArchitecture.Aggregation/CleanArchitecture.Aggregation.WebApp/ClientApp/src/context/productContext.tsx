import * as React from 'react';
import { faker } from '@faker-js/faker';
import { IProduct, ProductContextType } from '../@types/product';

export const ProductContext = React.createContext<ProductContextType | null>(null);

const ProductProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [products, setProducts] = React.useState<IProduct[]>([]);
  const [isLoading, setIsLoading] = React.useState<boolean>(false);

    const addProduct = React.useCallback((product: IProduct) => {
        setIsLoading(true);
        fetch('/api/v1/product', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(product),
        })
            .then((response) => response.json())
            .then((json) => {
                product.id = json.data;
                setProducts([...products, product]);
                setIsLoading(false);
            })
            .catch((error) => {
                console.error('Error adding product', error);
                setIsLoading(false);
            })
            .finally(() => {
                setIsLoading(false);
            });
    }
    , [products, setProducts, setIsLoading]);

  const saveProduct = (product: IProduct) => {
    const updatedProducts = products.map((p) => (p.barcode === product.barcode ? product : p));
    setProducts(updatedProducts);
  };

const gerneateProduct = () => {
    const product: IProduct = {
        id:0,
        rate: faker.number.int({ min: 99, max: 999}) ,
        name: faker.person.firstName() + ' ' + faker.person.lastName(),
        barcode: faker.internet.ipv4(),
        description: faker.lorem.paragraph()
    };
    return product;
}
  const updateProduct = React.useCallback((id: number) => {
    setIsLoading(true);
    var product = gerneateProduct();
    product.id = id;
    fetch(`/api/v1/product/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(product),
    })
        .then((response) => response.json())
        .then((json) => {
            const updatedProducts = products.map((p) => (p.id === id ? { ...p, rate: product.rate, name: product.name, description: product.description, barcode: product.barcode } : p));
            setProducts(updatedProducts);
            setIsLoading(false);
        })
        .catch((error) => {
            console.error('Error updating product', error);
            setIsLoading(false);
        })
        .finally(() => {
            setIsLoading(false);
        });
  }
, [products, setProducts, setIsLoading]);

  const removeProduct = React.useCallback((id: number) => {
    setIsLoading(true);
    fetch(`/api/v1/product/${id}`, {
        method: 'DELETE',
    })
        .then((response) => response.json())
        .then((json) => {
            const updatedProducts = products.filter((p) => p.id !== id);
            setProducts(updatedProducts);
            setIsLoading(false);
        })
        .catch((error) => {
            console.error('Error removing product', error);
            setIsLoading(false);
        })
        .finally(() => {
            setIsLoading(false);
        });
  }, [products, setProducts, setIsLoading]);

  const fetchProducts = React.useCallback(() => {
    setIsLoading(true);
    fetch('/api/v1/product')
        .then((response) => response.json())
        .then((json) => {
            setProducts(json.data);
            setIsLoading(false);
        })
        .catch((error) => {
            console.error('Error fetching products', error);
            setIsLoading(false);
        })
        .finally(() => {
            setIsLoading(false);
        });
  }, [setProducts, setIsLoading]);

  const addRangeProducts = React.useCallback((products: IProduct[]) => {
    setIsLoading(true);
    fetch('/api/v1/product/addrange', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({products: products}),
    })
        .then((response) => response.json())
        .then((json) => {
            setProducts([...products, ...products]);
            setIsLoading(false);
        })
        .catch((error) => {
            console.error('Error adding products', error);
            setIsLoading(false);
        })
        .finally(() => {
            setIsLoading(false);
        });
    }
    , [products, setProducts, setIsLoading]);

    const searchProducts = React.useCallback((search: string) => {
        setIsLoading(true);
        fetch(`/api/v1/product/search/?name=${search}`)
            .then((response) => response.json())
            .then((json) => {
                setProducts(json.data);
                setIsLoading(false);
            })
            .catch((error) => {
                console.error('Error searching products', error);
                setIsLoading(false);
            })
            .finally(() => {
                setIsLoading(false);
            });
    }
    , [products, setProducts, setIsLoading]);

  const productContextValue: ProductContextType = {
    products,
    addProduct,
    addRangeProducts,
    saveProduct,
    updateProduct,
    removeProduct,
    fetchProducts,
    searchProducts,
    isLoading: isLoading,
  };

  return <ProductContext.Provider value={productContextValue}>{children}</ProductContext.Provider>;
};

export default ProductProvider;