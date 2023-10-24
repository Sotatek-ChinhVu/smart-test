using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetListUser;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetListUserPresenter : IGetListUserOutputPort
    {
        public Response<GetListUserResponse> Result { get; private set; } = new();

        public void Complete(GetListUserOutputData output)
        {
            Result.Data = new GetListUserResponse(output.UserMsts);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetListUserStatus status) => status switch
        {
            GetListUserStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetListUserStatus.NoData => ResponseMessage.NoData,
            GetListUserStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
