using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListTenMstOrigin
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
