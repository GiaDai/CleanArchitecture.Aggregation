import React, { useState, useContext } from 'react';
import { UserContextType } from '../@types/user';
import { UserContext } from '../context/userContext';
import { Collapse, Button, Navbar, NavbarToggler, NavItem, NavLink, NavbarBrand, NavbarText, Nav } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

const NavMenu = () => {
    const [collapsed, setCollapsed] = useState<boolean>(true);
    const toggleNavbar = () => setCollapsed(!collapsed);
    const { userContext, logoutContext } = useContext(UserContext) as UserContextType;
    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                <NavbarBrand href="/">reactstrap</NavbarBrand>
                <NavbarToggler onClick={toggleNavbar} className="mr-2" />

                <Collapse className="d-sm-inline-flex" isOpen={!collapsed} navbar>
                    {userContext && userContext.email &&
                        <Nav className="me-auto" navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/products">Products</NavLink>
                                </NavItem>
                            </ul>
                        </Nav>
                    }
                    {userContext && userContext.email &&
                        <span>
                            <NavbarText>{userContext.email} | </NavbarText>
                            <Button outline color='info' onClick={logoutContext}>Logout</Button>
                        </span>}
                </Collapse>
            </Navbar>
        </header>
    );
}

export { NavMenu };