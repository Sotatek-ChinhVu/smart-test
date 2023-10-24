namespace EmrCloudApi.Requests.SystemConf
{
    public class SavePathRequest
    {
        public List<SystemConfListXmlPathItem> SystemConfListXmlPathModels { get; set; } = new();
    }

    public class SystemConfListXmlPathItem
    {
        public int GrpCd { get; set; }

        public int SeqNo { get; set; }

        public string Path { get; set; } = string.Empty;

        public string Biko { get; set; } = string.Empty;

        public string Machine { get; set; } = string.Empty;
    }
}
