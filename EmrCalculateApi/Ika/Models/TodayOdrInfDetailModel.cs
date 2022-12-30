using Helper.Constants;
using Entity.Tenant;
using System;
using System.Linq;
using Helper.Extension;

namespace EmrCalculateApi.Ika.Models
{
    public class TodayOdrInfDetailModel 
    {
        //public OdrInfDetail OdrInfDetail { get; } = null;

        //public TodayOdrInfDetailModel(OdrInfDetail odrInfDetail)
        //{
        //    OdrInfDetail = odrInfDetail;
        //    BackupSuryo = Suryo;
        //    PropertyChanged += TodayOdrInfDetailModel_PropertyChanged;
        //}

        ///// <summary>
        ///// オーダー情報詳細
        ///// </summary>
        ///// <summary>
        ///// 医療機関識別ID
        ///// </summary>
        //public int HpId
        //{
        //    get { return OdrInfDetail.HpId; }
        //    set
        //    {
        //        if (OdrInfDetail.HpId == value) return;
        //        OdrInfDetail.HpId = value;
        //        //RaisePropertyChanged(() => HpId);
        //    }
        //}

        ///// <summary>
        ///// 患者ID
        /////       患者を識別するためのシステム固有の番号
        ///// </summary>
        //public long PtId
        //{
        //    get { return OdrInfDetail.PtId; }
        //    set
        //    {
        //        if (OdrInfDetail.PtId == value) return;
        //        OdrInfDetail.PtId = value;
        //        //RaisePropertyChanged(() => PtId);
        //    }
        //}

        ///// <summary>
        ///// 診療日
        /////       yyyyMMdd
        ///// </summary>
        //public int SinDate
        //{
        //    get { return OdrInfDetail.SinDate; }
        //    set
        //    {
        //        if (OdrInfDetail.SinDate == value) return;
        //        OdrInfDetail.SinDate = value;
        //        //RaisePropertyChanged(() => SinDate);
        //    }
        //}

        ///// <summary>
        ///// 来院番号
        ///// </summary>
        //public long RaiinNo
        //{
        //    get { return OdrInfDetail.RaiinNo; }
        //    set
        //    {
        //        if (OdrInfDetail.RaiinNo == value) return;
        //        OdrInfDetail.RaiinNo = value;
        //        //RaisePropertyChanged(() => RaiinNo);
        //    }
        //}

        ///// <summary>
        ///// 剤番号
        /////     ODR_INF.RP_NO
        ///// </summary>
        //public long RpNo
        //{
        //    get { return OdrInfDetail.RpNo; }
        //    set
        //    {
        //        if (OdrInfDetail.RpNo == value) return;
        //        OdrInfDetail.RpNo = value;
        //        //RaisePropertyChanged(() => RpNo);
        //    }
        //}

        ///// <summary>
        ///// 剤枝番
        /////     ODR_INF.RP_EDA_NO
        ///// </summary>
        //public long RpEdaNo
        //{
        //    get { return OdrInfDetail.RpEdaNo; }
        //    set
        //    {
        //        if (OdrInfDetail.RpEdaNo == value) return;
        //        OdrInfDetail.RpEdaNo = value;
        //        //RaisePropertyChanged(() => RpEdaNo);
        //    }
        //}

        ///// <summary>
        ///// 行番号
        ///// </summary>
        //public int RowNo
        //{
        //    get { return OdrInfDetail.RowNo; }
        //    set
        //    {
        //        if (OdrInfDetail.RowNo == value) return;
        //        OdrInfDetail.RowNo = value;
        //        //RaisePropertyChanged(() => RowNo);
        //    }
        //}

        ///// <summary>
        ///// 診療行為区分
        ///// </summary>
        //public int SinKouiKbn
        //{
        //    get { return OdrInfDetail.SinKouiKbn; }
        //    set
        //    {
        //        if (OdrInfDetail.SinKouiKbn == value) return;
        //        OdrInfDetail.SinKouiKbn = value;
        //        //RaisePropertyChanged(() => SinKouiKbn);
        //        //RaisePropertyChanged(() => KensaGaichu);
        //    }
        //}

        ///// <summary>
        ///// 項目コード
        ///// </summary>
        //public string ItemCd
        //{
        //    get { return OdrInfDetail.ItemCd; }
        //    set
        //    {
        //        if (OdrInfDetail.ItemCd == value) return;
        //        OdrInfDetail.ItemCd = value;
        //        //RaisePropertyChanged(() => ItemCd);
        //        //RaisePropertyChanged(() => IsFreeComment);
        //        //RaisePropertyChanged(() => IsEmpty);
        //        //RaisePropertyChanged(() => KensaGaichu);
        //    }
        //}

