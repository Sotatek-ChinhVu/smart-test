using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetSendaiGeneration;
using UseCase.SetSendaiGeneration.Add;

namespace EmrCloudApi.Presenters.SetSendaiGeneration
{
    public class AddSetSendaiGenerationPresenter : IAddSetSendaiGenerationOutputPort
    {
        public Response<AddSetSendaiGenerationResponse> Result { get; private set; } = new();

        public void Complete(AddSetSendaiGenerationOutputData output)
        {
            Result.Data = new AddSetSendaiGenerationResponse(output.Result);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(AddSetSendaiGenerationStatus status) => status switch
        {
            AddSetSendaiGenerationStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            AddSetSendaiGenerationStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            AddSetSendaiGenerationStatus.InvalidStartDate => ResponseMessage.InvalidStartDate,
            AddSetSendaiGenerationStatus.Faild => ResponseMessage.Failed,
            AddSetSendaiGenerationStatus.Success => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
