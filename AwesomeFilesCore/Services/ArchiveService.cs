using System.Collections.Concurrent;
using System.IO.Compression;
using AwesomeFilesCore.Models;

namespace AwesomeFilesCore.Services
{
    // Сервис для архивации
    public class ArchiveService
    {
        public static ConcurrentDictionary<Guid, ArchiveTask> archiveTasks = new(); // Словарь задач архивации

        private readonly string _storagePath;
        public ArchiveService(string storagePath)
        {
            _storagePath = storagePath;
        }

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
            };
            archiveTasks.TryAdd(task.Id, task);
            return task.Id;
        }

        public string GetArchiveTaskStatus(Guid id)
        {
            ArchiveTask? task = GetById(id);
            if (task == null)
                return "Task not found";
            if (task.ErrorMessage != null)
                return task.Status + '\n' + task.ErrorMessage;
            return task.Status;
        }

        // Процесс архивации
        public async Task Archive(ArchiveTask task)
        {
            task.Status = "Processing";
            await Task.Run(() =>
            {
                var memoryStream = new MemoryStream();
                using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (string file in task.Files)
                    {
                        string filePath = Path.Combine("..\\..\\..\\..\\AwesomeStorage", file);
                        string filePathWeb = Path.Combine("..\\AwesomeStorage", file);
                        if (!File.Exists(filePath) && !File.Exists(filePathWeb))
                        {
                            task.Status = "Failed";
                            task.ErrorMessage = $"File {file} not found in storage";
                            return;
                        } 
                        else if (File.Exists(filePath))
                        {
                            zip.CreateEntryFromFile(filePath, file);
                        } 
                        else 
                        { 
                            zip.CreateEntryFromFile(filePathWeb, file);
                        }
                    }
                }
                task.ArchiveStream = memoryStream.ToArray();
                task.Status = "Done";
            });
        }
        public byte[] GetArchiveStream(Guid id)
        {
            ArchiveTask? task = GetById(id); // ищу таску в словаре тасков
            return task!.ArchiveStream;
        }
    }
}
