namespace MazeProblem.Models
{
    public class LazerPathStep
    {
        private Position _position;
        private string _orientation;

        public Position Position { get => _position; set => _position = value; }
        public string Orientation { get => _orientation; set => _orientation = value; }
    }
}
