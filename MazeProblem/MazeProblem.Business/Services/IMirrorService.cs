using MazeProblem.Domain.Enums;
using MazeProblem.Domain.Models;
using System.Collections.Generic;

namespace MazeProblem.Business.Services
{
    public interface IMirrorService
    {
        void AddMirrorsToMaze(ref Maze maze, List<string> mirrorPlacements);
        int GetMirrorYPosition(string mirrorPlacement);
        MirrorDirection GetMirrorDirection(string mirrorPlacement, int yPosition);
        MirrorReflectiveSide GetReflectiveSide(string mirrorPlacement, int yPosition);
    }
}
