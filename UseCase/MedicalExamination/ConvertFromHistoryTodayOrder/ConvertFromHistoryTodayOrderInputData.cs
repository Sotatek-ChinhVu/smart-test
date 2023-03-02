using Domain.Models.OrdInfs;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.ConvertFromHistoryTodayOrder
{
    public class ConvertFromHistoryTodayOrderInputData : IInputData<ConvertFromHistoryTodayOrderOutputData>
    {
        public ConvertFromHistoryTodayOrderInputData(int hpId, int sinDate, long raiinNo, long userId, long ptId, List<OrdInfModel> historyOdrInfModels)
        {
            HpId = hpId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            UserId = userId;
            PtId = ptId;
            HistoryOdrInfModels = historyOdrInfModels;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public long UserId { get; private set; }

        public long PtId { get; private set; }

        public List<OrdInfModel> HistoryOdrInfModels { get; private set; }
    }
}
