import { useAuth0 } from "@auth0/auth0-react"
import { HttpTransportType, HubConnectionBuilder } from "@microsoft/signalr"
import { useEffect, useState } from "react"
import Productss from "../components/products/Products"

export const Products = () => {
  const { getAccessTokenSilently } = useAuth0();
  const [conn, setConn] = useState<any>()
  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:5001/productHub',
        {
          accessTokenFactory: () => getAccessTokenSilently(),
          skipNegotiation: true,
          transport: HttpTransportType.WebSockets
        }
      )
      .withAutomaticReconnect()
      .build();
    setConn(newConnection)



    // newConnection.on('UpdateProduct', async (product: {}) => {
    //   const  index=state.find(products=>products.id===product.id)

    //   state.splice(index, 1, products)
    // });

    // newConnection.on('deleteProduct', async (product: {}) => {
    //   const  index=state.find(products=>products.id===product.id)

    //   state.splice(index, 1)
    // });

  }, [getAccessTokenSilently])

  useEffect(() => {
    if (conn) {
      conn.start().then(() => {
        console.log('conexion iniciada');
      }).catch((err: any) => console.error(err));

      conn.on('ReceiveProduct', (products: any) => {
        console.log(products);
      });
    }

  }, [conn])


  return (
    <div>
      <Productss />
    </div>
  )
}
