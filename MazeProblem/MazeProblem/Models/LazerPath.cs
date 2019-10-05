using System.Collections.Generic;

namespace MazeProblem.Models
{
    public class LazerPath
    {
        #region "Member Variables"
        private List<LazerPathStep> _lazerPathStep;
        #endregion

        #region "Properties
        public List<LazerPathStep> LazerPathStep { get => _lazerPathStep; set => _lazerPathStep = value; }
        #endregion

        #region "Constructors
        public LazerPath()
        {
            _lazerPathStep = new List<LazerPathStep>();
        }
        #endregion

        #region "Methods"
        public void AddMazeSquareToPath(MazeSquare mazeSquare, string orientation)
        {
            _lazerPathStep.Add(new LazerPathStep {
                Position = mazeSquare.Position, 
                Orientation = orientation
            });
        }
        #endregion
    }
}
