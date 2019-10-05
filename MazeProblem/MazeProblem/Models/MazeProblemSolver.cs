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

            UpdateDoorValues(ref maze);

            // AddMirrorsToMaze(ref maze, definitionFile.MirrorPlacements); // TODO: Comment out for testing of code now

            return maze;
        }

        private void UpdateDoorValues(ref Maze maze)
        {
            var topPosition = maze.Height - 1;
            var rightPosition = maze.Width - 1;

            var mazeSquaresWithSouthPerimDoor = maze.MazeSquares.Where((mazeSquare) => mazeSquare.Position.Y == 0).ToList();
            mazeSquaresWithSouthPerimDoor.ForEach((mazeSquare) => mazeSquare.HasSouthPerimeterDoor = true);

            var mazeSquaresWithNorthPerimDoor = maze.MazeSquares.Where((mazeSquare) => mazeSquare.Position.Y == topPosition).ToList();
            mazeSquaresWithNorthPerimDoor.ForEach((mazeSquare) => mazeSquare.HasNorthPerimeterDoor = true);

            var mazeSquaresWithWestPerimDoor = maze.MazeSquares.Where((mazeSquare) => mazeSquare.Position.X == 0).ToList();
            mazeSquaresWithWestPerimDoor.ForEach((mazeSquare) => mazeSquare.HasWestPerimeterDoor = true);

            var mazeSquaresWithEastPerimDoor = maze.MazeSquares.Where((mazeSquare) => mazeSquare.Position.X == rightPosition).ToList();
            mazeSquaresWithEastPerimDoor.ForEach((mazeSquare) => mazeSquare.HasEastPerimeterDoor = true);
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
                        }
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

                var reflectiveSide = GetReflectiveSide(mirrorPlacement, yPosition);

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

        private MirrorReflectiveSide GetReflectiveSide(string mirrorPlacement, int yPosition)
        {
            var indexOfYPosition = mirrorPlacement.LastIndexOf(yPosition.ToString());

            var mirrorDirectionAndReflectiveSide = mirrorPlacement.Substring(indexOfYPosition + yPosition.ToString().Length);

            if (mirrorDirectionAndReflectiveSide.Length == 1)
                return MirrorReflectiveSide.Both;
            else
            {
                var reflectiveSide = mirrorDirectionAndReflectiveSide[1];

                switch (reflectiveSide)
                {
                    case 'L':
                        return MirrorReflectiveSide.Left;
                    case 'R':
                        return MirrorReflectiveSide.Right;
                    default:
                        return MirrorReflectiveSide.Unspecified;
                }
            }
        }

        private string SendLazerThroughMaze(Maze maze, string lazerEntryRoom)
        {
            var entryCoordinates = GetLazerEntryCoordinates(lazerEntryRoom);

            var entryOrientation = lazerEntryRoom[lazerEntryRoom.Length - 1]
                .ToString();

            var path = new LazerPath();

            var currentSquare = maze.MazeSquares.FirstOrDefault((mazeSquare) => mazeSquare.Position.X == entryCoordinates.X && mazeSquare.Position.Y == entryCoordinates.Y);

            path.AddMazeSquareToPath(currentSquare, entryOrientation);

            var laserDirection = GetInitialLazerDirection(currentSquare, entryOrientation);

            while (GetNextSquare(maze, currentSquare, ref laserDirection) != null)
            {
                currentSquare = GetNextSquare(maze, currentSquare, ref laserDirection);
                var orientation = GetOrientation(laserDirection);
                path.AddMazeSquareToPath(currentSquare, orientation);
            }

            // TODO



            return string.Empty; // return path too? New data structure?
        }

        private MazeSquare GetNextSquare(Maze maze, MazeSquare currentMazeSquare, ref LazerDirection lazerDirection)
        {
            if (!MazeSquareHasMirror(currentMazeSquare))
            {
                var nextPosition = GetNextPositionWhenNoMirrors(currentMazeSquare, lazerDirection);

                var nextSquare = maze.MazeSquares.FirstOrDefault((mazeSquare) => mazeSquare.Position.X == nextPosition.X && mazeSquare.Position.Y == nextPosition.Y);

                return nextSquare != null ? nextSquare : null;
            }

            else
            {
                return null; // TODO
            }
        }

        private Position GetNextPositionWhenNoMirrors(MazeSquare currentMazeSquare, LazerDirection lazerDirection)
        {

            var newPosY = currentMazeSquare.Position.Y + 1;
            var newNegY = currentMazeSquare.Position.Y - 1;
            var newPosX = currentMazeSquare.Position.X + 1;
            var newNegX = currentMazeSquare.Position.X - 1;


            if (lazerDirection == LazerDirection.North)
                return new Position { X = currentMazeSquare.Position.X, Y = newPosY };

            if (lazerDirection == LazerDirection.South)
                return new Position { X = currentMazeSquare.Position.X, Y = newNegY };

            if (lazerDirection == LazerDirection.East)
                return new Position { X = newPosX, Y = currentMazeSquare.Position.Y };

            if (lazerDirection == LazerDirection.West)
                return new Position { X = newNegX, Y = currentMazeSquare.Position.Y };

            return null;
        }

        private string GetOrientation(LazerDirection lazerDirection)
        {
            if (lazerDirection == LazerDirection.North || lazerDirection == LazerDirection.South)
                return "V";

            // TODO: Finish

            return string.Empty;
        }

        private LazerDirection GetInitialLazerDirection(MazeSquare mazeSquare, string orientation)
        {
            if (mazeSquare.HasNorthPerimeterDoor && orientation == "V")
                return LazerDirection.South;

            if (mazeSquare.HasSouthPerimeterDoor && orientation == "V")
                return LazerDirection.North;

            if (mazeSquare.HasWestPerimeterDoor && orientation == "H")
                return LazerDirection.East;

            if (mazeSquare.HasEastPerimeterDoor && orientation == "H")
                return LazerDirection.West;

            return LazerDirection.Unspecified;
        }

        private bool MazeSquareHasMirror(MazeSquare square)
        {
            return square.Mirror != null;
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
