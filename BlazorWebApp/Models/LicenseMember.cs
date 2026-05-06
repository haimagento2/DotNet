namespace BlazorWebApp.Models
{
    public class LicenseMember
    {
        public int Id { get; set; }
        public int LicenseId { get; set; }
        public int CustomerId { get; set; }
        public int? OwnerId { get; set; }
        public int Permission { get; set; } = 3; // 1=Owner, 2=Admin, 3=Member
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public License? License { get; set; }
        public Customer? Customer { get; set; }
        public Customer? Owner { get; set; }
    }
}
