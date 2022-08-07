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
                Status = (int) outputData.Status
            };
        }
    }
}
