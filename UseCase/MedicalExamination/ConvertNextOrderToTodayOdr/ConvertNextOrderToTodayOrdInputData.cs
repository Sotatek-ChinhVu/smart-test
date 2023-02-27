using UseCase.Core.Sync.Core;
using UseCase.NextOrder;

namespace UseCase.MedicalExamination.ConvertNextOrderToTodayOdr
{
    public class ConvertNextOrderToTodayOrdInputData : IInputData<ConvertNextOrderToTodayOrdOutputData>
    {
        public ConvertNextOrderToTodayOrdInputData(int hpId, int sinDate, long raiinNo, int userId, long ptId, List<RsvKrtOrderInfItem> rsvkrtOdrInfItems)
        {
            HpId = hpId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            UserId = userId;
            RsvkrtOrderInfItems = rsvkrtOdrInfItems;
            PtId = ptId;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int UserId { get; private set; }

        public long PtId { get; private set; }

        public List<RsvKrtOrderInfItem> RsvkrtOrderInfItems { get; private set; }
    }
}
