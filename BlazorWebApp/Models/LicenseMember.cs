namespace BlazorWebApp.Models
{
    public class LicenseMember
    {
        public int Id { get; set; }
        public int LicenseId { get; set; }
        public int CustomerId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.Now;

        public License? License { get; set; }
        public Customer? Customer { get; set; }
    }
}
