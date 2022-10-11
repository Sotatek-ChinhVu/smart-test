using UseCase.Core.Sync.Core;
using static Helper.Constants.OrderInfConst;

namespace UseCase.OrdInfs.ValidationInputItem
{
    public class ValidationInputItemOutputData : IOutputData
    {
        public ValidationInputItemOutputData(Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> validations, ValidationInputItemStatus status)
        {
            Validations = validations;
            Status = status;
        }

        public Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> Validations { get; private set; }
        public ValidationInputItemStatus Status { get; private set; }

    }
}
