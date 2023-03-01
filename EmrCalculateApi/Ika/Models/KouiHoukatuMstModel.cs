using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class KouiHoukatuMstModel
    {
        public KouiHoukatuMst KouiHoukatuMst { get; } = null;

        public KouiHoukatuMstModel(KouiHoukatuMst kouiHoukatuMst)
        {
            KouiHoukatuMst = kouiHoukatuMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return KouiHoukatuMst.HpId; }
            set
            {
                if (KouiHoukatuMst.HpId == value) return;
                KouiHoukatuMst.HpId = value;
            }
        }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return KouiHoukatuMst.ItemCd; }
            set
            {
                if (KouiHoukatuMst.ItemCd == value) return;
                KouiHoukatuMst.ItemCd = value;
            }
        }

        /// <summary>
        /// 新設年月日
        /// レコード情報を新設した日付を西暦年4桁、月2桁及び日2桁の8桁で表す。
        /// </summary>
        public int StartDate
        {
            get { return KouiHoukatuMst.StartDate; }
            set
            {
                if (KouiHoukatuMst.StartDate == value) return;
                KouiHoukatuMst.StartDate = value;
            }
        }

        /// <summary>
        /// 廃止年月日
        /// "当該診療行為の使用が可能な最終日付を西暦年4桁、月2桁及び日2桁の8桁で表す。 
        /// なお、廃止診療行為でない場合は「99999999」とする。"
        /// </summary>
        public int EndDate
        {
            get { return KouiHoukatuMst.EndDate; }
            set
            {
                if (KouiHoukatuMst.EndDate == value) return;
                KouiHoukatuMst.EndDate = value;
            }
        }

        /// <summary>
        /// 対象保険種
        /// "0:健保・労災とも対象
        /// 1:健保のみ対象
        /// 2:労災のみ対象"
        /// </summary>
        public int TargetKbn
        {
            get { return KouiHoukatuMst.TargetKbn; }
            set
            {
                if (KouiHoukatuMst.TargetKbn == value) return;
                KouiHoukatuMst.TargetKbn = value;
            }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public long SeqNo
        {
            get { return KouiHoukatuMst.SeqNo; }
            set
            {
                if (KouiHoukatuMst.SeqNo == value) return;
                KouiHoukatuMst.SeqNo = value;
            }
        }

        /// <summary>
        /// 包括単位
        /// "包括する期間を表す
        /// 0:同来院
        /// 1:同日
        /// 2:同一月内（診療日以前）
        /// 3:同一月内（月末まで）"
        /// </summary>
        public int HoukatuTerm
        {
            get { return KouiHoukatuMst.HoukatuTerm; }
            set
            {
                if (KouiHoukatuMst.HoukatuTerm == value) return;
                KouiHoukatuMst.HoukatuTerm = value;
            }
        }

        /// <summary>
        /// 行為コードFROM
        /// "包括対象行為コード（基本2桁）
        /// SIN_RP_INF.SIN_KOUI_KBN"
        /// </summary>
        public int KouiFrom
        {
            get { return KouiHoukatuMst.KouiFrom; }
            set
            {
                if (KouiHoukatuMst.KouiFrom == value) return;
                KouiHoukatuMst.KouiFrom = value;
            }
        }

        /// <summary>
        /// 行為コードTO
        /// "包括対象行為コード（基本2桁）
        /// SIN_RP_INF.SIN_KOUI_KBN"
        /// </summary>
        public int KouiTo
        {
            get { return KouiHoukatuMst.KouiTo; }
            set
            {
                if (KouiHoukatuMst.KouiTo == value) return;
                KouiHoukatuMst.KouiTo = value;
            }
        }

        /// <summary>
        /// 設定者区分
        /// 0: 基金 1: メーカー 2: ユーザー
        /// </summary>
        public int UserSetting
        {
            get { return KouiHoukatuMst.UserSetting; }
            set
            {
                if (KouiHoukatuMst.UserSetting == value) return;
                KouiHoukatuMst.UserSetting = value;
            }
        }

        /// <summary>
        /// 算定区分無視
        /// 0: 無視しない 1:無視する
        /// </summary>
        public int IgnoreSanteiKbn
        {
            get { return KouiHoukatuMst.IgnoreSanteiKbn; }
            set
            {
                if (KouiHoukatuMst.IgnoreSanteiKbn == value) return;
                KouiHoukatuMst.IgnoreSanteiKbn = value;
            }
        }

        /// <summary>
        /// 無効区分
        /// "0: 有効
        /// 1: 無効"
        /// </summary>
        public int IsInvalid
        {
            get { return KouiHoukatuMst.IsInvalid; }
            set
            {
                if (KouiHoukatuMst.IsInvalid == value) return;
                KouiHoukatuMst.IsInvalid = value;
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return KouiHoukatuMst.CreateDate; }
            set
            {
                if (KouiHoukatuMst.CreateDate == value) return;
                KouiHoukatuMst.CreateDate = value;
            }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return KouiHoukatuMst.CreateId; }
            set
            {
                if (KouiHoukatuMst.CreateId == value) return;
                KouiHoukatuMst.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return KouiHoukatuMst.CreateMachine; }
            set
            {
                if (KouiHoukatuMst.CreateMachine == value) return;
                KouiHoukatuMst.CreateMachine = value;
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return KouiHoukatuMst.UpdateDate; }
            set
            {
                if (KouiHoukatuMst.UpdateDate == value) return;
                KouiHoukatuMst.UpdateDate = value;
            }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return KouiHoukatuMst.UpdateId; }
            set
            {
                if (KouiHoukatuMst.UpdateId == value) return;
                KouiHoukatuMst.UpdateId = value;
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return KouiHoukatuMst.UpdateMachine; }
            set
            {
                if (KouiHoukatuMst.UpdateMachine == value) return;
                KouiHoukatuMst.UpdateMachine = value;
            }
        }
    }
}
