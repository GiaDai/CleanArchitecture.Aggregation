import React, { memo } from 'react'
import { Link } from 'react-router-dom';
import { NavLink } from 'reactstrap';
import "bootstrap/dist/js/bootstrap.bundle.js";
import "bootstrap/dist/css/bootstrap.css";

const ProductList = memo(() => {
  return (
    <>
        <div>ProductList</div>
        <NavLink tag={Link} className="text-dark" to="/products/1">Product detail</NavLink>
    </>
  )
})
// https://codesandbox.io/p/sandbox/react-data-table-bootstrap5-z6gtg?file=%2Fsrc%2Findex.js%3A9%2C1-172%2C4
export default ProductList