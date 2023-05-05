using Domain.Models.User;

namespace EmrCloudApi.Responses.User
{
    public class GetAllPermissionResponse
    {
        public GetAllPermissionResponse(List<UserPermissionModel> userPermissionModels)
        {
            UserPermissionModels = userPermissionModels;
        }

        public List<UserPermissionModel> UserPermissionModels { get; private set; }
    }
}
