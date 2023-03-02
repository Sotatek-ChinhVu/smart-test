using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.ConvertFromHistoryTodayOrder
{
    public class ConvertFromHistoryTodayOrderOutputData : IOutputData
    {
        public ConvertFromHistoryTodayOrderOutputData(ConvertFromHistoryTodayOrderStatus status, Dictionary<string, string> checkedItemNames)
        {
            Status = status;
            CheckedItemNames = checkedItemNames;
        }

        public ConvertFromHistoryTodayOrderStatus Status { get; private set; }
        public Dictionary<string, string> CheckedItemNames { get; private set; }
    }
}