        ///// <summary>
        ///// 項目名称
        ///// </summary>
        //public string ItemName
        //{
        //    get { return OdrInfDetail.ItemName; }
        //    set
        //    {
        //        if (OdrInfDetail.ItemName == value) return;
        //        OdrInfDetail.ItemName = value;
        //        SearchingText = value;
        //        //RaisePropertyChanged(() => ItemName);
        //        //RaisePropertyChanged(() => DisplayItemName);
        //        //RaisePropertyChanged(() => IsFreeComment);
        //        //RaisePropertyChanged(() => SearchingText);
        //        //RaisePropertyChanged(() => IsEmpty);
        //        //RaisePropertyChanged(() => KensaGaichu);
        //    }
        //}

        //public string DisplayItemName
        //{
        //    get
        //    {
        //        if (ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
        //        {
        //            return OdrInfDetail.ItemName + TenUtils.GetBunkatu(BunkatuKoui, Bunkatu);
        //        }
        //        else if (Is840Cmt)
        //        {
        //            return "" + OdrInfDetail.ItemName;
        //        }
        //        else if (Is842Cmt)
        //        {
        //            return "" + OdrInfDetail.ItemName;
        //        }
        //        else if (Is830Cmt)
        //        {
        //            return "" + OdrInfDetail.ItemName;
        //        }
        //        else if (Is831Cmt)
        //        {
        //            return "" + OdrInfDetail.ItemName;
        //        }
        //        else if (Is850Cmt)
        //        {
        //            return "" + OdrInfDetail.ItemName;
        //        }
        //        else if (Is851Cmt)
        //        {
        //            return "" + OdrInfDetail.ItemName;
        //        }
        //        else if (Is852Cmt)
        //        {
        //            return "" + OdrInfDetail.ItemName;
        //        }
        //        else if (Is853Cmt)
        //        {
        //            return "" + OdrInfDetail.ItemName;
        //        }
        //        else if (Is880Cmt)
        //        {
        //            return "" + OdrInfDetail.ItemName;
        //        }
        //        else if (string.IsNullOrEmpty(ItemCd) && !IsShohoComment && !IsShohoBiko)
        //        {
        //            return "" + OdrInfDetail.ItemName;
        //        }
        //        return OdrInfDetail.ItemName;
        //    }
        //}

        ///// <summary>
        ///// 数量
        ///// </summary>
        //public double Suryo
        //{
        //    get { return OdrInfDetail.Suryo; }
        //    set
        //    {
        //        if (OdrInfDetail.Suryo == value) return;
        //        OdrInfDetail.Suryo = value;
        //        //RaisePropertyChanged(() => Suryo);
        //        //RaisePropertyChanged(() => DisplayedUnit);
        //        //RaisePropertyChanged(() => DisplayedQuantity);
        //    }
        //}

        ///// <summary>
        ///// 単位名称
        ///// </summary>
        //public string UnitName
        //{
        //    get { return OdrInfDetail.UnitName; }
        //    set
        //    {
        //        if (OdrInfDetail.UnitName == value) return;
        //        OdrInfDetail.UnitName = value;
        //        //RaisePropertyChanged(() => UnitName);
        //        //RaisePropertyChanged(() => DisplayedUnit);
        //    }
        //}

        ///// <summary>
        ///// 単位種別
        /////         0: 1,2以外
        /////         1: TEN_MST.単位
        /////         2: TEN_MST.数量換算単位
        ///// </summary>
        //public int UnitSBT
        //{
        //    get { return OdrInfDetail.UnitSBT; }
        //    set
        //    {
        //        if (OdrInfDetail.UnitSBT == value) return;
        //        OdrInfDetail.UnitSBT = value;
        //        //RaisePropertyChanged(() => UnitSBT);
        //    }
        //}

        ///// <summary>
        ///// 単位換算値
        /////          UNIT_SBT=0 -> TEN_MST.ODR_TERM_VAL
        /////          UNIT_SBT=0 -> TEN_MST.ODR_TERM_VAL
        ///// </summary>
        //public double TermVal
        //{
        //    get { return OdrInfDetail.TermVal; }
        //    set
        //    {
        //        if (OdrInfDetail.TermVal == value) return;
        //        OdrInfDetail.TermVal = value;
        //        //RaisePropertyChanged(() => TermVal);
        //    }
        //}

