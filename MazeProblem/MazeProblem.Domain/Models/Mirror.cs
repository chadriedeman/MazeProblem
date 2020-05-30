using MazeProblem.Domain.Enums;

namespace MazeProblem.Domain.Models
{
    public class Mirror
    {
        public MirrorType Type { get; set; }
        public MirrorDirection Direction { get; set; }
        public MirrorReflectiveSide ReflectiveSide { get; set; }
    }
}
