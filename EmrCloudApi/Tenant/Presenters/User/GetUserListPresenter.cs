using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.User;
using UseCase.User.GetList;

namespace EmrCloudApi.Tenant.Presenters.User;

public class GetUserListPresenter : IGetUserListOutputPort
{
    public Response<GetUserListResponse> Result { get; private set; } = new Response<GetUserListResponse>();
    
    public void Complete(GetUserListOutputData output)
    {
        Result.Data = new GetUserListResponse(output.Users);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetUserListStatus status) => status switch
    {
        GetUserListStatus.Success => ResponseMessage.Success,
        GetUserListStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
        _ => string.Empty
    };
}
