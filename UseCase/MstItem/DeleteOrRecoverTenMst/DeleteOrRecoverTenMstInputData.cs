using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.DeleteOrRecoverTenMst
{
    public class DeleteOrRecoverTenMstInputData : IInputData<DeleteOrRecoverTenMstOutputData>
    {
        public DeleteOrRecoverTenMstInputData(List<TenMstOriginModel> tenMsts, int userId)
        {
            TenMsts = tenMsts;
            UserId = userId;
        }

        public List<TenMstOriginModel> TenMsts { get; private set; }

        public int UserId { get; set; }
    }
}
