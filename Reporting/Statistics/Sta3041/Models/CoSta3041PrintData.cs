using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta3041.Models;

public class CoSta3041PrintData
{
    public CoSta3041PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// 合計行のタイトル
    /// </summary>
    public string TotalKbn { get; set; }

    /// <summary>
    /// 合計行のキャプション
    /// </summary>
    public string TotalCaption { get; set; }

    /// <summary>
    /// 合計行の件数
    /// </summary>
    public string TotalVal { get; set; }

    /// <summary>
    /// 診療年月
    /// </summary>
    public string SinYm { get; set; }

    /// <summary>
    /// 患者番号
    /// </summary>
    public string PtNum { get; set; }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNumL { get; set; }

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string KanaName { get; set; }

    /// <summary>
    /// 漢字氏名
    /// </summary>
    public string PtName { get; set; }

    /// <summary>
    /// 診療日
    /// </summary>
    public int SinDateI { get; set; }

    /// <summary>
    /// 診療日
    /// </summary>
    public string SinDate { get; set; }

    /// <summary>
    /// 向精神薬区分コード
    /// 1:抗不安薬 2:睡眠薬 3:抗うつ薬 4:抗精神病薬
    /// </summary>
    public string KouseisinKbnCd { get; set; }

    /// <summary>
    /// 向精神薬区分
    /// </summary>
    public string KouseisinKbn { get; set; }

    /// <summary>
    /// 種類数
    /// </summary>
    public string DrugCount { get; set; }

    /// <summary>
    /// 薬価基準コード
    /// </summary>
    public string YakkaCd { get; set; }

    /// <summary>
    /// 薬価基準コード前7桁
    /// </summary>
    public string YakkaCd7 { get; set; }

    /// <summary>
    /// 診療行為コード
    /// </summary>
    public string ItemCd { get; set; }

    /// <summary>
    /// 医薬品名称
    /// </summary>
    public string DrugName { get; set; }

    /// <summary>
    /// 多剤投与
    /// </summary>
    public string Tazai { get; set; }

    /// <summary>
    /// 多剤_抗不安薬
    /// </summary>
    public string TazaiFuan { get; set; }

    /// <summary>
    /// 多剤_睡眠薬
    /// </summary>
    public string TazaiSuimin { get; set; }

    /// <summary>
    /// 多剤_抗うつ薬
    /// </summary>
    public string TazaiUtu { get; set; }

    /// <summary>
    /// 多剤_抗精神病薬
    /// </summary>
    public string TazaiSeisin { get; set; }

    /// <summary>
    /// 多剤_抗うつ抗精神病
    /// </summary>
    public string TazaiUtuSeisin { get; set; }

    /// <summary>
    /// 多剤_抗不安睡眠
    /// </summary>
    public string TazaiFuanSuimin { get; set; }

    /// <summary>
    /// 明細区分
    /// 0:明細 1:小計 2:実人数計
    /// </summary>
    public int MeisaiKbn { get; set; }

    /// <summary>
    /// 計見出し
    /// </summary>
    public string MeisaiKbnName
    {
        get
        {
            switch (MeisaiKbn)
            {
                case 0: return "明細";
                case 1: return "小計";
                case 2: return "実人数計";
            }
            return "";
        }
    }

    /// <summary>
    /// 投与患者数
    /// 明細…0：該当しない、1：該当、-:該当するが、既に０でカウント済み
    /// </summary>
    public string PtCnt { get; set; }

    /// <summary>
    /// 多剤投与患者数
    /// </summary>
    public string TazaiPtCnt { get; set; }

    /// <summary>
    /// 多剤_抗不安薬投与患者数
    /// </summary>
    public string TazaiFuanPtCnt { get; set; }

    /// <summary>
    /// 多剤_睡眠薬投与患者数
    /// </summary>
    public string TazaiSuiminPtCnt { get; set; }

    /// <summary>
    /// 多剤_抗うつ薬投与患者数
    /// </summary>
    public string TazaiUtuPtCnt { get; set; }

    /// <summary>
    /// 多剤_抗精神病薬投与患者数
    /// </summary>
    public string TazaiSeisinPtCnt { get; set; }

    /// <summary>
    /// 多剤_抗うつ抗精神病投与患者数
    /// </summary>
    public string TazaiUtuSeisinPtCnt { get; set; }

    /// <summary>
    /// 多剤_抗不安睡眠投与患者数
    /// </summary>
    public string TazaiFuanSuiminPtCnt { get; set; }

    /// <summary>
    /// 抗うつ薬投与患者数
    /// </summary>
    public string UtuPtCnt { get; set; }

    /// <summary>
    /// 抗精神病薬投与患者数
    /// </summary>
    public string SeisinPtCnt { get; set; }

    /// <summary>
    /// 抗うつ抗精神病投与患者数
    /// </summary>
    public string UtuSeisinPtCnt { get; set; }
}