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
        [HttpGet] // Гет-запрос для получения файлов
        public List<string>? GetFiles()
        {
            FileService fileService = new FileService();
            var list = fileService.GetFileNames();

            return list;
        }
    }
}
