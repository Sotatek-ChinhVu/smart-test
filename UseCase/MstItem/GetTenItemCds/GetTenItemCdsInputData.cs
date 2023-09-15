using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenItemCds
{
    public class GetTenItemCdsInputData : IInputData<GetTenItemCdsOutputData>
    {
        public GetTenItemCdsInputData(int hpId) 
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
