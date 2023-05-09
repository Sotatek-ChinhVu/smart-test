using Domain.Constant;
using Entity.Tenant;
using Helper.Common;

namespace Reporting.Statistics.Sta9000.Models;

public class CoPtByomeiModel
{
    public PtByomei PtByomei { get; private set; }

    public CoPtByomeiModel(PtByomei ptByomei)
    {
        PtByomei = ptByomei;
    }

    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId
    {
        get => PtByomei.PtId;
    }

    /// <summary>
    /// 基本病名コード
    ///     コードを使用しない場合、「0000999」をセット
    /// </summary>
    public string ByomeiCd
    {
        get => PtByomei.ByomeiCd ?? string.Empty;
    }

    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd1
    {
        get => PtByomei.SyusyokuCd1 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd2
    {
        get => PtByomei.SyusyokuCd2 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd3
    {
        get => PtByomei.SyusyokuCd3 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd4
    {
        get => PtByomei.SyusyokuCd4 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd5
    {
        get => PtByomei.SyusyokuCd5 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd6
    {
        get => PtByomei.SyusyokuCd6 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd7
    {
        get => PtByomei.SyusyokuCd7 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd8
    {
        get => PtByomei.SyusyokuCd8 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd9
    {
        get => PtByomei.SyusyokuCd9 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd10
    {
        get => PtByomei.SyusyokuCd10 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd11
    {
        get => PtByomei.SyusyokuCd11 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd12
    {
        get => PtByomei.SyusyokuCd12 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd13
    {
        get => PtByomei.SyusyokuCd13 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd14
    {
        get => PtByomei.SyusyokuCd14 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd15
    {
        get => PtByomei.SyusyokuCd15 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd16
    {
        get => PtByomei.SyusyokuCd16 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd17
    {
        get => PtByomei.SyusyokuCd17 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd18
    {
        get => PtByomei.SyusyokuCd18 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd19
    {
        get => PtByomei.SyusyokuCd19 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd20
    {
        get => PtByomei.SyusyokuCd20 ?? string.Empty;
    }
    /// <summary>
    /// 修飾語コード
    /// </summary>
    public string SyusyokuCd21
    {
        get => PtByomei.SyusyokuCd21 ?? string.Empty;
    }

    /// <summary>
    /// 病名
    /// </summary>
    public string Byomei
    {
        get => PtByomei.Byomei ?? string.Empty;
    }

    /// <summary>
    /// 開始日
    /// </summary>
    public string StartDate
    {
        get => CIUtil.SDateToShowSDate(PtByomei.StartDate);
    }

    /// <summary>
    /// 転帰日
    /// </summary>
    public string TenkiDate
    {
        get => CIUtil.SDateToShowSDate(PtByomei.TenkiDate);
    }

    /// <summary>
    /// 転帰区分
    ///     転帰区分を表す。
    ///         1: 下記以外
    ///         2: 治ゆ
    ///         3: 死亡
    ///         4: 中止
    /// </summary>
    public string TenkiKbn
    {
        get
        {
            switch (PtByomei.TenkiKbn)
            {
                case TenkiKbnConst.Cured: return "治ゆ";
                case TenkiKbnConst.Dead: return "死亡";
                case TenkiKbnConst.Canceled: return "中止";
            }
            return "";
        }
    }

    /// <summary>
    /// 主病名区分
    ///     0: 主病名以外
    ///     1: 主病名
    /// </summary>
    public int SyubyoKbn
    {
        get => PtByomei.SyubyoKbn;
    }

    /// <summary>
    /// 慢性疾患区分
    /// </summary>
    public int SikkanKbn
    {
        get => PtByomei.SikkanKbn;
    }

    /// <summary>
    /// 慢性疾患区分名称
    /// </summary>
    public string SikkanKbnName
    {
        get
        {
            switch (PtByomei.SikkanKbn)
            {
                case 3: return "皮(1)";
                case 4: return "皮(2)";
                case 5: return "特疾";
                case 7: return "てんかん";
                case 8: return "特疾又はてんかん";
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// 難病外来コード
    ///     当該傷病名が難病外来指導管理料の算定対象であるか否かを表す。
    ///     00: 算定対象外
    ///     09: 難病外来指導管理料算定対象
    /// </summary>
    public int NanbyoCd
    {
        get => PtByomei.NanByoCd;
    }

    /// <summary>
    /// 補足コメント
    /// </summary>
    public string HosokuCmt
    {
        get => PtByomei.HosokuCmt ?? string.Empty;
    }

    /// <summary>
    /// 保険レセプトID
    /// </summary>
    public int HokenRid
    {
        get => PtByomei.HokenPid;
    }

    /// <summary>
    /// 当月病名区分
    ///     1: 当月病名
    /// </summary>
    public int TogetuByomei
    {
        get => PtByomei.TogetuByomei;
    }

    /// <summary>
    /// レセプト非表示区分
    ///     1: 非表示
    /// </summary>
    public int IsNodspRece
    {
        get => PtByomei.IsNodspRece;
    }

    /// <summary>
    /// カルテ非表示区分
    ///     1: 非表示
    /// </summary>
    public int IsNodspKarte
    {
        get => PtByomei.IsNodspKarte;
    }
}
