using MazeProblem.Domain.Enums;
using MazeProblem.Domain.Models;

namespace MazeProblem.Business.Services
{
    public class LazerService : ILazerService
    {
        public void ChangeLazerDirection(MirrorDirection mirrorDirection, ref LazerDirection lazerDirection)
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

        public LazerDirection GetInitialLazerDirection(MazeSquare mazeSquare, string orientation)
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

        public int GetLazerYPosition(string laserEntryRoom)
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
