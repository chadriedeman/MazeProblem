namespace MazeProblem.Models
{
    public class LazerPathStep
    {
        #region "Member Variables"
        private Position _position;
        private string _orientation;
        #endregion

        #region "Properties"
        public Position Position { get => _position; set => _position = value; }
        public string Orientation { get => _orientation; set => _orientation = value; }
        #endregion
    }
}
