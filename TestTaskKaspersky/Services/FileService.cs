namespace TestTaskKaspersky.Services
{
    // Сервис для получения файлов из хранилища
    public class FileService
    {
        private readonly DirectoryInfo storagePath = new DirectoryInfo(".\\AwesomeStorage");
        private List<string> files { get; set; } = [];

        public List<string> GetFiles()
        {
            FileInfo[] files = storagePath.GetFiles();
            foreach (FileInfo file in files)
            {
                this.files.Add(file.Name);
            }
            return this.files;
        }

    }
}
