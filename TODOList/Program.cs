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

Print(items.Count is 0 ? "No data available." : $"Loaded {items.Count} entries.", ConsoleColor.DarkGray);
Print("Welcome!", ConsoleColor.Cyan);
Print("Available commands:\n\nadd [text]\nAdd a task with [text].\n\ndone [index]\nMark task at [index] as done.\n\nlist\nList all tasks.\n\nexit\nExit the app.\n", ConsoleColor.Cyan);

while (true)
{
    /*
     * Parse the input.
     */
    string input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) continue;

    string[] parts = input.Split(' ', 2);
    string command = parts[0]; 
    /*
     * Did we get an argument?
     */
    string argument = parts.Length > 1 ? parts[1] : "";


    switch (command)
    {
        case "add":
            {
                items.Add(new Item(argument));
                Print("Added.", ConsoleColor.Cyan);
                Save();
                break;
            }
        case "list":
            {
                Console.WriteLine();
                if (items.Count is 0) Print("No items.", ConsoleColor.DarkGray);

                foreach (var item in items)
                {
                    char doneChar = item.IsDone ? 'x' : ' ';
                    Console.WriteLine($"{items.IndexOf(item) + 1}. {item.Text} [{doneChar}]");
                }
                Console.WriteLine();
                break;
            }
        case "done":
            {
                if (int.TryParse(argument, out int index) && index > 0 && index <= items.Count)
                {
                    items[index - 1] = items[index - 1] with { IsDone = true };

                    Save();

                    Print("Done.", ConsoleColor.Green);
                }
                else
                {
                    Print("Invalid index.", ConsoleColor.Red);
                }
                break;
            }
        case "exit":
            {
                Environment.Exit(0);
                break;
            }
    }
}

static void Print(string s, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine(s);
    Console.ResetColor();
}

void Save()
{
    /*
     * Fully rewrite to file since we can't just go to the middle of the file
     * and mark a task as done.
     */
    File.WriteAllLines(FILE, items.Select(i => i.ToCsv()));
}