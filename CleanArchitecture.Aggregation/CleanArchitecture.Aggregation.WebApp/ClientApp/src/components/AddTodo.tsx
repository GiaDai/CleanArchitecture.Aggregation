import * as React from 'react';
import { TodoContextType, ITodo } from '../@types/todo';
import { TodoContext } from '../context/todoContext';

const AddTodo: React.FC = () => {
    const { dispatch } = React.useContext(TodoContext)!;
    const [formData, setFormData] = React.useState<ITodo | {}>();

    const handleForm = (e:React.FormEvent<HTMLInputElement>): void => {
        setFormData({
            ...formData,
            [e.currentTarget.id]: e.currentTarget.value
        });
    }

    const handleSaveTodo = (e: React.FormEvent, formData: ITodo | any) => {
        e.preventDefault();
        dispatch({ type: 'ADD_TODO', payload: formData });
    }
    return (
        <form className='todo-form' onSubmit={(e) => handleSaveTodo(e, formData)}>

            <div className='form-control'>
                <label htmlFor='title'>Title</label>
                <input type='text' id='title' name='title' onChange={e => setFormData({ ...formData, title: e.target.value })} />
            </div>

            <div className='form-control'>
                <label htmlFor='description'>Description</label>
                <input type='text' id='description' name='description' onChange={e => setFormData({ ...formData, description: e.target.value })} />
            </div>

            <button disabled={formData === undefined ? true : false} type='submit'>Save</button>
        </form>
    );
}

export default AddTodo;