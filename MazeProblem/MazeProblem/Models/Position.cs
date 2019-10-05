namespace MazeProblem.Models
{
    public class Position
    {
        #region "Member Variables"
        private int _x;
        private int _y;
        #endregion

        #region "Properties"
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        #endregion
    }
}
