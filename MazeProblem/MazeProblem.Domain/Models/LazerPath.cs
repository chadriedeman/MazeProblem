using System.Collections.Generic;

namespace MazeProblem.Domain.Models
{
    public class LazerPath
    {
        public List<LazerPathStep> LazerPathStep { get; set; }

        public LazerPath()
        {
            LazerPathStep = new List<LazerPathStep>();
        }

        public void AddMazeSquareToPath(MazeSquare mazeSquare, string orientation)
        {
            LazerPathStep.Add(new LazerPathStep
            {
                Position = mazeSquare.Position,
                Orientation = orientation
            });
        }
    }
}
