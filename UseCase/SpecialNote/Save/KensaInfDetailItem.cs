using Domain.Models.SpecialNote.PatientInfo;
using System.Text.Json.Serialization;

namespace UseCase.SpecialNote.Save
{
    public class KensaInfDetailItem
    {
        [JsonConstructor]
        public KensaInfDetailItem(int hpId, long ptId, long iraiCd, long seqNo, int iraiDate, long raiinNo, string kensaItemCd, string resultVal, string resultType, string abnormalKbn, int isDeleted, string cmtCd1, string cmtCd2)
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
        }

        public KensaInfDetailItem(KensaInfDetailModel kensaInfDetailModel)
        {
            HpId = kensaInfDetailModel.HpId;
            PtId = kensaInfDetailModel.PtId;
            IraiCd = kensaInfDetailModel.IraiCd;
            SeqNo = kensaInfDetailModel.SeqNo;
            IraiDate = kensaInfDetailModel.IraiDate;
            RaiinNo = kensaInfDetailModel.RaiinNo;
            KensaItemCd = kensaInfDetailModel.KensaItemCd;
            ResultVal = kensaInfDetailModel.ResultVal;
            ResultType = kensaInfDetailModel.ResultType;
            AbnormalKbn = kensaInfDetailModel.AbnormalKbn;
            IsDeleted = kensaInfDetailModel.IsDeleted;
            CmtCd1 = kensaInfDetailModel.CmtCd1;
            CmtCd2 = kensaInfDetailModel.CmtCd2;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long IraiCd { get; private set; }

        public long SeqNo { get; private set; }

        public int IraiDate { get; private set; }

        public long RaiinNo { get; private set; }

        public string KensaItemCd { get; private set; } = String.Empty;

        public string ResultVal { get; private set; } = String.Empty;

        public string ResultType { get; private set; } = String.Empty;

        public string AbnormalKbn { get; private set; } = String.Empty;

        public int IsDeleted { get; private set; }

        public string CmtCd1 { get; private set; } = String.Empty;

        public string CmtCd2 { get; private set; } = String.Empty;
    }
}
