using Helper.Constants;
using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class OdrInfDetailDataModel
    {
        private int _hpId;
        private long _ptId;
        private int _sinDate;
        private long _raiinNo;
        private long _rpNo;
        private long _rpEdaNo;
        private int _rowNo;
        private string _itemCd;
        private string _itemName;
        private double _suryo;
        private string _unitName;
        private int _unitSBT;
        private double _termVal;
        private int _kohatuKbn;
        private int _syohoKbn;
        private int _syohoLimitKbn;
        private int _isNodspRece;
        private int _yohoKbn;
        private string _ipnCd;
        private string _ipnName;
        private string _bunkatu;
        private string _cmtName;
        private string _cmtOpt;
        private bool _isDummy;

        public OdrInfDetailDataModel(OdrInfDetail odrInfDetail)
        {
            _hpId = odrInfDetail.HpId;
            _ptId = odrInfDetail.PtId;
            _sinDate = odrInfDetail.SinDate;
            _raiinNo = odrInfDetail.RaiinNo;
            _rpNo = odrInfDetail.RpNo;
            _rpEdaNo = odrInfDetail.RpEdaNo;
            _rowNo = odrInfDetail.RowNo;
            _itemCd = odrInfDetail.ItemCd;
            _itemName = odrInfDetail.ItemName;
            _suryo = odrInfDetail.Suryo;
            _unitName = odrInfDetail.UnitName;
            _unitSBT = odrInfDetail.UnitSBT;
            _termVal = odrInfDetail.TermVal;
            _kohatuKbn = odrInfDetail.KohatuKbn;
            _syohoKbn = odrInfDetail.SyohoKbn;
            _syohoLimitKbn = odrInfDetail.SyohoLimitKbn;
            _isNodspRece = odrInfDetail.IsNodspRece;
            _yohoKbn = odrInfDetail.YohoKbn;
            _ipnCd = odrInfDetail.IpnCd;
            _ipnName = odrInfDetail.IpnName;
            _bunkatu = odrInfDetail.Bunkatu;
            _cmtName = odrInfDetail.CmtName;
            _cmtOpt = odrInfDetail.CmtOpt;
        }

        public OdrInfDetailDataModel(
            int hpId, long ptId, int sinDate, long raiinNo, 
            long rpNo, long rpEdaNo, int rowNo, 
            string itemCd, string itemName, double suryo, string unitName, int unitSBT, double termVal, 
            int kohatuKbn, int syohoKbn, int syohoLimitKbn, 
            int isNodspRece, int yohoKbn, string ipnCd, string ipnName, string bunkatu, string cmtName, string cmtOpt, bool isdummy)
        {
            _hpId = hpId;
            _ptId = ptId;
            _sinDate = sinDate;
            _raiinNo = raiinNo;
            _rpNo = rpNo;
            _rpEdaNo = rpEdaNo;
            _rowNo = rowNo;
            _yohoKbn = yohoKbn;
            _itemCd = itemCd;
            _itemName = itemName;
            _suryo = suryo;
            _unitName = unitName;
            _unitSBT = unitSBT;
            _termVal = termVal;
            _kohatuKbn = kohatuKbn;
            _syohoKbn = syohoKbn;
            _syohoLimitKbn = syohoLimitKbn;
            _isNodspRece = isNodspRece;
            _ipnCd = ipnCd;
            _ipnName = ipnName;
            _bunkatu = bunkatu;
            _cmtName = cmtName;
            _cmtOpt = cmtOpt;
            _isDummy = isdummy;
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return _hpId; }
        }

        /// <summary>
        /// 患者ID
        ///       患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return _ptId; }
        }

        /// <summary>
        /// 診療日
        ///       yyyyMMdd
        /// </summary>
        public int SinDate
        {
            get { return _sinDate; }
        }

        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo
        {
            get { return _raiinNo; }
        }

        /// <summary>
        /// 剤番号
        ///     ODR_INF.RP_NO
        /// </summary>
        public long RpNo
        {
            get { return _rpNo; }
            set { _rpNo = value; }
        }

        /// <summary>
        /// 剤枝番
        ///     ODR_INF.RP_EDA_NO
        /// </summary>
        public long RpEdaNo
        {
            get { return _rpEdaNo; }
            set { _rpEdaNo = value; }
        }

        /// <summary>
        /// 行番号
        /// </summary>
        public int RowNo
        {
            get { return _rowNo; }
            set { _rowNo = value; }
        }

        /// <summary>
        /// 項目コード
        /// 算定用項目コードに変換したもの
        /// </summary>
        public string ItemCd
        {
            get { return _itemCd ?? string.Empty; }
            set { _itemCd = value; }
        }

        /// <summary>
        /// 項目名称
        /// </summary>
        public string ItemName
        {
            get { return _itemName ?? string.Empty; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public double Suryo
        {
            get { return _suryo; }
            set { _suryo = value; }
        }

        /// <summary>
        /// 単位名称
        /// </summary>
        public string UnitName
        {
            get { return _unitName ?? string.Empty; }
        }

        /// <summary>
        /// 単位種別
        ///         0: 1,2以外
        ///         1: TEN_MST.単位
        ///         2: TEN_MST.数量換算単位
        /// </summary>
        public int UnitSBT
        {
            get { return _unitSBT; }
        }

        /// <summary>
        /// 単位換算値
        ///          UNIT_SBT=0 -> TEN_MST.ODR_TERM_VAL
        ///          UNIT_SBT=0 -> TEN_MST.ODR_TERM_VAL
        /// </summary>
        public double TermVal
        {
            get { return _termVal; }
        }

        /// <summary>
        /// 後発医薬品区分
        ///         当該医薬品が後発医薬品に該当するか否かを表す。
        ///             0: 後発医薬品のない先発医薬品
        ///             1: 先発医薬品がある後発医薬品である
        ///             2: 後発医薬品がある先発医薬品である
        ///             7: 先発医薬品のない後発医薬品である
        /// </summary>
        public int KohatuKbn
        {
            get { return _kohatuKbn; }
        }

        /// <summary>
        /// 処方せん記載区分
        ///             0: 指示なし（後発品のない先発品）
        ///             1: 変更不可
        ///             2: 後発品（他銘柄）への変更可 
        ///             3: 一般名処方
        /// </summary>
        public int SyohoKbn
        {
            get { return _syohoKbn; }
        }

        /// <summary>
        /// 処方せん記載制限区分
        ///             0: 制限なし
        ///             1: 剤形不可
        ///             2: 含量規格不可
        ///             3: 含量規格・剤形不可
        /// </summary>
        public int SyohoLimitKbn
        {
            get { return _syohoLimitKbn; }
        }

        /// <summary>
        /// レセ非表示区分
        ///          0: 表示
        ///          1: 非表示
        /// </summary>
        public int IsNodspRece
        {
            get { return _isNodspRece; }
        }

        /// <summary>
        /// 用法区分
        ///     0: 用法以外
        ///     1: 基本用法
        ///     2: 補助用法
        /// </summary>
        public int YohoKbn
        {
            get { return _yohoKbn; }
        }

        /// <summary>
        /// 一般名コード
        /// </summary>
        public string IpnCd
        {
            get { return _ipnCd ?? string.Empty; }
        }

        /// <summary>
        /// 一般名
        /// </summary>
        public string IpnName
        {
            get { return _ipnName ?? string.Empty; }
        }

        /// <summary>
        /// 分割調剤
        ///        7日単位の3分割の場合 "7+7+7"
        /// </summary>
        public string Bunkatu
        {
            get { return _bunkatu ?? string.Empty; }
        }

        /// <summary>
        /// コメントマスターの名称
        ///        ※当該項目がコメント項目の場合に使用
        /// </summary>
        public string CmtName
        {
            get { return _cmtName ?? string.Empty; }
        }

        /// <summary>
        /// コメント文
        ///        コメントマスターの定型文に組み合わせる文字情報
        ///        ※当該項目がコメント項目の場合に使用
        /// </summary>
        public string CmtOpt
        {
            get { return _cmtOpt ?? string.Empty; }
        }
        /// <summary>
        /// 算定漏れチェック用項目の場合、true
        /// </summary>
        public bool IsDummy
        {
            get { return _isDummy; }
        }

    }
}
