using Spectre.Console;
using TODO;

/*
 * Where all items would be saved.
 */
const string FILE = "todo.txt";

/*
 * If file does not exist, init empty.
 * If it does exist, load the CSV.
 */
List<Item> items = File.Exists(FILE)
    ? [.. File.ReadAllLines(FILE).Select(Item.Parse)]
    : [];

List<Command> commands = [
    new AddCommand(),
    new ListCommand(),
    new DoneCommand(),
    new DelCommand() 
];

foreach (var command in commands)
{
    AnsiConsole.MarkupLine($"[cyan][bold]{command.Name,-10} - {command.Description}[/][/]");
}

while (true)
{
    Console.Write("> ");
    string input = Console.ReadLine() ?? "";
    string[] parts = input.Split(' ', 2);
    string commandStr = parts[0];
    string arg = parts.Length > 1 ? parts[1] : "";

    if (commandStr is "exit") break;

    var command = commands.FirstOrDefault(c => c.Name == commandStr);

    if (command is not null)
    {
        /*
         * If returned true, then we need to save.
         */
        if (command.Parse(arg, items))
        {
            Save();
        }
    }
    else
    {
        AnsiConsole.MarkupLine("[red][bold]Error:[/][/] Invalid command.");
    }
}

void Save()
{
    /*
     * Fully rewrite to file since we can't just go to the middle of the file
     * and mark a task as done.
     */
    File.WriteAllLines(FILE, items.Select(i => i.ToCsv()));
}
