using Helper.Common;
using Helper.Constants;

namespace Reporting.Receipt.Models;

public class CoReceiptTensuModel
{
    private List<ReceSinKouiModel> _sinKoui;
    private List<SinMeiDataModel> _sinMei;
    private int _seikyuYm;
    private List<ReceFutanKbnModel> _allReceFutanKbnModels;

    public CoReceiptTensuModel(List<ReceSinKouiModel> sinKoui, List<SinMeiDataModel> sinMei, int seikyuYm, List<ReceFutanKbnModel> allReceFutanKbnModels)
    {
        _sinKoui = sinKoui;
        _sinMei = sinMei;
        _seikyuYm = seikyuYm;
        _allReceFutanKbnModels = allReceFutanKbnModels;
    }

    /// <summary>
    /// 指定の集計先の点数の合計
    /// </summary>
    /// <param name="syukeiSaki"></param>
    /// <returns></returns>
    public double Tensu(string syukeiSaki)
    {
        List<double> tensu = _sinKoui.Where(p => p.SyukeiSaki == syukeiSaki).GroupBy(p => p.Ten).Select(p => p.Key).ToList();
        double ret = 0;
        if (tensu.Count == 1)
        {
            ret = tensu[0];
        }
        return ret;
    }

    public double Tensu(List<string> syukeiSakis)
    {
        List<double> tensu = _sinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki)).GroupBy(p => p.Ten).Select(p => p.Key).ToList();
        double ret = 0;
        if (tensu.Count == 1)
        {
            ret = tensu[0];
        }
        return ret;
    }

    public double TensuSum(List<string> syukeiSakis)
    {
        double ret = _sinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki)).Sum(p => p.Ten);

        return ret;
    }

    /// <summary>
    /// 指定の集計先の回数の合計
    /// </summary>
    /// <param name="syukeiSaki"></param>
    /// <returns></returns>
    public int TenColCount(string syukeiSaki, bool onlySI)
    {
        int ret = 0;

        if (onlySI)
        {
            ret = _sinKoui.Where(p => p.SyukeiSaki == syukeiSaki && p.RecId == "SI").Sum(p => p.TenColCount);
        }
        else
        {
            ret = _sinKoui.Where(p => p.SyukeiSaki == syukeiSaki).Sum(p => p.TenColCount);
        }

        return ret;
    }
    public int TenColCount(List<string> syukeiSakis, bool onlySI)
    {
        int ret = 0;

        if (onlySI)
        {
            ret = _sinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki) && p.RecId == "SI").Sum(p => p.TenColCount);
        }
        else
        {
            ret = _sinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki)).Sum(p => p.TenColCount);
        }

        return ret;
    }
    /// <summary>
    /// 指定の集計先の合計点数
    /// </summary>
    /// <param name="syukeiSaki"></param>
    /// <returns></returns>
    public double TotalTen(string syukeiSaki)
    {
        double ret = _sinKoui.Where(p => p.SyukeiSaki == syukeiSaki).Sum(p => p.TotalTen);

        return ret;
    }
    public double TotalTen(List<string> syukeiSakis)
    {
        double ret = _sinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki)).Sum(p => p.TotalTen);

        return ret;
    }
    public double TotalKingaku(List<string> syukeiSakis)
    {
        double ret = _sinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki)).Sum(p => p.Kingaku);

        return ret;
    }

    public double TensuKohi(List<string> syukeiSakis, int kohiIndex)
    {
        List<int> pIds = GetKohiPids(kohiIndex);
        double ret = _sinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki) && pIds.Contains(p.HokenPid)).Sum(p => p.Ten);

        return ret;
    }
    public int TenColCountKohi(List<string> syukeiSakis, int kohiIndex)
    {
        List<int> pIds = GetKohiPids(kohiIndex);
        int ret = _sinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki) && pIds.Contains(p.HokenPid)).Sum(p => p.TenColCount);

        return ret;
    }
    public double TotalTenKohi(List<string> syukeiSakis, int kohiIndex)
    {
        List<int> pIds = GetKohiPids(kohiIndex);
        double ret = _sinKoui.Where(p => syukeiSakis.Contains(p.SyukeiSaki) && pIds.Contains(p.HokenPid)).Sum(p => p.TotalTen);

        return ret;
    }

    /// <summary>
    /// 指定の負担区分に関わる公費の番号を返す
    /// </summary>
    /// <param name="futanKbn"></param>
    /// <returns></returns>
    public List<int> FutanKbnToKohiIndex(string futanKbn)
    {
        List<int> ret = new List<int>();
        (string pattern, string futanKbn) findFutanPattern = FutanKbnConst.futanPatternls.FirstOrDefault(p => p.futanKbnCd == futanKbn);

        if (!string.IsNullOrEmpty(findFutanPattern.pattern) && !string.IsNullOrEmpty(findFutanPattern.futanKbn))
        {
            for (int i = 0; i < findFutanPattern.pattern.Length; i++)
            {
                if (findFutanPattern.pattern.Substring(i, 1) == "1")
                {
                    ret.Add(i);
                }
            }
        }

        return ret;
    }

    public List<string> FutanKbns
    {
        get
        {
            return _sinMei?.GroupBy(p => p.FutanKbn).Select(p => p.Key).ToList();
        }
    }
    public List<(int count, double kingaku)> TenColKingakuSonota(string syukeiSaki)
    {
        List<(int count, double kingaku)> ret = new List<(int count, double kingaku)>();

        _sinKoui.FindAll(p => p.SyukeiSaki == syukeiSaki).ForEach(p => ret.Add((p.Count, p.TotalTen)));

        return ret;
    }

    public string SyosinJikangai
    {
        get
        {
            string ret = "";

            if (_sinMei.Any(p => p.SyukeiSaki == ReceSyukeisaki.SyosinJikanGai))
            {
                ret += "時間外";
            }

            if (_sinMei.Any(p => p.SyukeiSaki == ReceSyukeisaki.SyosinKyujitu))
            {
                if (!string.IsNullOrEmpty(ret))
                {
                    ret += "・";
                }
                ret += "休日";
            }

            if (_sinMei.Any(p => p.SyukeiSaki == ReceSyukeisaki.SyosinSinya))
            {
                if (!string.IsNullOrEmpty(ret))
                {
                    ret += "・";
                }
                ret += "深夜";
            }

            if (!string.IsNullOrEmpty(ret))
            {
                ret = $"({ret})";
            }

            return ret;
        }

    }

    /// <summary>
    /// 指定の公費（第１、第２・・・）に紐づくPidを取得する
    /// </summary>
    /// <param name="Index">公費のインデックス</param>
    /// <returns></returns>
    public List<int> GetKohiPids(int Index)
    {
        List<int> ret = new();

        List<string> kohiFutanKbnList = new();

        switch (Index)
        {
            case 1:
                kohiFutanKbnList =
                    new List<string>
                    {
                            "5", "2", "7", "H", "I", "4", "M", "N", "R", "S", "T", "V", "W", "X", "Z", "9"
                    };
                break;
            case 2:
                kohiFutanKbnList =
                    new List<string>
                    {
                            "6", "3", "7", "J", "K", "4", "O", "P", "R", "S", "U", "V", "W", "Y", "Z", "9"
                    };
                break;
            case 3:
                kohiFutanKbnList =
                    new List<string>
                    {
                            "B", "E", "H", "J", "L", "M", "O", "Q", "R", "T", "U", "V", "X", "Y", "Z", "9"
                    };
                break;
            case 4:
                kohiFutanKbnList =
                    new List<string>
                    {
                            "C", "G", "I", "K", "L", "N", "P", "Q", "S", "T", "U", "W", "X", "Y", "Z", "9"
                    };
                break;
            default:
                kohiFutanKbnList = new List<string>();
                break;
        }

        var receFutanKbnModels = _allReceFutanKbnModels.FindAll(item => item.SeikyuYm == _seikyuYm);

        if (receFutanKbnModels != null)
        {
            ret = receFutanKbnModels.FindAll(p => kohiFutanKbnList.Contains(p.FutanKbnCd)).GroupBy(p => p.HokenPid).Select(p => p.Key).ToList();
        }

        return ret;
    }

    #region private function

    #endregion
}
