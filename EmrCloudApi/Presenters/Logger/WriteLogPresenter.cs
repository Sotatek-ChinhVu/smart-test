using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Logger;
using UseCase.Logger;

namespace EmrCloudApi.Presenters.Logger
{
    public class WriteLogPresenter : IWriteLogOutputPort
    {
        public Response<WriteLogResponse> Result { get; private set; } = new();

        public void Complete(WriteLogOutputData output)
        {
            Result.Data = new WriteLogResponse(output.Status);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(WriteLogStatus status) => status switch
        {
            WriteLogStatus.Successed => ResponseMessage.Success,
            WriteLogStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
