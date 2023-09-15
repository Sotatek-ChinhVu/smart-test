using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenOfIGEItem
{
    public class GetTenOfIGEItemInputData : IInputData<GetTenOfIGEItemOutputData>
    {
        public GetTenOfIGEItemInputData(int hpId) 
        {
            HpId = hpId;    
        }

        public int HpId { get; private set; }
    }
}
