namespace BlazorWebApp.Models
{
    public class License
    {
        public int Id { get; set; }
        public string LicenseKey { get; set; } = string.Empty;
        public int ProgramId { get; set; }
        public int? CompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int MaxMembers { get; set; } = 5;
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public AppProgram? Program { get; set; }
        public Company? Company { get; set; }
        public ICollection<LicenseMember> Members { get; set; } = new List<LicenseMember>();
    }
}
