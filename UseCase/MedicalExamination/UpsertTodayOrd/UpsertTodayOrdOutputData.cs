using UseCase.Core.Sync.Core;
using static Helper.Constants.RaiinInfConst;
using static Helper.Constants.TodayKarteConst;
using static Helper.Constants.OrderInfConst;

namespace UseCase.MedicalExamination.UpsertTodayOrd
{
    public class UpsertTodayOrdOutputData : IOutputData
    {
        public UpsertTodayOrdOutputData(UpsertTodayOrdStatus status, RaiinInfTodayOdrValidationStatus validationRaiinInf, Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> validationOdrs, Dictionary<int, TodayKarteValidationStatus> validationKartes)
        {
            Status = status;
            ValidationRaiinInf = validationRaiinInf;
            ValidationOdrs = validationOdrs;
            ValidationKartes = validationKartes;
        }

        public UpsertTodayOrdStatus Status { get; private set; }

        public RaiinInfTodayOdrValidationStatus ValidationRaiinInf { get; private set; }
        public Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> ValidationOdrs { get; private set; }
        public Dictionary<int, TodayKarteValidationStatus> ValidationKartes { get; private set; }

    }
}
