import React, { useState } from 'react';
import { ProductContextType, IProduct } from '../@types/product';
import { ProductContext } from '../context/productContext';
import { faker} from '@faker-js/faker';
import { useQuery } from '@tanstack/react-query';
import { getProducts } from '../apis/product.api';
import { putUpdateProduct } from '../apis/product.api';
import { useMutation } from '@tanstack/react-query';
const Counter = () => {
    const { data, isError } = useQuery({
        queryKey: ['products', 1, 20],
        queryFn: () => getProducts(1, 20),
    });
    const { products, addProduct, removeProduct, fetchProducts, updateProduct, addRangeProducts, isLoading } = React.useContext(ProductContext) as ProductContextType;
    const [currentCount, setCurrentCount] = useState<number>(0);
    const incrementCounter = () => {
        var product = gerneateProduct();
        addProduct(product);
        setCurrentCount(currentCount + 1);
    } 

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

    // generate 100 products
    const generateProducts = () => {
        var products: IProduct[] = [];
        for (let i = 0; i < 999; i++) {
            products.push(gerneateProduct());
        }
        return products;
    }

    React.useEffect(() => {
        fetchProducts();
    }, [fetchProducts]);

    return (
        <div>
            <h1>Counter</h1>

            <p>This is a simple example of a React component.</p>

            <p aria-live="polite">Current count: <strong>{currentCount}</strong></p>
            <p>Number of products is {products.length}</p>
            <button className="btn btn-primary" disabled={isLoading} onClick={incrementCounter}>{isLoading ? 'Processing' : 'Increment'}</button>
            <button className="btn btn-success" disabled={isLoading} onClick={() => addRangeProducts(generateProducts())}>{isLoading ? 'Processing' : 'Bulk insert'}</button>
            <DisplayProducts products={products} removeProduct={removeProduct} updateProduct={updateProduct}/>
        </div>
    );
}

const CounterOne = React.memo((props: any) => {
    const { count } = props;
    console.log('CounterOne rendered');
    return (
        <div>Counter One Counter:{count}</div>
    );
});

const CounterTwo = React.memo(() => {
    console.log('CounterTwo rendered');
    return (
        <div>Counter Two</div>
    );
});  

const DisplayProducts = React.memo((props: any) => {
    const { products, removeProduct, updateProduct } = props;
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

    const mutationPut = useMutation({
        mutationFn: (product: IProduct) => putUpdateProduct(product),
    });

    const updateProductMutation = (id: number) => {
        const product = gerneateProduct();
        product.id = id;
        mutationPut.mutate(product,{
            onSuccess: () => {
                console.log('Product updated successfully');
            },
            onError: (error) => {
                console.log('Product updated failed', error);
            }
        });
    }
    
    return (
        <div>
            {products.map((product: IProduct) => {
                return (
                    <div key={product.barcode}>
                        <h3>{product.name}</h3>
                        <p>{product.description}</p>
                        <button className='btn btn-danger' onClick={() => removeProduct(product.id)}>Remove</button>
                        <button className='btn btn-primary' onClick={() => updateProductMutation(product.id)}>Update</button>
                    </div>
                );
            })}
        </div>
    );
});

export { Counter };