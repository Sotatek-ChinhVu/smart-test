using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UserConf;
using UseCase.User.UpsertUserConfList;
using static Helper.Constants.UserConfConst;

namespace EmrCloudApi.Presenters.UserConf;

public class UpsertUserConfListPresenter : IUpsertUserConfListOutputPort
{
    public Response<UpsertUserConfListResponse> Result { get; private set; } = new Response<UpsertUserConfListResponse>();

    public void Complete(UpsertUserConfListOutputData output)
    {
        Result.Data = new UpsertUserConfListResponse(output.UserConfItemValidations.Select(v => new UserConfItemValidationResponse(v.Position, GetMessageUserConfItem(v.Status))).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpsertUserConfListStatus status) => status switch
    {
        UpsertUserConfListStatus.Successed => ResponseMessage.Success,
        UpsertUserConfListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        UpsertUserConfListStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        UpsertUserConfListStatus.InvalidUserConfs => ResponseMessage.InvalidUserConfs,
        UpsertUserConfListStatus.DuplicateUserConf => ResponseMessage.DuplicateUserConf,
        UpsertUserConfListStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };

    private string GetMessageUserConfItem(UserConfStatus status) => status switch
    {
        UserConfStatus.InvalidGrpCd => ResponseMessage.InvalidGrpCd,
        UserConfStatus.InvalidGrpItemEdaNo => ResponseMessage.InvalidGrpItemEdaNo,
        UserConfStatus.InvalidGrpItemCd => ResponseMessage.InvalidGrpItemCd,
        UserConfStatus.InvalidVal => ResponseMessage.InvalidValue,
        UserConfStatus.InvalidParam => ResponseMessage.InvalidParam,
        _ => string.Empty
    };
}
