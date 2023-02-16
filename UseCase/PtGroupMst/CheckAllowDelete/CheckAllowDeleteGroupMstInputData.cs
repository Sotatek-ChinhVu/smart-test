using UseCase.Core.Sync.Core;

namespace UseCase.PtGroupMst.CheckAllowDelete
{
    public class CheckAllowDeleteGroupMstInputData : IInputData<CheckAllowDeleteGroupMstOutputData>
    {
        public CheckAllowDeleteGroupMstInputData(int hpId, int groupId, string groupCode, bool checkAllowDeleteGroupName)
        {
            HpId = hpId;
            GroupId = groupId;
            GroupCode = groupCode;
            CheckAllowDeleteGroupName = checkAllowDeleteGroupName;
        }

        public int HpId { get; private set; }

        public int GroupId { get; private set; }

        public string GroupCode { get; private set; }

        /// <summary>
        /// true if check GroupName . else is groupItem
        /// GroupName required GroupId,GroupCode
        /// GroupItem reqired GroupId,GroupCode,SeqNo
        /// </summary>
        public bool CheckAllowDeleteGroupName { get; private set; }
    }
}
