using Domain.Constant;
using Domain.Models.OrdInf;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.CommonMasters.Models;

namespace Reporting.OrderLabel.Model
{
    public class RsvkrtOdrInfDetailModel : IOdrInfDetailModel
    {
        public RsvkrtOdrInfDetail RsvkrtOdrInfDetail { get; }

        public RsvkrtOdrInfDetailModel(RsvkrtOdrInfDetail rsvkrtOdrInfDetail)
        {
            RsvkrtOdrInfDetail = rsvkrtOdrInfDetail;
            KensaMstModel = new();
            IpnMinYakkaMstModel = new();
        }

        public RsvkrtOdrInfDetailModel()
        {
            RsvkrtOdrInfDetail = new();
            KensaMstModel = new();
            IpnMinYakkaMstModel = new();
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return RsvkrtOdrInfDetail.HpId; }
            set
            {
                if (RsvkrtOdrInfDetail.HpId == value) return;
                RsvkrtOdrInfDetail.HpId = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return RsvkrtOdrInfDetail.PtId; }
            set
            {
                if (RsvkrtOdrInfDetail.PtId == value) return;
                RsvkrtOdrInfDetail.PtId = value;
            }
        }

        /// <summary>
        /// 予約日
        /// yyyymmdd
        /// </summary>
        public int RsvDate
        {
            get { return RsvkrtOdrInfDetail.RsvDate; }
            set
            {
                if (RsvkrtOdrInfDetail.RsvDate == value) return;
                RsvkrtOdrInfDetail.RsvDate = value;
            }
        }

        /// <summary>
        /// 予約カルテ番号
        /// 
        /// </summary>
        public long RsvkrtNo
        {
            get { return RsvkrtOdrInfDetail.RsvkrtNo; }
            set
            {
                if (RsvkrtOdrInfDetail.RsvkrtNo == value) return;
                RsvkrtOdrInfDetail.RsvkrtNo = value;
            }
        }

        /// <summary>
        /// 剤番号
        /// ODR_INF.RP_NO
        /// </summary>
        public long RpNo
        {
            get { return RsvkrtOdrInfDetail.RpNo; }
            set
            {
                if (RsvkrtOdrInfDetail.RpNo == value) return;
                RsvkrtOdrInfDetail.RpNo = value;
            }
        }

        /// <summary>
        /// 剤枝番
        /// 
        /// </summary>
        public long RpEdaNo
        {
            get { return RsvkrtOdrInfDetail.RpEdaNo; }
            set
            {
                if (RsvkrtOdrInfDetail.RpEdaNo == value) return;
                RsvkrtOdrInfDetail.RpEdaNo = value;
            }
        }

        /// <summary>
        /// 行番号
        /// 
        /// </summary>
        public int RowNo
        {
            get { return RsvkrtOdrInfDetail.RowNo; }
            set
            {
                if (RsvkrtOdrInfDetail.RowNo == value) return;
                RsvkrtOdrInfDetail.RowNo = value;
            }
        }

        /// <summary>
        /// 診療行為区分
        /// TEN_MST.SIN_KOUI_KBN
        /// </summary>
        public int SinKouiKbn
        {
            get { return RsvkrtOdrInfDetail.SinKouiKbn; }
            set
            {
                if (RsvkrtOdrInfDetail.SinKouiKbn == value) return;
                RsvkrtOdrInfDetail.SinKouiKbn = value;
            }
        }

        /// <summary>
        /// 項目コード
        /// TEN_MST.ITEM_CD
        /// </summary>
        public string ItemCd
        {
            get { return RsvkrtOdrInfDetail.ItemCd ?? string.Empty; }
            set
            {
                if (RsvkrtOdrInfDetail.ItemCd == value) return;
                RsvkrtOdrInfDetail.ItemCd = value;
            }
        }

        /// <summary>
        /// 項目名称
        /// 
        /// </summary>
        public string ItemName
        {
            get
            {
                return RsvkrtOdrInfDetail.ItemName ?? string.Empty.Trim();
            }
            set
            {
                if (RsvkrtOdrInfDetail.ItemName == value) return;
                RsvkrtOdrInfDetail.ItemName = value;
                SearchingText = value;
            }
        }

