// Generate login form use style from bootstrap
// Form should have 2 input fields for username and password
// Form should have a submit button

import React, {useContext} from 'react';
import { useNavigate } from "react-router-dom";
import { ILogin, UserContextType } from '../../@types/user';
import { postLogin } from '../../apis/user.api';    
import { useMutation } from '@tanstack/react-query';
import { UserContext } from '../../context/userContext';
import * as Yup from "yup"
import { useFormik } from 'formik';
import { Button, Container, Row, FormGroup, Label, Form, Input, FormText, Col, Card, CardBody, Spinner, Alert } from "reactstrap";
import { isAxiosError } from '../../utils/utils';

// generate form with 2 input fields for username and password
// use formik to handle form state and validation
// use yup for validation
// use reactstrap for styling

const Login = () => {
    const navigate = useNavigate();
    const { loginContext } = useContext(UserContext) as UserContextType;
    const [errorMessage, setErrorMessage] = React.useState<string | null>(null);
    const [successMessage, setSuccessMessage] = React.useState<string | null>(null);
    const { mutate, reset, isPending, isError } = useMutation({
        mutationFn: (login: ILogin) => postLogin(login),
    });

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
            mutate(loginData,{
                onSuccess: (data) => {
                    reset();
                    if(data.status === 200 && data.data){
                        setSuccessMessage(data.data.message || "Login success");
                        loginContext(data.data.data);
                    }
                    // set timeout to navigate to fetch-data page
                    // setTimeout(() => {
                    //     navigate("/fetch-data");
                    // }, 2000);
                },
                onError: (error) => {
                    if (isAxiosError(error) && error.response?.status === 400) {
                        setErrorMessage((error.response.data as any).Message);
                    }
                }
            });
        },
    });

    return (
        <>
        <Container>
            <Row>
                <Col md={{
                    offset: 3,
                    size: 6
                }}
                    sm="12">
                    <Card>
                        <CardBody>
                            <h1>Login</h1>
                            <Form onSubmit={formik.handleSubmit}>
                                {isError && errorMessage ? (
                                    <Alert color="danger">
                                        {errorMessage}
                                    </Alert>
                                ) : null}
                                {successMessage ? (
                                    <Alert color="success">
                                        {successMessage}
                                    </Alert>
                                ) : null}
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
                                <Button color='primary' type="submit">Login 
                                <Spinner size="sm" color="light" style={{marginLeft: 10}} hidden={!isPending} />
                                </Button>
                            </Form>
                        </CardBody>
                    </Card>
                </Col>
            </Row>
        </Container>
        </>
    );
}

export default Login;