        ///// <summary>
        ///// 後発医薬品区分
        /////         当該医薬品が後発医薬品に該当するか否かを表す。
        /////             0: 後発医薬品のない先発医薬品
        /////             1: 先発医薬品がある後発医薬品である
        /////             2: 後発医薬品がある先発医薬品である
        /////             7: 先発医薬品のない後発医薬品である
        ///// </summary>
        //public int KohatuKbn
        //{
        //    get { return OdrInfDetail.KohatuKbn; }
        //    set
        //    {
        //        if (OdrInfDetail.KohatuKbn == value) return;
        //        OdrInfDetail.KohatuKbn = value;
        //        //RaisePropertyChanged(() => KohatuKbn);
        //        //RaisePropertyChanged(() => IsKohatu);
        //        //RaisePropertyChanged(() => IsShownReleasedDrug);
        //    }
        //}

        ///// <summary>
        ///// 処方せん記載区分
        /////             0: 指示なし（後発品のない先発品）
        /////             1: 変更不可
        /////             2: 後発品（他銘柄）への変更可 
        /////             3: 一般名処方
        ///// </summary>
        //public int SyohoKbn
        //{
        //    get { return OdrInfDetail.SyohoKbn; }
        //    set
        //    {
        //        if (OdrInfDetail.SyohoKbn == value) return;
        //        OdrInfDetail.SyohoKbn = value;
        //        //RaisePropertyChanged(() => SyohoKbn);
        //        //RaisePropertyChanged(() => ReleasedType);
        //        //RaisePropertyChanged(() => IsShownReleasedDrug);
        //    }
        //}

        ///// <summary>
        ///// 処方せん記載制限区分
        /////             0: 制限なし
        /////             1: 剤形不可
        /////             2: 含量規格不可
        /////             3: 含量規格・剤形不可
        ///// </summary>
        //public int SyohoLimitKbn
        //{
        //    get { return OdrInfDetail.SyohoLimitKbn; }
        //    set
        //    {
        //        if (OdrInfDetail.SyohoLimitKbn == value) return;
        //        OdrInfDetail.SyohoLimitKbn = value;
        //        //RaisePropertyChanged(() => SyohoLimitKbn);
        //        //RaisePropertyChanged(() => ReleasedType);
        //        //RaisePropertyChanged(() => IsShownReleasedDrug);
        //    }
        //}

        ///// <summary>
        ///// 薬剤区分
        /////        当該医薬品の薬剤区分を表す。
        /////             0: 薬剤以外
        /////             1: 内用薬
        /////             3: その他
        /////             4: 注射薬
        /////             6: 外用薬
        /////             8: 歯科用薬剤
        ///// </summary>
        //public int DrugKbn
        //{
        //    get { return OdrInfDetail.DrugKbn; }
        //    set
        //    {
        //        if (OdrInfDetail.DrugKbn == value) return;
        //        OdrInfDetail.DrugKbn = value;
        //        //RaisePropertyChanged(() => DrugKbn);
        //    }
        //}

        ///// <summary>
        ///// 用法区分
        /////          0: 用法以外
        /////          1: 基本用法
        /////          2: 補助用法
        ///// </summary>
        //public int YohoKbn
        //{
        //    get { return OdrInfDetail.YohoKbn; }
        //    set
        //    {
        //        if (OdrInfDetail.YohoKbn == value) return;
        //        OdrInfDetail.YohoKbn = value;
        //        //RaisePropertyChanged(() => YohoKbn);
        //    }
        //}

        ///// <summary>
        ///// 告示等識別区分（１）
        /////        当該診療行為についてコンピューター運用上の取扱い（磁気媒体に記録する際の取扱い）を表す。
        /////          1: 基本項目（告示）　※基本項目
        /////          3: 合成項目　　　　　※基本項目
        /////          5: 準用項目（通知）　※基本項目
        /////          7: 加算項目　　　　　※加算項目
        /////          9: 通則加算項目　　　※加算項目
        /////          0: 診療行為以外（薬剤、特材等）
        ///// </summary>
        //public string Kokuji1
        //{
        //    get { return OdrInfDetail.Kokuji1; }
        //    set
        //    {
        //        if (OdrInfDetail.Kokuji1 == value) return;
        //        OdrInfDetail.Kokuji1 = value;
        //        //RaisePropertyChanged(() => Kokuji1);
        //    }
        //}

        ///// <summary>
        ///// 告示等識別区分（２）
        /////        当該診療行為について点数表上の取扱いを表す。
        /////           1: 基本項目（告示）
        /////           3: 合成項目
        /////     （削）5: 準用項目（通知）
        /////          7: 加算項目（告示）
        /////       削）9: 通則加算項目
        /////           0: 診療行為以外（薬剤、特材等）
        ///// </summary>
        //public string Kokuji2
        //{
        //    get { return OdrInfDetail.Kokiji2; }
        //    set
        //    {
        //        if (OdrInfDetail.Kokiji2 == value) return;
        //        OdrInfDetail.Kokiji2 = value;
        //        //RaisePropertyChanged(() => Kokuji2);
        //    }
        //}

