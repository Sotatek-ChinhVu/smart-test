
using Entity.Tenant;
using Helper.Common;
using Helper.Extension;

namespace Reporting.Kensalrai.Model
{
    public class KensaInfModel
    {
        public KensaInf KensaInf { get; set; } = null;

        public KensaInfModel(KensaInf kensaInf, long ptNum, string ptName, int primaryKbn, string kensaCenterName, List<KensaInfDetailModel> kensaInfDetailModels)
        {
            KensaInf = kensaInf;
            PtNum = ptNum;
            PtName = ptName;
            PrimaryKbn = primaryKbn;
            KensaCenterName = kensaCenterName;
            KensaInfDetailModels = kensaInfDetailModels;
        }

        public bool IsSelected { get; set; }

        public long PtNum { get; set; }

        public string PtNumDisplay
        {
            get => PtNum == 0 ? "" : PtNum.AsString();
        }

        public string PtName { get; set; }

        public int PrimaryKbn { get; set; }

        public string KensaCenterName { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return KensaInf.HpId; }
            set
            {
                if (KensaInf.HpId == value) return;
                KensaInf.HpId = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return KensaInf.PtId; }
            set
            {
                if (KensaInf.PtId == value) return;
                KensaInf.PtId = value;
            }
        }

        /// <summary>
        /// 依頼日
        /// 
        /// </summary>
        public int IraiDate
        {
            get { return KensaInf.IraiDate; }
            set
            {
                if (KensaInf.IraiDate == value) return;
                KensaInf.IraiDate = value;
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return KensaInf.RaiinNo; }
            set
            {
                if (KensaInf.RaiinNo == value) return;
                KensaInf.RaiinNo = value;
            }
        }

        /// <summary>
        /// 検査依頼コード
        /// SEQUENCE
        /// </summary>
        public long IraiCd
        {
            get { return KensaInf.IraiCd; }
            set
            {
                if (KensaInf.IraiCd == value) return;
                KensaInf.IraiCd = value;
            }
        }

        public string IraiCdDisplay
        {
            get => IraiCd == 0 ? "" : IraiCd.AsString();
        }

        /// <summary>
        /// 院内院外区分
        /// "0: 院内
        /// 1: 院外"
        /// </summary>
        public int InoutKbn
        {
            get { return KensaInf.InoutKbn; }
            set
            {
                if (KensaInf.InoutKbn == value) return;
                KensaInf.InoutKbn = value;
            }
        }

        /// <summary>
        /// 実施状況
        /// "0: 依頼中
        /// 1: 検査中
        /// 2: 検査完了"
        /// </summary>
        public int Status
        {
            get { return KensaInf.Status; }
            set
            {
                if (KensaInf.Status == value) return;
                KensaInf.Status = value;
            }
        }

        public string Situation
        {
            get
            {
                if (CheckDefaultValue())
                {
                    return "";
                }
                if (IsDeleted == 1)
                {
                    return "取消";
                }
                if (Status == 0)
                {
                    return "依頼中";
                }
                if (Status == 1)
                {
                    return "検査中";
                }
                if (Status == 2)
                {
                    return "検査完了";
                }
                return "";
            }
        }

        /// <summary>
        /// 透析前後区分
        /// "0: 透析以外
        /// 1: 透析前
        /// 2: 透析後"
        /// </summary>
        public int TosekiKbn
        {
            get { return KensaInf.TosekiKbn; }
            set
            {
                if (KensaInf.TosekiKbn == value) return;
                KensaInf.TosekiKbn = value;
            }
        }

        /// <summary>
        /// 至急区分
        /// "0: 通常
        /// 1: 至急"
        /// </summary>
        public int SikyuKbn
        {
            get { return KensaInf.SikyuKbn; }
            set
            {
                if (KensaInf.SikyuKbn == value) return;
                KensaInf.SikyuKbn = value;
            }
        }

        /// <summary>
        /// 結果確認
        /// "0: 未確認
        /// 1: 確認済み"
        /// </summary>
        public int ResultCheck
        {
            get { return KensaInf.ResultCheck; }
            set
            {
                if (KensaInf.ResultCheck == value) return;
                KensaInf.ResultCheck = value;
            }
        }

        /// <summary>
        /// センターコード
        /// 
        /// </summary>
        public string CenterCd
        {
            get { return KensaInf.CenterCd; }
            set
            {
                if (KensaInf.CenterCd == value) return;
                KensaInf.CenterCd = value;
            }
        }

        /// <summary>
        /// 乳び
        /// 
        /// </summary>
        public string Nyubi
        {
            get { return KensaInf.Nyubi; }
            set
            {
                if (KensaInf.Nyubi == value) return;
                KensaInf.Nyubi = value;
            }
        }

        /// <summary>
        /// 溶血
        /// 
        /// </summary>
        public string Yoketu
        {
            get { return KensaInf.Yoketu; }
            set
            {
                if (KensaInf.Yoketu == value) return;
                KensaInf.Yoketu = value;
            }
        }

        /// <summary>
        /// ビリルビン
        /// 
        /// </summary>
        public string Bilirubin
        {
            get { return KensaInf.Bilirubin; }
            set
            {
                if (KensaInf.Bilirubin == value) return;
                KensaInf.Bilirubin = value;
            }
        }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        public int IsDeleted
        {
            get { return KensaInf.IsDeleted; }
            set
            {
                if (KensaInf.IsDeleted == value) return;
                KensaInf.IsDeleted = value;
            }
        }

        public int CreateDateToInt
        {
            get => CIUtil.DateTimeToInt(CreateDate);
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return KensaInf.CreateDate; }
            set
            {
                if (KensaInf.CreateDate == value) return;
                KensaInf.CreateDate = value;
            }
        }

        public string CreateDateDisplay
        {
            get { return CreateDate == DateTime.MinValue ? "" : CreateDate.ToString(); }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return KensaInf.CreateId; }
            set
            {
                if (KensaInf.CreateId == value) return;
                KensaInf.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return KensaInf.CreateMachine; }
            set
            {
                if (KensaInf.CreateMachine == value) return;
                KensaInf.CreateMachine = value;
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return KensaInf.UpdateDate; }
            set
            {
                if (KensaInf.UpdateDate == value) return;
                KensaInf.UpdateDate = value;
            }
        }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return KensaInf.UpdateId; }
            set
            {
                if (KensaInf.UpdateId == value) return;
                KensaInf.UpdateId = value;
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return KensaInf.UpdateMachine; }
            set
            {
                if (KensaInf.UpdateMachine == value) return;
                KensaInf.UpdateMachine = value;
            }
        }

        public List<KensaInfDetailModel> KensaInfDetailModels { get; set; }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(PtName) && PtNum == 0 && IraiDate == 0;
        }
    }
}
