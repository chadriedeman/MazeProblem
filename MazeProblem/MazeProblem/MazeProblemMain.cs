using MazeProblem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MazeProblem
{
    public class MazeProblemMain
    {
        public static void Main(string[] args)
        {

            if (!args.Any())
                Console.WriteLine("No definition file was given. Please enter the path to the definition file and run again.");

            var definitionFilePath = args[0];

            if (!File.Exists(definitionFilePath))
                Console.WriteLine($"No file exists for {definitionFilePath}.");

            var definitionFile = ReadDefinitionFile(definitionFilePath);



        }

        private static DefinitionFile ReadDefinitionFile(string definitionFilePath)
        {
            var definitionFileContents = File.ReadAllLines(definitionFilePath);

            // TODO: Check for valid file contents

            var definitionFile = new DefinitionFile
            {
                BoardSize = definitionFileContents[0],
                MirrorPlacements = new List<String>(),
                LazerEntryRoom = definitionFileContents[definitionFileContents.Length - 2]
            };

            for (int i = 2; i < definitionFileContents.Length - 3; i++)
            {
                definitionFile.MirrorPlacements.Add(definitionFileContents[i]);
            }

            return definitionFile;
        }
    }
}
