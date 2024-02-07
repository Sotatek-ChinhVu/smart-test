using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateService.Ika.Models
{
    public class WrkSinKouiDetailDelModel 
    {
        public WrkSinKouiDetailDel WrkSinKouiDetailDel { get; } = null;

        private List<string> _delItemCds;
        private int _isDeleted;

        public WrkSinKouiDetailDelModel(WrkSinKouiDetailDel wrkSinKouiDetailDel)
        {
            WrkSinKouiDetailDel = wrkSinKouiDetailDel;
            _delItemCds = new List<string>();
            _isDeleted = 0;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return WrkSinKouiDetailDel.HpId; }
            set
            {
                if (WrkSinKouiDetailDel.HpId == value) return;
                WrkSinKouiDetailDel.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return WrkSinKouiDetailDel.PtId; }
            set
            {
                if (WrkSinKouiDetailDel.PtId == value) return;
                WrkSinKouiDetailDel.PtId = value;
                //RaisePropertyChanged(() => PtId);
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return WrkSinKouiDetailDel.SinDate; }
            set
            {
                if (WrkSinKouiDetailDel.SinDate == value) return;
                WrkSinKouiDetailDel.SinDate = value;
                //RaisePropertyChanged(() => SinDate);
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return WrkSinKouiDetailDel.RaiinNo; }
            set
            {
                if (WrkSinKouiDetailDel.RaiinNo == value) return;
                WrkSinKouiDetailDel.RaiinNo = value;
                //RaisePropertyChanged(() => RaiinNo);
            }
        }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        public int HokenKbn
        {
            get { return WrkSinKouiDetailDel.HokenKbn; }
            set
            {
                if (WrkSinKouiDetailDel.HokenKbn == value) return;
                WrkSinKouiDetailDel.HokenKbn = value;
                //RaisePropertyChanged(() => HokenKbn);
            }
        }

        /// <summary>
        /// 剤番号
        /// WRK_SIN_KOUI_DETAIL.RP_NO
        /// </summary>
        public int RpNo
        {
            get { return WrkSinKouiDetailDel.RpNo; }
            set
            {
                if (WrkSinKouiDetailDel.RpNo == value) return;
                WrkSinKouiDetailDel.RpNo = value;
                //RaisePropertyChanged(() => RpNo);
            }
        }

        /// <summary>
        /// 連番
        /// WRK_SIN_KOUI_DETAIL.SEQ_NO
        /// </summary>
        public int SeqNo
        {
            get { return WrkSinKouiDetailDel.SeqNo; }
            set
            {
                if (WrkSinKouiDetailDel.SeqNo == value) return;
                WrkSinKouiDetailDel.SeqNo = value;
                //RaisePropertyChanged(() => SeqNo);
            }
        }

        /// <summary>
        /// 行番号
        /// WRK_SIN_KOUI_DETAIL.ROW_NO
        /// </summary>
        public int RowNo
        {
            get { return WrkSinKouiDetailDel.RowNo; }
            set
            {
                if (WrkSinKouiDetailDel.RowNo == value) return;
                WrkSinKouiDetailDel.RowNo = value;
                //RaisePropertyChanged(() => RowNo);
            }
        }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return WrkSinKouiDetailDel.ItemCd ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetailDel.ItemCd == value) return;
                WrkSinKouiDetailDel.ItemCd = value;
                //RaisePropertyChanged(() => ItemCd);
            }
        }

        /// <summary>
        /// 項目連番
        /// 同一WRK_CALC_NO,RP_NO,ROW_NO内の連番
        /// </summary>
        public int ItemSeqNo
        {
            get { return WrkSinKouiDetailDel.ItemSeqNo; }
            set
            {
                if (WrkSinKouiDetailDel.ItemSeqNo == value) return;
                WrkSinKouiDetailDel.ItemSeqNo = value;
                //RaisePropertyChanged(() => ItemSeqNo);
            }
        }

        /// <summary>
        /// 削除項目コード
        /// 当該項目が削除される理由となった項目のITEM_CD
        /// </summary>
        public string DelItemCd
        {
            get { return WrkSinKouiDetailDel.DelItemCd ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetailDel.DelItemCd == value) return;
                WrkSinKouiDetailDel.DelItemCd = value;
                //RaisePropertyChanged(() => DelItemCd);
            }
        }

        public List<string> DelItemCds
        {
            get { return _delItemCds; }
            set{_delItemCds = value;}
        }

        /// <summary>
        /// 削除項目算定日
        /// 削除項目の算定日
        /// 0の場合、当来院
        /// </summary>
        public int SanteiDate
        {
            get { return WrkSinKouiDetailDel.SanteiDate; }
            set
            {
                if (WrkSinKouiDetailDel.SanteiDate == value) return;
                WrkSinKouiDetailDel.SanteiDate = value;
                //RaisePropertyChanged(() => SanteiDate);
            }
        }
        /// <summary>
        /// 削除種別
        /// 0:包括 1:背反 2:マスタ外
        /// </summary>
        public int DelSbt
        {
            get { return WrkSinKouiDetailDel.DelSbt; }
            set
            {
                if (WrkSinKouiDetailDel.DelSbt == value) return;
                WrkSinKouiDetailDel.DelSbt = value;
                //RaisePropertyChanged(() => DelSbt);
            }
        }

        /// <summary>
        /// 警告
        /// 0:削除 1:警告 2:いずれかを算定（背反用） 3:いずれかを算定（背反用）(警告)
        /// </summary>
        public int IsWarning
        {
            get { return WrkSinKouiDetailDel.IsWarning; }
            set
            {
                if (WrkSinKouiDetailDel.IsWarning == value) return;
                WrkSinKouiDetailDel.IsWarning = value;
                //RaisePropertyChanged(() => IsWarning);
            }
        }

        /// <summary>
        /// チェック期間数
        /// TERM_SBTと組み合わせて使用
        /// ※TERM_SBT in (1,4)のときのみ有効
        /// 例）2日の場合、TERM_CNT=2, TERM_SBT=1と登録
        /// </summary>
        public int TermCnt
        {
            get { return WrkSinKouiDetailDel.TermCnt; }
            set
            {
                if (WrkSinKouiDetailDel.TermCnt == value) return;
                WrkSinKouiDetailDel.TermCnt = value;
                //RaisePropertyChanged(() => IsWarning);
            }
        }

        /// <summary>
        /// チェック期間種別
        /// 0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </summary>
        public int TermSbt
        {
            get { return WrkSinKouiDetailDel.TermSbt; }
            set
            {
                if (WrkSinKouiDetailDel.TermSbt == value) return;
                WrkSinKouiDetailDel.TermSbt = value;
                //RaisePropertyChanged(() => IsWarning);
            }
        }

        public int IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }

        public string CdKbn
        {
            get { return WrkSinKouiDetailDel.CdKbn; }
            set
            {
                if (WrkSinKouiDetailDel.CdKbn == value) return;
                WrkSinKouiDetailDel.CdKbn = value;
                //RaisePropertyChanged(() => CdKbn);
            }
        }

        public int CdKbnno
        {
            get { return WrkSinKouiDetailDel.CdKbnno; }
            set
            {
                if (WrkSinKouiDetailDel.CdKbnno == value) return;
                WrkSinKouiDetailDel.CdKbnno = value;
                //RaisePropertyChanged(() => CdKbnno);
            }
        }

        public int CdEdano 
        {
            get { return WrkSinKouiDetailDel.CdEdano; } 
            set
            {
                if (WrkSinKouiDetailDel.CdEdano == value) return;
                WrkSinKouiDetailDel.CdEdano = value;
                //RaisePropertyChanged(() => CdEdano);
            }
        }

        public int CdKouno 
        {
            get { return WrkSinKouiDetailDel.CdKouno; }
            set
            {
                if (WrkSinKouiDetailDel.CdKouno == value) return;
                WrkSinKouiDetailDel.CdKouno = value;
                //RaisePropertyChanged(() => CdKouno);
            }
        }

        public string Kokuji1 
        {
            get { return WrkSinKouiDetailDel.Kokuji1; }
            set
            {
                if (WrkSinKouiDetailDel.Kokuji1 == value) return;
                WrkSinKouiDetailDel.Kokuji1 = value;
                //RaisePropertyChanged(() => Kokuji1);
            }
        }

        public string Kokuji2 
        {
            get { return WrkSinKouiDetailDel.Kokuji2; }
            set
            {
                if (WrkSinKouiDetailDel.Kokuji2 == value) return;
                WrkSinKouiDetailDel.Kokuji2 = value;
                //RaisePropertyChanged(() => Kokuji2);
            }
        }

        /// <summary>
        /// 自動算定フラグ
        /// </summary>
        public int IsAutoAdd { get; set; } = 0;
        public int HokenId { get; set; } = 0;
        /// <summary>
        /// 対象が存在した期間
        /// 0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </summary>
        public int ExistTermSbt { get; set; } = 0;

        private List<(string cdKbn, int cdKbnno, int cdEdano, int cdKouno, string kokuji1, string kokuji2)> getEfTargetList()
        {
            List<(string cdKbn, int cdKbnno, int cdEdano, int cdKouno, string kokuji1, string kokuji2)> result;

            //if (SinDate >= 20230401)  // ※例です。世代を切る必要が生じた場合は、このように条件を追加して対応する
            //{
                result =
                    new List<(string cdKbn, int cdKbnno, int cdEdano, int cdKouno, string kokuji1, string kokuji2)>()
                    {
                                    // B001-15 慢性維持透析患者外来医学管理料
                                    ("B", 1, 0, 15, "1", "1"),
                                    // B001-2 小児科外来診療料（１日につき）
                                    ("B", 1, 2, 1, "1" , "1"),
                                    ("B", 1, 2, 2, "1" , "1"),
                                    // B001-2-9 地域包括診療料（月１回）
                                    ("B", 1, 2, 12, "1" , "1"),
                                    // B001-2-10 認知症地域包括診療料（月１回）
                                    ("B", 1, 2, 15, "1" , "1"),
                                    // B001-2-11 小児かかりつけ診療料（１日につき）
                                    ("B", 1, 2, 19, "1" , "1"),
                                    // B001-3 生活習慣病管理料
                                    ("B", 1, 3, 1, "1" , "1"),
                                    // B001-4 手術前医学管理料
                                    ("B", 1, 4, 0, "1" , "1"),
                                    // C002 在宅時医学総合管理料（月１回）
                                    ("C", 2, 0, 1, "1", "1"),
                                    ("C", 2, 0, 2, "1" , "1"),
                                    ("C", 2, 0, 3, "1" , "1"),
                                    // C002-2 施設入居時等医学総合管理料（月１回）
                                    ("C", 2, 2, 1, "1" , "1"),
                                    ("C", 2, 2, 2, "1" , "1"),
                                    ("C", 2, 2, 3, "1" , "1"),
                                    // C003 在宅がん医療総合診療料（１日につき）
                                    ("C", 3, 0, 1, "1" , "1"),
                                    ("C", 3, 0, 2, "1" , "1")
                    };
            //}

            return result;

        }
        /// <summary>
        /// EFファイル、対象医学管理等項目に該当するかどうか
        /// </summary>
        /// <returns>false - 該当しない true - 該当する</returns>
        public bool IsEFIgaku()
        {
            List<(string cdKbn, int cdKbnno, int cdEdano, int cdKouno, string kokuji1, string kokuji2)> efTargets =
                getEfTargetList();

            return efTargets.Any(p =>
                p.cdKbn == CdKbn &&
                p.cdKbnno == CdKbnno &&
                p.cdEdano == CdEdano &&
                p.cdKouno == CdKouno &&
                p.kokuji1 == Kokuji1 &&
                p.kokuji2 == Kokuji2
            );
        }
    }


}
