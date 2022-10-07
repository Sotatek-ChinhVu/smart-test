using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.User;
using UseCase.User.UpsertList;

namespace EmrCloudApi.Tenant.Presenters.User
{
    public class UpsertUserListPresenter : IUpsertUserListOutputPort
    {
        public Response<UpsertUserResponse> Result { get; private set; } = default!;

        public void Complete(UpsertUserListOutputData outputData)
        {
            Result = new Response<UpsertUserResponse>()
            {
                Data = new UpsertUserResponse(),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }
        private string GetMessage(UpsertUserListStatus status) => status switch 
        {
            UpsertUserListStatus.Success => ResponseMessage.Success,
            UpsertUserListStatus.Failed => ResponseMessage.Failed,
            UpsertUserListStatus.DuplicateId => ResponseMessage.DuplicateId,
            UpsertUserListStatus.ExistedId => ResponseMessage.ExistedId,
            _ => string.Empty
        };
    }
}