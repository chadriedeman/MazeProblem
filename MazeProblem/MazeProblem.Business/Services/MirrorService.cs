using MazeProblem.Domain.Enums;
using MazeProblem.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace MazeProblem.Business.Services
{
    public class MirrorService : IMirrorService
    {
        public void AddMirrorsToMaze(ref Maze maze, List<string> mirrorPlacements)
        {
            if (!mirrorPlacements.Any())
                return;

            foreach (var mirrorPlacement in mirrorPlacements)
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

        public int GetMirrorYPosition(string mirrorPlacement)
        {
            var startIndex = mirrorPlacement.IndexOf(',') + 1;

            var yPosition = string.Empty;

            for (int i = startIndex; (i < mirrorPlacement.Length) && (char.ToUpper(mirrorPlacement[i]) != 'L' && char.ToUpper(mirrorPlacement[i]) != 'R'); i++)
            {
                yPosition += mirrorPlacement[i];
            }

            return int.Parse(yPosition);
        }

        public MirrorDirection GetMirrorDirection(string mirrorPlacement, int yPosition)
        {
            var indexOfYPosition = mirrorPlacement.LastIndexOf(yPosition.ToString());

            var mirrorDirectionAndReflectiveSide = mirrorPlacement.Substring(indexOfYPosition + yPosition.ToString().Length);

            var direction = mirrorDirectionAndReflectiveSide[0];

            return direction == 'L' ? MirrorDirection.Left : MirrorDirection.Right;
        }

        public MirrorReflectiveSide GetReflectiveSide(string mirrorPlacement, int yPosition)
        {
            var indexOfYPosition = mirrorPlacement.LastIndexOf(yPosition.ToString());

            var mirrorDirectionAndReflectiveSide = mirrorPlacement.Substring(indexOfYPosition + yPosition.ToString().Length);

            if (mirrorDirectionAndReflectiveSide.Length == 1)
                return MirrorReflectiveSide.Both;
            else
            {
                var reflectiveSide = mirrorDirectionAndReflectiveSide[1];

                return reflectiveSide switch
                {
                    'L' => MirrorReflectiveSide.Left,
                    'R' => MirrorReflectiveSide.Right,
                    _ => MirrorReflectiveSide.Unspecified,
                };
            }
        }
    }
}
