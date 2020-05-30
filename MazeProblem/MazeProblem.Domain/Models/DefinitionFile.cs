using System.Collections.Generic;

namespace MazeProblem.Domain.Models
{
    public class DefinitionFile
    {
        public string BoardSize { get; set; }
        public List<string> MirrorPlacements { get; set; }
        public string LazerEntryRoom { get; set; }
    }
}
