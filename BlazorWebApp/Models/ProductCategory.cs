namespace BlazorWebApp.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public ProductCategory? ParentCategory { get; set; }
        public ICollection<ProductCategory> ChildCategories { get; set; } = new List<ProductCategory>();
        public string Image { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<CategoryProductLink> CategoryProductLinks { get; set; } = new List<CategoryProductLink>();
    }
}
