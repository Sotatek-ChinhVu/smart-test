using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.GetListKensaSet
{
    public class GetListKensaSetInputData : IInputData<GetListKensaSetOutputData>
    {
        public GetListKensaSetInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
