using UseCase.MstItem.ExistUsedKensaItemCd;

namespace EmrCloudApi.Responses.MstItem
{
    public class ExistUsedKensaItemCdResponse
    {
        public ExistUsedKensaItemCdResponse(ExistUsedKensaItemCdStatus status)
        {
            Status = status;
        }

        public ExistUsedKensaItemCdStatus Status { get; private set; }
    }
}
