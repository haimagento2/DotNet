namespace BlazorWebApp.Models
{
    public class LicenseMember
    {
        public int Id { get; set; }
        public int LicenseId { get; set; }
        public int CustomerId { get; set; }
        public int? OwnerId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public License? License { get; set; }
        public Customer? Customer { get; set; }
        public Customer? Owner { get; set; }
    }
}
