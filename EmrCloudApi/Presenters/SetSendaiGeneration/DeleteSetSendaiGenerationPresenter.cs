using EmrCloudApi.Constants;
using EmrCloudApi.Responses.GetSendaiGeneration;
using EmrCloudApi.Responses;
using UseCase.SetSendaiGeneration.GetList;
using UseCase.SetSendaiGeneration.Delete;
using EmrCloudApi.Responses.SetSendaiGeneration;

namespace EmrCloudApi.Presenters.SetSendaiGeneration
{
    public class DeleteSetSendaiGenerationPresenter : IDeleteSendaiGenerationOutputPort
    {
        public Response<DeleteSetSendaiGenerationResponse> Result { get; private set; } = new();

        public void Complete(DeleteSendaiGenerationOutputData output)
        {
            Result.Data = new DeleteSetSendaiGenerationResponse(output.CheckResult);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(DeleteSendaiGenerationStatus status) => status switch
        {
            DeleteSendaiGenerationStatus.InvalidRowIndex => ResponseMessage.InvalidRowIndex,
            DeleteSendaiGenerationStatus.InvalidRowIndex0 => ResponseMessage.DeleteRowIndex0,
            DeleteSendaiGenerationStatus.InvalidGenerationId => ResponseMessage.InvalidGenerationId,
            DeleteSendaiGenerationStatus.Faild => ResponseMessage.Failed,
            DeleteSendaiGenerationStatus.Success => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
