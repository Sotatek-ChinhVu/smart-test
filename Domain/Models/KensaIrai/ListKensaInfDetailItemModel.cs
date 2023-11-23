namespace Domain.Models.KensaIrai
{
    public class ListKensaInfDetailItemModel
    {
        public ListKensaInfDetailItemModel(long ptId, long iraiCd)
        {
            PtId = ptId;
            IraiCd = iraiCd;
            KensaName = string.Empty;
            KensaKana = string.Empty;
            KensaItemCd = string.Empty;
            ResultVal = string.Empty;
            ResultType = string.Empty;
            AbnormalKbn = string.Empty;
            CmtCd1 = string.Empty;
            CmtCd2 = string.Empty;
            Cmt1 = string.Empty;
            Cmt2 = string.Empty;
            MaleStd = string.Empty;
            FemaleStd = string.Empty;
            MaleStdLow = string.Empty;
            FemaleStdLow = string.Empty;
            MaleStdHigh = string.Empty;
            FemaleStdHigh = string.Empty;
            Unit = string.Empty;
            Nyubi = string.Empty;
            Yoketu = string.Empty;
            Bilirubin = string.Empty;
            RowSeqId = string.Empty;
        }

        public ListKensaInfDetailItemModel(long iraiCd, string kensaName, string resultVal, string abnormalKbn, string unit, string maleStd, string femaleStd, string resultType)
        {
            IraiCd = iraiCd;
            KensaName = kensaName;
            KensaKana = string.Empty;
            KensaItemCd = string.Empty;
            ResultVal = resultVal;
            ResultType = resultType;
            AbnormalKbn = abnormalKbn;
            CmtCd1 = string.Empty;
            CmtCd2 = string.Empty;
            Cmt1 = string.Empty;
            Cmt2 = string.Empty;
            MaleStd = maleStd;
            FemaleStd = femaleStd;
            MaleStdLow = string.Empty;
            FemaleStdLow = string.Empty;
            MaleStdHigh = string.Empty;
            FemaleStdHigh = string.Empty;
            Unit = unit;
            Nyubi = string.Empty;
            Yoketu = string.Empty;
            Bilirubin = string.Empty;
            RowSeqId = string.Empty;
        }


        public ListKensaInfDetailItemModel(
            long ptId, long iraiCd, long raiinNo, long iraiDate, long seqNo, long seqParentNo, string kensaName, string kensaKana, long sortNo, string kensaItemCd, string resultVal,
            string resultType, string abnormalKbn, string cmtCd1, string cmtCd2, string cmt1, string cmt2, string maleStd, string femaleStd, string maleStdLow, string femaleStdLow,
            string maleStdHigh, string femaleStdHigh, string unit, string nyubi, string yoketu, string bilirubin, int sikyuKbn, int tosekiKbn, int inoutKbn, int status, int isDeleted, long seqGroupNo)
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
            MaleStd = maleStd;
            FemaleStd = femaleStd;
            MaleStdLow = maleStdLow;
            FemaleStdLow = femaleStdLow;
            MaleStdHigh = maleStdHigh;
            FemaleStdHigh = femaleStdHigh;
            Unit = unit;
            Nyubi = nyubi;
            Yoketu = yoketu;
            Bilirubin = bilirubin;
            SikyuKbn = sikyuKbn;
            TosekiKbn = tosekiKbn;
            InoutKbn = inoutKbn;
            Status = status;
            IsDeleted = isDeleted;
            SeqGroupNo = seqGroupNo;
            RowSeqId = string.Empty;
        }

        public ListKensaInfDetailItemModel ChangeResultVal(string resultVal)
        {
            ResultVal = resultVal;
            return this;
        }

        public void SetRowSeqId(string newValue)
        {
            RowSeqId = newValue;
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

        public string MaleStd { get; private set; }

        public string FemaleStd { get; private set; }

        public string MaleStdLow { get; private set; }

        public string FemaleStdLow { get; private set; }

        public string MaleStdHigh { get; private set; }

        public string FemaleStdHigh { get; private set; }

        public string Unit { get; private set; }

        public string Nyubi { get; private set; }

        public string Yoketu { get; private set; }

        public string Bilirubin { get; private set; }

        public int SikyuKbn { get; private set; }

        public int TosekiKbn { get; private set; }

        public int InoutKbn { get; private set; }

        public int Status { get; private set; }

        public int IsDeleted { get; private set; }

        public string RowSeqId { get; private set; }

        public long SeqGroupNo { get; private set; } 
    }
}