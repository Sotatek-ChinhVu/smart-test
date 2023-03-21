using OdrInfItemU = UseCase.MedicalExamination.GetCheckedOrder.OdrInfItem;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetTrialAccountingMeiHoGaiRequest
    {
        public int HpId { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public long RaiinNo { get; set; }
        public List<OdrInfItemU> OdrInfItems { get; set; } = new();
    }
}
