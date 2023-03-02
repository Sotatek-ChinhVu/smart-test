using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.AutoCheckOrder;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.ChangeAfterAutoCheckOrder
{
    public class ChangeAfterAutoCheckOrderInputData : IInputData<ChangeAfterAutoCheckOrderOutputData>
    {
        public ChangeAfterAutoCheckOrderInputData(int hpId, int sinDate, int userId, long raiinNo, long ptId, List<OdrInfItemInputData> odrInfs, List<AutoCheckOrderItem> targetItems)
        {
            HpId = hpId;
            SinDate = sinDate;
            UserId = userId;
            RaiinNo = raiinNo;
            PtId = ptId;
            OdrInfs = odrInfs;
            TargetItems = targetItems;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public int UserId { get; private set; }

        public long RaiinNo { get; private set; }

        public long PtId { get; private set; }

        public List<OdrInfItemInputData> OdrInfs { get; private set; }

        public List<AutoCheckOrderItem> TargetItems { get; private set; }
    }
}
