using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.User;
using Microsoft.AspNetCore.Mvc;
using UseCase.User.Create;

namespace EmrCloudApi.Presenters.User
{
    public class CreateUserPresenter : ICreateUserOutputPort
    {
        public Response<CreateUserResponse> Result { get; private set; } = default!;

        public void Complete(CreateUserOutputData outputData)
        {
            Result = new Response<CreateUserResponse>
            {
                Data = new CreateUserResponse()
                {
                    UserId = outputData.UserId
                },
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case CreateUserStatus.InvalidName:
                    Result.Message = ResponseMessage.CreateUserInvalidName;
                    break;
                case CreateUserStatus.Success:
                    Result.Message = ResponseMessage.CreateUserSuccessed;
                    break;
            }
        }
    }
}
