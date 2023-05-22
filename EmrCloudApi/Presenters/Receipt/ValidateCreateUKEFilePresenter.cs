using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Receipt;
using EmrCloudApi.Responses;
using UseCase.Receipt.ValidateCreateUKEFile;

namespace EmrCloudApi.Presenters.Receipt
{
    public class ValidateCreateUKEFilePresenter : IValidateCreateUKEFileOutputPort
    {
        public Response<ValidateCreateUKEFileResponse> Result { get; private set; } = new Response<ValidateCreateUKEFileResponse>();

        public void Complete(ValidateCreateUKEFileOutputData outputData)
        {
            Result.Data = new ValidateCreateUKEFileResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = string.IsNullOrEmpty(outputData.Message) ? GetMessage(outputData.Status) : outputData.Message;
        }

        private string GetMessage(ValidateCreateUKEFileStatus status) => status switch
        {
            ValidateCreateUKEFileStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            ValidateCreateUKEFileStatus.InvaliSeikyuYm => ResponseMessage.InvalidSeikyuYm,
            ValidateCreateUKEFileStatus.Successful => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
