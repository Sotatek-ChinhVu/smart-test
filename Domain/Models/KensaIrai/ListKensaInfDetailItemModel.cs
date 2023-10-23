namespace Domain.Models.KensaIrai
{
    public class ListKensaInfDetailItemModel
    {
        public ListKensaInfDetailItemModel(long ptId, long iraiCd)
        {
            PtId = ptId;
            IraiCd = iraiCd;
            RaiinNo = 0;
            IraiDate = 0;
            SeqNo = 0;
            KensaName = string.Empty;
            KensaKana = string.Empty;
            SortNo = 0;
            KensaItemCd = string.Empty;
            ResultVal = string.Empty;
            ResultType = string.Empty;
            AbnormalKbn = string.Empty;
            CmtCd1 = string.Empty;
            CmtCd2 = string.Empty;
            Cmt1 = string.Empty;
            Cmt2 = string.Empty;
            Std = string.Empty;
            StdLow = string.Empty;
            StdHigh = string.Empty;
            MaleStd = string.Empty;
            FemaleStd = string.Empty;
            Unit = string.Empty;
            Nyubi = string.Empty;
            Yoketu = string.Empty;
            Bilirubin = string.Empty;
            SikyuKbn = 0;
            TosekiKbn = 0;
            InoutKbn = 0;
            Status = 0;
            IsDeleted = 0;
        }

        public ListKensaInfDetailItemModel(
            long ptId, long iraiCd, long raiinNo, long iraiDate, long seqNo, long seqParentNo, string kensaName, string kensaKana, long sortNo, string kensaItemCd, string resultVal,
            string resultType, string abnormalKbn, string cmtCd1, string cmtCd2, string cmt1, string cmt2, string std, string stdLow, string stdHigh, string maleStd,
            string femaleStd, string unit, string nyubi, string yoketu, string bilirubin, int sikyuKbn, int tosekiKbn, int inoutKbn, int status, int isDeleted)
        {
            PtId = ptId;
            IraiCd = iraiCd;
            RaiinNo = raiinNo;
            IraiDate = iraiDate;
            SeqNo = seqNo;
            SeqParentNo = seqParentNo;
            KensaName = kensaName;
            KensaKana = kensaKana;
            SortNo = sortNo;
            KensaItemCd = kensaItemCd;
            ResultVal = resultVal;
            ResultType = resultType;
            AbnormalKbn = abnormalKbn;
            CmtCd1 = cmtCd1;
            CmtCd2 = cmtCd2;
            Cmt1 = cmt1;
            Cmt2 = cmt2;
            Std = std;
            StdLow = stdLow;
            StdHigh = stdHigh;
            MaleStd = maleStd;
            FemaleStd = femaleStd;
            Unit = unit;
            Nyubi = nyubi;
            Yoketu = yoketu;
            Bilirubin = bilirubin;
            SikyuKbn = sikyuKbn;
            TosekiKbn = tosekiKbn;
            InoutKbn = inoutKbn;
            Status = status;
            IsDeleted = isDeleted;
        }

        public long PtId { get; private set; }

        public long IraiCd { get; private set; }

        public long RaiinNo { get; private set; }

        public long IraiDate { get; private set; }

        public long SeqNo { get; private set; }

        public long SeqParentNo { get; private set; }

        public string KensaName { get; private set; }

        public string KensaKana { get; private set; }

        public long SortNo { get; private set; }

        public string KensaItemCd { get; private set; }

        public string ResultVal { get; private set; }

        public string ResultType { get; private set; }

        public string AbnormalKbn { get; private set; }

        public string CmtCd1 { get; private set; }

        public string CmtCd2 { get; private set; }

        public string Cmt1 { get; private set; }

        public string Cmt2 { get; private set; }

        public string Std { get; private set; }

        public string StdLow { get; private set; }

        public string StdHigh { get; private set; }

        public string MaleStd { get; private set; }

        public string FemaleStd { get; private set; }

        public string Unit { get; private set; }

        public string Nyubi { get; private set; }

        public string Yoketu { get; private set; }

        public string Bilirubin { get; private set; }

        public int SikyuKbn { get; private set; }

        public int TosekiKbn { get; private set; }

        public int InoutKbn { get; private set; }

        public int Status { get; private set; }

        public int IsDeleted { get; private set; }
    }
}