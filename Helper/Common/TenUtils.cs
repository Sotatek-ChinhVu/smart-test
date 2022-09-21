using System.Text;

namespace Helper.Common;

public static class TenUtils
{
    public static string GetBunkatu(int KouiKbn, string bunkatu)
    {
        string result = string.Empty;
        if (string.IsNullOrEmpty(bunkatu))
        {
            return result;
        }
        string sTani;
        if (KouiKbn == 21)
        {
            // 内服
            sTani = "日分";
        }
        else
        {
            sTani = "回分";
        }

        string[] sKatu = bunkatu.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

        StringBuilder sbResult = new();
        foreach (string katu in sKatu)
        {
            if (!string.IsNullOrEmpty(result))
            {
                sbResult.Append("，");
            }
            sbResult.Append(katu + sTani);
        }
        result = sbResult.ToString();

        if (!string.IsNullOrEmpty(result))
        {
            result = "（" + result + "）";
        }

        return result;
    }
}
