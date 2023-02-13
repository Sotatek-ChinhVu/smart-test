using UseCase.Core.Sync.Core;
using static Helper.Constants.KarteConst;
using static Helper.Constants.OrderInfConst;
using static Helper.Constants.RaiinInfConst;

namespace UseCase.MedicalExamination.UpsertTodayOrd
{
    public class UpsertTodayOrdOutputData : IOutputData
    {
        public UpsertTodayOrdOutputData(
            UpsertTodayOrdStatus status, 
            RaiinInfTodayOdrValidationStatus validationRaiinInf, 
            Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> validationOdrs, 
            KarteValidationStatus validationKarte,
            int sinDate,
            long raiinNo,
            long ptId)
        {
            Status = status;
            ValidationRaiinInf = validationRaiinInf;
            ValidationOdrs = validationOdrs;
            ValidationKarte = validationKarte;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            PtId = ptId;
        }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public long PtId { get; private set; }

        public UpsertTodayOrdStatus Status { get; private set; }

        public RaiinInfTodayOdrValidationStatus ValidationRaiinInf { get; private set; }
        public Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> ValidationOdrs { get; private set; }
        public KarteValidationStatus ValidationKarte { get; private set; }
    }
}
