using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.GetListFunctionPermission
{
    public class GetListFunctionPermissionOutputData : IOutputData
    {
        public GetListFunctionPermissionOutputData(GetListFunctionPermissionStatus status, List<FunctionMstModel> functionPermissions)
        {
            Status = status;
            FunctionPermissions = functionPermissions;
        }

        public GetListFunctionPermissionStatus Status { get; private set; }

        public List<FunctionMstModel> FunctionPermissions { get; private set; }
    }
}
