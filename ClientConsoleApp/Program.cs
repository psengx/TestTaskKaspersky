using Microsoft.OpenApi.Any;

namespace ClientConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            CommandParser parser = new CommandParser();
            while (true)
            {
                Console.WriteLine(@"Enter command:
> list - to get file list
> create-archive file1, file2, ...fileN 
> status ID - to get archiving status
> download ID %path%

or press <Enter> to exit..." + '\n');
                string? input = Console.ReadLine();
                Command command = parser.TryParse(input!);

                if (command == null)
                    break;
                parser.HandleCommand(command);

            }
        }
    }
}
