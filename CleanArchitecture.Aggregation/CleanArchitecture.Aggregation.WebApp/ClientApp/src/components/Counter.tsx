import React, { useState } from 'react';
import { ProductContextType, IProduct } from '../@types/product';
import { ProductContext } from '../context/productContext';
import { faker} from '@faker-js/faker';
const Counter = () => {
    const { products, addProduct, removeProduct, fetchProducts, isLoading } = React.useContext(ProductContext) as ProductContextType;
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

            <DisplayProducts products={products} removeProduct={removeProduct} />
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
    const { products, removeProduct } = props;
    console.log('DisplayProducts rendered');
    return (
        <div>
            {products.map((product: IProduct) => {
                return (
                    <div>
                        <h3>{product.name}</h3>
                        <p>{product.description}</p>
                        <button className='btn btn-warning' onClick={() => removeProduct(product.id)}>Remove</button>
                    </div>
                );
            })}
        </div>
    );
});

export { Counter };