using LccApiNet.Core;

using System;

namespace LccApiNet.DartLibGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILccApi _ = new LccApi();

            //Console.Write("Enter path to the library project directory: ");
            //string libraryProjectPath = Console.ReadLine();

            ModelsGenerator.Generate(@"D:\Development\GitHub\LeagueClientControllers\LccApiNet\LccApiNet");
        }
    }
}
