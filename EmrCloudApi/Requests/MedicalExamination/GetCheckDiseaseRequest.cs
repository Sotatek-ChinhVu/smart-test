using EmrCloudApi.Requests.Diseases;
using EmrCloudApi.Requests.MedicalExamination;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetCheckDiseaseRequest
    {
        public int HpId { get; set; }

        public int SinDate { get; set; }

        public List<UpsertPtDiseaseListItem> TodayByomeis { get; set; } = new();

        public List<OdrInfItem> TodayOdrs { get; set; } = new();
    }
}
