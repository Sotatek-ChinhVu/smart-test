using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.SaveAddressMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class SaveAddressMstPresenter : ISaveAddressMstOutputPort
    {
        public Response<SaveAddressMstResponse> Result { get; private set; } = new Response<SaveAddressMstResponse>();

        private string GetMessage(SaveAddressMstStatus status) => status switch
        {
            SaveAddressMstStatus.Success => ResponseMessage.Success,
            SaveAddressMstStatus.Error => ResponseMessage.Error,
            SaveAddressMstStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };

        public void Complete(SaveAddressMstOutputData outputData)
        {
            Result.Data = new SaveAddressMstResponse(outputData.Id, outputData.PostCd, outputData.ErrorMessage, outputData.Status);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
    }
}
