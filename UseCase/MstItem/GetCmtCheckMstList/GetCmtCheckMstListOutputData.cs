using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetCmtCheckMstList
{
    public class GetCmtCheckMstListOutputData : IOutputData
    {
        public GetCmtCheckMstListOutputData(List<ItemCmtModel> itemCmtModels, GetCmtCheckMstListStatus status)
        {
            ItemCmtModels = itemCmtModels;
            Status = status;
        }

        public List<ItemCmtModel> ItemCmtModels { get; private set; }
        public GetCmtCheckMstListStatus Status { get; private set; }
    }
}
