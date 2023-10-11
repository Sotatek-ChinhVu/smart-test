using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.GetListKensaSetDetail
{
    public class GetListKensaSetDetailInputData : IInputData<GetListKensaSetDetailOutputData>
    {
        public GetListKensaSetDetailInputData(int hpId, int setId)
        {
            HpId = hpId;
            SetId = setId;
        }

        public int HpId { get; private set; }
        public int SetId { get; private set; }
    }
}
