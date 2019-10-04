using MazeProblem.Enums;

namespace MazeProblem.Models
{
    public class Mirror
    {
        private MirrorType _type;
        private MirrorDirection _direction;

        public MirrorType Type { get => _type; set => _type = value; }
        public MirrorDirection Direction { get => _direction; set => _direction = value; }

        // TODO: Add reflective side
    }
}
