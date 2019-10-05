namespace MazeProblem.Models
{
    public class MazeProblemResults
    {
        #region "Member Variables"
        private string _boardSize;
        private string _lazerEntryPositionAndOrientation;
        private string _exitPostionAndOrientation;
        private LazerPath _lazerPath;
        #endregion

        #region "Properties"
        public string BoardSize { get => _boardSize; set => _boardSize = value; }
        public string LazerEntryPositionAndOrientation { get => _lazerEntryPositionAndOrientation; set => _lazerEntryPositionAndOrientation = value; }
        public string ExitPostionAndOrientation { get => _exitPostionAndOrientation; set => _exitPostionAndOrientation = value; }
        public LazerPath LazerPath { get => _lazerPath; set => _lazerPath = value; }
        #endregion
    }
}
