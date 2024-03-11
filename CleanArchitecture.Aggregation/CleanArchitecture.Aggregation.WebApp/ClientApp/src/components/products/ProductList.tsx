import React, { memo } from 'react'
import { Link } from 'react-router-dom';
import { NavLink } from 'reactstrap';
const ProductList = memo(() => {
  return (
    <>
        <div>ProductList</div>
        <NavLink tag={Link} className="text-dark" to="/products/1">Product detail</NavLink>
    </>
  )
})

export default ProductList