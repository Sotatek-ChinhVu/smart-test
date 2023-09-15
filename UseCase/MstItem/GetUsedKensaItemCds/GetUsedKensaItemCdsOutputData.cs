using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetUsedKensaItemCds
{
    public class GetUsedKensaItemCdsOutputData : IOutputData
    {
        public GetUsedKensaItemCdsOutputData(List<string> kensaItemCd, GetUsedKensaItemCdsStatus status)
        {
            KensaItemCd = kensaItemCd;
            Status = status;
        }

        public List<string> KensaItemCd { get; private set; }

        public GetUsedKensaItemCdsStatus Status { get; private set; }
    }
}
