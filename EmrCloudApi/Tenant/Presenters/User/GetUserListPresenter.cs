using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.User;
using Microsoft.AspNetCore.Mvc;
using UseCase.User.GetList;

namespace EmrCloudApi.Tenant.Presenters.User
{
    public class GetUserListPresenter : IGetUserListOutputPort
    {
        public Response<GetUserListResponse> Result { get; private set; } = new Response<GetUserListResponse>();
        
        public void Complete(GetUserListOutputData outputData)
        {
            Result.Data = new GetUserListResponse
            {
                UserList = outputData.UserList
            };
        }
    }
}
