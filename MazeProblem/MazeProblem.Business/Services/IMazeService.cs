using MazeProblem.Domain.Models;

namespace MazeProblem.Business.Services
{
    public interface IMazeService
    {
        Maze CreateMaze(DefinitionFile definitionFile);
    }
}
