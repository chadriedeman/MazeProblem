using System.Collections.Generic;

namespace MazeProblem.Models
{
    public class DefinitionFile
    {
        #region "Member Variables"
        private string _boardSize;
        private List<string> _mirrorPlacements;
        private string _lazerEntryRoom;
        #endregion

        #region "Properties"
        public string BoardSize { get => _boardSize; set => _boardSize = value; }
        public List<string> MirrorPlacements { get => _mirrorPlacements; set => _mirrorPlacements = value; }
        public string LazerEntryRoom { get => _lazerEntryRoom; set => _lazerEntryRoom = value; }
        #endregion
    }
}
