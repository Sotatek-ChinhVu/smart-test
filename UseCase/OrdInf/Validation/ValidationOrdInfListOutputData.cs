using UseCase.Core.Sync.Core;
using static Helper.Constants.TodayOrderConst;

namespace UseCase.OrdInfs.Validation
{
    public class ValidationOrdInfListOutputData : IOutputData
    {
        public ValidationOrdInfListOutputData(Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>> validations, ValidationOrdInfListStatus status)
        {
            Validations = validations;
            Status = status;
        }

        public Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>> Validations { get; private set; }
        public ValidationOrdInfListStatus Status { get; private set; }

    }
}
