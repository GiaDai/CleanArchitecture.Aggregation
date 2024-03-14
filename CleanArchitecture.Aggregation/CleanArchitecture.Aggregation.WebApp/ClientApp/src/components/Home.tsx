import React, { useEffect,useState } from 'react';
import * as signalR from "@microsoft/signalr";
import { useNavigate } from "react-router-dom";
import SprintDetail from './board/SprintDetail';

const Home = () => {
    const navigate = useNavigate();
    const[connection, setConnection] = useState<any>(null);
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
        <SprintDetail/>
      </div>
    );
}

export { Home };
