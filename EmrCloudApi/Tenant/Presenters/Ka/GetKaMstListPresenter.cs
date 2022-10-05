using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Ka;
using UseCase.Ka.GetList;

namespace EmrCloudApi.Tenant.Presenters.Ka;

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
