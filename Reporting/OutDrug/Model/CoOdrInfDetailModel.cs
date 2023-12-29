using Entity.Tenant;
using Reporting.CommonMasters.Constants;

namespace Emr.Report.OutDrug.Model;

public class CoOdrInfDetailModel
{
    public OdrInfDetail OdrInfDetail { get; set; }
    public OdrInf OdrInf { get; set; }
    public TenMst TenMst { get; set; }
    public YohoMst YohoMst { get; set; }
    public PtHokenPattern PtHokenPattern { get; set; }

    readonly string[] _bunkatu;

    public CoOdrInfDetailModel(OdrInfDetail odrInfDetail, OdrInf odrInf, TenMst tenMst, PtHokenPattern ptHokenPattern, YohoMst yohoMst)
    {
        OdrInfDetail = odrInfDetail;
        OdrInf = odrInf;
        TenMst = tenMst;
        PtHokenPattern = ptHokenPattern;
        YohoMst = yohoMst;
        _bunkatu = new string[0];
        if (OdrInfDetail.Bunkatu != null && OdrInfDetail.Bunkatu != "")
        {
            _bunkatu = OdrInfDetail.Bunkatu.Split('+');
        }
    }

    /// <summary>
    /// オーダー情報詳細
    /// </summary>
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    public int HpId
    {
        get { return OdrInfDetail.HpId; }
    }

    /// <summary>
    /// 患者ID
    ///       患者を識別するためのシステム固有の番号
    /// </summary>
    public long PtId
    {
        get { return OdrInfDetail.PtId; }
    }

    /// <summary>
    /// 診療日
    ///       yyyyMMdd
    /// </summary>
    public int SinDate
    {
        get { return OdrInfDetail.SinDate; }
    }

    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo
    {
        get { return OdrInfDetail.RaiinNo; }
    }

    /// <summary>
    /// 剤番号
    ///     ODR_INF.RP_NO
    /// </summary>
    public long RpNo
    {
        get { return OdrInfDetail.RpNo; }
    }

    /// <summary>
    /// 剤枝番
    ///     ODR_INF.RP_EDA_NO
    /// </summary>
    public long RpEdaNo
    {
        get { return OdrInfDetail.RpEdaNo; }
    }

    /// <summary>
    /// 行番号
    /// </summary>
    public int RowNo
    {
        get { return OdrInfDetail.RowNo; }
    }

    /// <summary>
    /// 診療行為区分
    /// </summary>
    public int SinKouiKbn
    {
        get { return OdrInfDetail.SinKouiKbn; }
    }

    /// <summary>
    /// 項目コード
    /// </summary>
    public string ItemCd
    {
        get { return OdrInfDetail.ItemCd ?? ""; }
    }

    /// <summary>
    /// 項目名称
    /// </summary>
    public string ItemName
    {
        get { return OdrInfDetail.ItemName ?? ""; }
    }

    /// <summary>
    /// 数量
    /// </summary>
    public double Suryo
    {
        get { return OdrInfDetail.Suryo; }
    }

    /// <summary>
    /// 単位名称
    /// </summary>
    public string UnitName
    {
        get { return OdrInfDetail.UnitName ?? ""; }
    }

    /// <summary>
    /// 単位種別
    ///         0: 1,2以外
    ///         1: TEN_MST.単位
    ///         2: TEN_MST.数量換算単位
    /// </summary>
    public int UnitSBT
    {
        get { return OdrInfDetail.UnitSBT; }
    }

    /// <summary>
    /// 単位換算値
    ///          UNIT_SBT=0 -> TEN_MST.ODR_TERM_VAL
    ///          UNIT_SBT=0 -> TEN_MST.ODR_TERM_VAL
    /// </summary>
    public double TermVal
    {
        get { return OdrInfDetail.TermVal; }
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
        get { return OdrInfDetail.KohatuKbn; }
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
        get { return OdrInfDetail.SyohoKbn; }
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
        get { return OdrInfDetail.SyohoLimitKbn; }
    }

    /// <summary>
    /// 薬剤区分
    ///        当該医薬品の薬剤区分を表す。
    ///             0: 薬剤以外
    ///             1: 内用薬
    ///             3: その他
    ///             4: 注射薬
    ///             6: 外用薬
    ///             8: 歯科用薬剤
    /// </summary>
    public int DrugKbn
    {
        get
        {
            int ret = 0;
            if (TenMst != null)
            {
                ret = TenMst.DrugKbn;
            }
            return ret;
        }
    }