        ///// <summary>
        ///// レセ非表示区分
        /////          0: 表示
        /////          1: 非表示
        ///// </summary>
        //public int IsNodspRece
        //{
        //    get { return OdrInfDetail.IsNodspRece; }
        //    set
        //    {
        //        if (OdrInfDetail.IsNodspRece == value) return;
        //        OdrInfDetail.IsNodspRece = value;
        //        //RaisePropertyChanged(() => IsNodspRece);
        //        //RaisePropertyChanged(() => KensaGaichu);
        //    }
        //}

        ///// <summary>
        ///// 一般名コード
        ///// </summary>
        //public string IpnCd
        //{
        //    get { return OdrInfDetail.IpnCd; }
        //    set
        //    {
        //        if (OdrInfDetail.IpnCd == value) return;
        //        OdrInfDetail.IpnCd = value;
        //        //RaisePropertyChanged(() => IpnCd);
        //    }
        //}

        ///// <summary>
        ///// 一般名
        ///// </summary>
        //public string IpnName
        //{
        //    get { return OdrInfDetail.IpnName; }
        //    set
        //    {
        //        if (OdrInfDetail.IpnName == value) return;
        //        OdrInfDetail.IpnName = value;
        //        //RaisePropertyChanged(() => IpnName);
        //    }
        //}

        ///// <summary>
        ///// 実施区分
        /////          0: 未実施
        /////          1: 実施
        ///// </summary>
        //public int JissiKbn
        //{
        //    get { return OdrInfDetail.JissiKbn; }
        //    set
        //    {
        //        if (OdrInfDetail.JissiKbn == value) return;
        //        OdrInfDetail.JissiKbn = value;
        //        //RaisePropertyChanged(() => JissiKbn);
        //    }
        //}

        ///// <summary>
        ///// 実施日時
        ///// </summary>
        //public DateTime? JissiDate
        //{
        //    get { return OdrInfDetail.JissiDate; }
        //    set
        //    {
        //        if (OdrInfDetail.JissiDate == value) return;
        //        OdrInfDetail.JissiDate = value;
        //        //RaisePropertyChanged(() => JissiDate);
        //    }
        //}

        ///// <summary>
        ///// 実施者
        ///// </summary>
        //public int JissiId
        //{
        //    get { return OdrInfDetail.JissiId; }
        //    set
        //    {
        //        if (OdrInfDetail.JissiId == value) return;
        //        OdrInfDetail.JissiId = value;
        //        //RaisePropertyChanged(() => JissiId);
        //    }
        //}

        ///// <summary>
        ///// 実施端末
        ///// </summary>
        //public string JissiMachine
        //{
        //    get { return OdrInfDetail.JissiMachine; }
        //    set
        //    {
        //        if (OdrInfDetail.JissiMachine == value) return;
        //        OdrInfDetail.JissiMachine = value;
        //        //RaisePropertyChanged(() => JissiMachine);
        //    }
        //}

        ///// <summary>
        ///// 検査依頼コード
        ///// </summary>
        //public string ReqCd
        //{
        //    get { return OdrInfDetail.ReqCd; }
        //    set
        //    {
        //        if (OdrInfDetail.ReqCd == value) return;
        //        OdrInfDetail.ReqCd = value;
        //        //RaisePropertyChanged(() => ReqCd);
        //    }
        //}

        ///// <summary>
        ///// 分割調剤
        /////        7日単位の3分割の場合 "7+7+7"
        ///// </summary>
        //public string Bunkatu
        //{
        //    get { return OdrInfDetail.Bunkatu; }
        //    set
        //    {
        //        if (OdrInfDetail.Bunkatu == value) return;
        //        OdrInfDetail.Bunkatu = value;
        //        //RaisePropertyChanged(() => Bunkatu);
        //        //RaisePropertyChanged(() => DisplayItemName);
        //    }
        //}

        ///// <summary>
        ///// コメントマスターの名称
        /////        ※当該項目がコメント項目の場合に使用
        ///// </summary>
        //public string CmtName
        //{
        //    get { return OdrInfDetail.CmtName; }
        //    set
        //    {
        //        if (OdrInfDetail.CmtName == value) return;
        //        OdrInfDetail.CmtName = value;
        //        //RaisePropertyChanged(() => CmtName);
        //    }
        //}

