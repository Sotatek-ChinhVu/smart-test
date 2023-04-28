using UseCase.Core.Sync.Core;

namespace UseCase.ChartApproval.CheckSaveLogOut
{
    public class CheckSaveLogOutInputData : IInputData<CheckSaveLogOutOutputData>
    {
        public CheckSaveLogOutInputData(int hpId, int userId, int departmentId)
        {
            HpId = hpId;
            UserId = userId;
            DepartmentId = departmentId;
        }
        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public int DepartmentId { get; private set; }
    }
}