    /// <summary>
    /// 告示等識別区分（１）
    ///        当該診療行為についてコンピューター運用上の取扱い（磁気媒体に記録する際の取扱い）を表す。
    ///          1: 基本項目（告示）　※基本項目
    ///          3: 合成項目　　　　　※基本項目
    ///          5: 準用項目（通知）　※基本項目
    ///          7: 加算項目　　　　　※加算項目
    ///          9: 通則加算項目　　　※加算項目
    ///          0: 診療行為以外（薬剤、特材等）
    /// </summary>
    public string Kokuji1
    {
        get { return OdrInfDetail.Kokuji1 ?? string.Empty; }
    }

    /// <summary>
    /// 告示等識別区分（２）
    ///        当該診療行為について点数表上の取扱いを表す。
    ///           1: 基本項目（告示）
    ///           3: 合成項目
    ///     （削）5: 準用項目（通知）
    ///          7: 加算項目（告示）
    ///       削）9: 通則加算項目
    ///           0: 診療行為以外（薬剤、特材等）
    /// </summary>
    public string Kokiji2
    {
        get { return OdrInfDetail.Kokiji2 ?? string.Empty; }
    }

    /// <summary>
    /// レセ非表示区分
    ///          0: 表示
    ///          1: 非表示
    /// </summary>
    public int IsNodspRece
    {
        get { return OdrInfDetail.IsNodspRece; }
    }

    /// <summary>
    /// 一般名コード
    /// </summary>
    public string IpnCd
    {
        get { return OdrInfDetail.IpnCd ?? string.Empty; }
    }

    /// <summary>
    /// 一般名
    /// </summary>
    public string IpnName
    {
        get { return OdrInfDetail.IpnName ?? string.Empty; }
    }

    /// <summary>
    /// 実施区分
    ///          0: 未実施
    ///          1: 実施
    /// </summary>
    public int JissiKbn
    {
        get { return OdrInfDetail.JissiKbn; }
    }

    /// <summary>
    /// 実施日時
    /// </summary>
    public DateTime? JissiDate
    {
        get { return OdrInfDetail.JissiDate; }
    }

    /// <summary>
    /// 実施者
    /// </summary>
    public int JissiId
    {
        get { return OdrInfDetail.JissiId; }
    }

    /// <summary>
    /// 実施端末
    /// </summary>
    public string JissiMachine
    {
        get { return OdrInfDetail.JissiMachine ?? string.Empty; }
    }

    /// <summary>
    /// 検査依頼コード
    /// </summary>
    public string ReqCd
    {
        get { return OdrInfDetail.ReqCd ?? string.Empty; }
    }

    /// <summary>
    /// 分割調剤
    ///        7日単位の3分割の場合 "7+7+7"
    /// </summary>
    public string Bunkatu
    {
        get { return OdrInfDetail.Bunkatu ?? string.Empty; }
    }
    /// <summary>
    /// 分割回
    /// </summary>
    public string BunkatuKai(int index)
    {
        string ret = "";

        if (_bunkatu != null && _bunkatu.Length > 0 && _bunkatu.Length >= index)
        {
            ret = _bunkatu[index - 1];
        }

        return ret;

    }

    /// <summary>
    /// 分割回数
    /// </summary>
    public int BunkatuCount
    {
        get => _bunkatu == null ? 0 : _bunkatu.Count();
    }

    /// <summary>
    /// コメントマスターの名称
    ///        ※当該項目がコメント項目の場合に使用
    /// </summary>
    public string CmtName
    {
        get { return OdrInfDetail.CmtName ?? string.Empty; }
    }

    /// <summary>
    /// コメント文
    ///        コメントマスターの定型文に組み合わせる文字情報
    ///        ※当該項目がコメント項目の場合に使用
    /// </summary>
    public string CmtOpt
    {
        get { return OdrInfDetail.CmtOpt ?? string.Empty; }
    }

    /// <summary>
    /// 文字色
    /// </summary>
    public string FontColor
    {
        get { return OdrInfDetail.FontColor ?? string.Empty; }
    }

