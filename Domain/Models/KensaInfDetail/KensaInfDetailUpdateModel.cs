namespace Domain.Models.KensaInfDetail
{
    public class KensaInfDetailUpdateModel
    {
        public KensaInfDetailUpdateModel(string kensaItemCd, long ptId, long iraiCd, int seqNo, int iraiDate, long raiinNo, string maleStd, string feMaleStd, string unit, string resultVal, string resultType, string abnormalKbn, string cmtCd1, string cmtCd2, int isDeleted)
        {
            KensaItemCd = kensaItemCd;
            PtId = ptId;
            IraiCd = iraiCd;
            SeqNo = seqNo;
            IraiDate = iraiDate;
            RaiinNo = raiinNo;
            MaleStd = maleStd;
            FeMaleStd = feMaleStd;
            Unit = unit;
            ResultVal = resultVal;
            ResultType = resultType;
            AbnormalKbn = abnormalKbn;
            CmtCd1 = cmtCd1;
            CmtCd2 = cmtCd2;
            IsDeleted = isDeleted;
        }

        public string KensaItemCd { get; private set; }
        public long PtId { get; private set; }
        public long IraiCd { get; private set; }
        public int SeqNo { get; private set; }
        public int IraiDate { get; private set; }
        public long RaiinNo { get; private set; }
        public string MaleStd { get; private set; }
        public string FeMaleStd { get; private set; }
        public string Unit { get; private set; }
        public string ResultVal { get; private set; }
        public string ResultType { get; private set; }
        public string AbnormalKbn { get; private set; }
        public string CmtCd1 { get; private set; }
        public string CmtCd2 { get; private set; }
        public int IsDeleted { get; private set; }
    }
}