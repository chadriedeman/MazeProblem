using MazeProblem.Enums;

namespace MazeProblem.Models
{
    public class Mirror
    {
        private MirrorType _mirrorType;

        public MirrorType MirrorType { get => _mirrorType; set => _mirrorType = value; }
    }
}
