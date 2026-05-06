namespace BlazorWebApp.Models
{
    public enum LicenseType
    {
        TeamSize = 1,
        QtyPurchase = 2,
        Seminar = 3
    }

    public class License
    {
        public int Id { get; set; }
        public int LicenseKey { get; set; }
        public int ProgramId { get; set; }
        public int? CompanyId { get; set; }
        public int? CustomerGroupId { get; set; }
        public int? OwnerId { get; set; }
        public LicenseType SubLicenseType { get; set; } = LicenseType.TeamSize;
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int MaxMembers { get; set; } = 5;
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public AppProgram? Program { get; set; }
        public Company? Company { get; set; }
        public CustomerGroup? CustomerGroup { get; set; }
        public Customer? Owner { get; set; }
        public ICollection<LicenseMember> Members { get; set; } = new List<LicenseMember>();
    }
}
