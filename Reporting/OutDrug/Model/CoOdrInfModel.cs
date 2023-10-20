using Entity.Tenant;
using Reporting.CommonMasters.Constants;

namespace Reporting.OutDrug.Model;

public class CoOdrInfModel
{
    public OdrInf OdrInf { get; }
    public PtHokenPattern PtHokenPattern { get; }

    public CoOdrInfModel(OdrInf odrInf, PtHokenPattern ptHokenPattern)
    {
        OdrInf = odrInf;
        PtHokenPattern = ptHokenPattern;
    }

    /// <summary>
    /// オーダー情報
    /// </summary>
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    public int HpId
    {
        get { return OdrInf.HpId; }
    }

    /// <summary>
    /// 患者ID
    ///     患者を識別するためのシステム固有の番号
    /// </summary>
    public long PtId
    {
        get { return OdrInf.PtId; }
    }

    /// <summary>
    /// 診療日
    ///     yyyymmdd
    /// </summary>
    public int SinDate
    {
        get { return OdrInf.SinDate; }
    }

    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo
    {
        get { return OdrInf.RaiinNo; }
    }

    /// <summary>
    /// 剤番号
    /// </summary>
    public long RpNo
    {
        get { return OdrInf.RpNo; }
    }

    /// <summary>
    /// 剤枝番
    ///     剤に変更があった場合、カウントアップ
    /// </summary>
    public long RpEdaNo
    {
        get { return OdrInf.RpEdaNo; }
    }

    /// <summary>
    /// ID
    /// </summary>
    public long Id
    {
        get { return OdrInf.Id; }
    }

    /// <summary>
    /// 保険組合せID
    /// </summary>
    public int HokenPid
    {
        get { return OdrInf.HokenPid; }
    }

    /// <summary>
    /// オーダー行為区分
    /// </summary>
    public int OdrKouiKbn
    {
        get { return OdrInf.OdrKouiKbn; }
    }

    /// <summary>
    /// 剤名称
    /// </summary>
    public string RpName
    {
        get { return OdrInf.RpName ?? string.Empty; }
    }

    /// <summary>
    /// 院内院外区分
    ///     0: 院内
    ///     1: 院外
    /// </summary>
    public int InoutKbn
    {
        get { return OdrInf.InoutKbn; }
    }

    /// <summary>
    /// 至急区分
    ///     0: 通常 
    ///     1: 至急
    /// </summary>
    public int SikyuKbn
    {
        get { return OdrInf.SikyuKbn; }
    }

    /// <summary>
    /// 処方種別
    ///     0: 日数判断
    ///     1: 臨時
    ///     2: 常態
    /// </summary>
    public int SyohoSbt
    {
        get { return OdrInf.SyohoSbt; }
    }

    /// <summary>
    /// 算定区分
    ///     1: 算定外
    ///     2: 自費算定
    /// </summary>
    public int SanteiKbn
    {
        get { return OdrInf.SanteiKbn; }
    }

    /// <summary>
    /// 透析区分
    ///     0: 透析以外
    ///     1: 透析前
    ///     2: 透析後
    /// </summary>
    public int TosekiKbn
    {
        get { return OdrInf.TosekiKbn; }
    }

    /// <summary>
    /// 日数回数
    ///     処方日数
    /// </summary>
    public int DaysCnt
    {
        get { return OdrInf.DaysCnt; }
    }

    /// <summary>
    /// 並び順
    /// </summary>
    public int SortNo
    {
        get { return OdrInf.SortNo; }
    }

    /// <summary>
    /// 削除区分
    ///     1:削除
    ///     2:未確定削除
    /// </summary>
    public int IsDeleted
    {
        get { return OdrInf.IsDeleted; }
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
    /// 保険ID
    /// </summary>
    public int HokenId
    {
        get => PtHokenPattern == null ? 0 : PtHokenPattern.HokenId;
    }
    /// <summary>
    /// 公費１ID
    /// </summary>
    public int Kohi1Id
    {
        get => PtHokenPattern == null ? 0 : PtHokenPattern.Kohi1Id;
    }
    /// <summary>
    /// 公費２ID
    /// </summary>
    public int Kohi2Id
    {
        get => PtHokenPattern == null ? 0 : PtHokenPattern.Kohi2Id;
    }
    /// <summary>
    /// 公費３ID
    /// </summary>
    public int Kohi3Id
    {
        get => PtHokenPattern == null ? 0 : PtHokenPattern.Kohi3Id;
    }
    /// <summary>
    /// 公費４ID
    /// </summary>
    public int Kohi4Id
    {
        get => PtHokenPattern == null ? 0 : PtHokenPattern.Kohi4Id;
    }
    /// <summary>
    /// 公費IDを取得する
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int KohiId(int index)
    {
        int ret = 0;

        switch (index)
        {
            case 1:
                ret = Kohi1Id;
                break;
            case 2:
                ret = Kohi2Id;
                break;
            case 3:
                ret = Kohi3Id;
                break;
            case 4:
                ret = Kohi4Id;
                break;
        }

        return ret;
    }

    public int Kohi1Futan { get; set; }

    public int Kohi2Futan { get; set; }

    public int Kohi3Futan { get; set; }

    public int KohiSpFutan { get; set; }

    /// <summary>
    /// ソートキー
    /// 公費１が下になるようにする
    /// </summary>
    public int KohiSortKey
    {
        get
        {
            if (this.Kohi1Futan > 1) return 4;
            if (this.Kohi2Futan > 1) return 3;
            if (this.Kohi3Futan > 1) return 2;
            if (this.KohiSpFutan > 1) return 1;

            return 0;
        }
    }
    /// <summary>
    /// 負担番号
    /// 公費１を使用している場合は1、公費１を使用せず公費２を使用している場合は2・・・
    /// </summary>
    public int KohiFutan
    {
        get
        {
            if (this.Kohi1Futan > 1) return 1;
            if (this.Kohi2Futan > 1) return 2;
            if (this.Kohi3Futan > 1) return 3;
            if (this.KohiSpFutan > 1) return 4;

            return 0;
        }
    }
    /// <summary>
    /// リフィルの回数
    /// </summary>
    public int Refill { get; set; } = 0;
}
