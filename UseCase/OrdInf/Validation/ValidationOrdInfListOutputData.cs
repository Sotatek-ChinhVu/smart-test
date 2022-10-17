using UseCase.Core.Sync.Core;
using static Helper.Constants.OrderInfConst;

namespace UseCase.OrdInfs.Validation
{
    public class ValidationOrdInfListOutputData : IOutputData
    {
        public ValidationOrdInfListOutputData(Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> validations, ValidationOrdInfListStatus status)
        {
            Validations = validations;
            Status = status;
        }

        public Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> Validations { get; private set; }
        public ValidationOrdInfListStatus Status { get; private set; }

    }
}
