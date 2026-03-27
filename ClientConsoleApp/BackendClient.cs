using System.Threading.Tasks;
using AwesomeFilesCore.Services;

namespace ClientConsoleApp
{
    public class BackendClient
    {
        private FileService _fileService = new("..\\..\\..\\..\\AwesomeStorage\\");
        public ArchiveService _archiveService = new("..\\..\\..\\..\\AwesomeStorage\\");
        public void GetFileListCommand()
        {
            List<string> fileNamesList = _fileService.GetFileNames();
            Console.WriteLine(string.Join(" ", fileNamesList));
        }

        public void CreateArchiveCommand(List<string> files)
        {
            Console.Write("Task initialized. Your task id: ");
            Guid id = _archiveService.InitializeTask(files);
            Console.WriteLine(id.ToString());
            Task.Run(() => _archiveService.Archive(_archiveService.GetById(id)));
        }

        public void GetArchiveTaskStatusCommand(string id)
        {
            Guid guidId = Guid.Parse(id);
            Console.WriteLine(_archiveService.GetArchiveTaskStatus(guidId));
        }

        public void DownloadArchiveCommand(string id, string path)
        {
            byte[] bytes = _archiveService.GetArchiveStream(Guid.Parse(id));
            File.WriteAllBytes(path + $"/{id.Substring(0,7)}.zip", bytes);
            Console.WriteLine($"Archive was downloaded to {path}");
        }
    }
}
