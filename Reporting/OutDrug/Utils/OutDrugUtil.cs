using Reporting.OutDrug.Constants;

namespace Reporting.OutDrug.Utils;

public class OutDrugUtil
{

    /// <summary>
    /// 文字結合（追加先が空でない場合は、スペースを入れる）
    /// </summary>
    /// <param name="source">追加元文字列</param>
    /// <param name="addStr">追加文字列</param>
    /// <returns>追加後文字列</returns>
    public static string AppendStr(string source, string addStr)
    {
        string ret = source;

        if (addStr != string.Empty)
        {
            if (ret != string.Empty) ret += " ";
            ret += addStr;
        }

        return ret;
    }

    /// <summary>
    /// 行追加
    /// </summary>
    /// <param name="source">追加元文字列</param>
    /// <param name="addStr">追加文字列</param>
    /// <param name="newLineCd">改行コード</param>
    /// <returns>追加後文字列</returns>
    public static string AppendLine(string source, string addStr, string newLineCd = NewLineCd.EpsCsv)
    {

        string ret = source;
        string tmp = addStr;
        if (tmp != string.Empty)
        {
            if (ret != string.Empty) ret += newLineCd;
            ret += tmp;
        }

        return ret;
    }

    /// <summary>
    /// 数値を整数部最大６桁少数部最大５桁にフォーマット
    /// </summary>
    /// <param name="number">数値</param>
    /// <returns>整数部最大６桁少数部最大５桁の数値</returns>
    public static string FormatDoubleToString(double number)
    {
        string ret = string.Empty;
        if (number < 1000000)
        {
            number = Math.Round(number, 5, MidpointRounding.AwayFromZero);
            ret = string.Format("{0:#####0.#####}", number.ToString());
        }
        return ret;
    }

}
