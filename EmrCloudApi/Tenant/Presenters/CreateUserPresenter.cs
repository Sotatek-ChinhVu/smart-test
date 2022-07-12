using Microsoft.AspNetCore.Mvc;
using UseCase.User.Create;

namespace EmrCloudApi.Tenant.Presenters
{
    public class CreateUserPresenter : ICreateUserOutputPort
    {
        public void Complete(CreateUserOutputData outputData)
        {
            throw new NotImplementedException();
        }
    }
}
