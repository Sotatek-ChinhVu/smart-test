using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetMaxAuditTrailLogDateForPrint
{
    public class GetMaxAuditTrailLogDateForPrintInputData : IInputData<GetMaxAuditTrailLogDateForPrintOutputData>
    {
        public GetMaxAuditTrailLogDateForPrintInputData(long ptId, int sinDate, long raiinNo)
        {
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
        }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }
    }
}
