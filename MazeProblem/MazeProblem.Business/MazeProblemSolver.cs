using MazeProblem.Business.Services;
using MazeProblem.Domain.Enums;
using MazeProblem.Domain.Models;
using System.Linq;

namespace MazeProblem.Business
{
    public class MazeProblemSolver
    {
        private readonly IMazeService _mazeService;
        private readonly ILazerService _lazerService;

        public MazeProblemSolver()
        {
            var mirrorService = new MirrorService();

            _mazeService = new MazeService(mirrorService);

            _lazerService = new LazerService();
        }

        public Results SolveMazeProblem(string definitionFilePath)
        {
            var definitionFileReader = new DefinitionFileReader();

            var definitionFile = definitionFileReader.ReadDefinitionFile(definitionFilePath);

            var maze = _mazeService.CreateMaze(definitionFile);

            var results = SendLazerThroughMaze(maze, definitionFile.LazerEntryRoom);

            return new Results
            {
                BoardSize = definitionFile.BoardSize,
                LazerEntryPositionAndOrientation = definitionFile.LazerEntryRoom,
                ExitPostionAndOrientation = results.ExitPostionAndOrientation,
                LazerPath = results.LazerPath
            }; 
        }

        private Results SendLazerThroughMaze(Maze maze, string lazerEntryRoom)
        {
            var entryCoordinates = GetLazerEntryCoordinates(lazerEntryRoom);

            var entryOrientation = lazerEntryRoom[lazerEntryRoom.Length - 1]
                .ToString();

            var path = new LazerPath();

            var currentSquare = maze.MazeSquares
                .FirstOrDefault((mazeSquare) => mazeSquare.Position.X == entryCoordinates.X && mazeSquare.Position.Y == entryCoordinates.Y);

            path.AddMazeSquareToPath(currentSquare, entryOrientation);

            var laserDirection = _lazerService.GetInitialLazerDirection(currentSquare, entryOrientation);

            do
            {
                currentSquare = GetNextSquare(maze, currentSquare, ref laserDirection);
                var orientation = GetOrientation(laserDirection);

                if (currentSquare != null)
                    path.AddMazeSquareToPath(currentSquare, orientation);

            } while (currentSquare != null);

            return new Results
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

            return nextSquare ?? null;
        }

        private Position GetNextPositionWhenMirrorPresent(MazeSquare currentMazeSquare, ref LazerDirection lazerDirection)
        {
            if (currentMazeSquare.Mirror.Type == MirrorType.OneSided)
            {
                return GetPositionWhenOnesidedMirror(currentMazeSquare, ref lazerDirection);
            }

            else if (currentMazeSquare.Mirror.Type == MirrorType.TwoSided)
            {
                _lazerService.ChangeLazerDirection(currentMazeSquare.Mirror.Direction, ref lazerDirection);

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
                _lazerService.ChangeLazerDirection(currentMazeSquare.Mirror.Direction, ref lazerDirection);
                return GetNextPosition(currentMazeSquare, lazerDirection);
            }

            if (currentMazeSquare.Mirror.ReflectiveSide == MirrorReflectiveSide.Left && (lazerDirection == LazerDirection.South || lazerDirection == LazerDirection.East))
            {
                _lazerService.ChangeLazerDirection(currentMazeSquare.Mirror.Direction, ref lazerDirection);
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
                _lazerService.ChangeLazerDirection(currentMazeSquare.Mirror.Direction, ref lazerDirection);
                return GetNextPosition(currentMazeSquare, lazerDirection);
            }

            if (currentMazeSquare.Mirror.ReflectiveSide == MirrorReflectiveSide.Left && (lazerDirection == LazerDirection.North || lazerDirection == LazerDirection.East))
            {
                _lazerService.ChangeLazerDirection(currentMazeSquare.Mirror.Direction, ref lazerDirection);
                return GetNextPosition(currentMazeSquare, lazerDirection);
            }

            return null;
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

        private bool MazeSquareHasMirror(MazeSquare square)
        {
            return square.Mirror != null;
        }

        private Position GetLazerEntryCoordinates(string lazerEntryRoom)
        {
            var xPositionAsString = lazerEntryRoom.Split(',')[0];

            var xPosition = int.Parse(xPositionAsString);

            var yPosition = _lazerService.GetLazerYPosition(lazerEntryRoom);

            return new Position
            {
                X = xPosition,
                Y = yPosition
            };
        }
    }
}
