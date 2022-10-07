using UseCase.Core.Sync.Core;
using static Helper.Constants.RaiinInfConst;
using static Helper.Constants.TodayKarteConst;
using static Helper.Constants.TodayOrderConst;

namespace UseCase.MedicalExamination.UpsertTodayOrd
{
    public class UpsertTodayOrdOutputData : IOutputData
    {
        public UpsertTodayOrdOutputData(UpsertTodayOrdStatus status, RaiinInfTodayOdrValidationStatus validationRaiinInf, Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>> validationOdrs, Dictionary<int, TodayKarteValidationStatus> validationKartes)
        {
            Status = status;
            ValidationRaiinInf = validationRaiinInf;
            ValidationOdrs = validationOdrs;
            ValidationKartes = validationKartes;
        }

        public UpsertTodayOrdStatus Status { get; private set; }

        public RaiinInfTodayOdrValidationStatus ValidationRaiinInf { get; private set; }
        public Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>> ValidationOdrs { get; private set; }
        public Dictionary<int, TodayKarteValidationStatus> ValidationKartes { get; private set; }

    }
}
