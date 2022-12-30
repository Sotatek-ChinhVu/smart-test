using Entity.Tenant;
using EmrCalculateApi.Ika.Models;

namespace EmrCalculateApi.Ika.Models
{
    public class CalcLogModel
    {
        public CalcLog CalcLog { get; } = null;

        public CalcLogModel(CalcLog calcLog)
        {
            CalcLog = calcLog;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return CalcLog.HpId; }
            set
            {
                if (CalcLog.HpId == value) return;
                CalcLog.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return CalcLog.PtId; }
            set
            {
                if (CalcLog.PtId == value) return;
                CalcLog.PtId = value;
                //RaisePropertyChanged(() => PtId);
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return CalcLog.SinDate; }
            set
            {
                if (CalcLog.SinDate == value) return;
                CalcLog.SinDate = value;
                //RaisePropertyChanged(() => SinDate);
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return CalcLog.RaiinNo; }
            set
            {
                if (CalcLog.RaiinNo == value) return;
                CalcLog.RaiinNo = value;
                //RaisePropertyChanged(() => RaiinNo);
            }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return CalcLog.SeqNo; }
            set
            {
                if (CalcLog.SeqNo == value) return;
                CalcLog.SeqNo = value;
                //RaisePropertyChanged(() => SeqNo);
            }
        }

        /// <summary>
        /// ログ種別
        /// 0:通常 1:注意 2:警告
        /// </summary>
        public int LogSbt
        {
            get { return CalcLog.LogSbt; }
            set
            {
                if (CalcLog.LogSbt == value) return;
                CalcLog.LogSbt = value;
                //RaisePropertyChanged(() => LogSbt);
            }
        }

        /// <summary>
        /// ログ
        /// 
        /// </summary>
        public string Text
        {
            get { return CalcLog.Text ?? string.Empty; }
            set
            {
                if (CalcLog.Text == value) return;
                CalcLog.Text = value;
                //RaisePropertyChanged(() => Text);
            }
        }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return CalcLog.HokenId; }
            set
            {
                if (CalcLog.HokenId == value) return;
                CalcLog.HokenId = value;
                //RaisePropertyChanged(() => HokenId);
            }
        }
        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return CalcLog.ItemCd ?? string.Empty; }
            set
            {
                if (CalcLog.ItemCd == value) return;
                CalcLog.ItemCd = value;
                //RaisePropertyChanged(() => ItemCd);
            }
        }
        /// <summary>
        /// 削除項目コード
        /// 
        /// </summary>
        public string DelItemCd
        {
            get { return CalcLog.DelItemCd ?? string.Empty; }
            set
            {
                if (CalcLog.DelItemCd == value) return;
                CalcLog.DelItemCd = value;
                //RaisePropertyChanged(() => DelItemCd);
            }
        }
        /// <summary>
        /// 削除種別
        /// 0:包括
        /// 1:背反
        /// 2:特殊
        /// 3:外来管理加算
        /// 4:優先順背反
        /// 5:注加算
        /// 6:外来管理加算（同一診療）
        /// 7:ある項目が存在しないために算定できない場合
        /// 8:ある項目が存在しないために算定できない場合（警告）
        /// 9:削除項目に付随して削除される項目
        /// 10:注加算項目で、同一Rp内に対応する基本項目がない項目
        /// 11:背反特殊
        /// 12:加算基本項目なし
        /// 13:加算基本項目なし（警告）
        /// 14:自賠・自費以外の保険で自賠文書料を算定
        /// 15:注射手技で薬剤がないため算定できない
        /// 100:算定回数上限
        /// </summary>
        public int DelSbt
        {
            get { return CalcLog.DelSbt; }
            set
            {
                if (CalcLog.DelSbt == value) return;
                CalcLog.DelSbt = value;
                //RaisePropertyChanged(() => DelSbt);
            }
        }
        /// <summary>
        /// 警告
        /// 0:削除 1:警告
        /// </summary>
        public int IsWarning
        {
            get { return CalcLog.IsWarning; }
            set
            {
                if (CalcLog.IsWarning == value) return;
                CalcLog.IsWarning = value;
                //RaisePropertyChanged(() => IsWarning);
            }
        }
        /// <summary>
        /// チェック期間数
        /// TERM_SBTと組み合わせて使用
        /// ※TERM_SBT in (2,5,6)のときのみ有効
        /// 例）2日の場合、TERM_CNT=2, TERM_SBT=2と登録
        /// </summary>
        public int TermCnt
        {
            get { return CalcLog.TermCnt; }
            set
            {
                if (CalcLog.TermCnt == value) return;
                CalcLog.TermCnt = value;
                //RaisePropertyChanged(() => TermCnt);
            }
        }
        /// <summary>
        /// チェック期間種別
        /// 0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </summary>
        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public int TermSbt
        {
            get { return CalcLog.TermSbt; }
            set
            {
                if (CalcLog.TermSbt == value) return;
                CalcLog.TermSbt = value;
                //RaisePropertyChanged(() => TermSbt);
            }
        }
        public DateTime CreateDate
        {
            get { return CalcLog.CreateDate; }
            set
            {
                if (CalcLog.CreateDate == value) return;
                CalcLog.CreateDate = value;
                //RaisePropertyChanged(() => CreateDate);
            }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return CalcLog.CreateId; }
            set
            {
                if (CalcLog.CreateId == value) return;
                CalcLog.CreateId = value;
                //RaisePropertyChanged(() => CreateId);
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return CalcLog.CreateMachine ?? string.Empty; }
            set
            {
                if (CalcLog.CreateMachine == value) return;
                CalcLog.CreateMachine = value;
                //RaisePropertyChanged(() => CreateMachine);
            }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return CalcLog.UpdateDate; }
            set
            {
                if (CalcLog.UpdateDate == value) return;
                CalcLog.UpdateDate = value;
                //RaisePropertyChanged(() => UpdateDate);
            }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return CalcLog.UpdateId; }
            set
            {
                if (CalcLog.UpdateId == value) return;
                CalcLog.UpdateId = value;
                //RaisePropertyChanged(() => UpdateId);
            }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return CalcLog.UpdateMachine ?? string.Empty; }
            set
            {
                if (CalcLog.UpdateMachine == value) return;
                CalcLog.UpdateMachine = value;
                //RaisePropertyChanged(() => UpdateMachine);
            }
        }


    }

}
