using MazeProblem.Enums;
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
            for (int height = 0; height < maze.Height; height++)
            {
                for (int width = 0; width < maze.Width; width++)
                {
                    maze.MazeSquares.Add(new MazeSquare
                    {
                        Position = new Position
                        {
                            X = width,
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
            if (!mirrorPlacements.Any())
                return;

            foreach(var mirrorPlacement in mirrorPlacements)
            {
                var xPositionAsString = mirrorPlacement.Split(',')[0];

                var xPosition = Int32.Parse(xPositionAsString);
                var yPosition = GetMirrorYPosition(mirrorPlacement);

                var direction = GetMirrorDirection(mirrorPlacement, yPosition);

                var reflectiveSide = GetReflectiveSide(mirrorPlacement);

                var mirror = new Mirror
                {
                    Direction = direction,
                    Type = reflectiveSide == MirrorReflectiveSide.Both || reflectiveSide == MirrorReflectiveSide.Unspecified ? MirrorType.TwoSided : MirrorType.OneSided,
                    ReflectiveSide = reflectiveSide
                };

                var squareForMirror = maze.MazeSquares.FirstOrDefault((mazeSquare) => mazeSquare.Position.X == xPosition && mazeSquare.Position.Y == yPosition);

                squareForMirror.Mirror = mirror;
            }
        }

        private int GetMirrorYPosition(string mirrorPlacement)
        {
            var startIndex = mirrorPlacement.IndexOf(',') + 1;

            var yPosition = string.Empty;

            for(int i = startIndex; (i < mirrorPlacement.Length) && (Char.ToUpper(mirrorPlacement[i]) != 'L' && Char.ToUpper(mirrorPlacement[i]) != 'R'); i++)
            {
                yPosition += mirrorPlacement[i];
            }

            return Int32.Parse(yPosition);
        }

        private MirrorDirection GetMirrorDirection(string mirrorPlacement, int yPosition)
        {
            var indexOfYPosition = mirrorPlacement.LastIndexOf(yPosition.ToString());

            var mirrorDirectionAndReflectiveSide = mirrorPlacement.Substring(indexOfYPosition + yPosition.ToString().Length);

            var direction = mirrorDirectionAndReflectiveSide[0];

            return direction == 'L' ? MirrorDirection.Left : MirrorDirection.Right;
        }

        private MirrorReflectiveSide GetReflectiveSide(string mirrorPlacement)
        {
            return MirrorReflectiveSide.Unspecified; // TODO
        }

        private string SendLazerThroughMaze(Maze maze, string lazerEntryRoom)
        {
            var entryCoordinates = GetLazerEntryCoordinates(lazerEntryRoom);

            var entryOrientation = lazerEntryRoom[lazerEntryRoom.Length - 1]
                .ToString();



            return string.Empty; // TODO
        }

        private Position GetLazerEntryCoordinates(string lazerEntryRoom)
        {
            var xPositionAsString = lazerEntryRoom.Split(',')[0];

            var xPosition = Int32.Parse(xPositionAsString);
            var yPosition = GetLazerYPosition(lazerEntryRoom);

            return new Position
            {
                X = xPosition,
                Y = yPosition
            };
        }

        private int GetLazerYPosition(string laserEntryRoom)
        {
            var startIndex = laserEntryRoom.IndexOf(',') + 1;

            var yPosition = string.Empty;

            for (int i = startIndex; (i < laserEntryRoom.Length) && (Char.ToUpper(laserEntryRoom[i]) != 'V' && Char.ToUpper(laserEntryRoom[i]) != 'H'); i++)
            {
                yPosition += laserEntryRoom[i];
            }

            return Int32.Parse(yPosition);
        }
    }
}
