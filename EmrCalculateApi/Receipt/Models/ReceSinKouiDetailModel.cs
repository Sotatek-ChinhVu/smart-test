using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmrCalculateApi.Ika.Models;
using Helper.Constants;

namespace EmrCalculateApi.Receipt.Models
{
    class ReceSinKouiDetailModel
    {
        //public SinKouiDetail SinKouiDetail { get; } = null;

        private int _updateState = 0;
        private int _keyNo = 0;
        private int _itemSeqNo = 0;

        TenMstModel _tenMst;

        private int _hpId;
        private long _ptId;
        private int _sinYm;
        private int _rpNo;
        private int _seqNo;
        private int _rowNo;
        private string _recId;
        private int _itemSbt;
        private string _itemCd;
        private string _odrItemCd;
        private string _itemName;
        private double _suryo;
        private double _suryo2;
        private int _fmtKbn;
        private int _unitCd;
        private string _unitName;
        private double _ten;
        private double _zei;
        private int _isNodspRece;
        private int _isNodspPaperRece;
        private int _isNodspRyosyu;
        private string _cmtOpt;
        private string _cmt1;
        private string _cmtCd1;
        private string _cmtOpt1;
        private string _cmt2;
        private string _cmtCd2;
        private string _cmtOpt2;
        private string _cmt3;
        private string _cmtCd3;
        private string _cmtOpt3;
        private int _isDeleted;

        public ReceSinKouiDetailModel(SinKouiDetailModel sinKouiDetail)
        {
            _tenMst = sinKouiDetail.TenMst;

            _hpId = sinKouiDetail.HpId;
            _ptId = sinKouiDetail.PtId;
            _sinYm = sinKouiDetail.SinYm;
            _rpNo = sinKouiDetail.RpNo;
            _seqNo = sinKouiDetail.SeqNo;
            _rowNo = sinKouiDetail.RowNo;
            _recId = sinKouiDetail.RecId;
            _itemSbt = sinKouiDetail.ItemSbt;
             _itemCd = sinKouiDetail.ItemCd;
            _odrItemCd = sinKouiDetail.OdrItemCd;
            _itemName = sinKouiDetail.ItemName;
            _suryo = sinKouiDetail.Suryo;
            _suryo2 = sinKouiDetail.Suryo2;
            _fmtKbn = sinKouiDetail.FmtKbn;
            _unitCd = sinKouiDetail.UnitCd;
            _unitName = sinKouiDetail.UnitName;
            _ten = sinKouiDetail.Ten;
            _zei = sinKouiDetail.Zei;
            _isNodspRece = sinKouiDetail.IsNodspRece;
            _isNodspPaperRece = sinKouiDetail.IsNodspPaperRece;
            _isNodspRyosyu = sinKouiDetail.IsNodspRyosyu;
            _cmtOpt = sinKouiDetail.CmtOpt;
            _cmt1 = sinKouiDetail.Cmt1;
            _cmtCd1 = sinKouiDetail.CmtCd1;
            _cmtOpt1 = sinKouiDetail.CmtOpt1;
            _cmt2 = sinKouiDetail.Cmt2;
            _cmtCd2 = sinKouiDetail.CmtCd2;
            _cmtOpt2 = sinKouiDetail.CmtOpt2;
            _cmt3 = sinKouiDetail.Cmt3;
            _cmtCd3 = sinKouiDetail.CmtCd3;
            _cmtOpt3 = sinKouiDetail.CmtOpt3;
            _isDeleted = sinKouiDetail.IsDeleted;
            Z_TenId = sinKouiDetail.Z_TenId;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get => _hpId;
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get => _ptId;
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get => _sinYm;
        }

        /// <summary>
        /// 剤番号
        /// SIN_KOUI.RP_NO
        /// </summary>
        public int RpNo
        {
            get => _rpNo;
            set
            {
                _rpNo = value;
            }
        }

        /// <summary>
        /// 連番
        /// SIN_KOUI.SEQ_NO
        /// </summary>
        public int SeqNo
        {
            get => _seqNo;
            set
            {
                _seqNo = value;
            }
        }

        /// <summary>
        /// 行番号
        /// 
        /// </summary>
        public int RowNo
        {
            get => _rowNo;
            set
            {
                _rowNo = value;
            }
        }


        /// <summary>
        /// レコード識別
        /// レセプト電算に記録するレコード識別
        /// </summary>
        public string RecId
        {
            get => _recId;
            set
            {
                _recId = value;
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
                if (RecId == "CO" ||
                    (TenMst != null && TenMst.SanteiItemCd == ItemCdConst.NoSantei))
                {
                    ret = 0;
                }
                else if (RecId == "SI" || RecId == "RI")
                {
                    ret = 1;
                }
                else if (RecId == "IY")
                {
                    ret = 2;
                }
                else if (RecId == "TO")
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
            get => _itemSbt;
            set
            {
                _itemSbt = value;
            }
        }

        /// <summary>
        /// 診療行為コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get => _itemCd;
            set
            {
                _itemCd = value;
            }
        }

        /// <summary>
        /// オーダー診療行為コード
        /// </summary>
        public string OdrItemCd
        {
            get => _odrItemCd;
            set
            {
                _odrItemCd = value;
            }
        }