        ///// <summary>
        ///// コメント文
        /////        コメントマスターの定型文に組み合わせる文字情報
        /////        ※当該項目がコメント項目の場合に使用
        ///// </summary>
        //public string CmtOpt
        //{
        //    get { return OdrInfDetail.CmtOpt; }
        //    set
        //    {
        //        if (OdrInfDetail.CmtOpt == value) return;
        //        OdrInfDetail.CmtOpt = value;
        //        //RaisePropertyChanged(() => CmtOpt);
        //        if (CmtOpt != null)
        //        {
        //            ItemName = OdrUtil.GetItemNameComment(CmtName, CmtOpt,
        //                CmtCol1, CmtColKeta1,
        //                CmtCol2, CmtColKeta2,
        //                CmtCol3, CmtColKeta3,
        //                CmtCol4, CmtColKeta4);
        //        }
        //        //RaisePropertyChanged(() => DisplayItemName);
        //    }
        //}

        ///// <summary>
        ///// 文字色
        ///// </summary>
        //public string FontColor
        //{
        //    get { return OdrInfDetail.FontColor; }
        //    set
        //    {
        //        if (OdrInfDetail.FontColor == value) return;
        //        OdrInfDetail.FontColor = value;
        //        //RaisePropertyChanged(() => FontColor);
        //        //RaisePropertyChanged(() => DisplayFontColor);
        //    }
        //}

        ///// <summary>
        ///// コメント改行区分
        /////          0: 改行する
        /////          1: 改行しない
        ///// </summary>
        //public int CommentNewline
        //{
        //    get { return OdrInfDetail.CommentNewline; }
        //    set
        //    {
        //        if (OdrInfDetail.CommentNewline == value) return;
        //        OdrInfDetail.CommentNewline = value;
        //        //RaisePropertyChanged(() => CommentNewline);
        //    }
        //}

        //#region Exposed properties
        //private bool _isShownCheckbox = false;
        //public bool IsShownCheckbox
        //{
        //    get => _isShownCheckbox;
        //    set
        //    {
        //        Set(ref _isShownCheckbox, value);
        //    }
        //}

        //private bool _isChecked = false;
        //public bool IsChecked
        //{
        //    get => _isChecked;
        //    set
        //    {
        //        Set(ref _isChecked, value);
        //    }
        //}

        //private bool _isSelected;
        //public bool IsSelected
        //{
        //    get
        //    {
        //        return this._isSelected;
        //    }
        //    set
        //    {
        //        Set(ref this._isSelected, value);
        //    }
        //}

        //public string DisplayedQuantity
        //{
        //    get
        //    {
        //        // If item don't have UniName => No quantity displayed
        //        if (string.IsNullOrEmpty(DisplayedUnit))
        //        {
        //            return string.Empty;
        //        }
        //        return Suryo.AsDouble() != 0 && ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu ? Suryo.AsDouble().AsString() : "";
        //    }
        //}

        //public string DisplayedUnit
        //{
        //    get => Suryo.AsDouble() != 0 ? UnitName.AsString() : "";
        //}

        //public string EditingQuantity
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(ItemCd)) return string.Empty;

        //        if (ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
        //        {
        //            return Bunkatu;
        //        }
        //        else if (Is840Cmt)
        //        {
        //            return CmtOpt;
        //        }
        //        else if (Is842Cmt)
        //        {
        //            return CmtOpt;
        //        }
        //        else if (Is850Cmt)
        //        {
        //            return CmtOpt;
        //        }
        //        else if (Is851Cmt)
        //        {
        //            return CmtOpt;
        //        }
        //        else if (Is852Cmt)
        //        {
        //            return CmtOpt;
        //        }
        //        else if (Is853Cmt)
        //        {
        //            return CmtOpt;
        //        }
        //        else
        //        {
        //            return DisplayedQuantity;
        //        }
        //    }
        //    set
        //    {
        //        if (ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
        //        {
        //            CmtOpt = null;
        //            if (string.IsNullOrEmpty(value))
        //            {
        //                CmtOpt = null;
        //                Bunkatu = "";
        //                Suryo = 0;
        //            }
        //            else
        //            {
        //                var splitQties = value.Split(new[] { "+" }, StringSplitOptions.None).ToList();
        //                if (splitQties.Count < 2 || splitQties.Count > 3)
        //                {
        //                    CmtOpt = null;
        //                    Bunkatu = "";
        //                    Suryo = 0;
        //                }
        //                else
        //                {
        //                    // NOTICE: Need to set Bunkatu first then set Suryo
        //                    // When set Suryo we need to //RaisePropertyChanged for DisplayedQuantity
        //                    // After that, AfterValidate on ViewModel will call to correct data, in case we need correct Bunkatu data
        //                    CmtOpt = null;
        //                    Bunkatu = value.Replace(" ", "").Replace("　", "");
        //                    Suryo = splitQties.Select(Item => Item.AsDouble()).Sum();
        //                }
        //            }
        //        }
        //        else if (Is840Cmt)
        //        {
        //            int intConvertValue = 0;
        //            Suryo = 0;
        //            string halfSizeValue = HenkanJ.ZenToHank(value.AsString());
        //            if (!int.TryParse(halfSizeValue, out intConvertValue))
        //            {
        //                CmtOpt = null;
        //            }
        //            else
        //            {
        //                int length = CmtColKeta1 + CmtColKeta2 + CmtColKeta3 + CmtColKeta4;

