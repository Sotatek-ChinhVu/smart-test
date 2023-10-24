using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.ExistUsedKensaItemCd;

namespace EmrCloudApi.Presenters.MstItem
{
    public class ExistUsedKensaItemCdPresenter : IExistUsedKensaItemCdOutputPort
    {
        public Response<ExistUsedKensaItemCdResponse> Result { get; private set; } = default!;

        public void Complete(ExistUsedKensaItemCdOutputData outputData)
        {
            Result = new Response<ExistUsedKensaItemCdResponse>()
            {
                Data = new ExistUsedKensaItemCdResponse(outputData.Status),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case ExistUsedKensaItemCdStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
                case ExistUsedKensaItemCdStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
