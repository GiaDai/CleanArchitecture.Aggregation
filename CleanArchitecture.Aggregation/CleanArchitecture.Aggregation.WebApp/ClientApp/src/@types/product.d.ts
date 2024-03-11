export interface IProduct {
    id: number;
    rate: number;
    name: string;
    description: string;
    barcode: string;
}

export type ProductContextType = {
    products: IProduct[];
    addProduct: (product: IProduct) => void;
    saveProduct: (product: IProduct) => void;
    updateProduct: (id: number) => void;
    removeProduct: (id: number) => void;
    fetchProducts: () => void;
    isLoading: boolean;
};

export const productContextDefaultValue: ProductContextType = {
    products: [],
    addProduct: (product: IProduct) => {},
    saveProduct: (product: IProduct) => {},
    updateProduct: (id: number) => {},
    removeProduct: (id: number) => {},
    fetchProducts: () => {},
    isLoading: false,
};