using UseCase.MedicalExamination.GetCheckedOrder;
using OdrInfItemU = UseCase.MedicalExamination.GetCheckedOrder.OdrInfItem;
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

        public int HokenPid { get; set; }

        public List<OdrInfItemU> OdrInfItems { get; set; } = new();

        public List<DiseaseItem> DiseaseItems { get; set; } = new();
    }
}
