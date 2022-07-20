using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.KensaInfDetail
{
    public class KensaInfDetailModel
    {
        public KensaInfDetailModel(int hpId, long ptId, int iraiDate, long raiinNo, long iraiCd, long seqNo, string? kensaItemCd, string? resultVal, string? resultType, string? abnormalKbn, int isDeleted, string? cmtCd1, string? cmtCd2, DateTime createDate, int createId, string? createMachine, DateTime updateDate, int updateId, string? updateMachine)
        {
            HpId = hpId;
            PtId = ptId;
            IraiDate = iraiDate;
            RaiinNo = raiinNo;
            IraiCd = iraiCd;
            SeqNo = seqNo;
            KensaItemCd = kensaItemCd;
            ResultVal = resultVal;
            ResultType = resultType;
            AbnormalKbn = abnormalKbn;
            IsDeleted = isDeleted;
            CmtCd1 = cmtCd1;
            CmtCd2 = cmtCd2;
            CreateDate = createDate;
            CreateId = createId;
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;
        }

        public int HpId { get; set; }
        public long PtId { get; set; }
        public int IraiDate { get; set; }
        public long RaiinNo { get; set; }
        public long IraiCd { get; set; }
        public long SeqNo { get; set; }
        public string? KensaItemCd { get; set; }
        public string? ResultVal { get; set; }
        public string? ResultType { get; set; }
        public string? AbnormalKbn { get; set; }
        public int IsDeleted { get; set; }
        public string? CmtCd1 { get; set; }
        public string? CmtCd2 { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateId { get; set; }
        public string? CreateMachine { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateId { get; set; }
        public string? UpdateMachine { get; set; }
    }
}
