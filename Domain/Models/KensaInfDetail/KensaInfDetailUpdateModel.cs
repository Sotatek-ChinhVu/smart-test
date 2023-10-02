namespace Domain.Models.KensaInfDetail
{
    public class KensaInfDetailUpdateModel
    {
        public string KensaItemCd { get; set; }
        public long PtId { get; set; }
        public long IraiCd { get; set; }
        public int SeqNo { get; set; }
        public int IraiDate { get; set; }
        public long RaiinNo { get; set; }
        public string MaleStd { get; set; }
        public string FeMaleStd { get; set; }
        public string Unit { get; set; }
        public string ResultVal { get; set; }
        public string ResultType { get; set; }
        public string AbnormalKbn { get; set; }
        public string CmtCd1 { get; set; }
        public string CmtCd2 { get; set; }
        public int IsDeleted { get; set; }
    }
}