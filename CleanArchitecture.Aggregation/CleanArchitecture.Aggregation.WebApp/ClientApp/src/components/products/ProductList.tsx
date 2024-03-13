import React, { memo } from 'react'
import { Link } from 'react-router-dom';
import { NavLink, Row, Col, Button } from 'reactstrap';

const ProductList = memo(() => {
  return (
    <Row>
        <Col>
        <div>ProductList</div>
        <NavLink tag={Link} className="text-dark" to="/products/1">Product detail</NavLink>
        </Col>
        <Col>
          <Button tag={Link} to="/products/add" color="primary">Add Product</Button>
        </Col>
    </Row>
  )
})
// https://codesandbox.io/p/sandbox/react-data-table-bootstrap5-z6gtg?file=%2Fsrc%2Findex.js%3A9%2C1-172%2C4
export default ProductList