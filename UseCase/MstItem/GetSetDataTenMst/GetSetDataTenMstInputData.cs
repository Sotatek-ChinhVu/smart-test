using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetSetDataTenMst
{
    public class GetSetDataTenMstInputData : IInputData<GetSetDataTenMstOutputData>
    {
        public int HpId { get; private set; }

        public TenMstOriginModel ItemSelected { get; private set; }

        public int SinDate { get; private set; }
    }
}
