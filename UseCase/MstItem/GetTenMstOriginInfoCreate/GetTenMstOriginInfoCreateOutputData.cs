using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenMstOriginInfoCreate
{
    public class GetTenMstOriginInfoCreateOutputData : IOutputData
    {
        public GetTenMstOriginInfoCreateOutputData(GetTenMstOriginInfoCreateStatus status, string itemCd, int jihiSbt, TenMstOriginModel tenMstOriginModel)
        {
            Status = status;
            ItemCd = itemCd;
            JihiSbt = jihiSbt;
            TenMstOriginModel = tenMstOriginModel;
        }

        public GetTenMstOriginInfoCreateStatus Status { get; private set; }

        public string ItemCd { get; private set; }

        public int JihiSbt { get; private set; }

       public TenMstOriginModel TenMstOriginModel { get; private set; }
    }
}
