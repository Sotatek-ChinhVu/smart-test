using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UserConf;
using UseCase.User.GetUserConfModelList;

namespace EmrCloudApi.Presenters.UserConf;

public class GetUserConfModelListPresenter : IGetUserConfModelListOutputPort
{
    public Response<GetUserConfModelListResponse> Result { get; private set; } = new Response<GetUserConfModelListResponse>();

    public void Complete(GetUserConfModelListOutputData output)
    {
        Result.Data = new GetUserConfModelListResponse(output.UserConfs);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetUserConfModelListStatus status) => status switch
    {
        GetUserConfModelListStatus.Successed => ResponseMessage.Success,
        GetUserConfModelListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetUserConfModelListStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        GetUserConfModelListStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
