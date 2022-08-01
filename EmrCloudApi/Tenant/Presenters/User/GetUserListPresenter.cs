using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.User;
using Microsoft.AspNetCore.Mvc;
using UseCase.User.GetList;

namespace EmrCloudApi.Tenant.Presenters.User
{
    public class GetUserListPresenter : IGetUserListOutputPort
    {
        public Response<GetUserListResponse> Result { get; private set; } = default!;
        
        public void Complete(GetUserListOutputData outputData)
        {
            Result = new Response<GetUserListResponse>()
            {
                Data = new GetUserListResponse()
                {
                    UserList = outputData.UserList
                },
                Status = 1,
                Message = ResponseMessage.Success
            };
        }
    }
}
