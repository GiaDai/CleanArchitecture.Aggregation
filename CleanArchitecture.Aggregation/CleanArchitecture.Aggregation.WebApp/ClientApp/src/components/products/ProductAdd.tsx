import React, { memo, useMemo } from 'react'
import { useMatch } from 'react-router-dom';
import { useMutation } from '@tanstack/react-query';
import { postAddProduct } from '../../apis/product.api';
import { IProduct } from '../../@types/product';
import { da, faker } from '@faker-js/faker';
import { Row, Col, Button } from 'reactstrap';
import { isAxiosError } from '../../utils/utils';

type ProductAddProps = Omit<IProduct, 'id'>;
type FormError = {
  [key in keyof ProductAddProps]: string | null;
}
const ProductAdd = memo(() => {
  const { mutate, isSuccess, isError, data, error, reset } = useMutation({
    mutationFn: (product: Omit<IProduct, 'id'>) => postAddProduct(product),
  });

  if (isSuccess) {
    if (data.status === 200) {
      console.log('Product added successfully', data.data);
    }
  }

  const errorForm: FormError = useMemo(() => {
    if (isAxiosError<{ error: FormError }>(error) && error.response?.status === 400) {
      return error.response.data.error;
    }
    return null;
  }, [error]);

  // const addMatch = useMatch('/products/add');
  // if (!addMatch) return null;

  const gerneateProduct = () => {
    const product: ProductAddProps = {
      rate: faker.number.int({ min: 99, max: 999 }),
      name: faker.person.firstName() + ' ' + faker.person.lastName(),
      barcode: faker.internet.ipv4(),
      description: faker.lorem.paragraph()
    };
    console.log(product);
    return product;
  }

  // handle change input value from event and set to state
  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    console.log(event.target.value);
  }

  const handleAddProduct = () => {
    if (error || data) {
      reset();
    }
    mutate(gerneateProduct(),{
      onSuccess: () => {
        console.log('Product added successfully');
      },
      onError: (error) => {
        console.log('Product added failed', error);
      }
    
    });
  }

  return (
    <Row>
      <Col>
        <h1>Add Product</h1>
        <Button color="primary" onClick={handleAddProduct}>Add Product</Button>
        {isSuccess && <div className='text-success'>Product added successfully</div>}
        {isError && <div className='text-danger'>Product added failed</div>}
      </Col>
    </Row>
  )
})

export default ProductAdd