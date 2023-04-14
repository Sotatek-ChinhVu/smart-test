using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.GetKensaAuditTrailLog
{
    public class GetKensaAuditTrailLogInputData : IInputData<GetKensaAuditTrailLogOutputData>
    {
        public GetKensaAuditTrailLogInputData(int hpId, long raiinNo, int sinDate, long ptId, string eventCd)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            PtId = ptId;
            EventCd = eventCd;
        }

        public int HpId { get; private set; }

        public long RaiinNo { get; private set; }

        public int SinDate { get; private set; }

        public long PtId { get; private set; }

        public string EventCd { get; private set; }
    }
}
