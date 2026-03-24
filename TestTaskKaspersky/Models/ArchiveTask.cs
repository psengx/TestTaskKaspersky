namespace TestTaskKaspersky.Models
{
    public class ArchiveTask
    {
        public Guid Id { get; set; }
        public List<string> Files { get; set; }
        public string Status { get; set; }
        public string ArchivePath { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
