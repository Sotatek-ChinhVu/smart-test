using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemConf;
using UseCase.SystemConf.GetSystemConfForPrint;

namespace EmrCloudApi.Presenters.SytemConf
{
    public class GetSystemConfForPrintPresenter : IGetSystemConfForPrintOutputPort
    {
        public Response<GetSystemConfForPrintResponse> Result { get; private set; } = default!;

        public void Complete(GetSystemConfForPrintOutputData outputData)
        {
            Result = new Response<GetSystemConfForPrintResponse>()
            {
                Data = new GetSystemConfForPrintResponse(outputData.Values),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSystemConfForPrintStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetSystemConfForPrintStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