    /// <summary>
    /// コメント改行区分
    ///          0: 改行する
    ///          1: 改行しない
    /// </summary>
    public int CommentNewLine
    {
        get { return OdrInfDetail.CommentNewline; }
    }
    //-------------------------------------------------------------------------

    /// <summary>
    /// 保険区分
    ///  0:自費
    ///  1:社保
    ///  2:国保
    ///  11:労災(短期給付)
    ///  12:労災(傷病年金)
    ///  13:アフターケア
    ///  14:自賠責
    /// </summary>
    public int HokenSyu
    {
        get
        {
            if (new List<int> { 1, 2 }.Contains(PtHokenPattern.HokenKbn))
            {
                return HokenSyuConst.Kenpo;
            }
            else if (new List<int> { 11, 12 }.Contains(PtHokenPattern.HokenKbn))
            {
                return HokenSyuConst.Rosai;
            }
            else if (new List<int> { 13 }.Contains(PtHokenPattern.HokenKbn))
            {
                return HokenSyuConst.After;
            }
            else if (new List<int> { 14 }.Contains(PtHokenPattern.HokenKbn))
            {
                return HokenSyuConst.Jibai;
            }
            else
            {
                return HokenSyuConst.Jihi;
            }
        }
    }
    /// <summary>
    /// 保険パターンID
    /// </summary>
    public int HokenPId
    {
        get => PtHokenPattern.HokenPid;
    }

    /// <summary>
    /// マスタ種別
    /// </summary>
    public string MasterSbt
    {
        get
        {
            string ret = string.Empty;

            if (TenMst != null)
            {
                ret = TenMst.MasterSbt ?? string.Empty;
            }

            return ret;
        }
    }
    /// <summary>
    /// 算定区分
    /// </summary>
    public int SanteiKbn
    {
        get => OdrInf.SanteiKbn;
    }
    /// <summary>
    /// 麻毒区分
    /// </summary>
    public int MadokuKbn
    {
        get { return TenMst == null ? 0 : TenMst.MadokuKbn; }
    }
    /// <summary>
    /// 用法区分
    /// </summary>
    public int YohoKbn
    {
        get { return TenMst == null ? 0 : TenMst.YohoKbn; }
    }

    /// <summary>
    /// 用法補足区分
    /// </summary>
    public int YohoHosokuKbn
    {
        get { return TenMst == null ? 0 : TenMst.YohoHosokuKbn; }
    }

    /// <summary>
    /// 用法コード
    /// </summary>
    public string YohoCd
    {
        get { return YohoMst == null ? string.Empty : YohoMst.YohoCd; }
    }

    /// <summary>
    /// 用法名称
    /// </summary>
    public string YohoName
    {
        get { return YohoMst == null ? OdrInfDetail.ItemName ?? string.Empty : YohoMst.YohoName; }
    }

    public string ReceName
    {
        get { return TenMst == null ? string.Empty : TenMst.ReceName ?? string.Empty; }
    }

    public string SanteiItemCd
    {
        get { return TenMst == null ? string.Empty : TenMst.SanteiItemCd ?? string.Empty; }
    }

    public bool IsTokuzai
    {
        get
        {
            return (MasterSbt == "T" ||
              (!string.IsNullOrEmpty(ItemCd) && ItemCd.StartsWith("Z")));
        }
    }

    /// <summary>
    /// 不均等服用
    /// </summary>
    public bool IsFukinto
    {
        get
        {
            bool ret = false;

            if (TenMst != null && TenMst?.YohoKbn > 0)
            {
                int[] fukuyoTimings = {
                    TenMst.FukuyoRise,
                    TenMst.FukuyoMorning,
                    TenMst.FukuyoDaytime,
                    TenMst.FukuyoNight,
                    TenMst.FukuyoSleep};

                int fukuyo = 0;

                foreach (var fukuyoTiming in fukuyoTimings)
                {
                    if (fukuyoTiming > 0)
                    {
                        if (fukuyo > 0 && fukuyo != fukuyoTiming)
                        {
                            // 服用量がタイミングによって異なる場合は不均等
                            ret = true;
                            continue;
                        }
                        fukuyo = fukuyoTiming;
                    }
                }
            }

            return ret;
        }
    }

    /// <summary>
    /// YJコード
    /// </summary>
    public string YjCd
    {
        get { return TenMst == null ? string.Empty : TenMst.YjCd ?? string.Empty; }
    }
}
