using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetContainerMsts
{
    public class GetContainerMstsInputData : IInputData<GetContainerMstsOutputData>
    {
        public GetContainerMstsInputData(int hpId) 
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
