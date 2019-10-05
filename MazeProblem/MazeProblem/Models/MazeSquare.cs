namespace MazeProblem.Models
{
    public class MazeSquare
    {
        private Position _position;

        private bool _hasNorthPerimeterDoor;
        private bool _hasSouthPerimeterDoor;
        private bool _hasEastPerimeterDoor;
        private bool _hasWestPerimeterDoor;
        private Mirror _mirror;

        public Position Position { get => _position; set => _position = value; }
        public bool HasNorthPerimeterDoor { get => _hasNorthPerimeterDoor; set => _hasNorthPerimeterDoor = value; }
        public bool HasSouthPerimeterDoor { get => _hasSouthPerimeterDoor; set => _hasSouthPerimeterDoor = value; }
        public bool HasEastPerimeterDoor { get => _hasEastPerimeterDoor; set => _hasEastPerimeterDoor = value; }
        public bool HasWestPerimeterDoor { get => _hasWestPerimeterDoor; set => _hasWestPerimeterDoor = value; }
        public Mirror Mirror { get => _mirror; set => _mirror = value; }
    }
}
