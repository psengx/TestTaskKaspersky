namespace AwesomeFilesCore.Services
{
    // Сервис для получения файлов из хранилища
    public class FileService
    {
        private readonly string _storagePath;
        public FileService(string storagePath)
        {
            _storagePath = storagePath;
        }
        public List<string> GetFileNames()
        {
            List<string> result = new List<string>();
            string[] filePaths = Directory.GetFiles(_storagePath);
            foreach (string filePath in filePaths)
            {
                result.Add(Path.GetFileName(filePath));
            }
            return result;
        }
    }
}
