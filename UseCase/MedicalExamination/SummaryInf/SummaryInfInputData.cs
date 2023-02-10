using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.SummaryInf
{
    public class SummaryInfInputData : IInputData<SummaryInfOutputData>
    {
        public SummaryInfInputData(int hpId, long ptId, int sinDate, long raiinNo)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }
    }
}
