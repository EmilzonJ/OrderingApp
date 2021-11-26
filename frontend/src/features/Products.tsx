import { useAuth0 } from "@auth0/auth0-react"
import { HttpTransportType, HubConnectionBuilder } from "@microsoft/signalr"
import { useEffect, useState } from "react"
import { CreateProduct } from "../components/products/createProduct/CreateProduct"
import ProductsList from "../components/products/Products"
import { useWS } from "../hooks/useWS"
import { Product } from "../schemas/Product"

export const Products = () => {
  const [products, setProducts] = useState<Product[]>([])

  const { conn } = useWS({ connString: 'http://localhost:50001/productHub' })

  conn?.send('sss', { ssa: 1 })

  conn?.off("sss")
  
  return (
    <div>
      hola hola
      <ProductsList products={products} />
      <CreateProduct />
    </div>
  )
}
