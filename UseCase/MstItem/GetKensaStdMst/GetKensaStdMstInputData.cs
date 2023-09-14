using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetKensaStdMst
{
    public class GetKensaStdMstInputData : IInputData<GetKensaStdMstOutputData>
    {
        public GetKensaStdMstInputData(int hpId, string kensaItemCd)
        {
            HpId = hpId;
            KensaItemCd = kensaItemCd;
        }

        public int HpId { get; private set; }

        public string KensaItemCd { get; private set; }
    }
}
