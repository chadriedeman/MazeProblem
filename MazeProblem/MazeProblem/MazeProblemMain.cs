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
            {
                Console.WriteLine("No definition file was given. Please enter the path to the definition file and run again.");
                Environment.Exit(0);
            }

            var definitionFilePath = args[0].Trim();

            if (!File.Exists(definitionFilePath))
            {
                Console.WriteLine($"No file exists for {definitionFilePath}.");
                Environment.Exit(0);
            }

            var definitionFile = ReadDefinitionFile(definitionFilePath);

            var maze = CreateMaze(definitionFile);

            //var exitPositionAndOrientation = ShootLazerThroughMaze(maze);

            //PrintResults(definitionFile.BoardSize, definitionFile.LazerEntryRoom, exitPositionAndOrientation);
        }

        private static DefinitionFile ReadDefinitionFile(string definitionFilePath)
        {
            var definitionFileContents = File.ReadAllLines(definitionFilePath);

            if (!definitionFileContents.Any())
            {
                Console.WriteLine($"{definitionFilePath} is empty.");
                Environment.Exit(0);
            }
            // TODO: Check for valid file contents

            var definitionFile = new DefinitionFile
            {
                BoardSize = definitionFileContents[0].Trim(),
                MirrorPlacements = new List<String>(),
                LazerEntryRoom = definitionFileContents[definitionFileContents.Length - 2].Trim()
            };

            for (int i = 2; i < definitionFileContents.Length - 3; i++)
            {
                definitionFile.MirrorPlacements.Add(definitionFileContents[i].Trim());
            }

            return definitionFile;
        }

        private static Maze CreateMaze(DefinitionFile definitionFile)
        {
            var maze = new Maze
            {
                MazeSquares = new List<MazeSquare>()
            };

            var mazeWidthAndHeight = definitionFile.BoardSize.Split(',');

            maze.Width = Int32.Parse(mazeWidthAndHeight[0]);
            maze.Height = Int32.Parse(mazeWidthAndHeight[1]);

            AddMazeSquaresToMaze(ref maze);

            // TODO: UpdateDoorValues();

            AddMirrorsToMaze(ref maze, definitionFile.MirrorPlacements);

            return maze;
        }

        private static void AddMazeSquaresToMaze(ref Maze maze)
        {
            for (int row = 0; row < maze.Height; row++)
            {
                for (int column = 0; column < maze.Width; column++)
                {
                    maze.MazeSquares.Add(new MazeSquare
                    {
                        Position = new Position
                        {
                            X = column, //TODO: Check that this is right. Looks like it might be backwards because the bottom left square is 0, 0
                            Y = row
                        },
                        HasNorthDoor = true,
                        HasSouthDoor = true,
                        HasEastDoor = true,
                        HasWestDoor = true
                    });
                }
            }
        }

        private static void AddMirrorsToMaze(ref Maze maze, List<String> mirrorPlacements)
        {
            // TODO
        }
    }
}
