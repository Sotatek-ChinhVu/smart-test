namespace Domain.Models.GroupInf
{
    public class GroupInfModel
    {
        public GroupInfModel(int hpId, long ptId, int groupId, string groupCode, string groupName, string groupCodeName)
        {
            HpId = hpId;
            PtId = ptId;
            GroupId = groupId;
            GroupCode = groupCode;
            GroupName = groupName;
            GroupCodeName = groupCodeName;
        }
        
        public GroupInfModel(int hpId, long ptId, int groupId, string groupCode, string groupName)
        {
            HpId = hpId;
            PtId = ptId;
            GroupId = groupId;
            GroupCode = groupCode;
            GroupName = groupName;
            GroupCodeName = string.Empty;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int GroupId { get; private set; }

        public string GroupCode { get; private set; }

        public string GroupName { get; private set; }

        public string GroupCodeName { get; private set; }
    }
}
