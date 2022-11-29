using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Ka;
using UseCase.Ka.GetList;

namespace EmrCloudApi.Presenters.Ka;

public class GetKaMstListPresenter : IGetKaMstListOutputPort
{
    public Response<GetKaMstListResponse> Result { get; private set; } = new Response<GetKaMstListResponse>();
    
    public void Complete(GetKaMstListOutputData output)
    {
        Result.Data = new GetKaMstListResponse(output.Departments);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetKaMstListStatus status) => status switch
    {
        GetKaMstListStatus.Success => ResponseMessage.Success,
        GetKaMstListStatus.NoData => ResponseMessage.NoData,
        _ => string.Empty
    };
}
