import React, { memo } from 'react'
import { King } from '../../common/King';
import * as Yup from "yup";
import { useMutation } from '@tanstack/react-query';
import { createBoard } from '../../apis/scrum-poker-api';
import { useFormik } from 'formik';
import { Form, Label, FormGroup, Input,FormText, Button, Spinner, Alert } from 'reactstrap';
import { Board } from '../../@types/board';

const SprintDetail = memo(() => {

    const { mutate, reset, isPending, isError } = useMutation({
        mutationFn: (board: Board) => createBoard(board),
    });

    const [isSuccess, setIsSuccess] = React.useState<boolean>(false);
    // use formik to handle form state and validation
    // use yup for validation
    // use reactstrap for styling
    const formik = useFormik({
        initialValues: {
            boardName: '',
            description: '',
        },
        validationSchema: Yup.object({
            boardName: Yup.string()
                .required('Required'),
            description: Yup.string()
                .required('Required'),
        }),
        onSubmit: values => {
            console.log(values);
            const board: Board = { name: values.boardName, description: values.description };

            mutate(board,{
                onSuccess: (data) => {
                    reset();
                    setIsSuccess(true);
                    console.log(data);
                },
                onError: (error) => {
                    setIsSuccess(false);
                    console.log(error);
                }
            });
        },
    });



    return (
        <div className='container container-card'>
            <div className='card mb-3'>
                <div className="row no-gutters p-3">
                    <King></King>
                    <div className="col-md-8">
                        <div className="card-body">
                            <h5 className="card-title">Create your poker board</h5>
                            <Form onSubmit={formik.handleSubmit}>
                            <FormGroup>
                                {isError && ( <Alert color="danger">Error</Alert>)}
                                { isSuccess && ( <Alert color="success">Board created successfully</Alert>)}
                                <Label>Board Name</Label>
                                <Input
                                        id="boardName"
                                        name="boardName"
                                        type="text"
                                        onChange={formik.handleChange}
                                        onBlur={formik.handleBlur}
                                        value={formik.values.boardName}
                                    />
                                <FormText className='text-danger'>
                                        {formik.touched.boardName && formik.errors.boardName ? (
                                            <div className='text-danger'>{formik.errors.boardName}</div>
                                        ) : null}
                                    </FormText>
                            </FormGroup>
                            <FormGroup>
                                <Label>Description</Label>
                                <Input
                                        id="description"
                                        name="description"
                                        type="textarea"
                                        onChange={formik.handleChange}
                                        onBlur={formik.handleBlur}
                                        value={formik.values.description}
                                    />
                                <FormText className='text-danger'>
                                    {formik.touched.description && formik.errors.description ? (
                                        <div className='text-danger'>{formik.errors.description}</div>
                                    ) : null}
                                </FormText>
                            </FormGroup>
                            <Button color='primary' type="submit">
                                Create
                                <Spinner size="sm" color="light" style={{marginLeft: 10}} hidden={!isPending} />
                            </Button>
                            </Form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
})

export default SprintDetail