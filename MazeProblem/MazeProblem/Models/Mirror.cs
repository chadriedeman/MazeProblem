using MazeProblem.Enums;

namespace MazeProblem.Models
{
    public class Mirror
    {
        #region "Member Variables"
        private MirrorType _type;
        private MirrorDirection _direction;
        private MirrorReflectiveSide _reflectiveSide;
        #endregion

        #region "Properties"
        public MirrorType Type { get => _type; set => _type = value; }
        public MirrorDirection Direction { get => _direction; set => _direction = value; }
        public MirrorReflectiveSide ReflectiveSide { get => _reflectiveSide; set => _reflectiveSide = value; }
        #endregion
    }
}
