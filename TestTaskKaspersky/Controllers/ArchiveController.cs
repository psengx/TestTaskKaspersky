using Microsoft.AspNetCore.Mvc;
using TestTaskKaspersky.Models;
using TestTaskKaspersky.Services;

namespace TestTaskKaspersky.Controllers
{
    [Route("api/archive")]
    [ApiController]
    public class ArchiveController : ControllerBase
    {
        private ArchiveService _archiveService = new ArchiveService();

        // Создаю задачку архивации
        [HttpPut("/initialize")]
        public Guid InitializeArchiveTask(List<string> files)
        {
            Guid id = _archiveService.InitializeTask(files);
            ArchiveTask? task = _archiveService.GetById(id); // ищу таску в словаре тасков
            Task.Run(() => _archiveService.Archive(task)); // запускаю асинхронный процесс архивации
            return id;
        }

        [HttpGet("/status")]
        public string GetArchiveTaskStatus(Guid id)
        {
            ArchiveTask? task = _archiveService.GetById(id);
            if (task == null)
                return "Task not found";
            if (task.ErrorMessage != null)
                return task.Status + '\n' + task.ErrorMessage;
            return task.Status;
        }

        [HttpGet("/download")]
        public IActionResult GetArchive(Guid id)
        {
            ArchiveTask task = _archiveService.GetById(id); // ищу таску в словаре тасков
            if (task == null)
                return NotFound("TaskNotFound");
            if (task.Status == "Processing")
                return BadRequest("Archiving in process");
            if (task.Status == "Failed")
                return BadRequest(task.ErrorMessage);

            var stream = _archiveService.GetArchiveStream(id);
            return File(stream, "application/zip", $"{id.ToString().Substring(0, 7)}.zip");
        }
    }
}
