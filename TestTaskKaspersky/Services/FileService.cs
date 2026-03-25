using System.Net;

namespace TestTaskKaspersky.Services
{
    // Сервис для получения файлов из хранилища
    public class FileService
    {
        private readonly DirectoryInfo storagePath = new DirectoryInfo(".\\AwesomeStorage");

        public List<string> GetFileNames()
        {               
            return storagePath.GetFiles().Select(f => f.Name).ToList();
        }

    }
}
