using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UserConf;
using UseCase.User.GetUserConfList;

namespace EmrCloudApi.Presenters.UserConf;

public class GetUserConfListPresenter : IGetUserConfListOutputPort
{
    public Response<GetUserConfListResponse> Result { get; private set; } = new Response<GetUserConfListResponse>();

    public void Complete(GetUserConfListOutputData output)
    {
        Result.Data = new GetUserConfListResponse(output.UserConfs.Select(u => new GetUserConfItemResponse(u.Key, u.Value)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetUserConfListStatus status) => status switch
    {
        GetUserConfListStatus.Successed => ResponseMessage.Success,
        GetUserConfListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetUserConfListStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        GetUserConfListStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
