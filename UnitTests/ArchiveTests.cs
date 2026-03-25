using System.Collections.Concurrent;
using System.Threading.Tasks;
using TestTaskKaspersky.Controllers;
using TestTaskKaspersky.Models;
using TestTaskKaspersky.Services;

namespace UnitTests
{
    public class ArchiveTests
    {
        public ArchiveTests()
        {
            _archiveService = new ArchiveService();
            _archiveController = new ArchiveController();
            ConcurrentDictionary<Guid, ArchiveTask> dictTasks = ArchiveService.archiveTasks;
            _task = new()
            {
                Id = Guid.NewGuid(),
                Files = [],
                Status = "",
            };
            _doneTask = new()
            {
                Id = Guid.NewGuid(),
                Files = [],
                Status = "Done",
                ArchivePath = ".\\.\\Archives\\"
            };
            dictTasks.TryAdd(_task.Id, _task);
            dictTasks.TryAdd(_doneTask.Id, _doneTask);

        }
        public ArchiveTask _task;
        public ArchiveTask _doneTask;
        private readonly ArchiveService _archiveService;
        private readonly ArchiveController _archiveController;
        
        // GetByIdTests
        [Fact]
        public void GetById_ArchiveTaskNotFound()
        {
            ArchiveTask task = new()
            {
                Id = Guid.NewGuid(),
                Files = [],
                Status = ""
            };
            ArchiveTask? result = _archiveService.GetById(task.Id);
            Assert.Equal(null!, result);
        }

        [Fact]
        public void GetById_ArchiveTaskSuccess()
        {
            ArchiveTask? result = _archiveService.GetById(_task.Id);
            Assert.Equal(_task, result);
        }

        // GetTaskStatusTests
        [Fact]
        public void GetArchiveTaskStatus_TaskNotFound()
        {
            ArchiveTask task = new()
            {
                Id = Guid.NewGuid(),
                Files = [],
                Status = "",
            };
            string result = _archiveController.GetArchiveTaskStatus(task.Id);
            Assert.Equal("Task not found", result);
        }

        [Fact]
        public void GetArchiveTaskStatus_Pending()
        {
            _task.Status = "Pending";
            string result = _archiveController.GetArchiveTaskStatus(_task.Id);
            Assert.Equal("Pending", result);
        }

        [Fact]
        public void GetArchiveTaskStatus_Processing()
        {
            _task.Status = "Processing";
            string result = _archiveController.GetArchiveTaskStatus(_task.Id);
            Assert.Equal("Processing", result);
        }

        [Fact]
        public void GetArchiveTaskStatus_Done()
        {
            ArchiveTask? doneTask = _archiveService.GetById(_doneTask.Id);
            string result = _archiveController.GetArchiveTaskStatus(doneTask!.Id);
            Assert.Equal("Done", result);
            Assert.NotNull(doneTask.ArchivePath);
        }
    }
}