using MazeProblem.Domain.Models;
using System;

namespace MazeProblem.Business
{
    public class MazeProblemResultsPrinter
    {
        public static void PrintResults(MazeProblemResults results)
        {
            if (results == null)
                throw new ArgumentException("No results given to PrintResults");

            Console.WriteLine($"Board Size: {results.BoardSize}");

            Console.WriteLine($"Lazer Entry Point: {results.LazerEntryPositionAndOrientation}");

            Console.WriteLine($"Exit point: {results.ExitPostionAndOrientation}");

            Console.WriteLine("Lazer Path: ");

            results.LazerPath.LazerPathStep.ForEach((step) => Console.WriteLine($"    {step.Position.X},{step.Position.Y}{step.Orientation}"));
        }
    }
}
