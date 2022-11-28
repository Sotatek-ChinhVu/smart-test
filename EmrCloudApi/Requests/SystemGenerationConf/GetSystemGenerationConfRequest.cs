namespace EmrCloudApi.Requests.SystemGenerationConf
{
    public class GetSystemGenerationConfRequest
    {
        public int HpId { get; set; }

        public int GrpCd { get; set; }

        public int GrpEdaNo { get; set; }

        public int PresentDate { get; set; }

        public int DefaultValue { get; set; }

        public string DefaultParam { get; set; } = string.Empty;
    }
}
