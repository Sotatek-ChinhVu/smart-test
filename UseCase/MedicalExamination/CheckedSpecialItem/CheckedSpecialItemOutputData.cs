using Domain.Models.TodayOdr;
using UseCase.Core.Sync.Core;
using static Helper.Constants.KarteConst;
using static Helper.Constants.OrderInfConst;
using static Helper.Constants.RaiinInfConst;

namespace UseCase.OrdInfs.CheckedSpecialItem
{
    public class CheckedSpecialItemOutputData : IOutputData
    {
        public CheckedSpecialItemOutputData(List<CheckedSpecialItemModel> checkSpecialItemModels, CheckedSpecialItemStatus status)
        {
            Status = status;
            CheckSpecialItemModels = checkSpecialItemModels;
        }

        public CheckedSpecialItemStatus Status { get; private set; }
        public List<CheckedSpecialItemModel> CheckSpecialItemModels { get; private set; }
    }
}
