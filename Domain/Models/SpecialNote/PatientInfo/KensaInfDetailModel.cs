using Helper.Constants;

namespace Domain.Models.SpecialNote.PatientInfo
{
    public class KensaInfDetailModel
    {
        public KensaInfDetailModel(int hpId, long ptId, long iraiCd, long seqNo, int iraiDate, long raiinNo, string kensaItemCd, string resultVal, string resultType, string abnormalKbn, int isDeleted, string cmtCd1, string cmtCd2, DateTime updateDate, string unit, string kensaName, int sortNo)
        {
            HpId = hpId;
            PtId = ptId;
            IraiCd = iraiCd;
            SeqNo = seqNo;
            IraiDate = iraiDate;
            RaiinNo = raiinNo;
            KensaItemCd = kensaItemCd;
            ResultVal = resultVal;
            ResultType = resultType;
            AbnormalKbn = abnormalKbn;
            IsDeleted = isDeleted;
            CmtCd1 = cmtCd1;
            CmtCd2 = cmtCd2;
            UpdateDate = updateDate;
            Unit = unit;
            KensaName = kensaName;
            SortNo = sortNo;
        }

        public KensaInfDetailModel(string kensaItemCd, string unit, string kensaName, long sortNo)
        {
            KensaItemCd = kensaItemCd;
            Unit = unit;
            KensaName = kensaName;
            SortNo = sortNo;
            ResultVal = string.Empty;
            ResultType = string.Empty;
            AbnormalKbn = string.Empty;
            CmtCd1 = string.Empty;
            CmtCd2 = string.Empty;
        }

        public KensaInfDetailModel()
        {
            Unit = string.Empty;
            KensaName = string.Empty;
            ResultVal = string.Empty;
            ResultType = string.Empty;
            AbnormalKbn = string.Empty;
            CmtCd1 = string.Empty;
            CmtCd2 = string.Empty;
            KensaItemCd = string.Empty;
        }

        public KensaInfDetailModel(long sortNo, string kensaName, string kensaItemCd, string resultVal)
        {
            KensaName = kensaName;
            KensaItemCd = kensaItemCd;
            ResultVal = resultVal;
            SortNo = sortNo;
            Unit = string.Empty;
            ResultType = string.Empty;
            AbnormalKbn = string.Empty;
            CmtCd1 = string.Empty;
            CmtCd2 = string.Empty;
        }

        public KensaInfDetailModel(int iraiDate, string resultVal)
        {
            KensaName = string.Empty;
            KensaItemCd = string.Empty;
            ResultVal = resultVal;
            Unit = string.Empty;
            ResultType = string.Empty;
            AbnormalKbn = string.Empty;
            CmtCd1 = string.Empty;
            CmtCd2 = string.Empty;
            IraiDate = iraiDate;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long IraiCd { get; private set; }

        public long SeqNo { get; private set; }

        public int IraiDate { get; private set; }

        public long RaiinNo { get; private set; }

        public string KensaItemCd { get; private set; }

        public string ResultVal { get; private set; }

        public string ResultType { get; private set; }

        public string AbnormalKbn { get; private set; }

        public int IsDeleted { get; private set; }

        public string CmtCd1 { get; private set; }

        public string CmtCd2 { get; private set; }

        public string Unit { get; private set; }

        public string KensaName { get; private set; }

        public long SortNo { get; set; }

        public DateTime UpdateDate { get; private set; }
    }
}
