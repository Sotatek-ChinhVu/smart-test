namespace EmrCloudApi.Requests.UserConf
{
    public class GetUserConfigParamRequest
    {
        public int HpId { get; set; }
        public int UserId { get; set; }
        public int GrpCd { get; set; }
        public int GrpItemCd { get; set; }
    }
}
