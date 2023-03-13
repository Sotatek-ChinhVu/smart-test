using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.Save
{
    public class SaveReceSeiKyuOutputData : IOutputData
    {
        public SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public SaveReceSeiKyuStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
