using MazeProblem.Domain.Enums;
using MazeProblem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeProblem.Business.Services
{
    public interface ILazerService
    {
        void ChangeLazerDirection(MirrorDirection mirrorDirection, ref LazerDirection lazerDirection);
        LazerDirection GetInitialLazerDirection(MazeSquare mazeSquare, string orientation);
        int GetLazerYPosition(string laserEntryRoom);

    }
}
