using EmrCloudApi.Constants;
using EmrCloudApi.Responses.User;
using EmrCloudApi.Responses;
using UseCase.User.GetListUserByCurrentUser;

namespace EmrCloudApi.Presenters.User
{
    public class GetListUserByCurrentUserPresenter : IGetListUserByCurrentUserOutputPort
    {
        public Response<GetListUserByCurrentUserResponse> Result { get; private set; } = new Response<GetListUserByCurrentUserResponse>();

        public void Complete(GetListUserByCurrentUserOutputData output)
        {
            Result.Data = new GetListUserByCurrentUserResponse(output.Users, output.GetShowRenkeiCd1ColumnSetting, output.ManagerKbnCurrrentUser);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetListUserByCurrentUserStatus status) => status switch
        {
            GetListUserByCurrentUserStatus.Successful => ResponseMessage.Success,
            GetListUserByCurrentUserStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetListUserByCurrentUserStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            GetListUserByCurrentUserStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
