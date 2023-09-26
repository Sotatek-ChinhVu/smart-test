using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.IsUsingKensa;

namespace EmrCloudApi.Presenters.MstItem
{
    public class IsUsingKensaPresenter : IIsUsingKensaOutputPort
    {
        public Response<IsUsingKensaResponse> Result { get; private set; } = default!;

        public void Complete(IsUsingKensaOutputData outputData)
        {
            Result = new Response<IsUsingKensaResponse>()
            {
                Data = new IsUsingKensaResponse(outputData.Status),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case IsUsingKensaStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
                case IsUsingKensaStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
