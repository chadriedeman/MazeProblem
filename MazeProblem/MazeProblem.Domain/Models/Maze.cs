using System.Collections.Generic;

namespace MazeProblem.Domain.Models
{
    public class Maze
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public List<MazeSquare> MazeSquares { get; set; }
    }
}
