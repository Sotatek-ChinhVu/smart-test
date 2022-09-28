using UseCase.Core.Sync.Core;

namespace UseCase.MonshinInfor.Save
{
    public class SaveMonshinOutputData : IOutputData
    {
        public SaveMonshinOutputData(SaveMonshinStatus status)
        {
            Status = status;
        }

        public SaveMonshinStatus Status { get; private set; }
    }
}
