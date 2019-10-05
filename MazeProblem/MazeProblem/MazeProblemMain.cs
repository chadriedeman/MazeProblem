using MazeProblem.Models;
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

                var mazeProblemSolver = new MazeProblemSolver();

                var results = mazeProblemSolver.SolveMazeProblem(definitionFilePath);

                PrintResults(results);
            } 

            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Environment.Exit(0);
            }
        }

        private static void PrintResults(MazeProblemResults results)
        {
            Console.WriteLine($"Board Size: {results.BoardSize}");
            Console.WriteLine($"Lazer Entry Point: {results.LazerEntryPositionAndOrientation}");
            Console.WriteLine($"Exit point: {results.ExitPostionAndOrientation}");

            Console.WriteLine("Lazer Path: ");
            results.LazerPath.LazerPathStep.ForEach((step) => Console.WriteLine($"    {step.Position.X},{step.Position.Y}{step.Orientation}"));
        }
    }
}
