using Domain.Models.TodayOdr;

namespace EmrCloudApi.Tenant.Requests.MedicalExamination
{
    public class GetByomeiOfCheckDiseaseRequest
    {
        public bool IsGridStyle { get; set; }

        public int HpId { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public int SinDate { get; set; }

        public List<CheckedDiseaseItem> TodayByomeis { get; set; } = new();
    }
}
