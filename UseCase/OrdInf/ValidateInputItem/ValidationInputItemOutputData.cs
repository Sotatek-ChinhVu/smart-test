using UseCase.Core.Sync.Core;
using static Helper.Constants.TodayOrderConst;

namespace UseCase.OrdInfs.ValidationInputItem
{
    public class ValidationInputItemOutputData : IOutputData
    {
        public ValidationInputItemOutputData(Dictionary<int, KeyValuePair<int, InputItemValidationStatus>> validations, ValidationInputItemStatus status)
        {
            Validations = validations;
            Status = status;
        }

        public Dictionary<int, KeyValuePair<int, InputItemValidationStatus>> Validations { get; private set; }
        public ValidationInputItemStatus Status { get; private set; }

    }
}
