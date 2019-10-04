namespace MazeProblem.Models
{
    public class MazeSquare
    {
        private Position _position;

        private bool _hasNorthDoor;
        private bool _hasSouthDoor;
        private bool _hasEastDoor;
        private bool _hasWestDoor;

        private Mirror _mirror;

        public Position Position { get => _position; set => _position = value; }
        public bool HasNorthDoor { get => _hasNorthDoor; set => _hasNorthDoor = value; }
        public bool HasSouthDoor { get => _hasSouthDoor; set => _hasSouthDoor = value; }
        public bool HasEastDoor { get => _hasEastDoor; set => _hasEastDoor = value; }
        public bool HasWestDoor { get => _hasWestDoor; set => _hasWestDoor = value; }
        public Mirror Mirror { get => _mirror; set => _mirror = value; }
    }
}
