using UseCase.Core.Sync.Core;
using OdrInfItemOfTodayOrder = UseCase.OrdInfs.GetListTrees.OdrInfItem;

namespace UseCase.MedicalExamination.ConvertFromHistoryTodayOrder
{
    public class ConvertFromHistoryTodayOrderInputData : IInputData<ConvertFromHistoryTodayOrderOutputData>
    {
        public ConvertFromHistoryTodayOrderInputData(int hpId, int sinDate, long raiinNo, int santeiKbn, int userId, long ptId, List<OdrInfItemOfTodayOrder> historyOdrInfModels)
        {
            HpId = hpId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            SainteiKbn = santeiKbn;
            UserId = userId;
            PtId = ptId;
            HistoryOdrInfModels = historyOdrInfModels;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int SainteiKbn { get; private set; }

        public int UserId { get; private set; }

        public long PtId { get; private set; }

        public List<OdrInfItemOfTodayOrder> HistoryOdrInfModels { get; private set; }
    }
}
