using UseCase.Core.Sync.Core;
using static Helper.Constants.TodayOrderConst;

namespace UseCase.OrdInfs.ValidationInputItem
{
    public class ValidationInputItemOutputData : IOutputData
    {
        public ValidationInputItemOutputData(Dictionary<string, KeyValuePair<string, TodayOrdValidationStatus>> validations, ValidationInputItemStatus status)
        {
            Validations = validations;
            Status = status;
        }

        public Dictionary<string, KeyValuePair<string, TodayOrdValidationStatus>> Validations { get; private set; }
        public ValidationInputItemStatus Status { get; private set; }

    }
}
