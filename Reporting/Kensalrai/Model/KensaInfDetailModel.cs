using Entity.Tenant;

namespace Reporting.Kensalrai.Model
{
    public class KensaInfDetailModel
    {
        public KensaInfDetail KensaInfDetail { get; set; } = null;

        public KensaInfDetailModel(KensaInfDetail kensaInfDetail)
        {
            KensaInfDetail = kensaInfDetail;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return KensaInfDetail.HpId; }
            set
            {
                if (KensaInfDetail.HpId == value) return;
                KensaInfDetail.HpId = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return KensaInfDetail.PtId; }
            set
            {
                if (KensaInfDetail.PtId == value) return;
                KensaInfDetail.PtId = value;
            }
        }

        /// <summary>
        /// 依頼日
        /// 
        /// </summary>
        public int IraiDate
        {
            get { return KensaInfDetail.IraiDate; }
            set
            {
                if (KensaInfDetail.IraiDate == value) return;
                KensaInfDetail.IraiDate = value;
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return KensaInfDetail.RaiinNo; }
            set
            {
                if (KensaInfDetail.RaiinNo == value) return;
                KensaInfDetail.RaiinNo = value;
            }
        }

        /// <summary>
        /// 検査依頼コード
        /// 
        /// </summary>
        public long IraiCd
        {
            get { return KensaInfDetail.IraiCd; }
            set
            {
                if (KensaInfDetail.IraiCd == value) return;
                KensaInfDetail.IraiCd = value;
            }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public long SeqNo
        {
            get { return KensaInfDetail.SeqNo; }
            set
            {
                if (KensaInfDetail.SeqNo == value) return;
                KensaInfDetail.SeqNo = value;
            }
        }

        /// <summary>
        /// 検査項目コード
        /// 
        /// </summary>
        public string KensaItemCd
        {
            get { return KensaInfDetail.KensaItemCd; }
            set
            {
                if (KensaInfDetail.KensaItemCd == value) return;
                KensaInfDetail.KensaItemCd = value;
            }
        }

        /// <summary>
        /// 結果値
        /// 
        /// </summary>
        public string ResultVal
        {
            get { return KensaInfDetail.ResultVal; }
            set
            {
                if (KensaInfDetail.ResultVal == value) return;
                KensaInfDetail.ResultVal = value;
            }
        }

        /// <summary>
        /// 検査値形態
        /// "E: 以下
        /// L: 未満
        /// H: 以上"
        /// </summary>
        public string ResultType
        {
            get { return KensaInfDetail.ResultType; }
            set
            {
                if (KensaInfDetail.ResultType == value) return;
                KensaInfDetail.ResultType = value;
            }
        }

        /// <summary>
        /// 異常値区分
        /// "L: 基準値未満
        /// H: 基準値以上"
        /// </summary>
        public string AbnormalKbn
        {
            get { return KensaInfDetail.AbnormalKbn; }
            set
            {
                if (KensaInfDetail.AbnormalKbn == value) return;
                KensaInfDetail.AbnormalKbn = value;
            }
        }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        public int IsDeleted
        {
            get { return KensaInfDetail.IsDeleted; }
            set
            {
                if (KensaInfDetail.IsDeleted == value) return;
                KensaInfDetail.IsDeleted = value;
            }
        }

        /// <summary>
        /// 検査結果コメント１
        /// 
        /// </summary>
        public string CmtCd1
        {
            get { return KensaInfDetail.CmtCd1; }
            set
            {
                if (KensaInfDetail.CmtCd1 == value) return;
                KensaInfDetail.CmtCd1 = value;
            }
        }

        /// <summary>
        /// 検査結果コメント２
        /// 
        /// </summary>
        public string CmtCd2
        {
            get { return KensaInfDetail.CmtCd2; }
            set
            {
                if (KensaInfDetail.CmtCd2 == value) return;
                KensaInfDetail.CmtCd2 = value;
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return KensaInfDetail.CreateDate; }
            set
            {
                if (KensaInfDetail.CreateDate == value) return;
                KensaInfDetail.CreateDate = value;
            }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return KensaInfDetail.CreateId; }
            set
            {
                if (KensaInfDetail.CreateId == value) return;
                KensaInfDetail.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return KensaInfDetail.CreateMachine; }
            set
            {
                if (KensaInfDetail.CreateMachine == value) return;
                KensaInfDetail.CreateMachine = value;
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return KensaInfDetail.UpdateDate; }
            set
            {
                if (KensaInfDetail.UpdateDate == value) return;
                KensaInfDetail.UpdateDate = value;
            }
        }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return KensaInfDetail.UpdateId; }
            set
            {
                if (KensaInfDetail.UpdateId == value) return;
                KensaInfDetail.UpdateId = value;
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return KensaInfDetail.UpdateMachine; }
            set
            {
                if (KensaInfDetail.UpdateMachine == value) return;
                KensaInfDetail.UpdateMachine = value;
            }
        }
    }
}
