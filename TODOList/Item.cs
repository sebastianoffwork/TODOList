namespace TODO;

record Item(string Text, bool IsDone = false)
{
    public static Item Parse(string line)
    {
        string[] parts = line.Split(';');
        /*
         * Text -- string
         * IsDone -- bool
         */
        return new Item(
            parts[0],
            bool.Parse(parts[1])
        );
    }

    public string ToCsv()
    {
        return $"{Text};{IsDone}";
    }
}