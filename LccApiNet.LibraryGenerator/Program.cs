using LccApiNet.LibraryGenerator.Core;
using LccApiNet.LibraryGenerator.SchemeModel;
using LccApiNet.LibraryGenerator.Utilities;

using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

using System.Runtime.CompilerServices;

namespace LccApiNet.LibraryGenerator;

public class Program
{
    private const int SUPPORTED_SCHEMA_VERSION = 1;
    private const ConsoleColor DEFAULT_COLOR = ConsoleColor.Green;
        
    private static ApiScheme? _scheme;

    static void Main(string[] args)
    {
        Console.ForegroundColor = DEFAULT_COLOR;
        ConsoleUtils.ShowInfo("Parsing scheme...");

        string schemeContent;
        using (StreamReader reader = new StreamReader(new FileStream(@"D:\Development\GitHub\LeagueClientControllers\WebServer\api-scheme.json", FileMode.Open))) {
            schemeContent = reader.ReadToEnd();
        }

        try {
            _scheme = JsonConvert.DeserializeObject<ApiScheme>(schemeContent);
        } catch {
            ConsoleUtils.ShowError("Scheme JSON parsing error");
            return;
        }

        if (_scheme == null) {
            ConsoleUtils.ShowError("Scheme structure error");
            return;
        }

        ConsoleUtils.ShowInfo("Scheme is successfully parsed");

        if (_scheme.SchemeVersion != SUPPORTED_SCHEMA_VERSION) { 
            ConsoleUtils.ShowError($"Scheme version is unsupported - {SUPPORTED_SCHEMA_VERSION} required, {_scheme.SchemeVersion} requested");
            return;
        }

        Console.WriteLine();
        ShowHeader();
        Console.WriteLine();

        Generator.GenerateLibrary(_scheme);

        Console.ForegroundColor = ConsoleColor.White;
    }

    private static void ShowHeader()
    {
        int maxLineLength = $" SCHEME GENERATED: {_scheme!.GeneratedAt} ".Length + 7 * 2;

        void writeCenter(string line)
        {
            string starsString = "";
            int starsCount = (maxLineLength - line.Length - 2) / 2;
            for (int i = 0; i < starsCount; i++) {
                starsString += "*";
            }

            Console.WriteLine($"/{starsString} {line} {starsString}/");
        }

        void writeLeft(string line)
        {
            string starsString = "";
            int starsCount = maxLineLength - line.Length - 6;
            for (int i = 0; i < starsCount; i++) {
                starsString += "*";
            }

            Console.WriteLine($"/**** {line} {starsString}/");
        }

        string starsString = "";
        for (int i = 0; i < maxLineLength; i++) {
            starsString += "*";
        }

        Console.WriteLine($"/{starsString}/");
        writeCenter(      $"C# LARC API LIBRARY GENERATOR");
        Console.WriteLine($"/{starsString}/");
        Console.WriteLine($"/{starsString}/");
        writeLeft(        $"SCHEME VERSION: {_scheme!.SchemeVersion}");
        writeLeft(        $"API VERSION: {_scheme!.ApiVersion}");
        writeLeft(        $"SCHEME GENERATED: {_scheme!.GeneratedAt}");
        Console.WriteLine($"/{starsString}/");
    }
}