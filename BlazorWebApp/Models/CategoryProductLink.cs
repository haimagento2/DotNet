namespace BlazorWebApp.Models
{
    public class CategoryProductLink
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
