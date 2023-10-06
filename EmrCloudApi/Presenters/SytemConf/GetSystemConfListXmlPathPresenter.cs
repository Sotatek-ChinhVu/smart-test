using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemConf;
using UseCase.SystemConf.GetXmlPath;

namespace EmrCloudApi.Presenters.SytemConf
{
    public class GetSystemConfListXmlPathPresenter : IGetSystemConfListXmlPathOutputPort
    {
        public Response<GetSystemConfListXmlPathResponse> Result { get; private set; } = new();

        public void Complete(GetSystemConfListXmlPathOutputData output)
        {
            Result.Data = new GetSystemConfListXmlPathResponse(output.SystemConfListXmlPath);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetSystemConfListXmlPathStatus status) => status switch
        {
            GetSystemConfListXmlPathStatus.Successed => ResponseMessage.Success,
            GetSystemConfListXmlPathStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}