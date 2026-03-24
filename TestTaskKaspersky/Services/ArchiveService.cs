using System.Collections.Concurrent;
using System.IO.Compression;
using System.Net;
using TestTaskKaspersky.Models;

namespace TestTaskKaspersky.Services
{
    // Сервис для архивации
    public class ArchiveService
    {
        public static ConcurrentDictionary<Guid, ArchiveTask> archiveTasks = new(); // Словарь задач архивации
        private string archivesPath = ".\\Archives"; // Директория с готовыми архивами

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
                try
                {
                    // Создаю путь для конкретного архива (из части айдишника задачи архивации)
                    string archivePath = Path.Combine(archivesPath, $"{task.Id.ToString().Substring(0, 7)}.zip");
                    using (var zip = ZipFile.Open(archivePath, ZipArchiveMode.Create))
                    {
                        foreach (string file in task.Files)
                        {
                            // Надо разобраться че я тут сделала
                            zip.CreateEntryFromFile(Path.Combine(".\\AwesomeStorage", file), file);
                        }
                    }
                    task.Status = "Done";
                    task.ArchivePath = archivePath;
                }
                catch (Exception ex)
                {
                    task.ErrorMessage = ex.Message;
                    task.Status = "Failed";
                }
            });
            
        }
    }
}
