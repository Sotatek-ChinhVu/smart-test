using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.SavePath
{
    public class SavePathOutputData : IOutputData
    {
        public SavePathOutputData(SavePathStatus status)
        {
            Status = status;
        }

        public SavePathStatus Status { get; private set; }
    }
}
