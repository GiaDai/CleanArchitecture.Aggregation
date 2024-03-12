import React, { memo } from 'react'
import { Link } from 'react-router-dom';
import { NavLink } from 'reactstrap';
import "bootstrap/dist/js/bootstrap.bundle.js";
import "bootstrap/dist/css/bootstrap.css";
import * as movies from "./movies";
const movieData = movies.default;

function getNumberOfPages(rowCount: number, rowsPerPage: number) {
  return Math.ceil(rowCount / rowsPerPage);
}

function toPages(pages: number) {
  const results = [];

  for (let i = 1; i < pages; i++) {
    results.push(i);
  }

  return results;
}

const columns = [
  {
    name: "Title",
    selector: (row: any) => row.title,
    sortable: true
  },
  {
    name: "Directior",
    selector: (row: any) => row.director,
    sortable: true
  },
  {
    name: "Runtime (m)",
    selector: (row: any) => row.runtime,
    sortable: true,
    right: true
  },
  {
    button: true,
    cell: () => (
      <div className="App">
        <div className="openbtn text-center">
          <button
            type="button"
            className="btn btn-primary"
            data-bs-toggle="modal"
            data-bs-target="#myModal"
          >
            Open modal
          </button>
          <div className="modal" tabIndex="-1" id="myModal">
            <div className="modal-dialog">
              <div className="modal-content">
                <div className="modal-header">
                  <h5 className="modal-title">Modal title</h5>
                  <button
                    type="button"
                    className="btn-close"
                    data-bs-dismiss="modal"
                    aria-label="Close"
                  ></button>
                </div>
                <div className="modal-body">
                  <p>Modal body text goes here.</p>
                </div>
                <div className="modal-footer">
                  <button
                    type="button"
                    className="btn btn-secondary"
                    data-bs-dismiss="modal"
                  >
                    Close
                  </button>
                  <button type="button" className="btn btn-primary">
                    Save changes
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  }
];

// RDT exposes the following internal pagination properties
const BootyPagination = ({
  rowsPerPage,
  rowCount,
  onChangePage,
  onChangeRowsPerPage, // available but not used here
  currentPage
}) => {
  const handleBackButtonClick = () => {
    onChangePage(currentPage - 1);
  };

  const handleNextButtonClick = () => {
    onChangePage(currentPage + 1);
  };

  const handlePageNumber = (e) => {
    onChangePage(Number(e.target.value));
  };

  const pages = getNumberOfPages(rowCount, rowsPerPage);
  const pageItems = toPages(pages);
  const nextDisabled = currentPage === pageItems.length;
  const previosDisabled = currentPage === 1;

  return (
    <nav>
      <ul className="pagination">
        <li className="page-item">
          <button
            className="page-link"
            onClick={handleBackButtonClick}
            disabled={previosDisabled}
            aria-disabled={previosDisabled}
            aria-label="previous page"
          >
            Previous
          </button>
        </li>
        {pageItems.map((page) => {
          const className =
            page === currentPage ? "page-item active" : "page-item";

          return (
            <li key={page} className={className}>
              <button
                className="page-link"
                onClick={handlePageNumber}
                value={page}
              >
                {page}
              </button>
            </li>
          );
        })}
        <li className="page-item">
          <button
            className="page-link"
            onClick={handleNextButtonClick}
            disabled={nextDisabled}
            aria-disabled={nextDisabled}
            aria-label="next page"
          >
            Next
          </button>
        </li>
      </ul>
    </nav>
  );
};

const BootyCheckbox = React.forwardRef(({ onClick, ...rest }, ref) => (
  <div className="form-check">
    <input
      htmlFor="booty-check"
      type="checkbox"
      className="form-check-input"
      ref={ref}
      onClick={onClick}
      {...rest}
    />
    <label className="form-check-label" id="booty-check" />
  </div>
));

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