using MazeProblem.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MazeProblem.Business
{
    public class DefinitionFileReader
    {
        public DefinitionFile ReadDefinitionFile(string definitionFilePath)
        {
            var definitionFileContents = File.ReadAllLines(definitionFilePath);

            CheckForValidFileContents(definitionFileContents);

            var definitionFile = new DefinitionFile
            {
                BoardSize = definitionFileContents[0].Trim(),
                MirrorPlacements = new List<string>(),
                LazerEntryRoom = definitionFileContents[definitionFileContents.Length - 2].Trim()
            };

            for (int i = 2; i < definitionFileContents.Length - 3; i++)
            {
                definitionFile.MirrorPlacements.Add(definitionFileContents[i].Trim());
            }

            return definitionFile;
        }

        private void CheckForValidFileContents(string[] fileContents)
        {
            if (!fileContents.Any())
                throw new ArgumentException($"{fileContents} is empty.");

            if (fileContents.Length < 5)
                throw new ArgumentException($"{fileContents} does not have enough lines to be valid.");
        }
    }
}
