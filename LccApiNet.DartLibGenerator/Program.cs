using System;
using System.Collections.Generic;
using System.IO;

namespace LccApiNet.DartLibGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILccApi _ = new LccApi();

            //Console.Write("Enter path to the library project directory: ");
            //string libraryProjectPath = Console.ReadLine();

            string outputDir = Path.Combine(Environment.CurrentDirectory, "output");
            if (Directory.Exists(outputDir)) {
                Directory.Delete(outputDir, true);
            }

            Directory.CreateDirectory(outputDir);

            Dictionary<Type, string> dartModelTypes = ModelsGenerator.Generate(@"D:\Development\GitHub\LeagueClientControllers\LccApiNet\LccApiNet");
            CategoriesGenerator.Generate(@"D:\Development\GitHub\LeagueClientControllers\LccApiNet\LccApiNet", dartModelTypes);
        }
    }
}
