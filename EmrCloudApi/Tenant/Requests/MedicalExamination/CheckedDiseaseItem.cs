using EmrCloudApi.Tenant.Requests.Diseases;

namespace EmrCloudApi.Tenant.Requests.MedicalExamination
{
    public class CheckedDiseaseItem
    {
        public int SikkanCd { get; set; }

        public int NanByoCd { get; set; }

        public string Byomei { get; set; } = string.Empty;

        public string ItemCd { get; set; } = string.Empty;

        public int OdrItemNo { get; set; }

        public string OdrItemName { get; set; } = string.Empty;

        public UpsertPtDiseaseListItem TodayByomeis { get; set; } = new();

        public ByomeiMstItem ByomeiMst { get; set; } = new();
    }
}