        /// <summary>
        /// 項目名称
        /// 
        /// </summary>
        public string ItemName
        {
            get => _itemName;
            set
            {
                _itemName = value;
            }
        }

        /// <summary>
        /// 数量
        /// 
        /// </summary>
        public double Suryo
        {
            get => _suryo;
            set
            {
                _suryo = value;
            }
        }

        /// <summary>
        /// 数量２
        /// レセ電にのみ記載する数量（分、ｃｍ２など）
        /// </summary>
        public double Suryo2
        {
            get => _suryo2;
            set
            {
                _suryo2 = value;
            }
        }

        /// <summary>
        /// 書式区分
        /// 1: 列挙対象項目
        /// </summary>
        public int FmtKbn
        {
            get => _fmtKbn;
            set
            {
                _fmtKbn = value;
            }
        }

        /// <summary>
        /// 単位コード
        /// 
        /// </summary>
        public int UnitCd
        {
            get => _unitCd;
            set
            {
                _unitCd = value;
            }
        }

        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        public string UnitName
        {
            get => _unitName;
            set
            {
                _unitName = value;
            }
        }

        /// <summary>
        /// 点数
        /// 当該項目の点数。金額項目の場合、10で割ったものを記録
        /// </summary>
        public double Ten
        {
            get => _ten;
            set
            {
                _ten = value;
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
            get => _zei;
            set
            {
                _zei = value;
            }
        }

        /// <summary>
        /// レセ非表示区分
        /// "1:非表示
        /// 2:電算のみ非表示"
        /// </summary>
        public int IsNodspRece
        {
            get => _isNodspRece;
            set
            {
                _isNodspRece = value;
            }
        }

        /// <summary>
        /// 紙レセ非表示区分
        /// 1:非表示
        /// </summary>
        public int IsNodspPaperRece
        {
            get => _isNodspPaperRece;
            set
            {
                _isNodspPaperRece = value;
            }
        }

        /// <summary>
        /// 領収証非表示区分
        /// 1:非表示
        /// </summary>
        public int IsNodspRyosyu
        {
            get => _isNodspRyosyu;
            set
            {
                _isNodspRyosyu = value;
            }
        }

        /// <summary>
        /// コメント文
        /// 
        /// </summary>
        public string CmtOpt
        {
            get => _cmtOpt;
            set
            {
                _cmtOpt = value;
            }
        }

        /// <summary>
        /// コメント１
        /// 
        /// </summary>
        public string Cmt1
        {
            get => _cmt1;
            set
            {
                _cmt1 = value;
            }
        }

        /// <summary>
        /// コメントコード１
        /// 
        /// </summary>
        public string CmtCd1
        {
            get => _cmtCd1;
            set
            {
                _cmtCd1 = value;
            }
        }

        /// <summary>
        /// コメント文１
        /// 
        /// </summary>
        public string CmtOpt1
        {
            get => _cmtOpt1;
            set
            {
                _cmtOpt1 = value;
            }
        }

        /// <summary>
        /// コメント２
        /// 
        /// </summary>
        public string Cmt2
        {
            get => _cmt2;
            set
            {
                _cmt2 = value;
            }
        }

        /// <summary>
        /// コメントコード２
        /// 
        /// </summary>
        public string CmtCd2
        {
            get => _cmtCd2;
            set
            {
                _cmtCd2 = value;
            }
        }

        /// <summary>
        /// コメント文２
        /// 
        /// </summary>
        public string CmtOpt2
        {
            get => _cmtOpt2;
            set
            {
                _cmtOpt2 = value;
            }
        }

        /// <summary>
        /// コメント３
        /// 
        /// </summary>
        public string Cmt3
        {
            get => _cmt3;
            set
            {
                _cmt3 = value;
            }
        }

        /// <summary>
        /// コメントコード３
        /// 
        /// </summary>
        public string CmtCd3
        {
            get => _cmtCd3;
            set
            {
                _cmtCd3 = value;
            }
        }

        /// <summary>
        /// コメント文３
        /// 
        /// </summary>
        public string CmtOpt3
        {
            get => _cmtOpt3;
            set
            {
                _cmtOpt3 = value;
            }
        }

        /// <summary>
        /// 削除区分
        /// </summary>
        public int IsDeleted
        {
            get => _isDeleted;
            set
            {
                _isDeleted = value;
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
            get => _updateState;
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
                return (_tenMst == null ? false : new int[] { 2, 3, 4, 5 }.Contains(_tenMst.SansoKbn));
            }
        }

        /// <summary>
        /// 酸素区分
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
        /// </summary>
        public int TusokuTargetKbn
        {
            get
            {
                return (_tenMst == null ? 0 : _tenMst.TusokuTargetKbn);
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
            get
            {
                return (_tenMst == null ? 0 : _tenMst.DrugKbn);
            }
        }

        public string RyosyuName
        {
            get
            {
                string ret = "";

                if(_tenMst != null && string.IsNullOrEmpty(_tenMst.RyosyuName) == false)
                {
                    ret = _tenMst.RyosyuName;
                }

                return ret;
            }
        }

        /// <summary>
        /// 特材用 点数識別
        /// </summary>
        public int Z_TenId { get; set; } = 0;
    }
}
