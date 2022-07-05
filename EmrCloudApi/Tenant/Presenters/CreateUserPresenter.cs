using Microsoft.AspNetCore.Mvc;
using UseCase.User.Create;

namespace EmrCloudApi.Tenant.Presenters
{
    public class CreateUserPresenter : ICreateUserOutputPort
    {
        public ActionResult<String> Result { get; private set; }

        public void Complete(CreateUserOutputData outputData)
        {
            if (outputData.Status == CreateUserStatus.Success)
            {

            }
            else
            {

            }
        }
    }
}
