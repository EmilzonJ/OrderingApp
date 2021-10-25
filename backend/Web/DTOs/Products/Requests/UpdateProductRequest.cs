namespace Web.DTOs.Products.Requests
{
    public class UpdateProductRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
    }
}