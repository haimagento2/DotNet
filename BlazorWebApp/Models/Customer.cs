namespace BlazorWebApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int? CompanyId { get; set; }
        public int? CustomerGroupId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Company? Company { get; set; }
        public CustomerGroup? CustomerGroup { get; set; }
    }
}
