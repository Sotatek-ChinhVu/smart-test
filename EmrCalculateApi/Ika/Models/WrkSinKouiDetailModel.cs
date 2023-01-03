using Entity.Tenant;
using EmrCalculateApi.Ika.DB.Finder;
using Helper.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class WrkSinKouiDetailModel 
    {
        public WrkSinKouiDetail WrkSinKouiDetail { get; } = null;
        private TenMstModel _tenMst;
        private long _oyaRaiinNo;

        public WrkSinKouiDetailModel(WrkSinKouiDetail wrkSinKouiDetail)
        {
            WrkSinKouiDetail = wrkSinKouiDetail;
            _tenMst = null;
            BaseItemCd = "";
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return WrkSinKouiDetail.HpId; }
            set
            {
                if (WrkSinKouiDetail.HpId == value) return;
                WrkSinKouiDetail.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return WrkSinKouiDetail.PtId; }
            set
            {
                if (WrkSinKouiDetail.PtId == value) return;
                WrkSinKouiDetail.PtId = value;
                //RaisePropertyChanged(() => PtId);
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return WrkSinKouiDetail.SinDate; }
            set
            {
                if (WrkSinKouiDetail.SinDate == value) return;
                WrkSinKouiDetail.SinDate = value;
                //RaisePropertyChanged(() => SinDate);
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return WrkSinKouiDetail.RaiinNo; }
            set
            {
                if (WrkSinKouiDetail.RaiinNo == value) return;
                WrkSinKouiDetail.RaiinNo = value;
                //RaisePropertyChanged(() => RaiinNo);
            }
        }
        /// <summary>
        /// 診察開始時間
        /// </summary>
        public long OyaRaiinNo
        {
            get { return _oyaRaiinNo; }
            set { _oyaRaiinNo = value; }
        }
        /// <summary>
        /// 診察開始時間
        /// </summary>
        public string SinStartTime { get; set; }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        public int HokenKbn
        {
            get { return WrkSinKouiDetail.HokenKbn; }
            set
            {
                if (WrkSinKouiDetail.HokenKbn == value) return;
                WrkSinKouiDetail.HokenKbn = value;
                //RaisePropertyChanged(() => HokenKbn);
            }
        }

        /// <summary>
        /// 剤番号
        /// WRK_SIN_KOUI.RP_NO
        /// </summary>
        public int RpNo
        {
            get { return WrkSinKouiDetail.RpNo; }
            set
            {
                if (WrkSinKouiDetail.RpNo == value) return;
                WrkSinKouiDetail.RpNo = value;
                //RaisePropertyChanged(() => RpNo);
            }
        }

        /// <summary>
        /// 連番
        /// WRK_SIN_KOUI.SEQ_NO
        /// </summary>
        public int SeqNo
        {
            get { return WrkSinKouiDetail.SeqNo; }
            set
            {
                if (WrkSinKouiDetail.SeqNo == value) return;
                WrkSinKouiDetail.SeqNo = value;
                //RaisePropertyChanged(() => SeqNo);
            }
        }

        /// <summary>
        /// 行番号
        /// 
        /// </summary>
        public int RowNo
        {
            get { return WrkSinKouiDetail.RowNo; }
            set
            {
                if (WrkSinKouiDetail.RowNo == value) return;
                WrkSinKouiDetail.RowNo = value;
                //RaisePropertyChanged(() => RowNo);
            }
        }

        /// <summary>
        /// レコード識別
        /// レセプト電算に記録するレコード識別
        /// </summary>
        public string RecId
        {
            get { return WrkSinKouiDetail.RecId ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.RecId == value) return;
                WrkSinKouiDetail.RecId = value;
                //RaisePropertyChanged(() => RecId);
            }
        }

        /// <summary>
        /// 項目種別
        /// 1:コメント
        /// </summary>
        public int ItemSbt
        {
            get { return WrkSinKouiDetail.ItemSbt; }
            set
            {
                if (WrkSinKouiDetail.ItemSbt == value) return;
                WrkSinKouiDetail.ItemSbt = value;
                //RaisePropertyChanged(() => ItemSbt);
            }
        }

        /// <summary>
        /// 診療行為コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return WrkSinKouiDetail.ItemCd ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.ItemCd == value) return;
                WrkSinKouiDetail.ItemCd = value;
                //RaisePropertyChanged(() => ItemCd);
            }
        }

        /// <summary>
        /// オーダー診療行為コード
        /// </summary>
        public string OdrItemCd
        {
            get { return WrkSinKouiDetail.OdrItemCd ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.OdrItemCd == value) return;
                WrkSinKouiDetail.OdrItemCd = value;
                //RaisePropertyChanged(() => OdrItemCd);
            }
        }
        /// <summary>
        /// 項目名称
        /// 
        /// </summary>
        public string ItemName
        {
            get { return WrkSinKouiDetail.ItemName ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.ItemName == value) return;
                WrkSinKouiDetail.ItemName = value;
                //RaisePropertyChanged(() => ItemName);
            }
        }

        /// <summary>
        /// 数量
        /// 
        /// </summary>
        public double Suryo
        {
            get { return WrkSinKouiDetail.Suryo; }
            set
            {
                if (WrkSinKouiDetail.Suryo == value) return;
                WrkSinKouiDetail.Suryo = value;
                //RaisePropertyChanged(() => Suryo);
            }
        }

        /// <summary>
        /// 数量２
        /// レセ電にのみ記載する数量（分、ｃｍ２など）
        /// </summary>
        public double Suryo2
        {
            get { return WrkSinKouiDetail.Suryo2; }
            set
            {
                if (WrkSinKouiDetail.Suryo2 == value) return;
                WrkSinKouiDetail.Suryo2 = value;
                //RaisePropertyChanged(() => Suryo2);
            }
        }

        /// <summary>
        /// 書式区分
        /// 1: 列挙対象項目
        /// 2: 分
        /// </summary>
        public int FmtKbn
        {
            get { return WrkSinKouiDetail.FmtKbn; }
            set
            {
                if (WrkSinKouiDetail.FmtKbn == value) return;
                WrkSinKouiDetail.FmtKbn = value;
                //RaisePropertyChanged(() => FmtKbn);
            }
        }

        /// <summary>
        /// 単位コード
        /// 
        /// </summary>
        public int UnitCd
        {
            get { return WrkSinKouiDetail.UnitCd; }
            set
            {
                if (WrkSinKouiDetail.UnitCd == value) return;
                WrkSinKouiDetail.UnitCd = value;
                //RaisePropertyChanged(() => UnitCd);
            }
        }

        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        public string UnitName
        {
            get { return WrkSinKouiDetail.UnitName ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.UnitName == value) return;
                WrkSinKouiDetail.UnitName = value;
                //RaisePropertyChanged(() => UnitName);
            }
        }

        /// <summary>
        /// 点数識別
        /// "1: 金額（整数部7桁、小数部2桁）
        /// 2: 都道府県購入価格
        /// 3: 点数（プラス）
        /// 4: 都道府県購入価格（点数）、金額（整数部のみ）
        /// 5: %加算
        /// 6: %減算
        /// 7: 減点診療行為
        /// 8: 点数（マイナス）
        /// 9: 乗算割合
        /// 10: 除算金額（金額を10で除す。） ※ベントナイト用
        /// 11: 乗算金額（金額を10で乗ずる。） ※ステミラック注用"
        /// </summary>
        public int TenId
        {
            get { return WrkSinKouiDetail.TenId; }
            set
            {
                if (WrkSinKouiDetail.TenId == value) return;
                WrkSinKouiDetail.TenId = value;
                //RaisePropertyChanged(() => TenId);
            }
        }

        /// <summary>
        /// 点数
        /// 
        /// </summary>
        public double Ten
        {
            get { return WrkSinKouiDetail.Ten; }
            set
            {
                if (WrkSinKouiDetail.Ten == value) return;
                WrkSinKouiDetail.Ten = value;
                //RaisePropertyChanged(() => Ten);
            }
        }

        /// <summary>
        /// コード表用区分－区分
        /// "当該診療行為について医科点数表の章、部、区分番号及び項番を記録する。
        /// 区分（アルファベット部）：
        /// 　点数表の区分番号のアルファベット部を記録する。
        /// 　なお、介護老人保健施設入所者に係る診療料、医療観察法、入院時食事療養、入院時生活療養及び標準負担額については
        /// 　「－」（ハイホン）を、点数表に区分設定がないものは「＊」を記録する。
        /// 
        /// 章：
        /// 部：
        /// 区分番号：
        /// 枝番：
        /// 項番："
        /// </summary>
        public string CdKbn
        {
            get { return WrkSinKouiDetail.CdKbn ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.CdKbn == value) return;
                WrkSinKouiDetail.CdKbn = value;
                //RaisePropertyChanged(() => CdKbn);
            }
        }

        /// <summary>
        /// コード表用区分－区分番号
        /// コード表用区分－区分を参照。
        /// </summary>
        public int CdKbnno
        {
            get { return WrkSinKouiDetail.CdKbnno; }
            set
            {
                if (WrkSinKouiDetail.CdKbnno == value) return;
                WrkSinKouiDetail.CdKbnno = value;
                //RaisePropertyChanged(() => CdKbnno);
            }
        }

        /// <summary>
        /// コード表用区分－区分番号－枝番
        /// コード表用区分－区分を参照。
        /// </summary>
        public int CdEdano
        {
            get { return WrkSinKouiDetail.CdEdano; }
            set
            {
                if (WrkSinKouiDetail.CdEdano == value) return;
                WrkSinKouiDetail.CdEdano = value;
                //RaisePropertyChanged(() => CdEdano);
            }
        }

        /// <summary>
        /// コード表用区分－項番
        /// コード表用区分－区分を参照。
        /// </summary>
        public int CdKouno
        {
            get { return WrkSinKouiDetail.CdKouno; }
            set
            {
                if (WrkSinKouiDetail.CdKouno == value) return;
                WrkSinKouiDetail.CdKouno = value;
                //RaisePropertyChanged(() => CdKouno);
            }
        }


        /// <summary>
        /// 告示等識別区分１
        /// 
        /// </summary>
        public string Kokuji1
        {
            get { return WrkSinKouiDetail.Kokuji1 ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.Kokuji1 == value) return;
                WrkSinKouiDetail.Kokuji1 = value;
                //RaisePropertyChanged(() => Kokuji1);
            }
        }
        /// <summary>
        /// 告示等識別区分１
        /// 計算のソート順に使う時
        /// </summary>
        public string Kokuji1CalcSort
        {
            get
            {
                string ret = Kokuji1;

                if(Kokuji1 == "A" || string.IsNullOrEmpty(Kokuji1))
                {
                    if (string.IsNullOrEmpty(Kokuji1) && string.IsNullOrEmpty(ItemCd) == false && OdrItemCd.StartsWith("Z"))
                    {
                        ret = "0";
                    }
                    else
                    {
                        ret = "9";
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// 告示等識別区分２
        /// 
        /// </summary>
        public string Kokuji2
        {
            get { return WrkSinKouiDetail.Kokuji2 ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.Kokuji2 == value) return;
                WrkSinKouiDetail.Kokuji2 = value;
                //RaisePropertyChanged(() => Kokuji2);
            }
        }
        public string Kokuji2CalcSort
        {
            get 
            {
                string ret = Kokuji2;
                if (string.IsNullOrEmpty(Kokuji2) && string.IsNullOrEmpty(ItemCd) == false && ItemCd.StartsWith("Z"))
                {
                    ret = "0";
                }
                return ret;
            }
        }
        /// <summary>
        /// 通則年齢加算区分
        /// </summary>
        public int TusokuAge
        {
            get { return WrkSinKouiDetail.TusokuAge; }
            set
            {
                if (WrkSinKouiDetail.TusokuAge == value) return;
                WrkSinKouiDetail.TusokuAge = value;
                //RaisePropertyChanged(() => TusokuAge);
            }
        }

        /// <summary>
        /// 注加算コード
        /// 
        /// </summary>
        public string TyuCd
        {
            get { return WrkSinKouiDetail.TyuCd ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.TyuCd == value) return;
                WrkSinKouiDetail.TyuCd = value;
                //RaisePropertyChanged(() => TyuCd);
            }
        }

        /// <summary>
        /// 注加算通番
        /// 
        /// </summary>
        public string TyuSeq
        {
            get { return WrkSinKouiDetail.TyuSeq ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.TyuSeq == value) return;
                WrkSinKouiDetail.TyuSeq = value;
                //RaisePropertyChanged(() => TyuSeq);
            }
        }
        public string TyuSeqCalcSort
        {
            get
            {
                string ret = TyuSeq;
                if (string.IsNullOrEmpty(TyuSeq) && string.IsNullOrEmpty(ItemCd) == false && ItemCd.StartsWith("Z"))
                {
                    ret = "0";
                }
                return ret;
            }
        }
        /// <summary>
        /// 項目連番
        /// "コメント以外の項目で、オーダー順
        /// コメントの場合、付随する項目と同じ番号"
        /// </summary>
        public int ItemSeqNo
        {
            get { return WrkSinKouiDetail.ItemSeqNo; }
            set
            {
                if (WrkSinKouiDetail.ItemSeqNo == value) return;
                WrkSinKouiDetail.ItemSeqNo = value;
                //RaisePropertyChanged(() => ItemSeqNo);
            }
        }

        /// <summary>
        /// 項目枝番
        /// ITEM_SEQ_NO内の連番（オーダー順）
        /// </summary>
        public int ItemEdaNo
        {
            get { return WrkSinKouiDetail.ItemEdaNo; }
            set
            {
                if (WrkSinKouiDetail.ItemEdaNo == value) return;
                WrkSinKouiDetail.ItemEdaNo = value;
                //RaisePropertyChanged(() => ItemEdaNo);
            }
        }

        /// <summary>
        /// レセ非表示区分
        /// "1:非表示
        /// 2:電算のみ非表示"
        /// </summary>
        public int IsNodspRece
        {
            get { return WrkSinKouiDetail.IsNodspRece; }
            set
            {
                if (WrkSinKouiDetail.IsNodspRece == value) return;
                WrkSinKouiDetail.IsNodspRece = value;
                //RaisePropertyChanged(() => IsNodspRece);
            }
        }

        /// <summary>
        /// 紙レセ非表示区分
        /// 1:非表示
        /// </summary>
        public int IsNodspPaperRece
        {
            get { return WrkSinKouiDetail.IsNodspPaperRece; }
            set
            {
                if (WrkSinKouiDetail.IsNodspPaperRece == value) return;
                WrkSinKouiDetail.IsNodspPaperRece = value;
                //RaisePropertyChanged(() => IsNodspPaperRece);
            }
        }

        /// <summary>
        /// 領収証非表示区分
        /// 1:非表示
        /// </summary>
        public int IsNodspRyosyu
        {
            get { return WrkSinKouiDetail.IsNodspRyosyu; }
            set
            {
                if (WrkSinKouiDetail.IsNodspRyosyu == value) return;
                WrkSinKouiDetail.IsNodspRyosyu = value;
                //RaisePropertyChanged(() => IsNodspRyosyu);
            }
        }

        /// <summary>
        /// 自動発生項目
        /// 1:自動発生項目
        /// </summary>
        public int IsAutoAdd
        {
            get { return WrkSinKouiDetail.IsAutoAdd; }
            set
            {
                if (WrkSinKouiDetail.IsAutoAdd == value) return;
                WrkSinKouiDetail.IsAutoAdd = value;
                //RaisePropertyChanged(() => IsAutoAdd);
            }
        }

        /// <summary>
        /// コメント文
        /// 
        /// </summary>
        public string CmtOpt
        {
            get { return WrkSinKouiDetail.CmtOpt ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.CmtOpt == value) return;
                WrkSinKouiDetail.CmtOpt = value;
                //RaisePropertyChanged(() => CmtOpt);
            }
        }

        /// <summary>
        /// コメント１
        /// 
        /// </summary>
        public string Cmt1
        {
            get { return WrkSinKouiDetail.Cmt1 ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.Cmt1 == value) return;
                WrkSinKouiDetail.Cmt1 = value;
                //RaisePropertyChanged(() => Cmt1);
            }
        }

        /// <summary>
        /// コメントコード１
        /// 
        /// </summary>
        public string CmtCd1
        {
            get { return WrkSinKouiDetail.CmtCd1 ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.CmtCd1 == value) return;
                WrkSinKouiDetail.CmtCd1 = value;
                //RaisePropertyChanged(() => CmtCd1);
            }
        }

        /// <summary>
        /// コメント文１
        /// 
        /// </summary>
        public string CmtOpt1
        {
            get { return WrkSinKouiDetail.CmtOpt1 ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.CmtOpt1 == value) return;
                WrkSinKouiDetail.CmtOpt1 = value;
                //RaisePropertyChanged(() => CmtOpt1);
            }
        }

        /// <summary>
        /// コメント２
        /// 
        /// </summary>
        public string Cmt2
        {
            get { return WrkSinKouiDetail.Cmt2 ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.Cmt2 == value) return;
                WrkSinKouiDetail.Cmt2 = value;
                //RaisePropertyChanged(() => Cmt2);
            }
        }

        /// <summary>
        /// コメントコード２
        /// 
        /// </summary>
        public string CmtCd2
        {
            get { return WrkSinKouiDetail.CmtCd2 ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.CmtCd2 == value) return;
                WrkSinKouiDetail.CmtCd2 = value;
                //RaisePropertyChanged(() => CmtCd2);
            }
        }

        /// <summary>
        /// コメント文２
        /// 
        /// </summary>
        public string CmtOpt2
        {
            get { return WrkSinKouiDetail.CmtOpt2 ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.CmtOpt2 == value) return;
                WrkSinKouiDetail.CmtOpt2 = value;
                //RaisePropertyChanged(() => CmtOpt2);
            }
        }

        /// <summary>
        /// コメント３
        /// 
        /// </summary>
        public string Cmt3
        {
            get { return WrkSinKouiDetail.Cmt3 ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.Cmt3 == value) return;
                WrkSinKouiDetail.Cmt3 = value;
                //RaisePropertyChanged(() => Cmt3);
            }
        }

        /// <summary>
        /// コメントコード３
        /// 
        /// </summary>
        public string CmtCd3
        {
            get { return WrkSinKouiDetail.CmtCd3 ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.CmtCd3 == value) return;
                WrkSinKouiDetail.CmtCd3 = value;
                //RaisePropertyChanged(() => CmtCd3);
            }
        }

        /// <summary>
        /// コメント文３
        /// 
        /// </summary>
        public string CmtOpt3
        {
            get { return WrkSinKouiDetail.CmtOpt3 ?? string.Empty; }
            set
            {
                if (WrkSinKouiDetail.CmtOpt3 == value) return;
                WrkSinKouiDetail.CmtOpt3 = value;
                //RaisePropertyChanged(() => CmtOpt3);
            }
        }

        /// <summary>
        /// 削除フラグ
        ///     1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return WrkSinKouiDetail.IsDeleted; }
            set
            {
                if (WrkSinKouiDetail.IsDeleted == value) return;
                WrkSinKouiDetail.IsDeleted = value;
                //RaisePropertyChanged(() => IsDeleted);
            }
        }

        /// <summary>
        /// 自動発生コメントのベースになった項目の診療行為コード
        /// </summary>
        public string BaseItemCd { get; set; }
        /// <summary>
        /// 自動発生コメントのベースになった項目があるSEQ_NO
        /// </summary>
        public int BaseSeqNo { get; set; } = 0;

        /// <summary>
        /// オーダーコメントを追加する
        /// </summary>
        /// <param name="cmt">コメント</param>
        /// <param name="cd">診療行為コード</param>
        /// <param name="opt">オプション</param>
        public void AddComment(string cmt, string cd, string opt)
        {
            if (cmt != "" || cd != "" || opt != "")
            {
                if ((cmt != "" || opt != "") && cd == "")
                {
                    cd = ItemCdConst.CommentFree;
                }

                if (WrkSinKouiDetail.CmtCd1 == "")
                {
                    WrkSinKouiDetail.Cmt1 = cmt;
                    WrkSinKouiDetail.CmtCd1 = cd;
                    WrkSinKouiDetail.CmtOpt1 = opt;
                }
                else if (WrkSinKouiDetail.CmtCd2 == "")
                {
                    WrkSinKouiDetail.Cmt2 = cmt;
                    WrkSinKouiDetail.CmtCd2 = cd;
                    WrkSinKouiDetail.CmtOpt2 = opt;
                }
                else if (WrkSinKouiDetail.CmtCd3 == "")
                {
                    WrkSinKouiDetail.Cmt3 = cmt;
                    WrkSinKouiDetail.CmtCd3 = cd;
                    WrkSinKouiDetail.CmtOpt3 = opt;
                }
            }
        }

        public TenMstModel TenMst
        {
            get { return _tenMst; }
            set { _tenMst = value; }
        }
        /// <summary>
        /// マスター種別
        ///     S: 診療行為
        ///     Y: 医薬品
        ///     T: 特材
        ///     C: コメント
        ///     R: 労災
        ///     U: 労災特定器材
        ///     D: 労災コメントマスタ
        /// </summary>
        public string MasterSbt
        {
            get { return _tenMst != null ? _tenMst.MasterSbt : ""; }
        }
        /// <summary>
        /// 診療行為区分
        /// </summary>
        public int SinKouiKbn
        {
            get { return _tenMst != null ? _tenMst.SinKouiKbn : 0; }
        }
        /// <summary>
        /// 薬剤区分
        ///     当該医薬品の薬剤区分を表す。
        ///       0: 薬剤以外
        ///     　1: 内用薬
        ///     　3: その他
        ///     　4: 注射薬
        ///     　6: 外用薬
        ///     　8: 歯科用薬剤
        ///     （削）9: 歯科特定薬剤
        ///     ※レセプト電算マスターの項目「剤型」を収容する。
        /// </summary>
        public int DrugKbn
        {
            get { return _tenMst != null ? _tenMst.DrugKbn : 0; }
        }
        /// <summary>
        /// 包括区分
        /// 
        /// </summary>
        public int HokatuKbn
        {
            get { return _tenMst != null ? _tenMst.HokatuKbn : 0; }
        }
        /// <summary>
        /// 通則加算所定点数対象区分
        ///     通則加算を行う場合において、所定点数として取扱うか否かを表す。
        ///     　0: 所定点数として取扱う診療行為及び通則加算
        ///     　1: 所定点数として取扱わない基本診療行為
        /// </summary>
        public int TusokuTargetKbn
        {
            get { return _tenMst != null ? _tenMst.TusokuTargetKbn : 0; }
        }
        /// <summary>
        /// 部位区分
        /// </summary>
        public int BuiKbn
        {
            get { return _tenMst != null ? _tenMst.BuiKbn : 0; }
        }
        /// <summary>
        /// コメント種別
        /// コメントマスターの場合、コメントの種類を表す。
        /// 0:下記以外
        /// 1:初回日
        /// 2:前回日
        /// 3:実施日
        /// 4:手術日
        /// 5:発症日
        /// 6:治療開始日
        /// 7:発症日または治療開始日
        /// 8:急性憎悪
        /// 9:初回診断
        /// 10:診療時間
        /// 11:疾患名
        /// 20:撮影部位
        /// 21:撮影部位（胸部）
        /// 22:撮影部位（腹部）
        /// </summary>
        public int CmtSbt
        {
            get { return _tenMst != null ? _tenMst.CmtSbt　: 0; }
        }
        /// <summary>
        /// 基本項目(Kokuji1 in (1,3,5))の場合、tureを返す
        /// </summary>
        public bool IsKihonKoumoku
        {
            get { return (new string[] { "1", "3", "5" }.Contains(Kokuji1)); }
        }

        /// <summary>
        /// 基本項目(Kokuji2 in (1,3,5))の場合、tureを返す
        /// </summary>
        public bool IsKihonKoumoku2
        {
            get { return (new string[] { "1", "3", "5" }.Contains(Kokuji2)); }
        }

        /// <summary>
        /// 薬剤(DrugKbn > 0)の場合、trueを返す
        /// </summary>
        public bool IsYakuzai
        {
            get { return TenMst != null && TenMst.DrugKbn > 0; }
        }

        /// <summary>
        /// 特材(MasterSbt="T" or "U")の場合、trueを返す
        /// </summary>
        public bool IsTokuzai
        {
            get { return TenMst != null && (TenMst.MasterSbt == "T" || TenMst.MasterSbt == "U"); }
        }

        /// <summary>
        /// 円項目の場合、trueを返す
        /// </summary>
        public bool IsEnKoumoku
        {
            get
            {
                bool ret = false;

                if (TenMst != null)
                {
                    if (TenMst.MasterSbt != "Y" && TenMst.MasterSbt != "T" && TenMst.ItemCd.Length == 9 && TenMst.BuiKbn == 0)
                    {
                        if (new int[] { 1, 2, 4, 10, 11 }.Contains(TenId))
                        {
                            ret = true;
                        }
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// 労災円項目の場合、trueを返す
        /// </summary>
        public bool IsRosaiEnKoumoku
        {
            get
            {
                bool ret = false;

                if (TenMst != null)
                {
                    if (new int[] { 99 }.Contains(TenId))
                    {
                        ret = true;
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// 診断料項目の場合、trueを返す
        /// </summary>
        public bool IsSindan
        {
            get { return (TenMst != null && CdKbn == "E" && CdKbnno == 1 && TenMst.KizamiId == 1); }
        }

        /// <summary>
        /// 撮影料項目の場合、trueを返す
        /// </summary>
        public bool IsSatuei
        {
            get { return (TenMst != null && CdKbn == "E" && CdKbnno == 2 && TenMst.KizamiId == 1); }
        }
        /// <summary>
        /// ゼロ扱いする注コードかどうかチェック
        /// TYU_CD="0" or (TYU_CD=""で特材の場合)
        /// </summary>
        public bool IsZeroTyuCd
        {
            get
            {
                bool ret = false;
                if (TyuCd == "0" ||
                    (TyuCd == "" && ItemCd != null && (ItemCd.StartsWith("Z") || ItemCd.StartsWith("7"))))
                {
                    ret = true;
                }
                return ret;
            }        
        }
        /// <summary>
        /// true - 逓減対象項目は当来院に存在する（Adjustで再チェックが必要）
        /// </summary>
        public bool TeigenTargetInRaiin { get; set; } = false;
        public string TeigenCdKbn { get; set; } = "";
        public int TeigenCdKbnno { get; set; } = 0;
        public int TeigenCdEdano { get; set; } = 0;
        public int TeigenCdKouno { get; set; } = 0;
        public int TeigenHokatuKbn { get; set; } = 0;
        /// <summary>
        /// 最低薬価（院外処方一般名処方時のみセットする）
        /// </summary>
        public double MinYakka { get; set; } = 0;
        /// <summary>
        /// 点数を0にする　院外処方の薬剤に使用
        /// </summary>
        public bool TenZero { get; set; } = false;
        /// <summary>
        /// 点数調整のある項目
        /// 検査丸目の分点で使用
        /// </summary>
        public bool AdjustTensu { get; set; } = false;
        /// <summary>
        /// 算定漏れチェック用項目の場合、true
        /// </summary>
        public bool IsDummy { get; set; } = false;
    }


}
