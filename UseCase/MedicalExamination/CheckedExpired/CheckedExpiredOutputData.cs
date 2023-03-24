using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.CheckedExpired
{
    public class CheckedExpiredOutputData : IOutputData
    {
        public CheckedExpiredOutputData(CheckedExpiredStatus status, List<CheckedExpiredOutputItem> result)
        {
            Status = status;
            Result = result;
        }

        public CheckedExpiredStatus Status { get; private set; }
        public List<CheckedExpiredOutputItem> Result { get; private set; }
    }
}
