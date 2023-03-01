namespace EmrCloudApi.Requests.UserConf
{
    public class UserConfListItem
    {
        public int GrpCd { get; set; }

        public int GrpItemCd { get; set; }

        public int GrpItemEdaNo { get; set; }

        public int Val { get; set; }

        public string Param { get; set; } = string.Empty;
    }
}
