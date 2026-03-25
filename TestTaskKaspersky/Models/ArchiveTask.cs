namespace TestTaskKaspersky.Models
{
    public class ArchiveTask
    {
        public required Guid Id { get; set; } 
        public required List<string> Files { get; set; }
        public required string Status { get; set; }
        public string? ArchivePath { get; set; } = null;
        public string? ErrorMessage { get; set; } = null;
    }
}
