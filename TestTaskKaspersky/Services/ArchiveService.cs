using System.Collections.Concurrent;
using System.IO.Compression;
using TestTaskKaspersky.Models;

namespace TestTaskKaspersky.Services
{
    // Сервис для архивации
    public class ArchiveService
    {
        public static ConcurrentDictionary<Guid, ArchiveTask> archiveTasks = new(); // Словарь задач архивации
        private string archivesPath = ".\\Archives"; // Директория с готовыми архивами

        public ArchiveTask? GetById(Guid id)
        {
            if (!archiveTasks.ContainsKey(id))
                return null;

            return archiveTasks[id];
        }

        // Инициализация задачи архивации
        public Guid InitializeTask(List<string> fileNames)
        {
            ArchiveTask task = new()
            {
                Id = Guid.NewGuid(),
                Files = fileNames,
                Status = "Pending",
                ArchivePath = ""
            };
            archiveTasks.TryAdd(task.Id, task);
            return task.Id;
        }

        // Процесс архивации
        public async Task Archive(ArchiveTask task)
        {
            task.Status = "Processing";
            await Task.Run(() =>
            {
               // Создаю путь для конкретного архива (из части айдишника задачи архивации)
               string archivePath = Path.Combine(archivesPath, $"{task.Id.ToString().Substring(0, 7)}.zip");
               using (var zip = ZipFile.Open(archivePath, ZipArchiveMode.Create)) // создаю и открываю архив по пути archivePath
               {
                    foreach (string file in task.Files)
                    {
                        string filePath = Path.Combine(".\\AwesomeStorage", file);
                        if (!File.Exists(filePath))
                        {
                            task.Status = "Failed";
                            task.ErrorMessage = $"File {file} not found in storage";
                            return;
                        }
                        // Добавляю в архив каждый файл по своему пути, называю теми же именами внутри архива
                        zip.CreateEntryFromFile(filePath, file);
                    }
               }
               task.Status = "Done";
               task.ArchivePath = archivePath;
            });
        }
        public FileStream GetArchiveStream(Guid id)
        {
            ArchiveTask? task = GetById(id); // ищу таску в словаре тасков
            return new FileStream(task.ArchivePath, FileMode.Open, FileAccess.Read);
        }
    }
}
