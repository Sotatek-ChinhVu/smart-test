using Domain.Models.User;

namespace EmrCloudApi.Responses.User
{
    public class GetListFunctionPermissionResponse
    {
        public GetListFunctionPermissionResponse(List<FunctionMstModel> functionPermissions)
        {
            FunctionPermissions = functionPermissions;
        }

        public List<FunctionMstModel> FunctionPermissions { get; private set; }
    }
}
