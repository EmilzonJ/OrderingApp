import { useAuth0 } from "@auth0/auth0-react";
import { HttpTransportType, HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { useEffect, useState } from "react";



//write a custom hook that receives url as parameter and a callback function  that receives a connection object, 
// after return connection object and on and send function
export const useWS = (url: string, callback: (connection: HubConnection) => void) => {

  // initialize the connection object in a useEffect hook
  const { getAccessTokenSilently } = useAuth0();
  const [connection, setConnection] = useState<HubConnection>();

  useEffect(() => {
    const buildConnection = async () => {
      const token = await getAccessTokenSilently();
      const connection = new HubConnectionBuilder()
        .withUrl(url, {
          accessTokenFactory: () => token,
          skipNegotiation: true,
          transport: HttpTransportType.WebSockets
        })
        .withAutomaticReconnect()
        .build();

      await connection.start();
      callback(connection);
      setConnection(connection);
    };
    buildConnection();
    return () => { connection?.stop() };
  }, [url, getAccessTokenSilently, callback, connection]);

  // return the connection object and the send and on function
  return { connection, send: connection?.send, on: connection?.on };

}