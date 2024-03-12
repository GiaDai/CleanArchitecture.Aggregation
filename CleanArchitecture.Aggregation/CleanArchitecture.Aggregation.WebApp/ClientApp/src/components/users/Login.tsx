// Generate login form use style from bootstrap
// Form should have 2 input fields for username and password
// Form should have a submit button

import React from 'react';
import { UserContext } from '../../context/userContext';
import { useNavigate } from "react-router-dom";
import { UserContextType, ILogin } from '../../@types/user';
import * as Yup from "yup"
import { useFormik } from 'formik';
import { Button, Container, Row, FormGroup, Label, Form, Input, FormText, Col } from "reactstrap";

// generate form with 2 input fields for username and password
// use formik to handle form state and validation
// use yup for validation
// use reactstrap for styling

const Login = () => {
    const navigate = useNavigate();
    const { login } = React.useContext(UserContext) as UserContextType;

    const formik = useFormik({
        initialValues: {
            email: '',
            password: '',
        },
        validationSchema: Yup.object({
            email: Yup.string()
                .email('Invalid email address')
                .required('Required'),
            password: Yup.string()
                .required('Required'),
        }),
        onSubmit: values => {
            const loginData: ILogin = { email: values.email, password: values.password };
            login(loginData);
            navigate("/fetch-data");
        },
    });

    return (
        <Container>
            <Row>
                <Col sm={{size:4, offset:'auto'}}>
                <h1>Login</h1>
                <Form onSubmit={formik.handleSubmit}>
                    <FormGroup id='Email'>
                        <Label>Email</Label>
                        <Input
                            id="email"
                            name="email"
                            type="text"
                            onChange={formik.handleChange}
                            onBlur={formik.handleBlur}
                            value={formik.values.email}
                        />
                        <FormText className='text-danger'>
                            {formik.touched.email && formik.errors.email ? (
                                <div className='text-danger'>{formik.errors.email}</div>
                            ) : null}
                        </FormText>
                    </FormGroup>
                    <FormGroup id='Password'>
                        <Label>Password</Label>
                        <Input
                            id="password"
                            name="password"
                            type="password"
                            onChange={formik.handleChange}
                            onBlur={formik.handleBlur}
                            value={formik.values.password}
                        />
                        <FormText className='text-danger'>
                            {formik.touched.password && formik.errors.password ? (
                                <div className='text-danger'>{formik.errors.password}</div>
                            ) : null}
                        </FormText>
                    </FormGroup>
                    <Button type="submit">Submit</Button>
                </Form>
                </Col>
            </Row>
        </Container>
    );
}

export default Login;