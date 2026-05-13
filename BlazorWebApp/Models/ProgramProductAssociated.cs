namespace BlazorWebApp.Models
{
    public class ProgramProductAssociated
    {
        public int ProgramId { get; set; }
        public AppProgram Program { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string? Type { get; set; }
        public string? Period { get; set; }
        public int? CustomPeriodDays { get; set; }
        public bool? LinkingLicense { get; set; }
        public bool? CommunityOnly { get; set; }
    }
}
