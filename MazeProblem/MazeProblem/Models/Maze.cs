namespace MazeProblem.Models
{
    public class Maze
    {
        #region "Member Variables"
        private int _height;
        private int _width;
        #endregion

        #region "Properties"
        public int Height 
        { 
            get => _height;
            set { _height = value >= 0 ? value : 0; }
        }

        public int Width 
        { 
            get => _width;
            set { _width = value >= 0 ? value : 0; }
        }
        #endregion
    }
}