        //                if (intConvertValue == 0)
        //                {
        //                    CmtOpt = null;
        //                }
        //                else
        //                {
        //                    if (value.Length > length)
        //                    {
        //                        CmtOpt = null;
        //                    }
        //                    else
        //                    {
        //                        string convertValue = "";
        //                        if (!string.IsNullOrEmpty(halfSizeValue))
        //                        {
        //                            convertValue = halfSizeValue.PadLeft(length, '0');
        //                        }

        //                        CmtOpt = HenkanJ.HankToZen(convertValue);
        //                    }
        //                }
        //            }
        //        }
        //        else if (Is842Cmt)
        //        {
        //            Suryo = 0;
        //            CmtOpt = OdrUtil.GetCmtOpt842(value);
        //        }
        //        else if (Is850Cmt)
        //        {
        //            Suryo = 0;
        //            CmtOpt = OdrUtil.GetCmtOpt850(value, ItemName);
        //        }
        //        else if (Is851Cmt)
        //        {
        //            Suryo = 0;
        //            CmtOpt = OdrUtil.GetCmtOpt851(value);
        //        }
        //        else if (Is852Cmt)
        //        {
        //            Suryo = 0;
        //            CmtOpt = OdrUtil.GetCmtOpt852(value);
        //        }
        //        else if (Is853Cmt)
        //        {
        //            Suryo = 0;
        //            CmtOpt = OdrUtil.GetCmtOpt853(value);
        //        }
        //        else
        //        {
        //            var suryo = value.AsDouble();
        //            if (suryo > 0 || (suryo < 0 && IsJihi))
        //            {
        //                CmtOpt = null;
        //                Suryo = suryo;
        //            }
        //        }
        //        //RaisePropertyChanged(() => DisplayItemName);
        //    }
        //}

        //public int CmtCol1 { get; set; }

        //public int CmtCol2 { get; set; }

        //public int CmtCol3 { get; set; }

        //public int CmtCol4 { get; set; }

        //public int CmtColKeta1 { get; set; }

        //public int CmtColKeta2 { get; set; }

        //public int CmtColKeta3 { get; set; }

        //public int CmtColKeta4 { get; set; }

        //public ReleasedDrugType ReleasedType
        //{
        //    get
        //    {
        //        if (!IsInDrugOdr)
        //        {
        //            return ReleasedDrugType.None;
        //        }
        //        if (!IsDrug && !IsInjection)
        //        {
        //            return ReleasedDrugType.None;
        //        }
        //        return OdrUtil.SyohoToSempatu(SyohoKbn, SyohoLimitKbn);
        //    }
        //}

        //public bool IsShownReleasedDrug
        //{
        //    get => IsKohatu && ReleasedType != ReleasedDrugType.None;
        //}

        //public bool IsKohatu
        //{
        //    get => KohatuKbn == 1 || KohatuKbn == 2;
        //}

        //private KensaMstModel _kensaMstModel;
        //public KensaMstModel KensaMstModel
        //{
        //    get => _kensaMstModel;
        //    set
        //    {
        //        if (Set(ref _kensaMstModel, value))
        //        {
        //            //RaisePropertyChanged(() => KensaGaichu);
        //        }
        //    }
        //}

        //private double _ten;
        //public double Ten
        //{
        //    get => _ten;
        //    set
        //    {
        //        Set(ref _ten, value);
        //    }
        //}

        //private int _handanGrpKbn;
        //public int HandanGrpKbn
        //{
        //    get => _handanGrpKbn;
        //    set
        //    {
        //        Set(ref _handanGrpKbn, value);
        //    }
        //}

