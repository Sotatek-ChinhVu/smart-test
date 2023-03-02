using UseCase.Core.Sync.Core;
using UseCase.OrdInfs.GetListTrees;

namespace UseCase.MedicalExamination.ConvertFromHistoryTodayOrder
{
    public class ConvertFromHistoryTodayOrderOutputData : IOutputData
    {
        public ConvertFromHistoryTodayOrderOutputData(ConvertFromHistoryTodayOrderStatus status, List<OdrInfItem> odrInfItems)
        {
            Status = status;
            OdrInfItems = odrInfItems;
        }

        public ConvertFromHistoryTodayOrderStatus Status { get; private set; }
        public List<OdrInfItem> OdrInfItems { get; private set; }
    }
}
