namespace EmrCloudApi.Requests.SystemConf
{
    public class GetSystemConfListXmlPathRequest
    {
        public int GrpCd { get; set; }

        public string Machine { get; set; } = string.Empty;

        public bool IsKensaIrai { get; set; } = false;
    }
}