        //private string _masterSbt;
        //public string MasterSbt
        //{
        //    get => _masterSbt;
        //    set
        //    {
        //        Set(ref _masterSbt, value);
        //    }
        //}

        //public bool IsSpecialItem
        //{
        //    get => MasterSbt == "S"
        //        && SinKouiKbn == 20
        //        && DrugKbn == 0
        //        && ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu;
        //}

        //private MIpnMinYakkaMstModel _ipnMinYakkaMstModel;
        //public MIpnMinYakkaMstModel IpnMinYakkaMstModel
        //{
        //    get => _ipnMinYakkaMstModel;
        //    set
        //    {
        //        Set(ref _ipnMinYakkaMstModel, value);
        //    }
        //}

        ///// <summary>
        ///// check gaichu only
        ///// </summary>
        //public bool IsKensa
        //{
        //    get => SinKouiKbn == 61 || SinKouiKbn == 64;
        //}

        //#region From TodayOdrInf
        //private int _inOutKbn;
        //public int InOutKbn
        //{
        //    get => _inOutKbn;
        //    set
        //    {
        //        if (Set(ref _inOutKbn, value))
        //        {
        //            //RaisePropertyChanged(() => KensaGaichu);
        //        }
        //    }
        //}

        //private int _odrInfOdrKouiKbn;
        //public int OdrInfOdrKouiKbn
        //{
        //    get => _odrInfOdrKouiKbn;
        //    set
        //    {
        //        if (Set(ref _odrInfOdrKouiKbn, value))
        //        {
        //            //RaisePropertyChanged(() => KensaGaichu);
        //        }
        //    }
        //}
        //public bool IsInDrugOdr
        //{
        //    get => (OdrInfOdrKouiKbn >= 20 && OdrInfOdrKouiKbn <= 23) || OdrInfOdrKouiKbn == 28;
        //}
        //#endregion

        //public int KensaGaichu
        //{
        //    get
        //    {
        //        if (IsEmpty)
        //        {
        //            return KensaGaichuText.NONE;
        //        }
        //        if (IsKensa)
        //        {
        //            bool kensaCondition;
        //            if (_systemConfigProvider.CheckKensaIraiCondition == 0)
        //            {
        //                kensaCondition = IsKensa && Kokuji1 != "7" && Kokuji1 != "9";
        //            }
        //            else
        //            {
        //                kensaCondition = SinKouiKbn == 61 && Kokuji1 != "7" && Kokuji1 != "9" && HandanGrpKbn != 6;
        //            }

        //            if (kensaCondition && InOutKbn == 1)
        //            {
        //                int kensaSetting = _systemConfigProvider.CheckKensaIrai;
        //                if (KensaMstModel == null)
        //                {
        //                    if (kensaSetting > 0)
        //                    {
        //                        return KensaGaichuText.GAICHU_NONE;
        //                    }
        //                }
        //                else if (string.IsNullOrEmpty(KensaMstModel.CenterItemCd1)
        //                    && string.IsNullOrEmpty(KensaMstModel.CenterItemCd2))
        //                {
        //                    if (kensaSetting > 1)
        //                    {
        //                        return KensaGaichuText.GAICHU_NOT_SET;
        //                    }
        //                }
        //            }
        //        }
        //        if (IsNormalComment)
        //        {
        //            if (InOutKbn == 1 && IsInDrugOdr)
        //            {
        //                if (IsNodspRece == 0)
        //                {
        //                    return KensaGaichuText.IS_DISPLAY_RECE_ON;
        //                }
        //            }
        //            else
        //            {
        //                if (IsNodspRece == 1)
        //                {
        //                    return KensaGaichuText.IS_DISPLAY_RECE_OFF;
        //                }
        //            }
        //        }
        //        return KensaGaichuText.NONE;
        //    }
        //}

        //public bool IsStandardUsage
        //{
        //    get => YohoKbn == 1 || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai;
        //}

        //public bool IsSuppUsage
        //{
        //    get => YohoKbn == 2;
        //}

        //public bool IsInjectionUsage
        //{
        //    get => (SinKouiKbn >= 31 && SinKouiKbn <= 34) || (SinKouiKbn == 30 && ItemCd.StartsWith("Z") && MasterSbt == "S");
        //}

        //public bool IsHighlight
        //{
        //    get { return false; }
        //    set { }
        //}

        //public bool IsFocused
        //{
        //    get { return false; }
        //    set { }
        //}

        //public bool IsEmpty
        //{
        //    get
        //    {
        //        return string.IsNullOrEmpty(ItemCd) &&
        //               string.IsNullOrEmpty(ItemName?.Trim()) &&
        //               SinKouiKbn == 0;
        //    }
        //}

