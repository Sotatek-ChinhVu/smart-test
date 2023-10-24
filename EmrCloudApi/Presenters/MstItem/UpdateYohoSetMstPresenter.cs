using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.UpdateYohoSetMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class UpdateYohoSetMstPresenter : IUpdateYohoSetMstOutputPort
    {
        public Response<UpdateYohoSetMstResponse> Result { get; private set; } = default!;
        public void Complete(UpdateYohoSetMstOutputData outputData)
        {
            Result = new Response<UpdateYohoSetMstResponse>()
            {
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }

        private static string GetMessage(UpdateYohoSetMstStatus status) => status switch
        {
            UpdateYohoSetMstStatus.Successed => ResponseMessage.Success,
            UpdateYohoSetMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            UpdateYohoSetMstStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
