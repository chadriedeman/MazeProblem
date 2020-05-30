namespace MazeProblem.Domain.Models
{
    public class MazeSquare
    {
        public Position Position { get; set; }
        public bool HasNorthPerimeterDoor { get; set; }
        public bool HasSouthPerimeterDoor { get; set; }
        public bool HasEastPerimeterDoor { get; set; }
        public bool HasWestPerimeterDoor { get; set; }
        public Mirror Mirror { get; set; }
    }
}