        //public ModelStatus Status { get; private set; } = ModelStatus.None;

        ///// <summary>
        ///// Using to check need to open ACT
        ///// </summary>
        //public bool IsFreeComment => IsNormalComment || IsFree830Prefix || IsFree831Prefix || IsFree880Prefix;

        ///// <summary>
        ///// Comment input with // or ..
        ///// </summary>
        //public bool IsNormalComment => !string.IsNullOrEmpty(ItemName) && string.IsNullOrEmpty(ItemCd);

        //private bool _isStartComment;
        ///// <summary>
        ///// Input // then Enter
        ///// </summary>
        //public bool IsStartComment
        //{
        //    get => _isStartComment;
        //    set => Set(ref _isStartComment, value);
        //}

        //public bool IsComment
        //{
        //    get => IsNormalComment || Is820Cmt || Is830Cmt || Is831Cmt || Is840Cmt || Is842Cmt || Is850Cmt || Is851Cmt || Is852Cmt || Is853Cmt || Is880Cmt;
        //}

        //public bool IsFree830Prefix => Is830Cmt && OdrUtil.IsFree830Prefix(SearchingText, CmtName);

        //public bool IsFree831Prefix => Is831Cmt && OdrUtil.IsFree830Prefix(SearchingText, CmtName);

        //public bool IsFree880Prefix => Is880Cmt && OdrUtil.IsFree830Prefix(SearchingText, CmtName);

        //private string _searchingText = null;
        //public string SearchingText
        //{
        //    get
        //    {
        //        if (_searchingText == null)
        //        {
        //            _searchingText = ItemName;
        //        }
        //        return _searchingText;
        //    }
        //    set
        //    {
        //        if (_searchingText != value)
        //        {
        //            _searchingText = value;
        //        }
        //    }
        //}

        //public string DisplayFontColor
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(FontColor))
        //        {
        //            return "#" + FontColorConst.BLACK;
        //        }
        //        return "#" + FontColor;
        //    }
        //}

        //public bool IsUsage
        //{
        //    get => IsStandardUsage || IsSuppUsage || IsInjectionUsage;
        //}

        //private int _bunkatuKoui;
        //public int BunkatuKoui
        //{
        //    get => _bunkatuKoui;
        //    set
        //    {
        //        if (Set(ref _bunkatuKoui, value))
        //        {
        //            //RaisePropertyChanged(() => DisplayItemName);
        //        }
        //    }
        //}

        //public bool Is820Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment820Pattern);

        //public bool Is830Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment830Pattern);

        //public bool Is831Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment831Pattern);

        //public bool Is850Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment850Pattern);

        //public bool Is851Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment851Pattern);

        //public bool Is852Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment852Pattern);

        //public bool Is853Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment853Pattern);

        //public bool Is840Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment840Pattern) && ItemCd!=ItemCdConst.GazoDensibaitaiHozon;

        //public bool Is842Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment842Pattern);

        //public bool Is880Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment880Pattern);

        //public bool IsJihi => ItemCd != null && ItemCd.StartsWith(ItemCdConst.ItemJihi);

        //public bool IsShohoComment => SinKouiKbn == 100;

        //public bool IsShohoBiko => SinKouiKbn == 101;

        //public bool IsEditingQuantity { get; set; }

        //public bool IsDrugUsage
        //{
        //    get => YohoKbn > 0 || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai;
        //}

        //public bool IsDrug
        //{
        //    get => (SinKouiKbn == 20 && DrugKbn > 0) || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai
        //        || (SinKouiKbn == 20 && ItemCd.StartsWith("Z"));
        //}

        //public bool IsInjection
        //{
        //    get => SinKouiKbn == 30;
        //}

        //public double BackupSuryo { get; set; }

        //private int _alternationIndex;
        //public int AlternationIndex
        //{
        //    get => _alternationIndex;
        //    set => Set(ref _alternationIndex, value);
        //}

        ///// <summary>
        ///// Using to detect CmtOpt is changing
        ///// </summary>
        //public bool CmtOptChanging { get; }
        //#endregion

        //public void TodayOdrInfDetailModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == nameof(HpId))
        //    {
        //        Status = ModelStatus.Added;
        //    }
        //    else if (e.PropertyName != nameof(HpId) && Status != ModelStatus.Added)
        //    {
        //        Status = ModelStatus.Modified;
        //    }
        //}

        ///// <summary>
        ///// 算定漏れ確認用ダミー項目
        ///// </summary>
        //public bool IsDummy { get; set; } = false;
    }
}
