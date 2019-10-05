using System.Collections.Generic;

namespace MazeProblem.Models
{
    public class LazerPath
    {
        private List<LazerPathStep> _lazerPathStep;

        public List<LazerPathStep> LazerPathStep { get => _lazerPathStep; set => _lazerPathStep = value; }

        public LazerPath()
        {
            _lazerPathStep = new List<LazerPathStep>();
        }

        public void AddMazeSquareToPath(MazeSquare mazeSquare, string orientation)
        {
            _lazerPathStep.Add(new LazerPathStep {
                Position = mazeSquare.Position, 
                Orientation = orientation
            });
        }
    }
}
