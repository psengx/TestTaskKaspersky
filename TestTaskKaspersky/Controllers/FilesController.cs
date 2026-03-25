using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTaskKaspersky.Services;

namespace TestTaskKaspersky.Controllers
{
    // Контроллер для работы с файлами (их получение)
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        public FilesController(ILogger<FilesController> logger) { _logger = logger; }

        [HttpGet] // Гет-запрос для получения файлов
        public List<string> GetFiles()
        {
            FileService fileService = new FileService();
            var list = fileService.GetFileNames();

            _logger.LogInformation($"{DateTime.UtcNow}      List of files requested");
            return list;
        }
    }
}
