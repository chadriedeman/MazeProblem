using MazeProblem.Business;
using MazeProblem.Business.Services;
using System;
using System.IO;
using System.Linq;

namespace MazeProblem
{
    public class MazeProblemMain
    {
        public static void Main(string[] args)
        {
            try
            {
                if (!args.Any())
                    throw new ArgumentException("No definition file was given. Please enter the path to the definition file and run again.");

                var definitionFilePath = args[0].Trim();

                if (!File.Exists(definitionFilePath))
                    throw new ArgumentException($"No file exists for {definitionFilePath}.");

                var mirrorService = new MirrorService();

                var lazerService = new LazerService();

                var mazeService = new MazeService(mirrorService);

                var mazeProblemSolver = new MazeProblemSolver(mazeService, lazerService);

                var results = mazeProblemSolver.SolveMazeProblem(definitionFilePath);

                MazeProblemResultsPrinter.PrintResults(results);
            } 

            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Environment.Exit(0);
            }
        }
    }
}