        public string DisplayItemName
        {
            get
            {
                if (ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                {
                    return RsvkrtOdrInfDetail.ItemName + TenUtils.GetBunkatu(BunkatuKoui, Bunkatu);
                }
                else if (ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment840Pattern))
                {
                    // TODO Correct data
                    return "　" + RsvkrtOdrInfDetail.ItemName?.Trim();
                }
                else if (string.IsNullOrEmpty(ItemCd))
                {
                    return "　" + RsvkrtOdrInfDetail.ItemName?.Trim();
                }
                return RsvkrtOdrInfDetail.ItemName ?? string.Empty.Trim();
            }
        }

        /// <summary>
        /// 数量
        /// 
        /// </summary>
        public double Suryo
        {
            get { return RsvkrtOdrInfDetail.Suryo; }
            set
            {
                if (RsvkrtOdrInfDetail.Suryo == value) return;
                RsvkrtOdrInfDetail.Suryo = value;
            }
        }

        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        public string UnitName
        {
            get { return RsvkrtOdrInfDetail.UnitName ?? string.Empty; }
            set
            {
                if (RsvkrtOdrInfDetail.UnitName == value) return;
                RsvkrtOdrInfDetail.UnitName = value;
            }
        }

        /// <summary>
        /// 単位種別
        /// "0: TEN_MST.単位
        /// 1: TEN_MST.数量換算単位"
        /// </summary>
        public int UnitSBT
        {
            get { return RsvkrtOdrInfDetail.UnitSbt; }
            set
            {
                if (RsvkrtOdrInfDetail.UnitSbt == value) return;
                RsvkrtOdrInfDetail.UnitSbt = value;
            }
        }

        /// <summary>
        /// 単位換算値
        /// "UNIT_SBT=0 -> TEN_MST.ODR_TERM_VAL
        /// UNIT_SBT=1 -> TEN_MST.SURYO_TERM_VAL"
        /// </summary>
        public double TermVal
        {
            get { return RsvkrtOdrInfDetail.TermVal; }
            set
            {
                if (RsvkrtOdrInfDetail.TermVal == value) return;
                RsvkrtOdrInfDetail.TermVal = value;
            }
        }

        /// <summary>
        /// 後発医薬品区分
        /// "当該医薬品が後発医薬品に該当するか否かを表す。
        /// 　0: 後発医薬品のない先発医薬品
        /// 　1: 先発医薬品がある後発医薬品である
        /// 　2: 後発医薬品がある先発医薬品である
        /// 　7: 先発医薬品のない後発医薬品である"
        /// </summary>
        public int KohatuKbn
        {
            get { return RsvkrtOdrInfDetail.KohatuKbn; }
            set
            {
                if (RsvkrtOdrInfDetail.KohatuKbn == value) return;
                RsvkrtOdrInfDetail.KohatuKbn = value;
            }
        }

        /// <summary>
        /// 処方せん記載区分
        /// "0: 指示なし（後発品のない先発品）
        /// 1: 変更不可
        /// 2: 後発品（他銘柄）への変更可 
        /// 3: 一般名処方"
        /// </summary>
        public int SyohoKbn
        {
            get { return RsvkrtOdrInfDetail.SyohoKbn; }
            set
            {
                if (RsvkrtOdrInfDetail.SyohoKbn == value) return;
                RsvkrtOdrInfDetail.SyohoKbn = value;
            }
        }

        /// <summary>
        /// 処方せん記載制限区分
        /// "0: 制限なし
        /// 1: 剤形不可
        /// 2: 含量規格不可
        /// 3: 含量規格・剤形不可"
        /// </summary>
        public int SyohoLimitKbn
        {
            get { return RsvkrtOdrInfDetail.SyohoLimitKbn; }
            set
            {
                if (RsvkrtOdrInfDetail.SyohoLimitKbn == value) return;
                RsvkrtOdrInfDetail.SyohoLimitKbn = value;
            }
        }

