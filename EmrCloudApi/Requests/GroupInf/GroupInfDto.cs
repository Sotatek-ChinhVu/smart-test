namespace EmrCloudApi.Requests.GroupInf
{
    public class GroupInfDto
    {
        public GroupInfDto(int hpPt, long ptId, int groupId, string groupCode, string groupName)
        {
            HpPt = hpPt;
            PtId = ptId;
            GroupId = groupId;
            GroupCode = groupCode;
            GroupName = groupName;
        }

        public int HpPt { get; private set; }

        public long PtId { get; private set; }

        public int GroupId { get; private set; }

        public string GroupCode { get; private set; }

        public string GroupName { get; private set; }
    }
}
