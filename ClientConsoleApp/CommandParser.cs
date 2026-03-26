namespace ClientConsoleApp
{
    public class Command
    {
        public string Name { get; set; }
        public List<string> Args { get; set; } = new();
    }

    public class CommandParser
    {
        private BackendClient client = new BackendClient();
        public Command TryParse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return new Command
            {
                Name = parts[0].ToLower(),
                Args = parts.Skip(1).ToList()
            };
        }

        public void HandleCommand(Command command)
        {
            switch (command.Name)
            {
                case "list":
                    if (command.Args.Count > 0)
                    {
                        Console.WriteLine("Wrong command (maybe you wrote more than just command)\n");
                        break;
                    }
                    client.GetFileListCommand();
                    break;
                case "create-archive":
                    if (command.Args.Count == 0)
                    {
                        Console.WriteLine("Wrong command: enter file names\n");
                        break;
                    }
                    client.CreateArchiveCommand(command.Args);
                    break;
                case "status":
                    if (command.Args.Count == 0 || command.Args.Count > 1)
                    {
                        Console.WriteLine("Wrong command: command 'status' has 1 argument - Task ID\n");
                        break;
                    }
                    if (!Guid.TryParse(command.Args[0], out Guid guid))
                    {
                        Console.WriteLine("Argument must be Guid\n");
                        break;
                    }
                    client.GetArchiveTaskStatusCommand(command.Args[0]);
                    break;
                case "download":
                    if (command.Args.Count == 0 || command.Args.Count > 2)
                    {
                        Console.WriteLine("Wrong command: command 'download' has 2 arguments - Task ID and Download Path\n");
                        break;
                    }
                    if (!Guid.TryParse(command.Args[0], out guid))
                    {
                        Console.WriteLine("Argument must be Guid\n");
                        break;
                    }
                    if (client._archiveService.GetById(guid) == null)
                    {
                        Console.WriteLine("Task not found\n");
                        break;
                    }
                    if (!Path.Exists(command.Args[1]))
                    {
                        Console.WriteLine("Enter correct path\n");
                        break;
                    }
                    client.DownloadArchiveCommand(command.Args[0], command.Args[1]);
                    break;
                default:
                    Console.WriteLine("Enter correct command!\n");
                    break;
            }
        }
    }
}
