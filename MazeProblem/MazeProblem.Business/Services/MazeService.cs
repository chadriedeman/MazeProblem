using MazeProblem.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace MazeProblem.Business.Services
{
    public class MazeService : IMazeService
    {
        private readonly IMirrorService _mirrorService;

        public MazeService(IMirrorService mirrorService)
        {
            _mirrorService = mirrorService;
        }

        public Maze CreateMaze(DefinitionFile definitionFile)
        {
            var maze = new Maze
            {
                MazeSquares = new List<MazeSquare>()
            };

            SetMazeWidthAndHeight(ref maze, definitionFile.BoardSize);

            AddMazeSquaresToMaze(ref maze);

            IdentifyPerimeterDoors(ref maze);

            _mirrorService.AddMirrorsToMaze(ref maze, definitionFile.MirrorPlacements);

            return maze;
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
    }
}
