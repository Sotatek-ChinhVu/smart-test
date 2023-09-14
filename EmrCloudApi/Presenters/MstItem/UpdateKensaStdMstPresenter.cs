using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.UpdateKensaStdMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class UpdateKensaStdMstPresenter : IUpdateKensaStdMstOutputPort
    {
        public Response<UpdateKensaStdMstResponse> Result { get; private set; } = default!;

        public void Complete(UpdateKensaStdMstOutputData outputData)
        {
            Result = new Response<UpdateKensaStdMstResponse>()
            {
                Data = new UpdateKensaStdMstResponse(outputData.Status == UpdateKensaStdMstStatus.Success),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }

        private static string GetMessage(UpdateKensaStdMstStatus status) => status switch
        {
            UpdateKensaStdMstStatus.Success => ResponseMessage.Success,

            _ => string.Empty
        };
    }
}
