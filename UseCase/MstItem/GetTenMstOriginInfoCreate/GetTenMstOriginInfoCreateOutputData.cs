using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenMstOriginInfoCreate
{
    public class GetTenMstOriginInfoCreateOutputData : IOutputData
    {
        public GetTenMstOriginInfoCreateOutputData(GetTenMstOriginInfoCreateStatus status, string itemCd, int jihiSbt)
        {
            Status = status;
            ItemCd = itemCd;
            JihiSbt = jihiSbt;
        }

        public GetTenMstOriginInfoCreateStatus Status { get; private set; }

        public string ItemCd { get; private set; }

        public int JihiSbt { get; private set; }
    }
}
