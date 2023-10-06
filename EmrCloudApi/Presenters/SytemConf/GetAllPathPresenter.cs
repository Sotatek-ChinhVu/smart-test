using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemConf;
using UseCase.SystemConf.GetPathAll;

namespace EmrCloudApi.Presenters.SytemConf;

public class GetAllPathPresenter : IGetPathAllOutputPort
{
    public Response<GetAllPathResponse> Result { get; private set; } = new();

    public void Complete(GetPathAllOutputData outputData)
    {
        Result.Data = new GetAllPathResponse(outputData.SystemConfListXmlPathModels);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetPathAllStatus status) => status switch
    {
        GetPathAllStatus.Successed => ResponseMessage.Success,
        GetPathAllStatus.NoData => ResponseMessage.NoData,
        _ => string.Empty
    };
}

