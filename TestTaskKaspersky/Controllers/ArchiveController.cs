using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTaskKaspersky.Models;
using TestTaskKaspersky.Services;

namespace TestTaskKaspersky.Controllers
{
    [Route("api/archive")]
    [ApiController]
    public class ArchiveController : ControllerBase
    {
        // Создаю задачку архивации
        [HttpPut("/initialize")]
        public Guid InitializeArchiveTask(List<string> files)
        {
            ArchiveService archiveService = new ArchiveService();
            Guid id = archiveService.InitializeTask(files);
            ArchiveTask task = ArchiveService.archiveTasks.First(task => task.Key == id).Value; // ищу таску в словаре тасков
            Task.Run(() => archiveService.Archive(task)); // запускаю асинхронный процесс архивации
            return id;
        }

        [HttpGet]
        public string GetArchiveTaskStatus(Guid id)
        {
            return ArchiveService.archiveTasks.First(task => task.Key == id).Value.Status;
        }
    }
}
