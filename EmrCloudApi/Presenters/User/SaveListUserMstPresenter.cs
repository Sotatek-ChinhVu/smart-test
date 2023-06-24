using EmrCloudApi.Constants;
using EmrCloudApi.Responses.User;
using EmrCloudApi.Responses;
using UseCase.User.SaveListUserMst;

namespace EmrCloudApi.Presenters.User
{
    public class SaveListUserMstPresenter : ISaveListUserMstOutputPort
    {
        public Response<SaveListUserMstResponse> Result { get; private set; } = new Response<SaveListUserMstResponse>();

        public void Complete(SaveListUserMstOutputData output)
        {
            Result.Data = new SaveListUserMstResponse(output.Status);
            Result.Message = string.IsNullOrEmpty(output.Message) ? GetMessage(output.Status) : output.Message;
            Result.Status = (int)output.Status;
        }

        private string GetMessage(SaveListUserMstStatus status) => status switch
        {
            SaveListUserMstStatus.Successful => ResponseMessage.Success,
            SaveListUserMstStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            SaveListUserMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveListUserMstStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
