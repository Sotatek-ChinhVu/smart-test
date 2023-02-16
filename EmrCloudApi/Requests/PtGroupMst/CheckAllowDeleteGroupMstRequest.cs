namespace EmrCloudApi.Requests.PtGroupMst
{
    public class CheckAllowDeleteGroupMstRequest
    {
        public CheckAllowDeleteGroupMstRequest(int groupId, string groupCode, bool checkAllowDeleteGroupName)
        {
            GroupId = groupId;
            GroupCode = groupCode;
            CheckAllowDeleteGroupName = checkAllowDeleteGroupName;
        }

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
