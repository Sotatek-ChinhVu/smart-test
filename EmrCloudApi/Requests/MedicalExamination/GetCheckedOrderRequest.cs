using UseCase.Diseases.Upsert;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetCheckedOrderRequest
    {
        public int SinDate { get; set; }

        public int HokenId { get; set; }

        public long PtId { get; set; }

        public int IBirthDay { get; set; }

        public long RaiinNo { get; set; }

        public int SyosaisinKbn { get; set; }

        public long OyaRaiinNo { get; set; }

        public int PrimaryDoctor { get; set; }

        public int TantoId { get; set; }

        public List<OdrInfItemInputData> OdrInfItemInputDatas { get; private set; } = new();

        public List<UpsertPtDiseaseListInputItem> PtDiseaseListInputItems { get; private set; } = new();
    }
}
