import * as React from 'react';
import { IProduct, ProductContextType, productContextDefaultValue } from '../@types/product';

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
                setProducts([...products, json.data]);
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

  const updateProduct = (barcode: string) => {
    const updatedProducts = products.map((p) => (p.barcode === barcode ? { ...p, rate: p.rate + 1 } : p));
    setProducts(updatedProducts);
  };

//   const removeProduct = (barcode: string) => {
//     const updatedProducts = products.filter((p) => p.barcode !== barcode);
//     setProducts(updatedProducts);
//   }

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

  const productContextValue: ProductContextType = {
    products,
    addProduct,
    saveProduct,
    updateProduct,
    removeProduct,
    fetchProducts,
    isLoading: false,
  };

  return <ProductContext.Provider value={productContextValue}>{children}</ProductContext.Provider>;
};

export default ProductProvider;