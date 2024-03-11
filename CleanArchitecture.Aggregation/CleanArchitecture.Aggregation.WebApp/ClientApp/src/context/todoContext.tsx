import * as React from 'react';
import { TodoContextType, ITodo, TodoAction } from '../@types/todo';
import { todoReducer } from '../reducers/todoReducer';


export const TodoContext = React.createContext<{
    todos: ITodo[];
    dispatch: React.Dispatch<TodoAction>;

} | null>(null);

const TodoProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [todos, dispatch] = React.useReducer(todoReducer, [
        {
            id: 1,
            title: 'First todo',
            description: 'This is the first todo',
            status: false
        },
        {
            id: 2,
            title: 'Second todo',
            description: 'This is the second todo',
            status: false   
        }
    ]);

    return (
        <TodoContext.Provider value={{ todos, dispatch }}>
            {children}
        </TodoContext.Provider>
    );
};

export default TodoProvider;