using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.GetPtWeight
{
    public class GetPtWeightOutputData : IOutputData
    {
        public GetPtWeightOutputData(int hpId, long ptId, long iraiCd, long seqNo, int iraiDate, long raiinNo, string kensaItemCd, string resultVal, string resultType, string abnormalKbn, int isDeleted, string cmtCd1, string cmtCd2, DateTime updateDate, GetPtWeightStatus status)
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
            Status = status;
        }

        public GetPtWeightOutputData(GetPtWeightStatus status)
        {
            KensaItemCd = string.Empty;
            ResultVal = string.Empty;
            ResultType = string.Empty;
            AbnormalKbn = string.Empty;
            CmtCd1 = string.Empty;
            CmtCd2 = string.Empty;
            Status = status;
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

        public DateTime UpdateDate { get; private set; }

        public GetPtWeightStatus Status { get; private set; }
    }
}
