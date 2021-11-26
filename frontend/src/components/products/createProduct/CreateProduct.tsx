export const CreateProduct = () => {

  const onSubmit = (values: any) => {
    console.log(values);
  }

  return (
    <form onSubmit={onSubmit} >
      <input type="text" />
      <input type="number" />
      <input type="text" />
    </form>
  )
}

