using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SmartKartePort;
using UseCase.SmartKartePort.UpdatePort;

namespace EmrCloudApi.Presenters.SmartKartePort
{
    public class UpdatePortPresenter : IUpdatePortOutputPort
    {
        public Response<UpdatePortResponse> Result { get; private set; } = new Response<UpdatePortResponse>();

        public void Complete(UpdatePortOutputData output)
        {
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(UpdatePortStatus status) => status switch
        {
            UpdatePortStatus.Success => ResponseMessage.Success,
            UpdatePortStatus.Faild => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
