namespace Reporting.OutDrug.Model
{
    public class OutDrugRequest
    {
        public List<ReceptionDtoReq> Receptions { get; set; } = new();
    }

    public class ReceptionDtoReq
    {
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public long RaiinNo { get; set; }
    }
}
