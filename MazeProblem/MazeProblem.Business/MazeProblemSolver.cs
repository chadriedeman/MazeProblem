using MazeProblem.Domain.Enums;
using MazeProblem.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MazeProblem.Business
{
    public class MazeProblemSolver
    {
        public MazeProblemResults SolveMazeProblem(string definitionFilePath)
        {
            var definitionFile = ReadDefinitionFile(definitionFilePath);

            var maze = CreateMaze(definitionFile);

            var results = SendLazerThroughMaze(maze, definitionFile.LazerEntryRoom);

            return new MazeProblemResults
            {
                BoardSize = definitionFile.BoardSize,
                LazerEntryPositionAndOrientation = definitionFile.LazerEntryRoom,
                ExitPostionAndOrientation = results.ExitPostionAndOrientation,
                LazerPath = results.LazerPath
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

            if (fileContents.Length < 5)
                throw new ArgumentException($"{fileContents} does not have enough lines to be valid.");
        }

        private Maze CreateMaze(DefinitionFile definitionFile)
        {
            var maze = new Maze
            {
                MazeSquares = new List<MazeSquare>()
            };

            SetMazeWidthAndHeight(ref maze, definitionFile.BoardSize);

            AddMazeSquaresToMaze(ref maze);

            IdentifyPerimeterDoors(ref maze);

            AddMirrorsToMaze(ref maze, definitionFile.MirrorPlacements);

            return maze;
        }

        private void IdentifyPerimeterDoors(ref Maze maze)
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

            maze.Width = int.Parse(mazeWidthAndHeight[0]);
            maze.Height = int.Parse(mazeWidthAndHeight[1]);
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

                var xPosition = int.Parse(xPositionAsString);
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

            for(int i = startIndex; (i < mirrorPlacement.Length) && (char.ToUpper(mirrorPlacement[i]) != 'L' && char.ToUpper(mirrorPlacement[i]) != 'R'); i++)
            {
                yPosition += mirrorPlacement[i];
            }

            return int.Parse(yPosition);
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

        private MazeProblemResults SendLazerThroughMaze(Maze maze, string lazerEntryRoom)
        {
            var entryCoordinates = GetLazerEntryCoordinates(lazerEntryRoom);

            var entryOrientation = lazerEntryRoom[lazerEntryRoom.Length - 1]
                .ToString();

            var path = new LazerPath();

            var currentSquare = maze.MazeSquares.FirstOrDefault((mazeSquare) => mazeSquare.Position.X == entryCoordinates.X && mazeSquare.Position.Y == entryCoordinates.Y);

            path.AddMazeSquareToPath(currentSquare, entryOrientation);

            var laserDirection = GetInitialLazerDirection(currentSquare, entryOrientation);

            do
            {
                currentSquare = GetNextSquare(maze, currentSquare, ref laserDirection);
                var orientation = GetOrientation(laserDirection);

                if (currentSquare != null)
                    path.AddMazeSquareToPath(currentSquare, orientation);

            } while (currentSquare != null);

            return new MazeProblemResults
            {
                ExitPostionAndOrientation = $"{path.LazerPathStep.Last().Position.X},{path.LazerPathStep.Last().Position.Y}{path.LazerPathStep.Last().Orientation}",
                LazerPath = path
            };
        }

        private MazeSquare GetNextSquare(Maze maze, MazeSquare currentMazeSquare, ref LazerDirection lazerDirection)
        {
            if (!MazeSquareHasMirror(currentMazeSquare))
            {
                var nextPosition = GetNextPosition(currentMazeSquare, lazerDirection);
                return FindMazeSquareFromPosition(maze, nextPosition);
            }
            else
            {
                var nextPosition = GetNextPositionWhenMirrorPresent(currentMazeSquare, ref lazerDirection);
                return FindMazeSquareFromPosition(maze, nextPosition);
            }
        }

        private MazeSquare FindMazeSquareFromPosition(Maze maze, Position position)
        {
            var nextSquare = maze.MazeSquares.FirstOrDefault((mazeSquare) => mazeSquare.Position.X == position.X && mazeSquare.Position.Y == position.Y);

            return nextSquare != null ? nextSquare : null;
        }

        private Position GetNextPositionWhenMirrorPresent(MazeSquare currentMazeSquare, ref LazerDirection lazerDirection)
        {
            if (currentMazeSquare.Mirror.Type == MirrorType.OneSided)
            {
                return GetPositionWhenOnesidedMirror(currentMazeSquare, ref lazerDirection);
            }

            else if (currentMazeSquare.Mirror.Type == MirrorType.TwoSided)
            {
                ChangeLazerDirection(currentMazeSquare.Mirror.Direction, ref lazerDirection);

                return GetNextPosition(currentMazeSquare, lazerDirection);
            }

            else 
                return null;
        }

        private Position GetPositionWhenOnesidedMirror(MazeSquare currentMazeSquare, ref LazerDirection lazerDirection)
        {
            if (currentMazeSquare.Mirror.Direction == MirrorDirection.Right)
                return GetPositionForRightDirectionMirror(currentMazeSquare, ref lazerDirection);

            else if (currentMazeSquare.Mirror.Direction == MirrorDirection.Left)
                return GetPositionForLeftDirectionMirror(currentMazeSquare, ref lazerDirection);

            else
                return null;
        }

        private Position GetPositionForRightDirectionMirror(MazeSquare currentMazeSquare, ref LazerDirection lazerDirection)
        {
            if (currentMazeSquare.Mirror.ReflectiveSide == MirrorReflectiveSide.Left && (lazerDirection == LazerDirection.North || lazerDirection == LazerDirection.West))
                return GetNextPosition(currentMazeSquare, lazerDirection);

            if (currentMazeSquare.Mirror.ReflectiveSide == MirrorReflectiveSide.Right && (lazerDirection == LazerDirection.South || lazerDirection == LazerDirection.East))
                return GetNextPosition(currentMazeSquare, lazerDirection);

            if (currentMazeSquare.Mirror.ReflectiveSide == MirrorReflectiveSide.Right && (lazerDirection == LazerDirection.North || lazerDirection == LazerDirection.West))
            {
                ChangeLazerDirection(currentMazeSquare.Mirror.Direction, ref lazerDirection);
                return GetNextPosition(currentMazeSquare, lazerDirection);
            }

            if (currentMazeSquare.Mirror.ReflectiveSide == MirrorReflectiveSide.Left && (lazerDirection == LazerDirection.South || lazerDirection == LazerDirection.East))
            {
                ChangeLazerDirection(currentMazeSquare.Mirror.Direction, ref lazerDirection);
                return GetNextPosition(currentMazeSquare, lazerDirection);
            }

            return null;
        }

        private Position GetPositionForLeftDirectionMirror(MazeSquare currentMazeSquare, ref LazerDirection lazerDirection)
        {
            if (currentMazeSquare.Mirror.ReflectiveSide == MirrorReflectiveSide.Left && (lazerDirection == LazerDirection.South || lazerDirection == LazerDirection.West))
                return GetNextPosition(currentMazeSquare, lazerDirection);

            if (currentMazeSquare.Mirror.ReflectiveSide == MirrorReflectiveSide.Right && (lazerDirection == LazerDirection.North || lazerDirection == LazerDirection.East))
                return GetNextPosition(currentMazeSquare, lazerDirection);

            if (currentMazeSquare.Mirror.ReflectiveSide == MirrorReflectiveSide.Right && (lazerDirection == LazerDirection.South || lazerDirection == LazerDirection.West))
            {
                ChangeLazerDirection(currentMazeSquare.Mirror.Direction, ref lazerDirection);
                return GetNextPosition(currentMazeSquare, lazerDirection);
            }

            if (currentMazeSquare.Mirror.ReflectiveSide == MirrorReflectiveSide.Left && (lazerDirection == LazerDirection.North || lazerDirection == LazerDirection.East))
            {
                ChangeLazerDirection(currentMazeSquare.Mirror.Direction, ref lazerDirection);
                return GetNextPosition(currentMazeSquare, lazerDirection);
            }

            return null;
        }

        private void ChangeLazerDirection(MirrorDirection mirrorDirection, ref LazerDirection lazerDirection)
        {
            if (mirrorDirection == MirrorDirection.Left)
            {
                if (lazerDirection == LazerDirection.North)
                {
                    lazerDirection = LazerDirection.West;
                    return;
                }

                if (lazerDirection == LazerDirection.South)
                {
                    lazerDirection = LazerDirection.East;
                    return;
                }

                if (lazerDirection == LazerDirection.East)
                {
                    lazerDirection = LazerDirection.South;
                    return;
                }

                if (lazerDirection == LazerDirection.West)
                {
                    lazerDirection = LazerDirection.North;
                    return;
                }
            }

            if (mirrorDirection == MirrorDirection.Right)
            {
                if (lazerDirection == LazerDirection.North)
                {
                    lazerDirection = LazerDirection.East;
                    return;
                }

                if (lazerDirection == LazerDirection.South)
                {
                    lazerDirection = LazerDirection.West;
                    return;
                }

                if (lazerDirection == LazerDirection.East)
                {
                    lazerDirection = LazerDirection.North;
                    return;
                }

                if (lazerDirection == LazerDirection.West)
                {
                    lazerDirection = LazerDirection.South;
                    return;
                }
            }
        }

        private Position GetNextPosition(MazeSquare currentMazeSquare, LazerDirection lazerDirection)
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

            if (lazerDirection == LazerDirection.West || lazerDirection == LazerDirection.East)
                return "H";

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

            var xPosition = int.Parse(xPositionAsString);
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

            for (int i = startIndex; (i < laserEntryRoom.Length) && (char.ToUpper(laserEntryRoom[i]) != 'V' && char.ToUpper(laserEntryRoom[i]) != 'H'); i++)
            {
                yPosition += laserEntryRoom[i];
            }

            return int.Parse(yPosition);
        }
    }
}
