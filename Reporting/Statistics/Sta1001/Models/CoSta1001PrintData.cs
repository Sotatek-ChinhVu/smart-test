using Helper.Common;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta1001.Models;


public class CoSta1001PrintData
{
    public CoSta1001PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// 合計行のキャプション
    /// </summary>
    public string TotalCaption { get; set; }

    /// <summary>
    /// 合計行の件数
    /// </summary>
    public string TotalCount { get; set; }

    /// <summary>
    /// 合計行の実人数
    /// </summary>
    public string TotalPtCount { get; set; }

    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo { get; set; }

    /// <summary>
    /// 親来院番号
    /// </summary>
    public long OyaRaiinNo { get; set; }

    /// <summary>
    /// No.（連番）
    /// </summary>
    public string Seq { get; set; }

    /// <summary>
    /// 患者番号
    /// </summary>
    public string PtNum { get; set; }

    /// <summary>
    /// 診察日
    /// </summary>
    public int SinDate { get; set; }

    /// <summary>
    /// 診察日 (yyyy/MM/dd)
    /// </summary>
    public string SinDateFmt
    {
        get => CIUtil.SDateToShowSDate(SinDate);
    }

    /// <summary>
    /// 氏名
    /// </summary>
    public string PtName { get; set; }

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string PtKanaName { get; set; }

    /// <summary>
    /// 保険種別
    /// </summary>
    public string HokenSbt { get; set; }

    /// <summary>
    /// 初再診
    /// </summary>
    public string Syosaisin { get; set; }

    /// <summary>
    /// 合計点数
    /// </summary>
    public string Tensu { get; set; }

    /// <summary>
    /// 合計点数(新)
    /// </summary>
    public string NewTensu { get; set; }

    /// <summary>
    /// 負担金額
    /// </summary>
    public string PtFutan { get; set; }

    /// <summary>
    /// 負担金額(自費レセ)
    /// </summary>
    public string PtFutanJihiRece { get; set; }

    /// <summary>
    /// 負担金額(その他)
    /// </summary>
    public string PtFutanElse { get; set; }

    /// <summary>
    /// 保険外金額
    /// </summary>
    public string JihiFutan { get; set; }

    /// <summary>
    /// 内消費税
    /// </summary>
    public string JihiTax { get; set; }

    /// <summary>
    /// 調整額
    /// </summary>
    public string AdjustFutan { get; set; }

    /// <summary>
    /// 合計請求額
    /// </summary>
    public string SeikyuGaku { get; set; }

    /// <summary>
    /// 合計請求額(新)
    /// </summary>
    public string NewSeikyuGaku { get; set; }

    /// <summary>
    /// 入金区分
    /// </summary>
    public string NyukinKbn { get; set; }

    /// <summary>
    /// 支払方法略称
    /// </summary>
    public string PaySname { get; set; }

    /// <summary>
    /// 支払方法コード
    /// </summary>
    public int PayCd { get; set; }

    /// <summary>
    /// 免除額
    /// </summary>
    public string MenjyoGaku { get; set; }

    /// <summary>
    /// 入金額
    /// </summary>
    public string NyukinGaku { get; set; }

    /// <summary>
    /// 入金日
    /// </summary>
    public int NyukinDate { get; set; }

    /// <summary>
    /// 入金日 (yyyy/MM/dd)
    /// </summary>
    public string NyukinDateFmt
    {
        get => CIUtil.SDateToShowSDate(NyukinDate);
    }

    /// <summary>
    /// 前回入金額
    /// </summary>
    public string PreNyukinGaku { get; set; }

    /// <summary>
    /// 未収額
    /// </summary>
    public string MisyuGaku { get; set; }

    /// <summary>
    /// 受付種別
    /// </summary>
    public int UketukeSbt { get; set; }

    /// <summary>
    /// 受付種別名称
    /// </summary>
    public string UketukeSbtName { get; set; }

    /// <summary>
    /// 入金者ID
    /// </summary>
    public int NyukinUserId { get; set; }

    /// <summary>
    /// 入金者名
    /// </summary>
    public string NyukinUserSname { get; set; }

    /// <summary>
    /// 入金時間
    /// </summary>
    public string NyukinTime { get; set; }

    /// <summary>
    /// 診療科ID
    /// </summary>
    public int KaId { get; set; }

    /// <summary>
    /// 診療科略称
    /// </summary>
    public string KaSname { get; set; }

    /// <summary>
    /// 担当医ID
    /// </summary>
    public int TantoId { get; set; }

    /// <summary>
    /// 担当医略称
    /// </summary>
    public string TantoSname { get; set; }

    /// <summary>
    /// 受付者ID
    /// </summary>
    public int UketukeId { get; set; }

    /// <summary>
    /// 受付者名
    /// </summary>
    public string UketukeSname { get; set; }

    /// <summary>
    /// 来院時間
    /// </summary>
    public string UketukeTime { get; set; }

    /// <summary>
    /// 精算時間
    /// </summary>
    public string KaikeiTime { get; set; }

    /// <summary>
    /// 来院コメント
    /// </summary>
    public string RaiinCmt { get; set; }

    /// <summary>
    /// 入金コメント
    /// </summary>
    public string NyukinCmt { get; set; }

    /// <summary>
    /// 保険外金額（非課税分）
    /// </summary>
    public string JihiFutanTaxFree { get; set; }

    #region '課税分'
    /// <summary>
    /// 保険外金額（課税分/通常税率）
    /// </summary>
    public string JihiFutanTaxNrSum { get; set; }

    /// <summary>
    /// 保険外金額（課税分/軽減税率）
    /// </summary>
    public string JihiFutanTaxGenSum { get; set; }

    /// <summary>
    /// 内消費税額（通常税率）
    /// </summary>
    public string JihiTaxNrSum { get; set; }

    /// <summary>
    /// 内消費税額（軽減税率）
    /// </summary>
    public string JihiTaxGenSum { get; set; }
    #endregion

    #region '内税分'
    /// <summary>
    /// 保険外金額（内税/通常税率）
    /// </summary>
    public string JihiFutanTaxNr { get; set; }

    /// <summary>
    /// 保険外金額（内税/軽減税率）
    /// </summary>
    public string JihiFutanTaxGen { get; set; }

    /// <summary>
    /// 内消費税額（内税/通常税率）
    /// </summary>
    public string JihiTaxNr { get; set; }

    /// <summary>
    /// 内消費税額（内税/軽減税率）
    /// </summary>
    public string JihiTaxGen { get; set; }
    #endregion

    #region '外税分'
    /// <summary>
    /// 保険外金額（外税/通常税率）
    /// </summary>
    public string JihiFutanOuttaxNr { get; set; }

    /// <summary>
    /// 保険外金額（外税/軽減税率）
    /// </summary>
    public string JihiFutanOuttaxGen { get; set; }

    /// <summary>
    /// 内消費税額（外税/通常税率）
    /// </summary>
    public string JihiOuttaxNr { get; set; }

    /// <summary>
    /// 内消費税額（外税/軽減税率）
    /// </summary>
    public string JihiOuttaxGen { get; set; }
    #endregion

    /// <summary>
    /// 保険種別ごとの金額
    /// </summary>
    public List<string> JihiSbtFutans { get; set; }
}
