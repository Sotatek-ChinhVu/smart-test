using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetKensaCenterMsts
{
    public class GetKensaCenterMstsInputData : IInputData<GetKensaCenterMstsOutputData>
    {
        public GetKensaCenterMstsInputData(int hpId) 
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
