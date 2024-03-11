export interface IProduct {
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
    removeProduct: (barcode: string) => void;
    fetchProducts: () => void;
    isLoading: boolean;
};

export const productContextDefaultValue: ProductContextType = {
    products: [],
    addProduct: (product: IProduct) => {},
    saveProduct: (product: IProduct) => {},
    updateProduct: (barcode: string) => {},
    removeProduct: (barcode: string) => {},
    fetchProducts: () => {},
    isLoading: false,
};