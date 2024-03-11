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
    updateProduct: (barcode: string) => void;
    removeProduct: (id: number) => void;
    fetchProducts: () => void;
    isLoading: boolean;
};

export const productContextDefaultValue: ProductContextType = {
    products: [],
    addProduct: (product: IProduct) => {},
    saveProduct: (product: IProduct) => {},
    updateProduct: (barcode: string) => {},
    removeProduct: (id: number) => {},
    fetchProducts: () => {},
    isLoading: false,
};