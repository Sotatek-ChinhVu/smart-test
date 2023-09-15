using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetMaterialMsts
{
    public class GetMaterialMstsInputData : IInputData<GetMaterialMstsOutputData>
    {
        public GetMaterialMstsInputData(int hpId) 
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
