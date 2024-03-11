import * as React from 'react';
import { TodoContextType, ITodo } from '../@types/todo';
import { TodoContext } from '../context/todoContext';
import Todo from '../components/Todo';

const Todos: React.FC = () => {
    const { todos, dispatch } = React.useContext(TodoContext)!;

    return (
        <div>
            {todos.map((todo: ITodo) => (
                <Todo key={todo.id} todo={todo} updateTodo={() => dispatch({type:'UPDATE_TODO', payload: todo.id }) } />
            ))}
        </div>
    );
}

export default Todos;