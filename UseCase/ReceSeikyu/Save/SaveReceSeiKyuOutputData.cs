using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.Save
{
    public class SaveReceSeiKyuOutputData : IOutputData
    {
        public SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus status)
        {
            Status = status;
        }

        public SaveReceSeiKyuStatus Status { get; private set; }
    }
}
