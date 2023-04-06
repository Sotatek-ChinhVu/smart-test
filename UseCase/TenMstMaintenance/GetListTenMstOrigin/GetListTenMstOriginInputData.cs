using UseCase.Core.Sync.Core;

namespace UseCase.TenMstMaintenance.GetListTenMstOrigin
{
    public class GetListTenMstOriginInputData : IInputData<GetListTenMstOriginOutputData>
    {
        public GetListTenMstOriginInputData(string itemCd)
        {
            ItemCd = itemCd;
        }

        public string ItemCd { get; private set; }
    }
}
