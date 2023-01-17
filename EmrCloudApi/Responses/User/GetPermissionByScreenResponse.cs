using static Helper.Constants.UserConst;

namespace EmrCloudApi.Responses.User
{
    public class GetPermissionByScreenResponse
    {
        public GetPermissionByScreenResponse(PermissionType permissionType)
        {
            PermissionType = permissionType;
        }

        public PermissionType PermissionType { get; private set; }
    }
}
