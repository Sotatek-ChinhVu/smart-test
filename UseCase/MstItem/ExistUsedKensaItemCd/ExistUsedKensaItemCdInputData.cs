using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.ExistUsedKensaItemCd
{
    public class ExistUsedKensaItemCdInputData : IInputData<ExistUsedKensaItemCdOutputData>
    {
        public ExistUsedKensaItemCdInputData(int hpId, string kensaItemCd, int kensaSeqNo)
        {
            HpId = hpId;
            KensaItemCd = kensaItemCd;
            KensaSeqNo = kensaSeqNo;
        }

        public int HpId { get; private set; }

        public string KensaItemCd { get; private set; }

        public int KensaSeqNo { get; private set; }
    }
}
