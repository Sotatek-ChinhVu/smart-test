using UseCase.Core.Sync.Core;
using static Helper.Constants.TodayOrderConst;

namespace UseCase.OrdInfs.Validation
{
    public class ValidationOrdInfListOutputData : IOutputData
    {
        public ValidationOrdInfListOutputData(Dictionary<string, KeyValuePair<string, TodayOrdValidationStatus>> validations, ValidationOrdInfListStatus status)
        {
            Validations = validations;
            Status = status;
        }

        public Dictionary<string, KeyValuePair<string, TodayOrdValidationStatus>> Validations { get; private set; }
        public ValidationOrdInfListStatus Status { get; private set; }

    }
}
