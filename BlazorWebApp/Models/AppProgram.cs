namespace BlazorWebApp.Models
{
    public class AppProgram
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // New fields for Program Image
        public string ProgramImage { get; set; } = string.Empty;

        // New fields for Lock Services
        public string PadlockDisplay { get; set; } = "None"; // None, DisplayAsPadlock, Hidden
        public bool RedirectToLockedUrlKey { get; set; } = false;
        public string LockedUrlKey { get; set; } = string.Empty;

        public ICollection<License> Licenses { get; set; } = new List<License>();
        public ICollection<ProgramProductAssociated> ProgramProductAssociations { get; set; } = new List<ProgramProductAssociated>();
    }
}
