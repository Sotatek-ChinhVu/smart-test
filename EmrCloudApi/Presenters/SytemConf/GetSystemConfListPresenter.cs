using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemConf;
using UseCase.SystemConf.GetSystemConfList;

namespace EmrCloudApi.Presenters.SytemConf;

public class GetSystemConfListPresenter : IGetSystemConfListOutputPort
{
    public Response<GetSystemConfListResponse> Result { get; private set; } = new();

    public void Complete(GetSystemConfListOutputData output)
    {
        Result.Data = new GetSystemConfListResponse(output.SystemConfList);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetSystemConfListStatus status) => status switch
    {
        GetSystemConfListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