        /// <summary>
        /// 薬剤区分
        /// "当該医薬品の薬剤区分を表す。
        ///  0: 薬剤以外
        /// 　1: 内用薬
        /// 　3: その他
        /// 　4: 注射薬
        /// 　6: 外用薬
        /// 　8: 歯科用薬剤"
        /// </summary>
        public int DrugKbn
        {
            get { return RsvkrtOdrInfDetail.DrugKbn; }
            set
            {
                if (RsvkrtOdrInfDetail.DrugKbn == value) return;
                RsvkrtOdrInfDetail.DrugKbn = value;
            }
        }

        /// <summary>
        /// 用法区分
        /// "0: 用法以外
        /// 1: 基本用法
        /// 2: 補助用法"
        /// </summary>
        public int YohoKbn
        {
            get { return RsvkrtOdrInfDetail.YohoKbn; }
            set
            {
                if (RsvkrtOdrInfDetail.YohoKbn == value) return;
                RsvkrtOdrInfDetail.YohoKbn = value;
            }
        }

        /// <summary>
        /// 告示等識別区分（１）
        /// "当該診療行為についてコンピューター運用上の取扱い（磁気媒体に記録する際の取扱い）を表す。
        /// 　1: 基本項目（告示）　※基本項目
        /// 　3: 合成項目　　　　　※基本項目
        /// 　5: 準用項目（通知）　※基本項目
        /// 　7: 加算項目　　　　　※加算項目
        /// 　9: 通則加算項目　　　※加算項目
        ///  0: 診療行為以外（薬剤、特材等）"
        /// </summary>
        public string Kokuji1
        {
            get { return RsvkrtOdrInfDetail.Kokuji1 ?? string.Empty; }
            set
            {
                if (RsvkrtOdrInfDetail.Kokuji1 == value) return;
                RsvkrtOdrInfDetail.Kokuji1 = value;
            }
        }

        /// <summary>
        /// 告示等識別区分（２）
        /// "当該診療行為について点数表上の取扱いを表す。
        /// 　1: 基本項目（告示）
        /// 　3: 合成項目
        /// （削）5: 準用項目（通知）
        /// 　7: 加算項目（告示）
        /// （削）9: 通則加算項目
        ///  0: 診療行為以外（薬剤、特材等）"
        /// </summary>
        public string Kokuji2
        {
            get { return RsvkrtOdrInfDetail.Kokuji2 ?? string.Empty; }
            set
            {
                if (RsvkrtOdrInfDetail.Kokuji2 == value) return;
                RsvkrtOdrInfDetail.Kokuji2 = value;
            }
        }

        /// <summary>
        /// レセ非表示区分
        /// "0: 表示
        /// 1: 非表示"
        /// </summary>
        public int IsNodspRece
        {
            get { return RsvkrtOdrInfDetail.IsNodspRece; }
            set
            {
                if (RsvkrtOdrInfDetail.IsNodspRece == value) return;
                RsvkrtOdrInfDetail.IsNodspRece = value;
            }
        }

        /// <summary>
        /// 一般名コード
        /// 
        /// </summary>
        public string IpnCd
        {
            get { return RsvkrtOdrInfDetail.IpnCd ?? string.Empty; }
            set
            {
                if (RsvkrtOdrInfDetail.IpnCd == value) return;
                RsvkrtOdrInfDetail.IpnCd = value;
            }
        }

        /// <summary>
        /// 一般名
        /// 
        /// </summary>
        public string IpnName
        {
            get { return RsvkrtOdrInfDetail.IpnName ?? string.Empty; }
            set
            {
                if (RsvkrtOdrInfDetail.IpnName == value) return;
                RsvkrtOdrInfDetail.IpnName = value;
            }
        }

        /// <summary>
        /// 分割調剤
        /// 7日単位の3分割の場合 "7+7+7"
        /// </summary>
        public string Bunkatu
        {
            get { return RsvkrtOdrInfDetail.Bunkatu ?? string.Empty; }
            set
            {
                if (RsvkrtOdrInfDetail.Bunkatu == value) return;
                RsvkrtOdrInfDetail.Bunkatu = value;
            }
        }

