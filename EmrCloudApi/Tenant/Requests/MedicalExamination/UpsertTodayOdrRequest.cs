namespace EmrCloudApi.Tenant.Requests.MedicalExamination
{
    public class UpsertTodayOdrRequest
    {
        public int Status { get; private set; }
        public int SyosaiKbn { get; private set; }
        public int JikanKbn { get; private set; }
        public int HokenPid { get; private set; }
        public int SanteiKbn { get; private set; }
        public int TantoId { get; private set; }
        public int KaId { get; private set; }
        public List<OdrInfItem> OdrInfs { get; set; } = new();
        public List<KarteItem> KarteItems { get; set; } = new();
    }
}
