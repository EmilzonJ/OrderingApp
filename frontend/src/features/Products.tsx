import { useAuth0 } from "@auth0/auth0-react"
import { HubConnectionBuilder } from "@microsoft/signalr"
import { useEffect, useState, } from "react"
import Productss from "../components/products/Products"

export const Products = () => {
  const { getAccessTokenSilently } = useAuth0();

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:5001/productHub', {
        accessTokenFactory: () => getAccessTokenSilently()
      })
      .withAutomaticReconnect()
      .build();

    newConnection.start()
      .then(() => {

      })
      .catch(err => console.error(err));

    newConnection.on('ReceiveProduct', async (products) => {
      console.log(products);
    });

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

    const addProduct = async () => {
      const pp = {
        Name: "string",
        Price: 0,
        Size: "string"
      }

      try {
        await fetch('https://localhost:5001/api/Product/AddProduct', {
          method: 'POST',
          body: JSON.stringify(pp),
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${await getAccessTokenSilently()}`,
            mode: 'no-cors'
          }
        });
      }
      catch (e) {
        console.log('Sending message failed.', e);
      }
    }
    // addProduct().then(e => console.log(e))
  }, [getAccessTokenSilently])

  return (
    <div>
      <Productss />
    </div>
  )
}