        /// <summary>
        /// コメント名称
        /// "コメントマスターの名称
        /// ※当該項目がコメント項目の場合に使用"
        /// </summary>
        public string CmtName
        {
            get { return RsvkrtOdrInfDetail.CmtName ?? string.Empty; }
            set
            {
                if (RsvkrtOdrInfDetail.CmtName == value) return;
                RsvkrtOdrInfDetail.CmtName = value;
            }
        }

        /// <summary>
        /// コメント文
        /// "コメントマスターの定型文に組み合わせる文字情報
        /// ※当該項目がコメント項目の場合に使用"
        /// </summary>
        public string CmtOpt
        {
            get { return RsvkrtOdrInfDetail.CmtOpt ?? string.Empty; }
            set
            {
                if (RsvkrtOdrInfDetail.CmtOpt == value) return;
                RsvkrtOdrInfDetail.CmtOpt = value;
                if (CmtOpt != null)
                {
                    ItemName = OdrUtil.GetItemNameComment(CmtName, CmtOpt,
                        CmtCol1, CmtColKeta1,
                        CmtCol2, CmtColKeta2,
                        CmtCol3, CmtColKeta3,
                        CmtCol4, CmtColKeta4);
                }
            }
        }

        /// <summary>
        /// 文字色
        /// 
        /// </summary>
        public string FontColor
        {
            get { return RsvkrtOdrInfDetail.FontColor ?? string.Empty; }
            set
            {
                if (RsvkrtOdrInfDetail.FontColor == value) return;
                RsvkrtOdrInfDetail.FontColor = value;
            }
        }

        #region Exposed properties
        public bool IsShownCheckbox
        {
            get => !IsEmpty;
        }

        public bool IsChecked { get; set; }

        public string DisplayedQuantity
        {
            get
            {
                // If item don't have UniName => No quantity displayed
                if (string.IsNullOrEmpty(DisplayedUnit))
                {
                    return string.Empty;
                }
                return Suryo.AsDouble() != 0 && ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu ? Suryo.AsDouble().AsString() : "";
            }
        }

        public string DisplayedUnit
        {
            get => UnitName.AsString();
        }

        public string EditingQuantity
        {
            get
            {
                if (string.IsNullOrEmpty(ItemCd)) return string.Empty;

                if (ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                {
                    return Bunkatu;
                }
                else if (ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment840Pattern))
                {
                    return CmtOpt;
                }
                else
                {
                    return DisplayedQuantity;
                }
            }
            set
            {
                if (ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                {
                    CmtOpt = string.Empty;
                    if (string.IsNullOrEmpty(value))
                    {
                        CmtOpt = string.Empty;
                        Suryo = 0;
                        Bunkatu = "";
                    }
                    else
                    {
                        var splitQties = value.Split(new[] { "+" }, StringSplitOptions.None).ToList();
                        if (splitQties.Count < 2 || splitQties.Count > 3)
                        {
                            CmtOpt = string.Empty;
                            Suryo = 0;
                            Bunkatu = "";
                        }
                        else
                        {
                            CmtOpt = string.Empty;
                            Suryo = splitQties.Select(Item => Item.AsDouble()).Sum();
                            Bunkatu = value.Replace(" ", "").Replace("　", "");
                        }
                    }
                }
                else if (ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment840Pattern))
                {
                    int intConvertValue = 0;
                    Suryo = 0;
                    string halfSizeValue = HenkanJ.Instance.ToHalfsize(value.AsString());
                    if (!int.TryParse(halfSizeValue, out intConvertValue))
                    {
                        CmtOpt = string.Empty;
                    }
                    else
                    {
                        int length = CmtColKeta1 + CmtColKeta2 + CmtColKeta3 + CmtColKeta4;

                        if (intConvertValue == 0)
                        {
                            CmtOpt = string.Empty;
                        }
                        else
                        {
                            if (value.Length > length)
                            {
                                CmtOpt = string.Empty;
                            }
                            else
                            {
                                string convertValue = "";
                                if (!string.IsNullOrEmpty(halfSizeValue))
                                {
                                    convertValue = halfSizeValue.PadLeft(length, '0');
                                }

                                CmtOpt = HenkanJ.Instance.ToFullsize(convertValue);
                            }
                        }
                    }
                }
                else
                {
                    CmtOpt = string.Empty;
                    Suryo = value.AsDouble();
                }
            }
        }

