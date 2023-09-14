using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetUsedKensaItemCds
{
    public class GetUsedKensaItemCdsInputData : IInputData<GetUsedKensaItemCdsOutputData>
    {
        public GetUsedKensaItemCdsInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
