using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.User;
using Microsoft.AspNetCore.Mvc;
using UseCase.User.Create;

namespace EmrCloudApi.Tenant.Presenters.User
{
    public class CreateUserPresenter : ICreateUserOutputPort
    {
        public ActionResult<Response<GetUserListResponse>> Result { get; private set; } = default!;

        public void Complete(CreateUserOutputData outputData)
        {
            throw new NotImplementedException();
        }
    }
}
