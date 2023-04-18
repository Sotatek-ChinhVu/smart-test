namespace EmrCloudApi.Requests.UserConf
{
    public class GetUserConfigParamRequest
    {
        public int HpId { get; set; }
        public int UserId { get; set; }
        public List<GroupCode> GroupCodes { get; set; } = new();
    }

    public class GroupCode
    {
        public int GrpCd { get; set; }

        public int GrpItemCd { get; set; }
    }
}