        public int CmtCol1 { get; set; }

        public int CmtCol2 { get; set; }

        public int CmtCol3 { get; set; }

        public int CmtCol4 { get; set; }

        public int CmtColKeta1 { get; set; }

        public int CmtColKeta2 { get; set; }

        public int CmtColKeta3 { get; set; }

        public int CmtColKeta4 { get; set; }

        public bool IsSelected { get; set; }

        public ReleasedDrugType ReleasedType
        {
            get => OdrUtil.SyohoToSempatu(SyohoKbn, SyohoLimitKbn);
        }

        public bool IsShownReleasedDrug
        {
            get => IsKohatu && ReleasedType != ReleasedDrugType.None;
        }

        public bool IsKohatu
        {
            get => KohatuKbn == 1 || KohatuKbn == 2;
        }

        public KensaMstModel KensaMstModel { get; set; }

        public IpnMinYakkaMstModel IpnMinYakkaMstModel { get; set; }

        public double Yakka
        {
            get => IpnMinYakkaMstModel == null ? 0 : IpnMinYakkaMstModel.Yakka;
        }

        public double Ten { get; set; }

        public bool IsYoho
        {
            get => YohoKbn > 0;
        }

        /// <summary>
        /// check gaichu only
        /// </summary>
        public bool IsKensa
        {
            get => SinKouiKbn == 61 || SinKouiKbn == 64;
        }

        // Kensa gaichu apply date by date => alway 
        public int KensaGaichu
        {
            get
            {
                if (IsEmpty)
                {
                    return KensaGaichuText.NONE;
                }
                if (SinKouiKbn == 61 || SinKouiKbn == 64)
                {
                    if (KensaMstModel == null)
                    {
                        return KensaGaichuText.GAICHU_NONE;
                    }
                    if (string.IsNullOrEmpty(KensaMstModel.CenterItemCd1)
                        && string.IsNullOrEmpty(KensaMstModel.CenterItemCd2))
                    {
                        return KensaGaichuText.GAICHU_NOT_SET;
                    }
                }
                if (IsNodspRece == 1)
                {
                    return KensaGaichuText.IS_DISPLAY_RECE_ON;
                }
                return KensaGaichuText.NONE;
            }
        }

        public bool IsStandardUsage
        {
            get => YohoKbn == 1;
        }

        public bool IsSuppUsage
        {
            get => YohoKbn == 2;
        }

        public bool IsInjectionUsage
        {
            get => SinKouiKbn >= 31 && SinKouiKbn <= 34;
        }

        public bool IsHighlight
        {
            get { return false; }
        }

        public bool IsFocused
        {
            get { return false; }
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(ItemCd) &&
                       string.IsNullOrEmpty(ItemName);
            }
        }

        public ModelStatus Status { get; private set; } = ModelStatus.None;

        public bool IsComment
        {
            get
            {
                return !string.IsNullOrEmpty(ItemName) &&
                       string.IsNullOrEmpty(ItemCd);
            }
        }

        private string _searchingText = string.Empty;
        public string SearchingText
        {
            get
            {
                if (_searchingText == null)
                {
                    _searchingText = ItemName;
                }
                return _searchingText;
            }
            set
            {
                if (_searchingText != value)
                {
                    _searchingText = value;
                }
            }
        }

        public string DisplayFontColor
        {
            get
            {
                if (string.IsNullOrEmpty(FontColor))
                {
                    return "#" + FontColorConst.BLACK;
                }
                return "#" + FontColor;
            }
        }

        public bool IsUsage
        {
            get => IsStandardUsage || IsSuppUsage || IsInjectionUsage;
        }

        public int BunkatuKoui { get; set; }
        #endregion

        public void RsvkrtOdrInfDetailModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HpId))
            {
                Status = ModelStatus.Added;
            }
            else if (e.PropertyName != nameof(HpId) && Status != ModelStatus.Added)
            {
                Status = ModelStatus.Modified;
            }
        }
    }
}
