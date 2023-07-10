using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.GetCheckedOrder;

namespace UseCase.MedicalExamination.TrailAccounting
{
    public class GetTrialAccountingInputData : IInputData<GetTrialAccountingOutputData>
    {
        public GetTrialAccountingInputData(int hpId, int userId, long ptId, int sinDate, long raiinNo, List<OdrInfItem> odrInfItems)
        {
            HpId = hpId;
            UserId = userId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            OdrInfItems = odrInfItems;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public long RaiinNo { get; private set; }
        public List<OdrInfItem> OdrInfItems { get; private set; }

    }
}
