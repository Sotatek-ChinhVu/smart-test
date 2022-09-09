using Domain.Models.Byomei;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class ByomeiRepository : IByomeiRepository
{
    private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
    public ByomeiRepository(ITenantProvider tenantProvider)
    {
        _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
    }
    public List<ByomeiMstModel> DiseaseSearch(bool isPrefix, bool isByomei, bool isSuffix, string keyword, int pageIndex, int pageCount)
    {
        var keywordAfterConvert = keyword != String.Empty ? keyword.ToUpper()
                                        .Replace("ｧ", "ｱ")
                                        .Replace("ｨ", "ｲ")
                                        .Replace("ｩ", "ｳ")
                                        .Replace("ｪ", "ｴ")
                                        .Replace("ｫ", "ｵ")
                                        .Replace("ｬ", "ﾔ")
                                        .Replace("ｭ", "ﾕ")
                                        .Replace("ｮ", "ﾖ")
                                        .Replace("ｯ", "ﾂ")
                                        : String.Empty;

        var query = _tenantNoTrackingDataContext.ByomeiMsts.Where(item =>
                                (item.KanaName1 != null &&
                                item.KanaName1.ToUpper()
                                .Replace("ｧ", "ｱ")
                                .Replace("ｨ", "ｲ")
                                .Replace("ｩ", "ｳ")
                                .Replace("ｪ", "ｴ")
                                .Replace("ｫ", "ｵ")
                                .Replace("ｬ", "ﾔ")
                                .Replace("ｭ", "ﾕ")
                                .Replace("ｮ", "ﾖ")
                                .Replace("ｯ", "ﾂ")
                                .StartsWith(keywordAfterConvert))
                                ||
                                (item.KanaName2 != null &&
                                 item.KanaName2.ToUpper()
                                 .Replace("ｧ", "ｱ")
                                .Replace("ｨ", "ｲ")
                                .Replace("ｩ", "ｳ")
                                .Replace("ｪ", "ｴ")
                                .Replace("ｫ", "ｵ")
                                .Replace("ｬ", "ﾔ")
                                .Replace("ｭ", "ﾕ")
                                .Replace("ｮ", "ﾖ")
                                .Replace("ｯ", "ﾂ")
                                .StartsWith(keywordAfterConvert))
                                ||
                                (item.KanaName3 != null &&
                                item.KanaName3.ToUpper()
                                .Replace("ｧ", "ｱ")
                                .Replace("ｨ", "ｲ")
                                .Replace("ｩ", "ｳ")
                                .Replace("ｪ", "ｴ")
                                .Replace("ｫ", "ｵ")
                                .Replace("ｬ", "ﾔ")
                                .Replace("ｭ", "ﾕ")
                                .Replace("ｮ", "ﾖ")
                                .Replace("ｯ", "ﾂ")
                                .StartsWith(keywordAfterConvert))
                                ||
                                (item.KanaName4 != null &&
                                item.KanaName4.ToUpper()
                                .Replace("ｧ", "ｱ")
                                .Replace("ｨ", "ｲ")
                                .Replace("ｩ", "ｳ")
                                .Replace("ｪ", "ｴ")
                                .Replace("ｫ", "ｵ")
                                .Replace("ｬ", "ﾔ")
                                .Replace("ｭ", "ﾕ")
                                .Replace("ｮ", "ﾖ")
                                .Replace("ｯ", "ﾂ")
                                .StartsWith(keywordAfterConvert))
                                ||
                                (item.KanaName5 != null &&
                                item.KanaName5.ToUpper()
                                .Replace("ｧ", "ｱ")
                                .Replace("ｨ", "ｲ")
                                .Replace("ｩ", "ｳ")
                                .Replace("ｪ", "ｴ")
                                .Replace("ｫ", "ｵ")
                                .Replace("ｬ", "ﾔ")
                                .Replace("ｭ", "ﾕ")
                                .Replace("ｮ", "ﾖ")
                                .Replace("ｯ", "ﾂ")
                                .StartsWith(keywordAfterConvert))
                                ||
                                (item.KanaName6 != null &&
                                item.KanaName6.ToUpper()
                                .Replace("ｧ", "ｱ")
                                .Replace("ｨ", "ｲ")
                                .Replace("ｩ", "ｳ")
                                .Replace("ｪ", "ｴ")
                                .Replace("ｫ", "ｵ")
                                .Replace("ｬ", "ﾔ")
                                .Replace("ｭ", "ﾕ")
                                .Replace("ｮ", "ﾖ")
                                .Replace("ｯ", "ﾂ")
                                .StartsWith(keywordAfterConvert))
                                ||
                                (item.KanaName7 != null &&
                                item.KanaName7.ToUpper()
                                .Replace("ｧ", "ｱ")
                                .Replace("ｨ", "ｲ")
                                .Replace("ｩ", "ｳ")
                                .Replace("ｪ", "ｴ")
                                .Replace("ｫ", "ｵ")
                                .Replace("ｬ", "ﾔ")
                                .Replace("ｭ", "ﾕ")
                                .Replace("ｮ", "ﾖ")
                                .Replace("ｯ", "ﾂ")
                                .StartsWith(keywordAfterConvert))
                                ||
                                (item.Sbyomei != null &&
                                item.Sbyomei.ToUpper()
                                .Replace("ｧ", "ｱ")
                                .Replace("ｨ", "ｲ")
                                .Replace("ｩ", "ｳ")
                                .Replace("ｪ", "ｴ")
                                .Replace("ｫ", "ｵ")
                                .Replace("ｬ", "ﾔ")
                                .Replace("ｭ", "ﾕ")
                                .Replace("ｮ", "ﾖ")
                                .Replace("ｯ", "ﾂ")
                                .Contains(keywordAfterConvert))
                                ||
                                (item.Icd101 != null &&
                                item.Icd101.StartsWith(keyword))
                                ||
                                (item.Icd1012013 != null &&
                                item.Icd1012013.StartsWith(keyword))
                                ||
                                (item.Icd102 != null &&
                                item.Icd102.StartsWith(keyword))
                                ||
                                (item.Icd1022013 != null &&
                                item.Icd1022013.StartsWith(keyword))
                             );

        query = query.Where(item => (isByomei && item.ByomeiCd.Length != 4)
                                        || (isPrefix && item.ByomeiCd.Length == 4 && !item.ByomeiCd.StartsWith("9"))
                                        || (isSuffix && item.ByomeiCd.Length == 4 && item.ByomeiCd.StartsWith("8"))
                                    );

        var listDatas = query.OrderBy(item => item.KanaName1)
                             .ThenByDescending(item => item.IsAdopted)
                             .OrderByDescending(item => item.ByomeiCd.Length != 4)
                             .ThenByDescending(item => item.ByomeiCd.Length == 4)
                             .Skip((pageIndex - 1) * pageCount).Take(pageCount).ToList();

        List<ByomeiMstModel> listByomeies = new();

        if (listDatas != null)
        {
            listByomeies = listDatas.Select(mst => ConvertToByomeiMstModel(mst)).ToList();
        }
        return listByomeies;
    }

    private ByomeiMstModel ConvertToByomeiMstModel(ByomeiMst mst)
    {
        return new ByomeiMstModel(
                mst.ByomeiCd,
                convertByomeiCdDisplay(mst.ByomeiCd),
                mst.Sbyomei ?? String.Empty,
                mst.KanaName1 ?? String.Empty,
                convertSikkanDisplay(mst.SikkanCd),
                mst.NanbyoCd == NanbyoConst.Gairai ? "難病" : string.Empty,
                convertIcd10Display(mst.Icd101 ?? String.Empty, mst.Icd102 ?? String.Empty),
                convertIcd102013Display(mst.Icd1012013 ?? String.Empty, mst.Icd1022013 ?? String.Empty)
            );
    }

    /// Get the ByomeiCdDisplay depend on ByomeiCd
    private string convertByomeiCdDisplay(string byomeiCd)
    {
        string result = "";

        if (string.IsNullOrEmpty(byomeiCd)) return result;

        if (byomeiCd.Length != 4)
        {
            result = "病名";
        }
        else
        {
            if (byomeiCd.StartsWith("8"))
            {
                result = "接尾語";
            }
            else if (byomeiCd.StartsWith("9"))
            {
                result = "その他";
            }
            else
            {
                result = "接頭語";
            }
        }
        return result;
    }

    /// Get the SikkanCd for display
    private string convertSikkanDisplay(int SikkanCd)
    {
        string sikkanDisplay = "";
        switch (SikkanCd)
        {
            case 0:
                sikkanDisplay = "";
                break;
            case 5:
                sikkanDisplay = "特疾";
                break;
            case 3:
                sikkanDisplay = "皮１";
                break;
            case 4:
                sikkanDisplay = "皮２";
                break;
            case 7:
                sikkanDisplay = "てんかん";
                break;
            case 8:
                sikkanDisplay = "特疾又はてんかん";
                break;
        }
        return sikkanDisplay;
    }

    /// Get the Icd10Display depend on Icd101 and Icd102
    private string convertIcd10Display(string icd101, string icd102)
    {
        string result = icd101;
        if (!string.IsNullOrWhiteSpace(result))
        {
            if (!string.IsNullOrWhiteSpace(icd102))
            {
                result = result + "/" + icd102;
            }
        }
        else
        {
            result = icd102;
        }
        return result;
    }

    /// Get the Icd10Display depend on Icd1012013 and Icd1022013
    private string convertIcd102013Display(string icd1012013, string icd1022013)
    {
        string rs = icd1012013;
        if (!string.IsNullOrWhiteSpace(rs))
        {
            if (!string.IsNullOrWhiteSpace(icd1022013))
            {
                rs = rs + "/" + icd1022013;
            }
        }
        else
        {
            rs = icd1022013;
        }
        return rs;
    }
}
