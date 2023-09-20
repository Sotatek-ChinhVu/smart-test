using UseCase.MstItem.IsUsingKensa;

namespace EmrCloudApi.Responses.MstItem
{
    public class IsUsingKensaResponse
    {
        public IsUsingKensaResponse(IsUsingKensaStatus datas)
        {
            Status = datas;
        }

        public IsUsingKensaStatus Status { get; private set; }
    }
}
