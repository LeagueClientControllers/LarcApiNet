namespace LccApiNet.LibraryGenerator.Utilities
{
    public class ConsoleUtils
    {
        public const ConsoleColor DEFAULT_COLOR = ConsoleColor.Green;

        public static void ShowInfo(string line)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"[INFO]\t{line}");
            Console.ForegroundColor = DEFAULT_COLOR;
        }

        public static void ShowError(string line)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Error.WriteLine($"[ERROR]\t{line}");
            Console.ForegroundColor = DEFAULT_COLOR;
        }
    }
}
