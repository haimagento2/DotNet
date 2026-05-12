namespace BlazorWebApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public bool EnableSubscriptionLinking { get; set; }
        public bool LicenseOnly { get; set; }
        public bool CommunityOnly { get; set; }
        public bool SendEmailInstruction { get; set; }
        public int? ProductCategoryId { get; set; }
        public ProductCategory? ProductCategory { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<ProgramProductAssociated> ProgramProductAssociations { get; set; } = new List<ProgramProductAssociated>();
        public ICollection<CategoryProductLink> CategoryProductLinks { get; set; } = new List<CategoryProductLink>();
    }
}
