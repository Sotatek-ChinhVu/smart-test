using Domain.Models.MstItem;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class MstItemRepository : IMstItemRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        private readonly TenantDataContext _tenantDataContextTracking;
        public MstItemRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<DosageDrugModel> GetDosages(List<string> yjCds)
        {
            var result = _tenantDataContext.DosageDrugs.Where(d => yjCds.Contains(d.YjCd));
            return result == null ? new List<DosageDrugModel>() : result.Select(
                    r => new DosageDrugModel(
                            r.YjCd,
                            r.DoeiCd,
                            r.DgurKbn,
                            r.KikakiUnit,
                            r.YakkaiUnit,
                            r.RikikaRate,
                            r.RikikaUnit,
                            r.YoukaiekiCd
                   )).ToList();
        }

        public (List<OtcItemModel>, int) SearchOTCModels(string searchValue, int pageIndex, int pageSize)
        {
            searchValue = searchValue.Trim();
            var OtcFormCodes = _tenantDataContext.M38OtcFormCode.AsQueryable();
            var OtcMakerCodes = _tenantDataContext.M38OtcMakerCode.AsQueryable();
            var OtcMains = _tenantDataContext.M38OtcMain.AsQueryable();
            var UsageCodes = _tenantDataContext.M56UsageCode.AsQueryable();
            var OtcClassCodes = _tenantDataContext.M38ClassCode.AsQueryable();
            var query = from main in OtcMains
                        join classcode in OtcClassCodes on main.ClassCd equals classcode.ClassCd into classLeft
                        from clas in classLeft.DefaultIfEmpty()
                        join makercode in OtcMakerCodes on main.CompanyCd equals makercode.MakerCd into makerLeft
                        from maker in makerLeft.DefaultIfEmpty()
                        join formcode in OtcFormCodes on main.DrugFormCd equals formcode.FormCd into formLeft
                        from form in formLeft.DefaultIfEmpty()
                        join usagecode in UsageCodes on main.YohoCd equals usagecode.YohoCd into usageLeft
                        from usage in usageLeft.DefaultIfEmpty()
                        where (main.TradeKana.Contains(searchValue)
                                || main.TradeName.Contains(searchValue)
                                || maker.MakerKana.Contains(searchValue)
                                || maker.MakerName.Contains(searchValue))
                        select new OtcItemModel(
                            main.SerialNum,
                            main.OtcCd,
                            main.TradeName,
                            main.TradeKana,
                            main.ClassCd,
                            main.CompanyCd,
                            main.TradeCd,
                            main.DrugFormCd,
                            main.YohoCd,
                            form.Form,
                            maker.MakerName,
                            maker.MakerKana,
                            usage.Yoho,
                            clas.ClassName,
                            clas.MajorDivCd
                        );
            var total = query.Count();
            var models = query.OrderBy(u => u.TradeKana).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return (models, total);
        }

        public List<FoodAlrgyKbnModel> GetFoodAlrgyMasterData()
        {
            List<FoodAlrgyKbnModel> m12FoodAlrgies = new List<FoodAlrgyKbnModel>();
            int i = 0;
            var aleFoodKbns = _tenantDataContext.M12FoodAlrgyKbn.Select(x => new FoodAlrgyKbnModel(
                 x.FoodKbn,
                 x.FoodName,
                 int.TryParse(x.FoodKbn, out i) && int.Parse(x.FoodKbn) > 50
            )).OrderBy(x => x.FoodKbn).ToList();
            return aleFoodKbns;
        }

        public (List<SearchSupplementModel>, int) GetListSupplement(string searchValue, int pageIndex, int pageSize)
        {
            searchValue = searchValue.Trim();
            try
            {
                var listSuppleIndexCode = _tenantDataContext.M41SuppleIndexcodes.AsQueryable();
                var listSuppleIndexDef = _tenantDataContext.M41SuppleIndexdefs.AsQueryable();
                var listSuppleIngre = _tenantDataContext.M41SuppleIngres.AsQueryable();

                var query = (from indexDef in listSuppleIndexDef
                             orderby indexDef.IndexWord
                             join indexCode in listSuppleIndexCode on indexDef.SeibunCd equals indexCode.IndexCd into supplementList
                             from supplement in supplementList.DefaultIfEmpty()
                             join ingre in listSuppleIngre on supplement.SeibunCd equals ingre.SeibunCd into suppleIngreList
                             from ingreItem in suppleIngreList.DefaultIfEmpty()
                             where indexDef.IndexWord.Contains(searchValue)
                             select new SearchSupplementModel(
                                 ingreItem.SeibunCd,
                                 ingreItem.Seibun,
                                 indexDef.IndexWord,
                                 indexDef.TokuhoFlg,
                                 supplement.IndexCd
                             )).AsQueryable();

                var total = query.Count();
                var result = query.Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

                if (result == null || !result.Any())
                {
                    return (new List<SearchSupplementModel>(), 0);
                }

                return (result, total);
            }
            catch (Exception)
            {
                return (new List<SearchSupplementModel>(), 0);
            }
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

            var query = _tenantDataContext.ByomeiMsts.Where(item =>
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

        #region Private Function
        private ByomeiMstModel ConvertToByomeiMstModel(ByomeiMst mst)
        {
            return new ByomeiMstModel(
                    mst.ByomeiCd,
                    ConvertByomeiCdDisplay(mst.ByomeiCd),
                    mst.Sbyomei ?? String.Empty,
                    mst.KanaName1 ?? String.Empty,
                    ConvertSikkanDisplay(mst.SikkanCd),
                    mst.NanbyoCd == NanbyoConst.Gairai ? "難病" : string.Empty,
                    ConvertIcd10Display(mst.Icd101 ?? String.Empty, mst.Icd102 ?? String.Empty),
                    ConvertIcd102013Display(mst.Icd1012013 ?? String.Empty, mst.Icd1022013 ?? String.Empty)
                );
        }

        /// Get the ByomeiCdDisplay depend on ByomeiCd
        private string ConvertByomeiCdDisplay(string byomeiCd)
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
        private string ConvertSikkanDisplay(int SikkanCd)
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
        private string ConvertIcd10Display(string icd101, string icd102)
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
        private string ConvertIcd102013Display(string icd1012013, string icd1022013)
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
        #endregion
    }
}
