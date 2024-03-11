import React, { useEffect,useState } from 'react';
import * as signalR from "@microsoft/signalr";
import Todos from '../containers/Todos';
import AddTodo from './AddTodo';

const Home = () => {
    const[connection, setConnection] = useState<any>(null)
    useEffect(() => {
        document.title = "Home - Clean Architecture";
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/hub",{
              skipNegotiation: true,
              transport: signalR.HttpTransportType.WebSockets,
            })
            .configureLogging(signalR.LogLevel.Information)
            .withAutomaticReconnect()
            .build();
        setConnection(connection);
    }, []);

    useEffect(() => {

      if(connection){
        console.log('Connection started');
        connection.start()
        .then((result: void) => { // Explicitly specify the type of 'result' as 'void'
          console.log('Connected!');
          connection.on('messageReceived', (message: any) => { // Explicitly specify the type of 'message' as 'any'
            console.log(message);
          });
        })
        .catch((e: any) => console.log('Connection failed: ', e)); // Explicitly specify the type of 'e' as 'any'
      }
    }
    , [connection]);
    return (
      <div>
        <h1>Hello, world!</h1>
        <p>Welcome to your new single-page application, built with:</p>
        <ul>
          <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
          <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
          <li><a href='http://getbootstrap.com/'>Bootstrap</a> for layout and styling</li>
        </ul>
        <p>To help you get started, we have also set up:</p>
        <ul>
          <li><strong>Client-side navigation</strong>. For example, click <em>Counter</em> then <em>Back</em> to return here.</li>
          <li><strong>Development server integration</strong>. In development mode, the development server from <code>create-react-app</code> runs in the background automatically, so your client-side resources are dynamically built on demand and the page refreshes when you modify any file.</li>
          <li><strong>Efficient production builds</strong>. In production mode, development-time features are disabled, and your <code>dotnet publish</code> configuration produces minified, efficiently bundled JavaScript files.</li>
        </ul>
        <p>The <code>ClientApp</code> subdirectory is a standard React application based on the <code>create-react-app</code> template. If you open a command prompt in that directory, you can run <code>npm</code> commands such as <code>npm test</code> or <code>npm install</code>.</p>
      
        <AddTodo />
        <Todos />
      </div>
    );
}

export { Home };
