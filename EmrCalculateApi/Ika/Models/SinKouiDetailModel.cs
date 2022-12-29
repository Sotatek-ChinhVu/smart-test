using Helper.Constants;
using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class SinKouiDetailModel
    {
        public SinKouiDetail SinKouiDetail { get; } = null;

        private int _updateState = 0;
        private int _keyNo = 0;
        private int _itemSeqNo = 0;

        TenMstModel _tenMst;

        public SinKouiDetailModel(SinKouiDetail sinKouiDetail, TenMst tenMst)
        {
            SinKouiDetail = sinKouiDetail;
            if (tenMst != null)
            {
                _tenMst = new TenMstModel(tenMst);
            }
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SinKouiDetail.HpId; }
            set
            {
                if (SinKouiDetail.HpId == value) return;
                SinKouiDetail.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return SinKouiDetail.PtId; }
            set
            {
                if (SinKouiDetail.PtId == value) return;
                SinKouiDetail.PtId = value;
                //RaisePropertyChanged(() => PtId);
            }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return SinKouiDetail.SinYm; }
            set
            {
                if (SinKouiDetail.SinYm == value) return;
                SinKouiDetail.SinYm = value;
                //RaisePropertyChanged(() => SinYm);
            }
        }

        /// <summary>
        /// 剤番号
        /// SIN_KOUI.RP_NO
        /// </summary>
        public int RpNo
        {
            get { return SinKouiDetail.RpNo; }
            set
            {
                if (SinKouiDetail.RpNo == value) return;
                SinKouiDetail.RpNo = value;
                //RaisePropertyChanged(() => RpNo);
            }
        }

        /// <summary>
        /// 連番
        /// SIN_KOUI.SEQ_NO
        /// </summary>
        public int SeqNo
        {
            get { return SinKouiDetail.SeqNo; }
            set
            {
                if (SinKouiDetail.SeqNo == value) return;
                SinKouiDetail.SeqNo = value;
                //RaisePropertyChanged(() => SeqNo);
            }
        }

        /// <summary>
        /// 行番号
        /// 
        /// </summary>
        public int RowNo
        {
            get { return SinKouiDetail.RowNo; }
            set
            {
                if (SinKouiDetail.RowNo == value) return;
                SinKouiDetail.RowNo = value;
                //RaisePropertyChanged(() => RowNo);
            }
        }


        /// <summary>
        /// レコード識別
        /// レセプト電算に記録するレコード識別
        /// </summary>
        public string RecId
        {
            get { return SinKouiDetail.RecId ?? string.Empty; }
            set
            {
                if (SinKouiDetail.RecId == value) return;
                SinKouiDetail.RecId = value;
                //RaisePropertyChanged(() => RecId);
            }
        }

        /// <summary>
        /// レコード識別を並び順に変換した値を返す
        /// レセプト電算ソート用
        /// </summary>
        public int RecIdNo
        {
            get
            {
                int ret = 0;
                if(RecId == "CO")
                {
                    ret = 0;
                }
                else if(RecId == "SI" || RecId == "RI")
                {
                    ret = 1;
                }
                else if(RecId == "IY")
                {
                    ret = 2;
                }
                else if(RecId == "TO")
                {
                    ret = 3;
                }
                return ret;
            }
        }

        /// <summary>
        /// 項目種別
        /// 1:コメント
        /// </summary>
        public int ItemSbt
        {
            get { return SinKouiDetail.ItemSbt; }
            set
            {
                if (SinKouiDetail.ItemSbt == value) return;
                SinKouiDetail.ItemSbt = value;
                //RaisePropertyChanged(() => ItemSbt);
            }
        }

        /// <summary>
        /// 診療行為コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return SinKouiDetail.ItemCd ?? string.Empty; }
            set
            {
                if (SinKouiDetail.ItemCd == value) return;
                SinKouiDetail.ItemCd = value;
                //RaisePropertyChanged(() => ItemCd);
            }
        }

        /// <summary>
        /// オーダー診療行為コード
        /// </summary>
        public string OdrItemCd
        {
            get { return SinKouiDetail.OdrItemCd ?? string.Empty; }
            set
            {
                if (SinKouiDetail.OdrItemCd == value) return;
                SinKouiDetail.OdrItemCd = value;
                //RaisePropertyChanged(() => OdrItemCd);
            }
        }

        /// <summary>
        /// 項目名称
        /// 
        /// </summary>
        public string ItemName
        {
            get { return SinKouiDetail.ItemName ?? string.Empty; }
            set
            {
                if (SinKouiDetail.ItemName == value) return;
                SinKouiDetail.ItemName = value;
                //RaisePropertyChanged(() => ItemName);
            }
        }

        /// <summary>
        /// 数量
        /// 
        /// </summary>
        public double Suryo
        {
            get { return SinKouiDetail.Suryo; }
            set
            {
                if (SinKouiDetail.Suryo == value) return;
                SinKouiDetail.Suryo = value;
                //RaisePropertyChanged(() => Suryo);
            }
        }

        /// <summary>
        /// 数量２
        /// レセ電にのみ記載する数量（分、ｃｍ２など）
        /// </summary>
        public double Suryo2
        {
            get { return SinKouiDetail.Suryo2; }
            set
            {
                if (SinKouiDetail.Suryo2 == value) return;
                SinKouiDetail.Suryo2 = value;
                //RaisePropertyChanged(() => Suryo2);
            }
        }

        /// <summary>
        /// 書式区分
        /// 1: 列挙対象項目
        /// </summary>
        public int FmtKbn
        {
            get { return SinKouiDetail.FmtKbn; }
            set
            {
                if (SinKouiDetail.FmtKbn == value) return;
                SinKouiDetail.FmtKbn = value;
                //RaisePropertyChanged(() => FmtKbn);
            }
        }

        /// <summary>
        /// 単位コード
        /// 
        /// </summary>
        public int UnitCd
        {
            get { return SinKouiDetail.UnitCd; }
            set
            {
                if (SinKouiDetail.UnitCd == value) return;
                SinKouiDetail.UnitCd = value;
                //RaisePropertyChanged(() => UnitCd);
            }
        }

        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        public string UnitName
        {
            get { return SinKouiDetail.UnitName ?? string.Empty; }
            set
            {
                if (SinKouiDetail.UnitName == value) return;
                SinKouiDetail.UnitName = value;
                //RaisePropertyChanged(() => UnitName);
            }
        }

        /// <summary>
        /// 点数
        /// 当該項目の点数。金額項目の場合、10で割ったものを記録
        /// </summary>
        public double Ten
        {
            get { return SinKouiDetail.Ten; }
            set
            {
                if (SinKouiDetail.Ten == value) return;
                SinKouiDetail.Ten = value;
                //RaisePropertyChanged(() => Ten);
            }
        }

        /// <summary>
        /// 消費税
        /// "自費の場合の税金分
        /// TEN+ZEIになるようにする
        /// 内税項目の場合は、単価/(1+税率)*税率
        /// 単価-消費税をTENとする"
        /// </summary>
        public double Zei
        {
            get { return SinKouiDetail.Zei; }
            set
            {
                if (SinKouiDetail.Zei == value) return;
                SinKouiDetail.Zei = value;
                //RaisePropertyChanged(() => Zei);
            }
        }

        /// <summary>
        /// レセ非表示区分
        /// "1:非表示
        /// 2:電算のみ非表示"
        /// </summary>
        public int IsNodspRece
        {
            get { return SinKouiDetail.IsNodspRece; }
            set
            {
                if (SinKouiDetail.IsNodspRece == value) return;
                SinKouiDetail.IsNodspRece = value;
                //RaisePropertyChanged(() => IsNodspRece);
            }
        }

        /// <summary>
        /// 紙レセ非表示区分
        /// 1:非表示
        /// </summary>
        public int IsNodspPaperRece
        {
            get { return SinKouiDetail.IsNodspPaperRece; }
            set
            {
                if (SinKouiDetail.IsNodspPaperRece == value) return;
                SinKouiDetail.IsNodspPaperRece = value;
                //RaisePropertyChanged(() => IsNodspPaperRece);
            }
        }

        /// <summary>
        /// 領収証非表示区分
        /// 1:非表示
        /// </summary>
        public int IsNodspRyosyu
        {
            get { return SinKouiDetail.IsNodspRyosyu; }
            set
            {
                if (SinKouiDetail.IsNodspRyosyu == value) return;
                SinKouiDetail.IsNodspRyosyu = value;
                //RaisePropertyChanged(() => IsNodspRyosyu);
            }
        }

        /// <summary>
        /// コメント文
        /// 
        /// </summary>
        public string CmtOpt
        {
            get { return SinKouiDetail.CmtOpt ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtOpt == value) return;
                SinKouiDetail.CmtOpt = value;
                //RaisePropertyChanged(() => CmtOpt);
            }
        }

        /// <summary>
        /// コメント１
        /// 
        /// </summary>
        public string Cmt1
        {
            get { return SinKouiDetail.Cmt1 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.Cmt1 == value) return;
                SinKouiDetail.Cmt1 = value;
                //RaisePropertyChanged(() => Cmt1);
            }
        }

        /// <summary>
        /// コメントコード１
        /// 
        /// </summary>
        public string CmtCd1
        {
            get { return SinKouiDetail.CmtCd1 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtCd1 == value) return;
                SinKouiDetail.CmtCd1 = value;
                //RaisePropertyChanged(() => CmtCd1);
            }
        }

        /// <summary>
        /// コメント文１
        /// 
        /// </summary>
        public string CmtOpt1
        {
            get { return SinKouiDetail.CmtOpt1 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtOpt1 == value) return;
                SinKouiDetail.CmtOpt1 = value;
                //RaisePropertyChanged(() => CmtOpt1);
            }
        }

        /// <summary>
        /// コメント２
        /// 
        /// </summary>
        public string Cmt2
        {
            get { return SinKouiDetail.Cmt2 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.Cmt2 == value) return;
                SinKouiDetail.Cmt2 = value;
                //RaisePropertyChanged(() => Cmt2);
            }
        }

        /// <summary>
        /// コメントコード２
        /// 
        /// </summary>
        public string CmtCd2
        {
            get { return SinKouiDetail.CmtCd2 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtCd2 == value) return;
                SinKouiDetail.CmtCd2 = value;
                //RaisePropertyChanged(() => CmtCd2);
            }
        }

        /// <summary>
        /// コメント文２
        /// 
        /// </summary>
        public string CmtOpt2
        {
            get { return SinKouiDetail.CmtOpt2 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtOpt2 == value) return;
                SinKouiDetail.CmtOpt2 = value;
                //RaisePropertyChanged(() => CmtOpt2);
            }
        }

        /// <summary>
        /// コメント３
        /// 
        /// </summary>
        public string Cmt3
        {
            get { return SinKouiDetail.Cmt3 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.Cmt3 == value) return;
                SinKouiDetail.Cmt3 = value;
                //RaisePropertyChanged(() => Cmt3);
            }
        }

        /// <summary>
        /// コメントコード３
        /// 
        /// </summary>
        public string CmtCd3
        {
            get { return SinKouiDetail.CmtCd3 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtCd3 == value) return;
                SinKouiDetail.CmtCd3 = value;
                //RaisePropertyChanged(() => CmtCd3);
            }
        }

        /// <summary>
        /// コメント文３
        /// 
        /// </summary>
        public string CmtOpt3
        {
            get { return SinKouiDetail.CmtOpt3 ?? string.Empty; }
            set
            {
                if (SinKouiDetail.CmtOpt3 == value) return;
                SinKouiDetail.CmtOpt3 = value;
                //RaisePropertyChanged(() => CmtOpt3);
            }
        }

        /// <summary>
        /// 削除区分
        /// </summary>
        public int IsDeleted
        {
            get { return SinKouiDetail.IsDeleted; }
            set
            {
                if (SinKouiDetail.IsDeleted == value) return;
                SinKouiDetail.IsDeleted = value;
                //RaisePropertyChanged(() => IsDeleted);
            }
        }

        /// <summary>
        /// 更新情報
        ///     0: 変更なし
        ///     1: 追加
        ///     2: 削除
        /// </summary>
        public int UpdateState
        {
            get { return _updateState; }
            set { _updateState = value; }
        }

        /// <summary>
        /// キー番号
        /// </summary>
        public int KeyNo
        {
            get { return _keyNo; }
            set { _keyNo = value; }
        }

        public int ItemSeqNo
        {
            get { return _itemSeqNo; }
            set { _itemSeqNo = value; }
        }

        public TenMstModel TenMst
        {
            get { return _tenMst; }
            set { _tenMst = value; }
        }

        /// <summary>
        /// マスタ種別
        /// </summary>
        public string MasterSbt
        {
            get
            {
                return (_tenMst == null ? "" : _tenMst.MasterSbt ?? string.Empty);
            }
        }

        /// <summary>
        /// 酸素項目かどうか
        /// true-酸素項目
        /// </summary>
        public bool IsSanso
        {
            get
            {
                return (_tenMst == null ? false: new int[] { 2, 3, 4, 5 }.Contains(_tenMst.SansoKbn));
            }
        }

        /// <summary>
        /// リハビリ項目かどうか
        /// ture-リハビリ項目
        /// </summary>
        public bool IsRihabiri
        {
            get
            {
                bool ret = false;
                if (_tenMst != null)
                {
                    if (_tenMst.CdKbn == "H" &&
                        (_tenMst.Kokuji1 == "1" || _tenMst.Kokuji1 == "3") &&
                        (
                            (_tenMst.CdKbnno == 0 && _tenMst.CdEdano == 0) ||
                            (_tenMst.CdKbnno == 1 && _tenMst.CdEdano == 0) ||
                            (_tenMst.CdKbnno == 1 && _tenMst.CdEdano == 2) ||
                            (_tenMst.CdKbnno == 2 && _tenMst.CdEdano == 0) ||
                            (_tenMst.CdKbnno == 3 && _tenMst.CdEdano == 0)))
                    {
                        ret = true;
                    }
                }
                return ret;
            }
        }
        /// <summary>
        /// 酸素区分
        ///     当該特定器材が酸素又は窒素に関するものであるか否かを表す。
        ///     　0: 酸素、窒素、酸素補正率及び高気圧酸素加算以外
        ///     　1: 酸素補正率及び高気圧酸素加算
        ///     　2: 定置式液化酸素貯槽（ＣＥ）
        ///     　3: 可搬式液化酸素容器（ＬＧＣ）
        ///     　4: 大型ボンベ
        ///     　5: 小型ボンベ
        ///     　9: 窒素
        /// </summary>
        public int SansoKbn
        {
            get
            {
                return (_tenMst == null ? 0 : _tenMst.SansoKbn);
            }
        }

        /// <summary>
        /// 診療行為区分
        /// </summary>
        public int SinKouiKbn
        {
            get
            {
                return (_tenMst == null ? 0 : _tenMst.SinKouiKbn);
            }
        }

        /// <summary>
        /// 点数識別
        ///     1: 金額（整数部7桁、小数部2桁）
        ///     2: 都道府県購入価格
        ///     3: 点数（プラス）
        ///     4: 都道府県購入価格（点数）、金額（整数部のみ）
        ///     5: %加算
        ///     6: %減算
        ///     7: 減点診療行為
        ///     8: 点数（マイナス）
        ///     9: 乗算割合
        ///     10: 除算金額（金額を１０で除す。） ※ベントナイト用
        ///     11: 乗算金額（金額を10で乗ずる。） ※ステミラック注用
        ///     99: 労災円項目
        /// </summary>
        public int TenId
        {
            get
            {
                return (_tenMst == null ? 0 : _tenMst.TenId);
            }
        }

        /// <summary>
        /// 診療行為コード
        /// </summary>
        public string MstItemCd
        {
            get
            {
                return (_tenMst == null ? "" : _tenMst.ItemCd ?? string.Empty);
            }
        }

        /// <summary>
        /// 告示等識別区分１
        /// 　１：基本項目（告示） 
        /// 　３：合成項目 
        /// 　５：準用項目（通知） 
        /// 　７：加算項目 
        /// 　９：通則加算項目
        /// </summary>
        public string Kokuji1
        {
            get
            {
                return (_tenMst == null ? "" : _tenMst.Kokuji1 ?? string.Empty);
            }
        }

        /// <summary>
        /// 告示等識別区分２
        ///  １：基本項目 
        ///  ３：合成項目 
        ///  ７：加算項目
        /// </summary>
        public string Kokuji2
        {
            get
            {
                return (_tenMst == null ? "" : _tenMst.Kokuji2 ?? string.Empty);
            }
        }

        /// <summary>
        /// 通則加算対象区分
        ///  ０：所定点数の対象となる診療行為 
        ///  １：所定点数の対象とならない診療行為
        /// </summary>
        public int TusokuTargetKbn
        {
            get
            {
                return (_tenMst == null ? 0 : _tenMst.TusokuTargetKbn);
            }

        }
        /// <summary>
        /// 通則年齢
        ///     当該診療行為が年齢の通則加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 通則年齢加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 通則年齢加算自体
        /// </summary>
        public int TusokuAge
        {
            get
            {
                return (_tenMst == null ? 0 : _tenMst.TusokuAge);
            }
        }
        /// <summary>
        /// きざみ識別
        /// </summary>
        public int KizamiId
        {
            get
            {
                return (_tenMst == null ? 0 : _tenMst.KizamiId);
            }
        }
        /// <summary>
        /// 時間加算区分
        ///     当該診療行為が時間加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1, 3以外の診療行為
        ///     　1: 時間加算が算定可能な診療行為（含む合成項目）
        ///     　3: 初診料の休日加算に係る診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 下記以外の診療行為
        ///     　1: 時間外加算自体
        ///     　2: 休日加算自体
        ///     　3: 初診料の休日加算自体
        ///     　4: 深夜加算自体
        ///     　5: 時間外特例加算自体
        ///     　6: 夜間・早朝加算自体
        ///     　7: 夜間加算自体
        ///     　8: 時間外、深夜、時間外特例加算（手術又は、1000 点以上の処置）（注加算又は通則加算）自体
        ///     　9: 休日加算（手術又は、1000 点以上の処置）（注加算又は通則加算）自体
        /// </summary>
        public int TimeKasanKbn
        {
            get
            {
                return (_tenMst == null ? 0 : _tenMst.TimeKasanKbn);
            }
        }

        public double ItemTen
        {
            get
            {
                double ret = 0;
                if (TenZero == false)
                {
                    if (_tenMst != null)
                    {
                        ret = _tenMst.Ten;
                    }
                    if (MinYakka > 0)
                    {
                        ret = MinYakka;
                    }
                }

                return ret;
            }
        }

        public bool IsSisiKasanSyujyutu
        {
            get
            {
                bool ret = false;

                if (string.IsNullOrEmpty(ItemCd) == false && 
                    new string[] { ItemCdConst.SyujyutuRosaiSisiKasan, ItemCdConst.SyujyutuRosaiSisiKasan2 }.Contains(ItemCd))
                {
                    ret = true;
                }

                return ret;
            }
        }

        public double MinYakka { get; set; } = 0;
        /// <summary>
        /// 点数を0にする　院外処方の時に使用する
        /// </summary>
        public bool TenZero { get; set; } = false;
        /// <summary>
        /// Z特材用 点数識別
        /// </summary>
        public int Z_TenId { get; set; } = 0;
    }

}
