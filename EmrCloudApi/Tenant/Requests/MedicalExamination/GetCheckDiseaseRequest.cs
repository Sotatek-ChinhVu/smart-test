using Domain.Models.Diseases;
using EmrCloudApi.Tenant.Requests.Diseases;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Tenant.Requests.MedicalExamination
{
    public class GetCheckDiseaseRequest
    {
        public int HpId { get; set; }

        public int SinDate { get; set; }

        public List<UpsertPtDiseaseListItem> TodayByomeis { get; set; } = new();

        public List<OdrInfItem> TodayOdrs { get; set; } = new();
    }
}
