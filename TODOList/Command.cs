using Spectre.Console;

namespace TODO;

internal abstract class Command(string name, string description)
{
    public string Name { get; } = name;
    public string Description { get; } = description;

    /*
     * Returns whether the file should be saved after execution.
     */
    public abstract bool Parse(string arg, List<Item> items);

    protected static bool TryGetIndex(string arg, int count, out int index)
    {
        if (int.TryParse(arg, out int i) && i > 0 && i <= count)
        {
            index = i - 1;
            return true;
        }
        index = -1;
        return false;
    }
}

class AddCommand() : Command("add", "Add a new task")
{
    public override bool Parse(string arg, List<Item> items)
    {
        if (string.IsNullOrWhiteSpace(arg))
        {
            AnsiConsole.MarkupLine("[red][bold]Error:[/][/] Argument cannot be empty.");
            return false;
        }

        items.Add(new Item(arg));
        AnsiConsole.MarkupLine("Done.");
        return true;
    }
}

class ListCommand() : Command("list", "Show tasks")
{
    public override bool Parse(string arg, List<Item> items)
    {
        if (items.Count is 0)
        {
            AnsiConsole.MarkupLine("[gray]No items available.[/]");
        }
        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Status");
        table.AddColumn("Task");

        foreach (var item in items)
        {
            string status = item.IsDone ? "[bold]  x  [/]" : " ";
            table.AddRow(
                (items.IndexOf(item) + 1).ToString(),
                status,
                item.Text
            );
        }

        AnsiConsole.Write(table);
        return false;
    }
}

class DoneCommand() : Command("done", "Mark task as done")
{
    public override bool Parse(string arg, List<Item> items)
    {
        if (TryGetIndex(arg, items.Count, out int index))
        {
            items[index] = items[index] with { IsDone = true };
            Console.WriteLine("Done.");
            return true;
        }
        AnsiConsole.MarkupLine("[red][bold]Error:[/][/] Invalid index.");
        return false;
    }
}

class DelCommand() : Command("del", "Delete task")
{
    public override bool Parse(string arg, List<Item> items)
    {
        if (TryGetIndex(arg, items.Count, out int index))
        {

            items.RemoveAt(index + 1);
            Console.WriteLine("Done.");
            return true;
        }

        AnsiConsole.MarkupLine("[red][bold]Error:[/][/] Invalid index.");
        return false;
    }
}

