using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SmartKartePort;
using UseCase.SmartKartePort.GetPort;

namespace EmrCloudApi.Presenters.SmartKartePort
{
    public class GetPortPresenter
    {
        public Response<GetPortResponse> Result { get; private set; } = new();

        public void Complete(GetPortOutputData output)
        {
            Result.Data = new GetPortResponse(output.SignalRPort);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetPortStatus status) => status switch
        {
            GetPortStatus.Success => ResponseMessage.Success,
            GetPortStatus.Faild => ResponseMessage.Failed,
            GetPortStatus.Nodata => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
