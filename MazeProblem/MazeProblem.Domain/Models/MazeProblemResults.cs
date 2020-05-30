namespace MazeProblem.Domain.Models
{
    public class MazeProblemResults
    {
        public string BoardSize { get; set; }
        public string LazerEntryPositionAndOrientation { get; set; }
        public string ExitPostionAndOrientation { get; set; }
        public LazerPath LazerPath { get; set; }
    }
}
