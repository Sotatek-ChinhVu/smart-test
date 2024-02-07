using System.Drawing.Text;
using UseCase.Core.Sync.Core;

namespace UseCase.User.GetListFunctionPermission
{
    public class GetListFunctionPermissionInputData : IInputData<GetListFunctionPermissionOutputData>
    {
        public GetListFunctionPermissionInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
