using Domain.Models.TodayOdr;
using UseCase.Core.Sync.Core;
using static Helper.Constants.KarteConst;
using static Helper.Constants.OrderInfConst;
using static Helper.Constants.RaiinInfConst;

namespace UseCase.MedicalExamination.CheckedItemName
{
    public class CheckedItemNameOutputData : IOutputData
    {
        public CheckedItemNameOutputData(CheckedItemNameStatus status, Dictionary<string, string> checkedItemName)
        {
            Status = status;
            CheckedItemName = checkedItemName;
        }

        public CheckedItemNameStatus Status { get; private set; }
        public Dictionary<string, string> CheckedItemName { get; private set; }
    }
}
