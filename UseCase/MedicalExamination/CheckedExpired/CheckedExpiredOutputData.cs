using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.CheckedExpired
{
    public class CheckedExpiredOutputData : IOutputData
    {
        public CheckedExpiredOutputData(CheckedExpiredStatus status, Dictionary<string, (string, List<TenItemModel>)> result)
        {
            Status = status;
            Result = result;
        }

        public CheckedExpiredStatus Status { get; private set; }
        public Dictionary<string, (string, List<TenItemModel>)> Result { get; private set; }
    }
}
