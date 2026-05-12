namespace BlazorWebApp.Models
{
    public class ProgramProductAssociated
    {
        public int ProgramId { get; set; }
        public AppProgram Program { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
