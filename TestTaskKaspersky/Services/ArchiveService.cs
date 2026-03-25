using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using TestTaskKaspersky.Models;

namespace TestTaskKaspersky.Services
{
    // Сервис для архивации
    public class ArchiveService
    {
        public static ConcurrentDictionary<Guid, ArchiveTask> archiveTasks = new(); // Словарь задач архивации
        private DirectoryInfo archivesDirectory; // Директория с готовыми архивами
        public ArchiveService()
        {
            archivesDirectory = Directory.CreateDirectory(".\\Archives");
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
