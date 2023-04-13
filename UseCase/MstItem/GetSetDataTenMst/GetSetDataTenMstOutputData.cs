using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetSetDataTenMst
{
    public class GetSetDataTenMstOutputData : IOutputData
    {
        public GetSetDataTenMstOutputData(GetSetDataTenMstStatus status, SetDataTenMstOriginModel setData)
        {
            Status = status;
            SetData = setData;
        }

        public GetSetDataTenMstStatus Status { get; private set; }

        public SetDataTenMstOriginModel SetData { get; private set; }
    }
}
