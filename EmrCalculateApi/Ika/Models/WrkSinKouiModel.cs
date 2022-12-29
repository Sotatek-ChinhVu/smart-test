using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class WrkSinKouiModel
    {
        public WrkSinKoui WrkSinKoui { get; } = null;
        private List<int> _weekCalcAppendDays;
        private long _odrRpNo;

        public WrkSinKouiModel(WrkSinKoui wrkSinKoui)
        {
            WrkSinKoui = wrkSinKoui;
            _weekCalcAppendDays = new List<int>();
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return WrkSinKoui.HpId; }
            set
            {
                if (WrkSinKoui.HpId == value) return;
                WrkSinKoui.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return WrkSinKoui.PtId; }
            set
            {
                if (WrkSinKoui.PtId == value) return;
                WrkSinKoui.PtId = value;
                //RaisePropertyChanged(() => PtId);
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return WrkSinKoui.SinDate; }
            set
            {
                if (WrkSinKoui.SinDate == value) return;
                WrkSinKoui.SinDate = value;
                //RaisePropertyChanged(() => SinDate);
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return WrkSinKoui.RaiinNo; }
            set
            {
                if (WrkSinKoui.RaiinNo == value) return;
                WrkSinKoui.RaiinNo = value;
                //RaisePropertyChanged(() => RaiinNo);
            }
        }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        public int HokenKbn
        {
            get { return WrkSinKoui.HokenKbn; }
            set
            {
                if (WrkSinKoui.HokenKbn == value) return;
                WrkSinKoui.HokenKbn = value;
                //RaisePropertyChanged(() => HokenKbn);
            }
        }

        /// <summary>
        /// 剤番号
        /// 
        /// </summary>
        public int RpNo
        {
            get { return WrkSinKoui.RpNo; }
            set
            {
                if (WrkSinKoui.RpNo == value) return;
                WrkSinKoui.RpNo = value;
                //RaisePropertyChanged(() => RpNo);
            }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return WrkSinKoui.SeqNo; }
            set
            {
                if (WrkSinKoui.SeqNo == value) return;
                WrkSinKoui.SeqNo = value;
                //RaisePropertyChanged(() => SeqNo);
            }
        }
        
        /// <summary>
        /// 保険組合せID
        /// 
        /// </summary>
        public int HokenPid
        {
            get { return WrkSinKoui.HokenPid; }
            set
            {
                if (WrkSinKoui.HokenPid == value) return;
                WrkSinKoui.HokenPid = value;
                //RaisePropertyChanged(() => HokenPid);
            }
        }

        /// <summary>
        /// 保険ID
        /// </summary>
        public int HokenId
        {
            get { return WrkSinKoui.HokenId; }
            set
            {
                if (WrkSinKoui.HokenId == value) return;
                WrkSinKoui.HokenId = value;
                //RaisePropertyChanged(() => HokenId);
            }
        }

        /// <summary>
        /// 点数欄集計先
        /// TEN_MST.SYUKEI_SAKI + 枝番 ※別シート参照
        /// </summary>
        public string SyukeiSaki
        {
            get { return WrkSinKoui.SyukeiSaki; }
            set
            {
                if (WrkSinKoui.SyukeiSaki == value) return;
                WrkSinKoui.SyukeiSaki = value;
                //RaisePropertyChanged(() => SyukeiSaki);
            }
        }

        /// <summary>
        /// 0: 1～12以外の診療行為 
        /// 1: 血液化学検査の包括項目 
        /// 2: 内分泌学的検査の包括項目 
        /// 3: 肝炎ウイルス関連検査の包括項目 
        /// 5: 腫瘍マーカーの包括項目 
        /// 6: 出血・凝固検査の包括項目 
        /// 7: 自己抗体検査の包括項目 
        /// 8: 内分泌負荷試験の包括項目 
        /// 9: 感染症免疫学的検査のうち、ウイルス抗体価（定性・半定量・定量） 
        /// 10: 感染症免疫学的検査のうち、グロブリンクラス別ウイルス抗体価 
        /// 11:血漿蛋白免疫学的検査のうち、特異的ＩｇＥ半定量・定量及びアレルゲン刺激性遊離ヒスタミン（ＨＲＴ） 
        /// 12: 悪性腫瘍遺伝子検査の包括項目
        /// </summary>
        public int HokatuKensa
        {
            get { return WrkSinKoui.HokatuKensa; }
            set
            {
                if (WrkSinKoui.HokatuKensa == value) return;
                WrkSinKoui.HokatuKensa = value;
                //RaisePropertyChanged(() => HokatuKensa);
            }
        }

        /// <summary>
        /// 回数小計
        /// 
        /// </summary>
        public int Count
        {
            get { return WrkSinKoui.Count; }
            set
            {
                if (WrkSinKoui.Count == value) return;
                WrkSinKoui.Count = value;
                //RaisePropertyChanged(() => Count);
            }
        }

        /// <summary>
        /// レセ非表示区分
        /// 1:非表示
        /// </summary>
        public int IsNodspRece
        {
            get { return WrkSinKoui.IsNodspRece; }
            set
            {
                if (WrkSinKoui.IsNodspRece == value) return;
                WrkSinKoui.IsNodspRece = value;
                //RaisePropertyChanged(() => IsNodspRece);
            }
        }

        /// <summary>
        /// 紙レセ非表示区分
        /// 1:非表示
        /// </summary>
        public int IsNodspPaperRece
        {
            get { return WrkSinKoui.IsNodspPaperRece; }
            set
            {
                if (WrkSinKoui.IsNodspPaperRece == value) return;
                WrkSinKoui.IsNodspPaperRece = value;
                //RaisePropertyChanged(() => IsNodspPaperRece);
            }
        }

        /// <summary>
        /// 院外処方区分
        /// 1:院外処方
        /// </summary>
        public int InoutKbn
        {
            get { return WrkSinKoui.InoutKbn; }
            set
            {
                if (WrkSinKoui.InoutKbn == value) return;
                WrkSinKoui.InoutKbn = value;
                //RaisePropertyChanged(() => InoutKbn);
            }
        }

        /// <summary>
        /// コード区分
        /// 代表項目のTEN_MST.CD_KBN
        /// </summary>
        public string CdKbn
        {
            get { return WrkSinKoui.CdKbn; }
            set
            {
                if (WrkSinKoui.CdKbn == value) return;
                WrkSinKoui.CdKbn = value;
                //RaisePropertyChanged(() => CdKbn);
            }
        }

        /// <summary>
        /// 代表レコード識別
        /// </summary>
        public string RecId
        {
            get { return WrkSinKoui.RecId; }
            set
            {
                if (WrkSinKoui.RecId == value) return;
                WrkSinKoui.RecId = value;
                //RaisePropertyChanged(() => RecId);
            }
        }

        /// <summary>
        /// 自費種別
        /// 代表項目のJIHI_SBT_MST.JIHI_SBT
        /// </summary>
        public int JihiSbt
        {
            get { return WrkSinKoui.JihiSbt; }
            set
            {
                if (WrkSinKoui.JihiSbt == value) return;
                WrkSinKoui.JihiSbt = value;
                //RaisePropertyChanged(() => JihiSbt);
            }
        }

        /// <summary>
        /// 課税区分
        /// TEN_MST.KAZEI_KBN
        /// </summary>
        public int KazeiKbn
        {
            get { return WrkSinKoui.KazeiKbn; }
            set
            {
                if (WrkSinKoui.KazeiKbn == value) return;
                WrkSinKoui.KazeiKbn = value;
                //RaisePropertyChanged(() => KazeiKbn);
            }
        }

        /// <summary>
        /// 削除フラグ
        ///     1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return WrkSinKoui.IsDeleted; }
            set
            {
                if (WrkSinKoui.IsDeleted == value) return;
                WrkSinKoui.IsDeleted = value;
                //RaisePropertyChanged(() => IsDeleted);
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return WrkSinKoui.CreateDate; }
            set
            {
                if (WrkSinKoui.CreateDate == value) return;
                WrkSinKoui.CreateDate = value;
                //RaisePropertyChanged(() => CreateDate);
            }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return WrkSinKoui.CreateId; }
            set
            {
                if (WrkSinKoui.CreateId == value) return;
                WrkSinKoui.CreateId = value;
                //RaisePropertyChanged(() => CreateId);
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return WrkSinKoui.CreateMachine ?? string.Empty; }
            set
            {
                if (WrkSinKoui.CreateMachine == value) return;
                WrkSinKoui.CreateMachine = value;
                //RaisePropertyChanged(() => CreateMachine);
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return WrkSinKoui.UpdateDate; }
            set
            {
                if (WrkSinKoui.UpdateDate == value) return;
                WrkSinKoui.UpdateDate = value;
                //RaisePropertyChanged(() => UpdateDate);
            }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return WrkSinKoui.UpdateId; }
            set
            {
                if (WrkSinKoui.UpdateId == value) return;
                WrkSinKoui.UpdateId = value;
                //RaisePropertyChanged(() => UpdateId);
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return WrkSinKoui.UpdateMachine ?? string.Empty; }
            set
            {
                if (WrkSinKoui.UpdateMachine == value) return;
                WrkSinKoui.UpdateMachine = value;
                //RaisePropertyChanged(() => UpdateMachine);
            }
        }

        public List<int> WeekCalcAppendDays
        {
            get { return _weekCalcAppendDays; }
        }

        public long OdrRpNo
        {
            get { return _odrRpNo; }
            set { _odrRpNo = value; }
        }
    }


}
