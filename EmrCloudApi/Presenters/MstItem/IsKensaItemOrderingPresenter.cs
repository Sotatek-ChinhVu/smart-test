using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.IsKensaItemOrdering;

namespace EmrCloudApi.Presenters.MstItem
{
    public class IsKensaItemOrderingPresenter : IIsKensaItemOrderingOutputPort
    {
        public Response<IsKensaItemOrderingResponse> Result { get; private set; } = default!;

        public void Complete(IsKensaItemOrderingOutputData outputData)
        {
            Result = new Response<IsKensaItemOrderingResponse>()
            {
                Data = new IsKensaItemOrderingResponse(outputData.Status == IsKensaItemOrderingStatus.Success),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }

        private static string GetMessage(IsKensaItemOrderingStatus status) => status switch
        {
            IsKensaItemOrderingStatus.Success => ResponseMessage.Success,
            IsKensaItemOrderingStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
