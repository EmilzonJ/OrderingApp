import { FC } from "react"
import { Product } from "../../schemas/Product"

interface IPorductList {
  products: Product[]
}

const ProductsList: FC<IPorductList> = ({ products }) => {
  return (
    <div>
      {
        products.map((product) => (
          <ul>
            <li>{product.name}</li>
            <li>{product.size}</li>
            <li>{product.price}</li>
          </ul>
        ))}
    </div>

  )
}

export default ProductsList
