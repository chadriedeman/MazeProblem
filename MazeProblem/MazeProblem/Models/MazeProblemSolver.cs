using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MazeProblem.Models
{
    public class MazeProblemSolver
    {
        public MazeProblemResults SolveMazeProblem(string definitionFilePath)
        {
            var definitionFile = ReadDefinitionFile(definitionFilePath);

            var maze = CreateMaze(definitionFile);

            var exitPosition = SendLazerThroughMaze(maze, definitionFile.LazerEntryRoom);

            return new MazeProblemResults
            {
                BoardSize = definitionFile.BoardSize,
                LazerEntryPositionAndOrientation = definitionFile.LazerEntryRoom,
                ExitPostionAndOrientation = exitPosition
            }; 
        }

        private DefinitionFile ReadDefinitionFile(string definitionFilePath)
        {
            var definitionFileContents = File.ReadAllLines(definitionFilePath);

            CheckForValidFileContents(definitionFileContents);

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

        private void CheckForValidFileContents(string[] fileContents)
        {
            if (!fileContents.Any())
                throw new ArgumentException($"{fileContents} is empty.");

            // TODO: Finish
        }

        private Maze CreateMaze(DefinitionFile definitionFile)
        {
            var maze = new Maze
            {
                MazeSquares = new List<MazeSquare>()
            };

            SetMazeWidthAndHeight(ref maze, definitionFile.BoardSize);

            AddMazeSquaresToMaze(ref maze);

            // TODO: UpdateDoorValues();

            AddMirrorsToMaze(ref maze, definitionFile.MirrorPlacements);

            return maze;
        }

        private void SetMazeWidthAndHeight(ref Maze maze, string boardSize)
        {
            var mazeWidthAndHeight = boardSize.Split(',');

            maze.Width = Int32.Parse(mazeWidthAndHeight[0]);
            maze.Height = Int32.Parse(mazeWidthAndHeight[1]);
        }

        private void AddMazeSquaresToMaze(ref Maze maze)
        {
            // Use 2D array instead? 

            for (int height = maze.Height - 1; height >= 0; height--)
            {
                for (int width = maze.Width - 1; width >= 0; width--)
                {
                    maze.MazeSquares.Add(new MazeSquare
                    {
                        Position = new Position
                        {
                            X = width, //TODO: Check that this is right. Looks like it might be backwards because the bottom left square is 0, 0
                            Y = height
                        },
                        HasNorthDoor = true,
                        HasSouthDoor = true,
                        HasEastDoor = true,
                        HasWestDoor = true
                    });
                }
            }
        }

        private void AddMirrorsToMaze(ref Maze maze, List<String> mirrorPlacements)
        {
            // TODO
        }

        private string SendLazerThroughMaze(Maze maze, string lazerEntryRoom)
        {
            return null; // TODO
        }
    }
}
