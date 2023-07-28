using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.CheckExisReceInfEdit;

namespace EmrCloudApi.Presenters.Receipt
{
    public class CheckExisReceInfEditPresenter : ICheckExisReceInfEditOutputPort
    {
        public Response<CheckExisReceInfEditResponse> Result { get; private set; } = new();

        public void Complete(CheckExisReceInfEditOutputData outputData)
        {
            Result.Data = new CheckExisReceInfEditResponse(outputData.IsExisted);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(CheckExisReceInfEditStatus status) => status switch
        {
            CheckExisReceInfEditStatus.Success => ResponseMessage.Success,
            CheckExisReceInfEditStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
