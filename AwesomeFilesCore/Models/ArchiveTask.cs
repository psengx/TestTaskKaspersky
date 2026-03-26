namespace AwesomeFilesCore.Models
{
    public class ArchiveTask
    {
        public required Guid Id { get; set; }
        public required List<string> Files { get; set; }
        public required string Status { get; set; }
        public byte[] ArchiveStream { get; set; } = null;
        public string? ErrorMessage { get; set; } = null;
    }
}
