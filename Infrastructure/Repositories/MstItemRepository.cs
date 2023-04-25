using Amazon.Runtime.Internal.Transform;
using Domain.Constant;
using Domain.Enum;
using Domain.Models.FlowSheet;
using Domain.Models.MstItem;
using Domain.Models.OrdInf;
using Domain.Models.TodayOdr;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Enum;
using Helper.Extension;
using Helper.Mapping;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class MstItemRepository : RepositoryBase, IMstItemRepository
    {
        public MstItemRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        private readonly List<int> _HoukatuTermExclude = new List<int> { 0, 5, 6 };

        public List<DosageDrugModel> GetDosages(List<string> yjCds)
        {
            var listDosageDrugs = NoTrackingDataContext.DosageDrugs.Where(d => yjCds.Contains(d.YjCd)).ToList();
            var listDoeiCd = listDosageDrugs.Select(item => item.DoeiCd).ToList();
            var listDosageDosages = NoTrackingDataContext.DosageDosages.Where(item => listDoeiCd.Contains(item.DoeiCd)).ToList();
            return listDosageDrugs == null ? new List<DosageDrugModel>() : listDosageDrugs.Select(
                    r => new DosageDrugModel(
                            r.YjCd,
                            r.DoeiCd,
                            r.DgurKbn ?? string.Empty,
                            r.KikakiUnit ?? string.Empty,
                            r.YakkaiUnit ?? string.Empty,
                            r.RikikaRate,
                            r.RikikaUnit ?? string.Empty,
                            r.YoukaiekiCd ?? string.Empty,
                            listDosageDosages.FirstOrDefault(item => item.DoeiCd == r.DoeiCd)?.UsageDosage?.Replace("；", Environment.NewLine) ?? string.Empty
                   )).ToList();
        }

        public (List<OtcItemModel>, int) SearchOTCModels(string searchValue, int pageIndex, int pageSize)
        {
            searchValue = searchValue.Trim();
            var OtcFormCodes = NoTrackingDataContext.M38OtcFormCode.AsQueryable();
            var OtcMakerCodes = NoTrackingDataContext.M38OtcMakerCode.AsQueryable();
            var OtcMains = NoTrackingDataContext.M38OtcMain.AsQueryable();
            var UsageCodes = NoTrackingDataContext.M56UsageCode.AsQueryable();
            var OtcClassCodes = NoTrackingDataContext.M38ClassCode.AsQueryable();
            var checkSerialNum = int.TryParse(searchValue, out int serialNum);
            var query = from main in OtcMains.AsEnumerable()
                        join classcode in OtcClassCodes on main.ClassCd equals classcode.ClassCd into classLeft
                        from clas in classLeft.DefaultIfEmpty()
                        join makercode in OtcMakerCodes on main.CompanyCd equals makercode.MakerCd into makerLeft
                        from maker in makerLeft.DefaultIfEmpty()
                        join formcode in OtcFormCodes on main.DrugFormCd equals formcode.FormCd into formLeft
                        from form in formLeft.DefaultIfEmpty()
                        join usagecode in UsageCodes on main.YohoCd equals usagecode.YohoCd into usageLeft
                        from usage in usageLeft.DefaultIfEmpty()
                        where ((main.TradeKana ?? string.Empty).Contains(searchValue)
                                || (main.TradeName ?? string.Empty).Contains(searchValue)
                                || (maker.MakerKana ?? string.Empty).Contains(searchValue)
                                || (maker.MakerName ?? string.Empty).Contains(searchValue)
                                || (checkSerialNum && main.SerialNum == serialNum)
                                )
                        select new OtcItemModel(
                            main.SerialNum,
                            main.OtcCd ?? string.Empty,
                            main.TradeName ?? string.Empty,
                            main.TradeKana ?? string.Empty,
                            main.ClassCd ?? string.Empty,
                            main.CompanyCd ?? string.Empty,
                            main.TradeCd ?? string.Empty,
                            main.DrugFormCd ?? string.Empty,
                            main.YohoCd ?? string.Empty,
                            form?.Form ?? string.Empty,
                            maker?.MakerName ?? string.Empty,
                            maker?.MakerKana ?? string.Empty,
                            usage?.Yoho ?? string.Empty,
                            clas?.ClassName ?? string.Empty,
                            clas?.MajorDivCd ?? string.Empty
                        );
            var total = query.Count();
            var models = query.OrderBy(u => u.TradeKana).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return (models, total);
        }

        public List<FoodAlrgyKbnModel> GetFoodAlrgyMasterData()
        {
            var aleFoodKbns = NoTrackingDataContext.M12FoodAlrgyKbn.AsEnumerable()
                .OrderBy(x => x.FoodKbn)
                .Select(x => new FoodAlrgyKbnModel(
                 x.FoodKbn,
                 x.FoodName ?? string.Empty
            )).ToList();
            return aleFoodKbns;
        }

        public (List<SearchSupplementModel>, int) GetListSupplement(string searchValue, int pageIndex, int pageSize)
        {
            searchValue = searchValue.Trim();
            try
            {
                var listSuppleIndexCode = NoTrackingDataContext.M41SuppleIndexcodes.AsQueryable();
                var listSuppleIndexDef = NoTrackingDataContext.M41SuppleIndexdefs.AsQueryable();
                var listSuppleIngre = NoTrackingDataContext.M41SuppleIngres.AsQueryable();

                var query = (from indexDef in listSuppleIndexDef
                             orderby indexDef.IndexWord
                             join indexCode in listSuppleIndexCode on indexDef.SeibunCd equals indexCode.IndexCd into supplementList
                             from supplement in supplementList.DefaultIfEmpty()
                             join ingre in listSuppleIngre on supplement.SeibunCd equals ingre.SeibunCd into suppleIngreList
                             from ingreItem in suppleIngreList.DefaultIfEmpty()
                             where ((indexDef.IndexWord ?? string.Empty).Contains(searchValue) || supplement.IndexCd.StartsWith(searchValue))
                             select new SearchSupplementModel(
                                 ingreItem.SeibunCd,
                                 ingreItem.Seibun ?? string.Empty,
                                 indexDef.IndexWord ?? string.Empty,
                                 indexDef.TokuhoFlg ?? string.Empty,
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

        public TenItemModel GetTenMst(int hpId, int sinDate, string itemCd)
        {
            var tenMst = NoTrackingDataContext.TenMsts.FirstOrDefault(t => t.HpId == hpId && t.ItemCd == itemCd && t.StartDate <= sinDate && t.EndDate >= sinDate);

            return new TenItemModel(
                tenMst?.HpId ?? 0,
                tenMst?.ItemCd ?? string.Empty,
                tenMst?.RousaiKbn ?? 0,
                tenMst?.KanaName1 ?? string.Empty,
                tenMst?.Name ?? string.Empty,
                tenMst?.KohatuKbn ?? 0,
                tenMst?.MadokuKbn ?? 0,
                tenMst?.KouseisinKbn ?? 0,
                tenMst?.OdrUnitName ?? string.Empty,
                tenMst?.EndDate ?? 0,
                tenMst?.DrugKbn ?? 0,
                tenMst?.MasterSbt ?? string.Empty,
                tenMst?.BuiKbn ?? 0,
                tenMst?.IsAdopted ?? 0,
                tenMst?.Ten ?? 0,
                tenMst?.TenId ?? 0,
                "",
                "",
                tenMst?.CmtCol1 ?? 0,
                tenMst?.IpnNameCd ?? string.Empty,
                tenMst?.SinKouiKbn ?? 0,
                tenMst?.YjCd ?? string.Empty,
                tenMst?.CnvUnitName ?? string.Empty,
                tenMst?.StartDate ?? 0,
                tenMst?.YohoKbn ?? 0,
                tenMst?.CmtColKeta1 ?? 0,
                tenMst?.CmtColKeta2 ?? 0,
                tenMst?.CmtColKeta3 ?? 0,
                tenMst?.CmtColKeta4 ?? 0,
                tenMst?.CmtCol2 ?? 0,
                tenMst?.CmtCol3 ?? 0,
                tenMst?.CmtCol4 ?? 0,
                tenMst?.IpnNameCd ?? string.Empty,
                tenMst?.MinAge ?? string.Empty,
                tenMst?.MaxAge ?? string.Empty,
                tenMst?.SanteiItemCd ?? string.Empty,
                tenMst?.OdrTermVal ?? 0,
                tenMst?.CnvTermVal ?? 0,
                tenMst?.DefaultVal ?? 0,
                tenMst?.Kokuji1 ?? string.Empty,
                tenMst?.Kokuji2 ?? string.Empty,
                string.Empty
            );
        }

        public List<TenItemModel> GetCheckTenItemModels(int hpId, int sinDate, List<string> itemCds)
        {
            var tenMsts = NoTrackingDataContext.TenMsts.Where(t => t.HpId == hpId && itemCds.Contains(t.ItemCd) && t.StartDate <= sinDate && t.EndDate >= sinDate);

            return tenMsts.Select(tenMst => new TenItemModel(
                tenMst.HpId,
                tenMst.ItemCd,
                tenMst.RousaiKbn,
                tenMst.KanaName1 ?? string.Empty,
                tenMst.Name ?? string.Empty,
                tenMst.KohatuKbn,
                tenMst.MadokuKbn,
                tenMst.KouseisinKbn,
                tenMst.OdrUnitName ?? string.Empty,
                tenMst.EndDate,
                tenMst.DrugKbn,
                tenMst.MasterSbt ?? string.Empty,
                tenMst.BuiKbn,
                tenMst.IsAdopted,
                tenMst.Ten,
                tenMst.TenId,
                "",
                "",
                tenMst.CmtCol1,
                tenMst.IpnNameCd ?? string.Empty,
                tenMst.SinKouiKbn,
                tenMst.YjCd ?? string.Empty,
                tenMst.CnvUnitName ?? string.Empty,
                tenMst.StartDate,
                tenMst.YohoKbn,
                tenMst.CmtColKeta1,
                tenMst.CmtColKeta2,
                tenMst.CmtColKeta3,
                tenMst.CmtColKeta4,
                tenMst.CmtCol2,
                tenMst.CmtCol3,
                tenMst.CmtCol4,
                tenMst.IpnNameCd ?? string.Empty,
                tenMst.MinAge ?? string.Empty,
                tenMst.MaxAge ?? string.Empty,
                tenMst.SanteiItemCd ?? string.Empty,
                tenMst.OdrTermVal,
                tenMst.CnvTermVal,
                tenMst.DefaultVal,
                tenMst.Kokuji1 ?? string.Empty,
                tenMst.Kokuji2 ?? string.Empty,
                string.Empty
            )).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="kouiKbn"></param>
        /// <param name="sinDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="genericOrSameItem"></param>
        /// <param name="yjCd"></param>
        /// <param name="hpId"></param>
        /// <param name="pointFrom"></param>
        /// <param name="pointTo"></param>
        /// <param name="isRosai"></param>
        /// <param name="isMirai"></param>
        /// <param name="isExpired"></param>
        /// <param name="itemCodeStartWith"></param>
        /// <param name="isMasterSearch"></param>
        /// <param name="isSearch831SuffixOnly"></param>
        /// <param name="isSearchSanteiItem"></param>
        /// <param name="searchFollowUsage"></param> (0: all, 1: search no usage, 2: search usage) 
        /// <returns></returns>
        public (List<TenItemModel> tenItemModels, int totalCount) SearchTenMst(string keyword, int kouiKbn, int sinDate, int pageIndex, int pageCount, int genericOrSameItem, string yjCd, int hpId, double pointFrom, double pointTo, bool isRosai, bool isMirai, bool isExpired, string itemCodeStartWith, bool isMasterSearch, bool isSearch831SuffixOnly, bool isSearchSanteiItem, byte searchFollowUsage, List<int> kouiKbns, string masterSBT)
        {
            string kanaKeyword = keyword;
            if (!WanaKana.IsKana(keyword) && WanaKana.IsRomaji(keyword))
            {
                var inputKeyword = keyword;
                kanaKeyword = CIUtil.ToHalfsize(keyword);
                if (WanaKana.IsRomaji(kanaKeyword)) //If after convert to kana. type still is IsRomaji, back to base input keyword
                    kanaKeyword = inputKeyword;
            }

            var listTenMstModels = new List<TenItemModel>();
            string sBigKeyword = kanaKeyword.ToUpper()
                                        .Replace("ｧ", "ｱ")
                                        .Replace("ｨ", "ｲ")
                                        .Replace("ｩ", "ｳ")
                                        .Replace("ｪ", "ｴ")
                                        .Replace("ｫ", "ｵ")
                                        .Replace("ｬ", "ﾔ")
                                        .Replace("ｭ", "ﾕ")
                                        .Replace("ｮ", "ﾖ")
                                        .Replace("ｯ", "ﾂ");

            var queryResult = NoTrackingDataContext.TenMsts.Where(t =>
                                t.ItemCd.StartsWith(keyword)
                                || (t.SanteiItemCd != null && t.SanteiItemCd.StartsWith(keyword))
                                || (!String.IsNullOrEmpty(t.KanaName1) && t.KanaName1.ToUpper()
                                  .Replace("ｧ", "ｱ")
                                  .Replace("ｨ", "ｲ")
                                  .Replace("ｩ", "ｳ")
                                  .Replace("ｪ", "ｴ")
                                  .Replace("ｫ", "ｵ")
                                  .Replace("ｬ", "ﾔ")
                                  .Replace("ｭ", "ﾕ")
                                  .Replace("ｮ", "ﾖ")
                                  .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword))
                                ||
                                  (!String.IsNullOrEmpty(t.KanaName2) && t.KanaName2.ToUpper()
                                  .Replace("ｧ", "ｱ")
                                  .Replace("ｨ", "ｲ")
                                  .Replace("ｩ", "ｳ")
                                  .Replace("ｪ", "ｴ")
                                  .Replace("ｫ", "ｵ")
                                  .Replace("ｬ", "ﾔ")
                                  .Replace("ｭ", "ﾕ")
                                  .Replace("ｮ", "ﾖ")
                                  .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword))

                                || (
                                  !String.IsNullOrEmpty(t.KanaName3) && t.KanaName3.ToUpper()
                                  .Replace("ｧ", "ｱ")
                                  .Replace("ｨ", "ｲ")
                                  .Replace("ｩ", "ｳ")
                                  .Replace("ｪ", "ｴ")
                                  .Replace("ｫ", "ｵ")
                                  .Replace("ｬ", "ﾔ")
                                  .Replace("ｭ", "ﾕ")
                                  .Replace("ｮ", "ﾖ")
                                  .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword))
                                || (
                                  !String.IsNullOrEmpty(t.KanaName4) && t.KanaName4.ToUpper()
                                  .Replace("ｧ", "ｱ")
                                  .Replace("ｨ", "ｲ")
                                  .Replace("ｩ", "ｳ")
                                  .Replace("ｪ", "ｴ")
                                  .Replace("ｫ", "ｵ")
                                  .Replace("ｬ", "ﾔ")
                                  .Replace("ｭ", "ﾕ")
                                  .Replace("ｮ", "ﾖ")
                                  .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword))
                                ||
                                (!String.IsNullOrEmpty(t.KanaName5) && t.KanaName5.ToUpper()
                                  .Replace("ｧ", "ｱ")
                                  .Replace("ｨ", "ｲ")
                                  .Replace("ｩ", "ｳ")
                                  .Replace("ｪ", "ｴ")
                                  .Replace("ｫ", "ｵ")
                                  .Replace("ｬ", "ﾔ")
                                  .Replace("ｭ", "ﾕ")
                                  .Replace("ｮ", "ﾖ")
                                  .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword))
                                ||
                                (!String.IsNullOrEmpty(t.KanaName6) && t.KanaName6.ToUpper()
                                  .Replace("ｧ", "ｱ")
                                  .Replace("ｨ", "ｲ")
                                  .Replace("ｩ", "ｳ")
                                  .Replace("ｪ", "ｴ")
                                  .Replace("ｫ", "ｵ")
                                  .Replace("ｬ", "ﾔ")
                                  .Replace("ｭ", "ﾕ")
                                  .Replace("ｮ", "ﾖ")
                                  .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword))
                                || (
                                  !String.IsNullOrEmpty(t.KanaName7) && t.KanaName7.ToUpper()
                                  .Replace("ｧ", "ｱ")
                                  .Replace("ｨ", "ｲ")
                                  .Replace("ｩ", "ｳ")
                                  .Replace("ｪ", "ｴ")
                                  .Replace("ｫ", "ｵ")
                                  .Replace("ｬ", "ﾔ")
                                  .Replace("ｭ", "ﾕ")
                                  .Replace("ｮ", "ﾖ")
                                  .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword))
                                ||
                                (!String.IsNullOrEmpty(t.Name) && t.Name.Contains(keyword)));

            var count = queryResult.Count();

            if (!string.IsNullOrEmpty(masterSBT))
            {
                queryResult = queryResult.Where(t => t.MasterSbt == masterSBT);
            }

            if (kouiKbns.Count == 0)
            {
                if (kouiKbn > 0)
                {
                    //2019-12-04 @duong.vu said: this is a self injection -> search items relate to injection only
                    var SELF_INJECTION_KOUIKBN = 28;
                    if (kouiKbn == SELF_INJECTION_KOUIKBN)
                    {
                        kouiKbn = 30;
                    }

                    switch (kouiKbn)
                    {
                        case 11:
                            queryResult = queryResult.Where(t => new[] { 11, 99 }.Contains(t.SinKouiKbn) || new[] { 1, 3, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T");
                            break;
                        case 12:
                            queryResult = queryResult.Where(t => new[] { 12, 99 }.Contains(t.SinKouiKbn) || new[] { 1, 3, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T");
                            break;
                        case 13:
                            queryResult = queryResult.Where(t => new[] { 13, 99 }.Contains(t.SinKouiKbn) || new[] { 1, 3, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T");
                            break;
                        case 14:
                            queryResult = queryResult.Where(t => new[] { 14, 99 }.Contains(t.SinKouiKbn) || new[] { 1, 3, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T");
                            break;
                        case 21:
                        case 22:
                        case 23:
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.YohoKbn > 0 || new[] { 1, 3, 6 }.Contains(t.DrugKbn) || t.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu);
                            break;
                        case 20:
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || (t.SinKouiKbn >= 20 && t.SinKouiKbn <= 29) || t.DrugKbn == 3 || t.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu);
                            break;
                        case 24:
                        case 25:
                        case 26:
                        case 27:
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || (t.SinKouiKbn >= 20 && t.SinKouiKbn <= 29) || t.DrugKbn == 3);
                            break;
                        case 28:
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T");
                            break;
                        case 30:
                        case 31:
                        case 32:
                        case 33:
                        case 34:
                        case 35:
                        case 36:
                        case 37:
                        case 38:
                        case 39:
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || (t.SinKouiKbn >= 30 && t.SinKouiKbn <= 39) || t.MasterSbt == "T" || new[] { 4, 6 }.Contains(t.DrugKbn));
                            break;
                        case 40:
                        case 41:
                        case 42:
                        case 43:
                        case 44:
                        case 45:
                        case 46:
                        case 47:
                        case 48:
                        case 49:
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 40 && t.SinKouiKbn <= 49) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T");
                            break;
                        case 50:
                        case 51:
                        case 52:
                        case 53:
                        case 54:
                        case 55:
                        case 56:
                        case 57:
                        case 58:
                        case 59:
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 50 && t.SinKouiKbn <= 59) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T");
                            break;
                        case 60:
                        case 61:
                        case 62:
                        case 63:
                        case 64:
                        case 65:
                        case 66:
                        case 67:
                        case 68:
                        case 69:
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 60 && t.SinKouiKbn <= 69) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T");
                            break;
                        case 70:
                        case 71:
                        case 72:
                        case 73:
                        case 74:
                        case 75:
                        case 76:
                        case 77:
                        case 78:
                        case 79:
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 70 && t.SinKouiKbn <= 79) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T");
                            break;
                        case 80:
                        case 81:
                        case 82:
                        case 83:
                        case 84:
                        case 85:
                        case 86:
                        case 87:
                        case 88:
                        case 89:
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 80 && t.SinKouiKbn <= 89) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T");
                            break;
                        case 95:
                        case 96:
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 95 && t.SinKouiKbn <= 96));
                            break;
                        case 100:
                        case 101:
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99);
                            break;
                    }

                    if (kouiKbn >= 20 && kouiKbn <= 27 || kouiKbn >= 30 && kouiKbn <= 39)
                    {
                        queryResult = queryResult.Where(t => !(new[] {
                        ItemCdConst.TouyakuTokuSyo1Syoho,
                        ItemCdConst.TouyakuTokuSyo2Syoho,
                        ItemCdConst.TouyakuTokuSyo1Syohosen,
                        ItemCdConst.TouyakuTokuSyo2Syohosen,
                        ItemCdConst.ZanGigi,
                        ItemCdConst.ZanTeiKyo}.Contains(t.ItemCd)));
                    }
                }
            }
            else
            {
                queryResult = queryResult.Where(t => kouiKbns.Distinct().Contains(t.SinKouiKbn));
            }

            if (sinDate > 0)
            {
                queryResult = queryResult.Where(t => t.StartDate <= sinDate && t.EndDate >= sinDate);
            }
            else
            {
                var newQuery = queryResult.ToList();
                if (newQuery != null)
                {
                    queryResult = queryResult.GroupBy(item => item.ItemCd, (key, group) => group.OrderByDescending(item => item.EndDate).FirstOrDefault() ?? new TenMst());
                }
            }

            string YJCode = "";
            if (genericOrSameItem == 1)
            {
                if (yjCd.Length >= 9)
                {
                    YJCode = CIUtil.Copy(yjCd, 1, 9);
                }
                else
                {
                    YJCode = yjCd;
                }
            }
            else if (genericOrSameItem == 2)
            {
                if (yjCd.Length >= 4)
                {
                    YJCode = CIUtil.Copy(yjCd, 1, 4);
                }
                else
                {
                    YJCode = yjCd;
                }
            }

            if (!string.IsNullOrEmpty(itemCodeStartWith))
            {
                queryResult = queryResult.Where(t => t.ItemCd.StartsWith(itemCodeStartWith));
            }

            if (!string.IsNullOrEmpty(YJCode))
            {
                queryResult = queryResult.Where(t => !String.IsNullOrEmpty(t.YjCd) && t.YjCd.StartsWith(YJCode));
            }

            if (!isMasterSearch && !isSearch831SuffixOnly)
            {
                if (isSearchSanteiItem)
                {
                    queryResult = queryResult.Where(t => t.IsNosearch == 0 ||
                                                        (t.ItemCd.StartsWith("16") &&
                                                        t.SinKouiKbn >= 60 && t.SinKouiKbn <= 69 &&
                                                        t.IsNosearch == 1));
                }
                else
                {
                    queryResult = queryResult.Where(t => t.IsNosearch == 0);
                }
            }

            if (isSearch831SuffixOnly)
            {
                queryResult = queryResult.Where(t => t.ItemCd.Length == 9 && !t.ItemCd.StartsWith("8") && (t.MasterSbt == "S" || t.MasterSbt == "R"));
            }

            if (pointFrom > 0)
            {
                queryResult = queryResult.Where(t => t.Ten >= pointFrom);
            }

            if (pointTo > 0)
            {
                queryResult = queryResult.Where(t => t.Ten <= pointTo);
            }

            if (searchFollowUsage == 1)
            {
                queryResult = queryResult.Where(t => t.YohoKbn == 0);
            }

            if (searchFollowUsage == 2)
            {
                queryResult = queryResult.Where(t => t.YohoKbn != 0);
            }

            //!searchItemCondition.IncludeRosai
            if (!isRosai)
            {
                queryResult = queryResult.Where(t => t.RousaiKbn != 1);
            }

            //!searchItemCondition.IncludeMisai
            if (!isMirai)
            {
                queryResult = queryResult.Where(t => t.IsAdopted == 1);
            }

            var tenKnList = queryResult.ToList();
            var santeiItemCdList = tenKnList.Where(t => t.ItemCd.StartsWith("KN")).Select(t => t.SanteiItemCd).ToList();

            // Query 点数 for KN% item
            var tenMstList = NoTrackingDataContext.TenMsts
                .Where(item => item.HpId == hpId
                               && item.StartDate <= sinDate
                               && item.EndDate >= sinDate
                               && santeiItemCdList.Contains(item.ItemCd))
                .ToList();

            var knTensuList = (from tenKN in tenKnList
                               join ten in tenMstList on new { tenKN.SanteiItemCd } equals new { SanteiItemCd = ten.ItemCd }
                               select new { tenKN.ItemCd, ten.Ten }).ToList();

            var queryFinal = (from ten in tenKnList
                              join tenKN in knTensuList
                              on ten.ItemCd equals tenKN.ItemCd into tenKNLeft
                              from tenKN in tenKNLeft.DefaultIfEmpty()
                              select new { TenMst = ten, tenKN }).ToList();

            var kensaItemCdList = queryFinal.Select(q => q.TenMst.KensaItemCd).ToList();
            var kensaMstList = NoTrackingDataContext.KensaMsts.Where(k => kensaItemCdList.Contains(k.KensaItemCd)).ToList();

            var ipnCdList = queryFinal.Select(q => q.TenMst.IpnNameCd).ToList();
            var ipnNameMstList = NoTrackingDataContext.IpnNameMsts.Where(i => ipnCdList.Contains(i.IpnNameCd)).ToList();

            var queryJoinWithKensaIpnName = from q in queryFinal
                                            join k in kensaMstList
                                            on q.TenMst.KensaItemCd equals k.KensaItemCd into kensaMsts
                                            from kensaMst in kensaMsts.DefaultIfEmpty()
                                            join i in ipnNameMstList
                                            on q.TenMst.IpnNameCd equals i.IpnNameCd into ipnNameMsts
                                            from ipnNameMst in ipnNameMsts.DefaultIfEmpty()
                                            select new { q.TenMst, q.tenKN, KensaMst = kensaMst, IpnName = ipnNameMst?.IpnName ?? string.Empty };
            var totalCount = queryJoinWithKensaIpnName.Count(item => item.TenMst != null);

            var listTenMst = queryJoinWithKensaIpnName.Where(item => item.TenMst != null).OrderBy(item => item.TenMst.KanaName1).ThenBy(item => item.TenMst.Name).Skip((pageIndex - 1) * pageCount);
            if (pageCount > 0)
            {
                listTenMst = listTenMst.Take(pageCount);
            }

            if (listTenMst != null && listTenMst.Any())
            {

                listTenMstModels = listTenMst.Select(item => new TenItemModel(
                                                           item.TenMst.HpId,
                                                           item.TenMst.ItemCd ?? string.Empty,
                                                           item.TenMst.RousaiKbn,
                                                           item.TenMst.KanaName1 ?? string.Empty,
                                                           item.TenMst?.Name ?? string.Empty,
                                                           item.TenMst?.KohatuKbn ?? 0,
                                                           item.TenMst?.MadokuKbn ?? 0,
                                                           item.TenMst?.KouseisinKbn ?? 0,
                                                           item.TenMst?.OdrUnitName ?? string.Empty,
                                                           item.TenMst?.EndDate ?? 0,
                                                           item.TenMst?.DrugKbn ?? 0,
                                                           item.TenMst?.MasterSbt ?? string.Empty,
                                                           item.TenMst?.BuiKbn ?? 0,
                                                           item.TenMst?.IsAdopted ?? 0,
                                                           item.tenKN != null ? item.tenKN.Ten : (item.TenMst?.Ten ?? 0),
                                                           item.TenMst?.TenId ?? 0,
                                                           item.KensaMst != null ? (item.KensaMst.CenterItemCd1 ?? string.Empty) : string.Empty,
                                                           item.KensaMst != null ? (item.KensaMst.CenterItemCd2 ?? string.Empty) : string.Empty,
                                                           item.TenMst?.CmtCol1 ?? 0,
                                                           item.TenMst?.IpnNameCd ?? string.Empty,
                                                           item.TenMst?.SinKouiKbn ?? 0,
                                                           item.TenMst?.YjCd ?? string.Empty,
                                                           item.TenMst?.CnvUnitName ?? string.Empty,
                                                           item.TenMst?.StartDate ?? 0,
                                                           item.TenMst?.YohoKbn ?? 0,
                                                           item.TenMst?.CmtColKeta1 ?? 0,
                                                           item.TenMst?.CmtColKeta2 ?? 0,
                                                           item.TenMst?.CmtColKeta3 ?? 0,
                                                           item.TenMst?.CmtColKeta4 ?? 0,
                                                           item.TenMst?.CmtCol2 ?? 0,
                                                           item.TenMst?.CmtCol3 ?? 0,
                                                           item.TenMst?.CmtCol4 ?? 0,
                                                           item.TenMst?.IpnNameCd ?? string.Empty,
                                                           item.TenMst?.MinAge ?? string.Empty,
                                                           item.TenMst?.MaxAge ?? string.Empty,
                                                           item.TenMst?.SanteiItemCd ?? string.Empty,
                                                           item.TenMst?.OdrTermVal ?? 0,
                                                           item.TenMst?.CnvTermVal ?? 0,
                                                           item.TenMst?.DefaultVal ?? 0,
                                                           item.TenMst?.Kokuji1 ?? string.Empty,
                                                           item.TenMst?.Kokuji2 ?? string.Empty,
                                                           item.IpnName
                                                            )).ToList();
            }
            return (listTenMstModels, totalCount);
        }

        public bool UpdateAdoptedItemAndItemConfig(int valueAdopted, string itemCdInputItem, int startDateInputItem, int hpId, int userId)
        {
            // Update IsAdopted Item TenMst
            var tenMst = TrackingDataContext.TenMsts.FirstOrDefault(t => t.HpId == hpId && t.ItemCd == itemCdInputItem && t.StartDate == startDateInputItem);

            if (tenMst == null) return false;

            if (tenMst.IsAdopted == valueAdopted) return false;

            tenMst.IsAdopted = valueAdopted;

            tenMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
            tenMst.UpdateId = userId;

            TrackingDataContext.SaveChanges();

            return true;
        }

        public bool UpdateAdoptedItems(int valueAdopted, List<string> itemCds, int sinDate, int hpId, int userId)
        {
            // Update IsAdopted Item TenMst
            var tenMsts = TrackingDataContext.TenMsts.Where(t => t.HpId == hpId && itemCds.Contains(t.ItemCd) && t.StartDate <= sinDate && t.EndDate >= sinDate && t.IsDeleted == DeleteTypes.None).ToList();

            if (tenMsts.Count == 0) return false;

            for (int i = 0; i < tenMsts.Count; i++)
            {
                var tenMst = tenMsts[i];
                if (tenMst.IsAdopted == valueAdopted) return false;

                tenMst.IsAdopted = valueAdopted;

                tenMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                tenMst.UpdateId = userId;
            }

            return TrackingDataContext.SaveChanges() > 0;
        }

        public List<TenItemModel> GetAdoptedItems(List<string> itemCds, int sinDate, int hpId)
        {
            // Update IsAdopted Item TenMst
            var tenMsts = NoTrackingDataContext.TenMsts.Where(t => t.HpId == hpId && itemCds.Contains(t.ItemCd) && t.StartDate <= sinDate && t.EndDate >= sinDate);
            var tenMstModels = new List<TenItemModel>();
            if (tenMsts != null && tenMsts.Any())
            {
                tenMstModels = tenMsts.Select(item => new TenItemModel(
                                                           item.HpId,
                                                           item.ItemCd ?? string.Empty,
                                                           item.RousaiKbn,
                                                           item.KanaName1 ?? string.Empty,
                                                           item.Name ?? string.Empty,
                                                           item.KohatuKbn,
                                                           item.MadokuKbn,
                                                           item.KouseisinKbn,
                                                           item.OdrUnitName ?? string.Empty,
                                                           item.EndDate,
                                                           item.DrugKbn,
                                                           item.MasterSbt ?? string.Empty,
                                                           item.BuiKbn,
                                                           item.IsAdopted,
                                                           0,
                                                           item.TenId,
                                                           string.Empty,
                                                           string.Empty,
                                                           item.CmtCol1,
                                                           item.IpnNameCd ?? string.Empty,
                                                           item.SinKouiKbn,
                                                           item.YjCd ?? string.Empty,
                                                           item.CnvUnitName ?? string.Empty,
                                                           item.StartDate,
                                                           item.YohoKbn,
                                                           item.CmtColKeta1,
                                                           item.CmtColKeta2,
                                                           item.CmtColKeta3,
                                                           item.CmtColKeta4,
                                                           item.CmtCol2,
                                                           item.CmtCol3,
                                                           item.CmtCol4,
                                                           item.IpnNameCd ?? string.Empty,
                                                           item.MinAge ?? string.Empty,
                                                           item.MaxAge ?? string.Empty,
                                                           item.SanteiItemCd ?? string.Empty,
                                                           item.OdrTermVal,
                                                           item.CnvTermVal,
                                                           item.DefaultVal,
                                                           item.Kokuji1 ?? string.Empty,
                                                           item.Kokuji2 ?? string.Empty,
                                                           string.Empty
                                                           )).ToList();
            }

            return tenMstModels;
        }

        public List<ByomeiMstModel> DiseaseSearch(bool isPrefix, bool isByomei, bool isSuffix, bool isMisaiyou, string keyword, int sindate, int pageIndex, int pageSize)
        {
            var keywordHalfSize = keyword != String.Empty ? CIUtil.ToHalfsize(keyword) : "";

            var query = NoTrackingDataContext.ByomeiMsts.Where(item =>
                                    (item.KanaName1 != null &&
                                    item.KanaName1.StartsWith(keywordHalfSize))
                                    ||
                                    (item.KanaName2 != null &&
                                     item.KanaName2.StartsWith(keywordHalfSize))
                                    ||
                                    (item.KanaName3 != null &&
                                    item.KanaName3.StartsWith(keywordHalfSize))
                                    ||
                                    (item.KanaName4 != null &&
                                    item.KanaName4.StartsWith(keywordHalfSize))
                                    ||
                                    (item.KanaName5 != null &&
                                    item.KanaName5.StartsWith(keywordHalfSize))
                                    ||
                                    (item.KanaName6 != null &&
                                    item.KanaName6.StartsWith(keywordHalfSize))
                                    ||
                                    (item.KanaName7 != null &&
                                    item.KanaName7.StartsWith(keywordHalfSize))
                                    ||
                                    (item.Sbyomei != null &&
                                    item.Sbyomei.Contains(keyword))
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
                                    ||
                                    item.ByomeiCd.StartsWith(keyword)
                                 );

            query = query.Where(item => (item.DelDate == 0 || item.DelDate >= sindate) && (isMisaiyou || item.IsAdopted == 1));

            query = query.Where(item =>
                   (isByomei && item.ByomeiCd.Length != 4)
                || (isPrefix && (item.ByomeiCd.Length == 4 && !item.ByomeiCd.StartsWith("9") && !item.ByomeiCd.StartsWith("8")))
                || (isSuffix && (item.ByomeiCd.Length == 4 && item.ByomeiCd.StartsWith("8")))
            );

            var listDatas = query.OrderBy(item => item.KanaName1)
                                 .ThenByDescending(item => item.IsAdopted)
                                 .OrderByDescending(item => item.ByomeiCd.Length != 4)
                                 .ThenByDescending(item => item.ByomeiCd.Length == 4)
                                 .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            List<ByomeiMstModel> listByomeies = new();

            if (listDatas != null)
            {
                listByomeies = listDatas.Select(mst => ConvertToByomeiMstModel(mst)).ToList();
            }
            return listByomeies;
        }
        public List<ByomeiMstModel> DiseaseSearch(List<string> keyCodes)
        {
            var listDatas = NoTrackingDataContext.ByomeiMsts.Where(item => keyCodes.Contains(item.ByomeiCd)).ToList();
            List<ByomeiMstModel> listByomeies = new();

            if (listDatas != null)
            {
                listByomeies = listDatas.Select(mst => ConvertToByomeiMstModel(mst)).ToList();
            }
            return listByomeies;
        }
        public bool UpdateAdoptedByomei(int hpId, string byomeiCd, int userId)
        {
            if (hpId <= 0 || string.IsNullOrEmpty(byomeiCd)) return false;
            var byomeiMst = TrackingDataContext.ByomeiMsts.Where(p => p.HpId == hpId && p.ByomeiCd == byomeiCd).FirstOrDefault();
            if (byomeiMst != null)
            {
                byomeiMst.IsAdopted = 1;
                byomeiMst.UpdateId = userId;
                byomeiMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                TrackingDataContext.SaveChanges();
            }
            return true;
        }

        public bool CheckItemCd(string itemCd)
        {
            return NoTrackingDataContext.TenMsts.Any(t => t.ItemCd == itemCd.Trim());
        }

        public List<string> GetCheckItemCds(List<string> itemCds)
        {
            itemCds = itemCds.Distinct().ToList();
            var result = NoTrackingDataContext.TenMsts.Where(t => itemCds.Contains(t.ItemCd.Trim())).Select(t => t.ItemCd).Distinct().ToList();
            return result;
        }

        public List<Tuple<string, string>> GetCheckIpnCds(List<string> ipnCds)
        {
            return NoTrackingDataContext.IpnNameMsts.Where(t => ipnCds.Contains(t.IpnNameCd.Trim())).Select(t => new Tuple<string, string>(t.IpnNameCd, t.IpnName ?? string.Empty)).ToList();
        }

        public TenItemModel FindTenMst(int hpId, string itemCd, int sinDate)
        {
            var entity = NoTrackingDataContext.TenMsts.FirstOrDefault(p =>
                   p.HpId == hpId &&
                   p.StartDate <= sinDate &&
                   p.EndDate >= sinDate &&
                   p.ItemCd == itemCd);

            return new TenItemModel(
                    entity?.HpId ?? 0,
                    entity?.ItemCd ?? string.Empty,
                    entity?.RousaiKbn ?? 0,
                    entity?.KanaName1 ?? string.Empty,
                    entity?.Name ?? string.Empty,
                    entity?.KohatuKbn ?? 0,
                    entity?.MadokuKbn ?? 0,
                    entity?.KouseisinKbn ?? 0,
                    entity?.OdrUnitName ?? string.Empty,
                    entity?.EndDate ?? 0,
                    entity?.DrugKbn ?? 0,
                    entity?.MasterSbt ?? string.Empty,
                    entity?.BuiKbn ?? 0,
                    entity?.IsAdopted ?? 0,
                    entity?.Ten != null ? entity.Ten : 0,
                    entity?.TenId ?? 0,
                    string.Empty,
                    string.Empty,
                    entity?.CmtCol1 ?? 0,
                    entity?.IpnNameCd ?? string.Empty,
                    entity?.SinKouiKbn ?? 0,
                    entity?.YjCd ?? string.Empty,
                    entity?.CnvUnitName ?? string.Empty,
                    entity?.StartDate ?? 0,
                    entity?.YohoKbn ?? 0,
                    entity?.CmtColKeta1 ?? 0,
                    entity?.CmtColKeta2 ?? 0,
                    entity?.CmtColKeta3 ?? 0,
                    entity?.CmtColKeta4 ?? 0,
                    entity?.CmtCol2 ?? 0,
                    entity?.CmtCol3 ?? 0,
                    entity?.CmtCol4 ?? 0,
                    entity?.IpnNameCd ?? string.Empty,
                    entity?.MinAge ?? string.Empty,
                    entity?.MaxAge ?? string.Empty,
                    entity?.SanteiItemCd ?? string.Empty,
                    entity?.OdrTermVal ?? 0,
                    entity?.CnvTermVal ?? 0,
                    entity?.DefaultVal ?? 0,
                    entity?.Kokuji1 ?? string.Empty,
                    entity?.Kokuji2 ?? string.Empty,
                    string.Empty
               );
        }

        public List<TenItemModel> FindTenMst(int hpId, List<string> itemCds, int minSinDate, int maxSinDate)
        {
            itemCds = itemCds.Distinct().ToList();
            var entities = NoTrackingDataContext.TenMsts.Where(p =>
                   p.HpId == hpId &&
                   p.StartDate <= minSinDate &&
                   p.EndDate >= maxSinDate &&
                   itemCds.Contains(p.ItemCd));

            return entities.Select(entity => new TenItemModel(
                    entity.HpId,
                    entity.ItemCd,
                    entity.RousaiKbn,
                    entity.KanaName1 ?? string.Empty,
                    entity.Name ?? string.Empty,
                    entity.KohatuKbn,
                    entity.MadokuKbn,
                    entity.KouseisinKbn,
                    entity.OdrUnitName ?? string.Empty,
                    entity.EndDate,
                    entity.DrugKbn,
                    entity.MasterSbt ?? string.Empty,
                    entity.BuiKbn,
                    entity.IsAdopted,
                    entity.Ten,
                    entity.TenId,
                    string.Empty,
                    string.Empty,
                    entity.CmtCol1,
                    entity.IpnNameCd ?? string.Empty,
                    entity.SinKouiKbn,
                    entity.YjCd ?? string.Empty,
                    entity.CnvUnitName ?? string.Empty,
                    entity.StartDate,
                    entity.YohoKbn,
                    entity.CmtColKeta1,
                    entity.CmtColKeta2,
                    entity.CmtColKeta3,
                    entity.CmtColKeta4,
                    entity.CmtCol2,
                    entity.CmtCol3,
                    entity.CmtCol4,
                    entity.IpnNameCd ?? string.Empty,
                    entity.MinAge ?? string.Empty,
                    entity.MaxAge ?? string.Empty,
                    entity.SanteiItemCd ?? string.Empty,
                    entity.OdrTermVal,
                    entity.CnvTermVal,
                    entity.DefaultVal,
                    entity.Kokuji1 ?? string.Empty,
                    entity.Kokuji2 ?? string.Empty,
                    string.Empty
               )).ToList();
        }

        public List<TenItemModel> FindTenMst(int hpId, List<string> itemCds)
        {
            var entities = NoTrackingDataContext.TenMsts.Where(p =>
                   p.HpId == hpId &&
                   itemCds.Contains(p.ItemCd));

            return entities.Select(entity => new TenItemModel(
                    entity.HpId,
                    entity.ItemCd,
                    entity.RousaiKbn,
                    entity.KanaName1 ?? string.Empty,
                    entity.Name ?? string.Empty,
                    entity.KohatuKbn,
                    entity.MadokuKbn,
                    entity.KouseisinKbn,
                    entity.OdrUnitName ?? string.Empty,
                    entity.EndDate,
                    entity.DrugKbn,
                    entity.MasterSbt ?? string.Empty,
                    entity.BuiKbn,
                    entity.IsAdopted,
                    entity.Ten,
                    entity.TenId,
                    string.Empty,
                    string.Empty,
                    entity.CmtCol1,
                    entity.IpnNameCd ?? string.Empty,
                    entity.SinKouiKbn,
                    entity.YjCd ?? string.Empty,
                    entity.CnvUnitName ?? string.Empty,
                    entity.StartDate,
                    entity.YohoKbn,
                    entity.CmtColKeta1,
                    entity.CmtColKeta2,
                    entity.CmtColKeta3,
                    entity.CmtColKeta4,
                    entity.CmtCol2,
                    entity.CmtCol3,
                    entity.CmtCol4,
                    entity.IpnNameCd ?? string.Empty,
                    entity.MinAge ?? string.Empty,
                    entity.MaxAge ?? string.Empty,
                    entity.SanteiItemCd ?? string.Empty,
                    entity.OdrTermVal,
                    entity.CnvTermVal,
                    entity.DefaultVal,
                    entity.Kokuji1 ?? string.Empty,
                    entity.Kokuji2 ?? string.Empty,
                    string.Empty
               )).ToList();
        }

        public List<TenItemModel> GetTenMstList(int hpId, List<string> itemCds)
        {
            itemCds = itemCds.Distinct().ToList();
            var entities = NoTrackingDataContext.TenMsts.Where(p =>
                  p.HpId == hpId &&
                  itemCds.Contains(p.ItemCd));

            return entities.Select(entity => new TenItemModel(
                    entity.HpId,
                    entity.ItemCd,
                    entity.RousaiKbn,
                    entity.KanaName1 ?? string.Empty,
                    entity.Name ?? string.Empty,
                    entity.KohatuKbn,
                    entity.MadokuKbn,
                    entity.KouseisinKbn,
                    entity.OdrUnitName ?? string.Empty,
                    entity.EndDate,
                    entity.DrugKbn,
                    entity.MasterSbt ?? string.Empty,
                    entity.BuiKbn,
                    entity.IsAdopted,
                    entity.Ten,
                    entity.TenId,
                    string.Empty,
                    string.Empty,
                    entity.CmtCol1,
                    entity.IpnNameCd ?? string.Empty,
                    entity.SinKouiKbn,
                    entity.YjCd ?? string.Empty,
                    entity.CnvUnitName ?? string.Empty,
                    entity.StartDate,
                    entity.YohoKbn,
                    entity.CmtColKeta1,
                    entity.CmtColKeta2,
                    entity.CmtColKeta3,
                    entity.CmtColKeta4,
                    entity.CmtCol2,
                    entity.CmtCol3,
                    entity.CmtCol4,
                    entity.IpnNameCd ?? string.Empty,
                    entity.MinAge ?? string.Empty,
                    entity.MaxAge ?? string.Empty,
                    entity.SanteiItemCd ?? string.Empty,
                    entity.OdrTermVal,
                    entity.CnvTermVal,
                    entity.DefaultVal,
                    entity.Kokuji1 ?? string.Empty,
                    entity.Kokuji2 ?? string.Empty,
                    string.Empty
               )).ToList();
        }

        public (int, List<PostCodeMstModel>) PostCodeMstModels(int hpId, string postCode1, string postCode2, string address, int pageIndex, int pageSize)
        {
            var entities = NoTrackingDataContext.PostCodeMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0);

            if (!string.IsNullOrEmpty(postCode1) && !string.IsNullOrEmpty(postCode2))
                entities = entities.Where(e => e.PostCd != null && e.PostCd.StartsWith(postCode1) && e.PostCd.EndsWith(postCode2));

            else if (!string.IsNullOrEmpty(postCode1))
                entities = entities.Where(e => e.PostCd != null && e.PostCd.StartsWith(postCode1));

            else if (!string.IsNullOrEmpty(postCode2))
                entities = entities.Where(e => e.PostCd != null && e.PostCd.EndsWith(postCode2));

            if (!string.IsNullOrEmpty(address))
            {
                entities = entities.Where(e => (e.PrefName + e.CityName + e.Banti).Contains(address)
                                                || (e.PrefName + e.CityName).Contains(address)
                                                || (e.PrefName != null && e.PrefName.Contains(address)));
            }

            var totalCount = entities.Count();

            var result = entities.OrderBy(x => x.PostCd)
                                  .ThenBy(x => x.PrefName)
                                  .ThenBy(x => x.CityName)
                                  .ThenBy(x => x.Banti)
                                  .Select(x => new PostCodeMstModel(
                                      x.Id,
                                      x.HpId,
                                      x.PostCd ?? string.Empty,
                                      x.PrefKana ?? string.Empty,
                                      x.CityKana ?? string.Empty,
                                      x.PostalTermKana ?? string.Empty,
                                      x.PrefName ?? string.Empty,
                                      x.CityName ?? string.Empty,
                                      x.Banti ?? string.Empty,
                                      x.IsDeleted))
                                  .Skip(pageIndex - 1).Take(pageSize)
                                  .ToList();

            return (totalCount, result);
        }

        public List<ItemCmtModel> GetCmtCheckMsts(int hpId, int userId, List<string> itemCds)
        {
            var result = new List<ItemCmtModel>();
            var cmtCheckMsts = NoTrackingDataContext.CmtCheckMsts.Where(p => p.KarteKbn == KarteConst.KarteKbn && p.HpId == hpId &&
                                                                                    p.IsDeleted == DeleteTypes.None &&
                                                                                    itemCds.Contains(p.ItemCd));

            foreach (var itemCd in itemCds)
            {
                var entities = cmtCheckMsts.Where(p => p.ItemCd == itemCd).OrderBy(p => p.SortNo);
                if (entities == null) continue;
                foreach (var entity in entities)
                {
                    result.Add(new ItemCmtModel(itemCd, entity.Cmt ?? string.Empty, entity.SortNo));
                }
            }
            return result;
        }

        /// <summary>
        /// 項目グループマスタ取得
        /// </summary>
        /// <param name="sinDate">診療日</param>
        /// <param name="grpSbt">項目グループ種別 1:算定回数マスタ</param>
        /// <param name="itemGrpCd">項目グループコード</param>
        /// <returns></returns>
        public List<ItemGrpMstModel> FindItemGrpMst(int hpId, int sinDate, int grpSbt, List<long> itemGrpCds)
        {
            itemGrpCds = itemGrpCds.Distinct().ToList();
            List<ItemGrpMstModel> result = new List<ItemGrpMstModel>();

            var query =
                NoTrackingDataContext.itemGrpMsts.Where(p =>
                    p.HpId == hpId &&
                    p.GrpSbt == grpSbt &&
                    itemGrpCds.Contains(p.ItemGrpCd) &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenBy(p => p.SeqNo)
                .ToList();
            foreach (var entity in query)
            {
                result.Add(new ItemGrpMstModel(entity.HpId, entity.GrpSbt, entity.ItemGrpCd, entity.StartDate, entity.EndDate, entity.ItemCd ?? string.Empty, entity.SeqNo));
            }
            return result;
        }

        public List<ItemGrpMstModel> FindItemGrpMst(int hpId, int minSinDate, int maxSinDate, int grpSbt, List<long> itemGrpCds)
        {
            itemGrpCds = itemGrpCds.Distinct().ToList();
            List<ItemGrpMstModel> result = new List<ItemGrpMstModel>();

            var query =
                NoTrackingDataContext.itemGrpMsts.Where(p =>
                    p.HpId == hpId &&
                    p.GrpSbt == grpSbt &&
                    itemGrpCds.Contains(p.ItemGrpCd) &&
                    p.StartDate <= minSinDate &&
                    p.EndDate >= maxSinDate)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenBy(p => p.SeqNo)
                .ToList();
            foreach (var entity in query)
            {
                result.Add(new ItemGrpMstModel(entity.HpId, entity.GrpSbt, entity.ItemGrpCd, entity.StartDate, entity.EndDate, entity.ItemCd ?? string.Empty, entity.SeqNo));
            }
            return result;
        }

        public List<ItemCommentSuggestionModel> GetSelectiveComment(int hpId, List<string> listItemCd, int sinDate, List<int> isInvalidList, bool isRecalculation = false)
        {
            List<ItemCommentSuggestionModel> result = NoTrackingDataContext.TenMsts.Where(item => listItemCd.Contains(item.ItemCd) &&
                                                                              item.StartDate <= sinDate &&
                                                                              sinDate <= item.EndDate)
                                                 .AsEnumerable()
                                                 .Select(item => new ItemCommentSuggestionModel(
                                                     item.ItemCd,
                                                     "【" + item.Name + "】",
                                                     item?.SanteiItemCd ?? string.Empty,
                                                     new List<RecedenCmtSelectModel>()
                                                 ))
                                                 .ToList();

            if (result.Count <= 0)
                return new List<ItemCommentSuggestionModel>();

            var listItemCdCmtSelect = result.Select(item => item.SanteiItemCd).ToList();
            listItemCdCmtSelect.AddRange(listItemCd);
            listItemCdCmtSelect = listItemCdCmtSelect.Distinct().ToList();

            var listRecedenCmtSelectAll = NoTrackingDataContext.RecedenCmtSelects
                .Where(item => item.HpId == hpId &&
                                               listItemCdCmtSelect.Contains(item.ItemCd ?? string.Empty) &&
                                               item.StartDate <= sinDate &&
                                               sinDate <= item.EndDate &&
                                               isInvalidList.Contains(item.IsInvalid))
                .ToList();

            if (listRecedenCmtSelectAll.Count <= 0)
                return new List<ItemCommentSuggestionModel>();

            var listItemNo = listRecedenCmtSelectAll.GroupBy(item => new { item.ItemCd, item.CommentCd })
                .Select(grp => grp.OrderBy(r => r.ItemNo).First())
                .Select(item => item.ItemNo)
                .Distinct()
                .ToList();

            List<RecedenCmtSelect> listRecedenCmtMinEda = new List<RecedenCmtSelect>();

            var listRecedenCmtSelect = NoTrackingDataContext.RecedenCmtSelects
               .Where(item => item.HpId == hpId &&
                                              listItemCdCmtSelect.Contains(item.ItemCd) &&
                                              listItemNo.Contains(item.ItemNo) &&
                                              item.StartDate <= sinDate &&
                                              sinDate <= item.EndDate &&
                                              isInvalidList.Contains(item.IsInvalid))
               .ToList();
            listRecedenCmtMinEda.AddRange(listRecedenCmtSelect);

            if (!isRecalculation)
            {
                var listRecedenCmtSelectShinryo = NoTrackingDataContext.RecedenCmtSelects
                .Where(item => item.HpId == hpId &&
                                      item.ItemCd == "199999999" &&
                                      listItemNo.Contains(item.ItemNo) &&
                                      item.StartDate <= sinDate &&
                                      sinDate <= item.EndDate &&
                                      isInvalidList.Contains(item.IsInvalid));
                listRecedenCmtMinEda.AddRange(listRecedenCmtSelectShinryo);
            }

            var listCommentCd = listRecedenCmtMinEda.Select(item => item.CommentCd).Distinct();

            var listTenMst = NoTrackingDataContext.TenMsts
                .Where(item => item.HpId == hpId &&
                                         listCommentCd.Contains(item.ItemCd) &&
                                         item.StartDate <= sinDate &&
                                         sinDate <= item.EndDate);

            var listComment = (from recedenCmtSelect in listRecedenCmtMinEda
                               join tenMst in listTenMst on
                                   recedenCmtSelect.CommentCd equals tenMst.ItemCd
                               select new RecedenCmtSelectModel(
                                   tenMst.CmtSbt,
                                   recedenCmtSelect.ItemCd,
                                   recedenCmtSelect.CommentCd ?? string.Empty,
                                   tenMst.Name ?? string.Empty,
                                   recedenCmtSelect.ItemNo,
                                   recedenCmtSelect.EdaNo,
                                   tenMst.Name ?? string.Empty,
                                   recedenCmtSelect.SortNo,
                                   recedenCmtSelect.CondKbn,
                                   ConvertTenMstToModel(tenMst)
                               )).ToList();
            foreach (var inputCodeItem in result)
            {
                var listCommentWithCode = listComment.Where(item =>
                    item.ItemCd == inputCodeItem.ItemCd || item.ItemCd == inputCodeItem.SanteiItemCd)
                    .OrderBy(item => item.ItemNo)
                    .ToList();
                inputCodeItem.SetData(listCommentWithCode);
            }

            return result;
        }


        #region Private Function
        private static ByomeiMstModel ConvertToByomeiMstModel(ByomeiMst mst)
        {
            return new ByomeiMstModel(
                    mst.ByomeiCd,
                    mst.Byomei ?? string.Empty,
                    ConvertByomeiCdDisplay(mst.ByomeiCd),
                    mst.Sbyomei ?? string.Empty,
                    mst.KanaName1 ?? string.Empty,
                    mst.SikkanCd,
                    ConvertSikkanDisplay(mst.SikkanCd),
                    mst.NanbyoCd == NanbyoConst.Gairai ? "難病" : string.Empty,
                    ConvertIcd10Display(mst.Icd101 ?? string.Empty, mst.Icd102 ?? string.Empty),
                    ConvertIcd102013Display(mst.Icd1012013 ?? string.Empty, mst.Icd1022013 ?? string.Empty),
                    mst.IsAdopted == 1,
                    mst.NanbyoCd
                );
        }

        /// Get the ByomeiCdDisplay depend on ByomeiCd
        private static string ConvertByomeiCdDisplay(string byomeiCd)
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
        private static string ConvertSikkanDisplay(int SikkanCd)
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
        private static string ConvertIcd10Display(string icd101, string icd102)
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
        private static string ConvertIcd102013Display(string icd1012013, string icd1022013)
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

        private TenItemModel ConvertTenMstToModel(TenMst tenMst)
        {
            return new TenItemModel(
                        tenMst?.HpId ?? 0,
                        tenMst?.ItemCd ?? string.Empty,
                        tenMst?.RousaiKbn ?? 0,
                        tenMst?.KanaName1 ?? string.Empty,
                        tenMst?.Name ?? string.Empty,
                        tenMst?.KohatuKbn ?? 0,
                        tenMst?.MadokuKbn ?? 0,
                        tenMst?.KouseisinKbn ?? 0,
                        tenMst?.OdrUnitName ?? string.Empty,
                        tenMst?.EndDate ?? 0,
                        tenMst?.DrugKbn ?? 0,
                        tenMst?.MasterSbt ?? string.Empty,
                        tenMst?.BuiKbn ?? 0,
                        tenMst?.IsAdopted ?? 0,
                        tenMst?.Ten ?? 0,
                        tenMst?.TenId ?? 0,
                        "",
                        "",
                        tenMst?.CmtCol1 ?? 0,
                        tenMst?.IpnNameCd ?? string.Empty,
                        tenMst?.SinKouiKbn ?? 0,
                        tenMst?.YjCd ?? string.Empty,
                        tenMst?.CnvUnitName ?? string.Empty,
                        tenMst?.StartDate ?? 0,
                        tenMst?.YohoKbn ?? 0,
                        tenMst?.CmtColKeta1 ?? 0,
                        tenMst?.CmtColKeta2 ?? 0,
                        tenMst?.CmtColKeta3 ?? 0,
                        tenMst?.CmtColKeta4 ?? 0,
                        tenMst?.CmtCol2 ?? 0,
                        tenMst?.CmtCol3 ?? 0,
                        tenMst?.CmtCol4 ?? 0,
                        tenMst?.IpnNameCd ?? string.Empty,
                        tenMst?.MinAge ?? string.Empty,
                        tenMst?.MaxAge ?? string.Empty,
                        tenMst?.SanteiItemCd ?? string.Empty,
                        0,
                        0,
                        tenMst?.DefaultVal ?? 0,
                        tenMst?.Kokuji1 ?? string.Empty,
                        tenMst?.Kokuji2 ?? string.Empty,
                        string.Empty
                        );
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public List<string> GetListSanteiByomeis(int hpId, long ptId, int sinDate, int hokenPid)
        {
            return NoTrackingDataContext.PtByomeis.Where(item => item.HpId == hpId
                                                                         && item.PtId == ptId
                                                                         && item.IsDeleted != 1
                                                                         && (item.HokenPid == hokenPid || item.HokenPid == 0)
                                                                         && item.IsNodspKarte == 0
                                                                         && (item.TenkiKbn <= TenkiKbnConst.Continued || (item.StartDate <= (sinDate / 100 * 100 + 31) && item.TenkiDate >= (sinDate / 100 * 100 + 1)))
                                                                    ).OrderBy(p => p.TenkiKbn)
                                                                     .ThenByDescending(x => x.IsImportant)
                                                                     .ThenBy(p => p.SortNo)
                                                                     .ThenByDescending(p => p.StartDate)
                                                                     .ThenByDescending(p => p.TenkiDate)
                                                                     .ThenBy(p => p.Id)
                                                                     .Select(item => item.Byomei ?? string.Empty)
                                                                     .ToList();

        }

        //Key of Dictionary is itemCd
        public Dictionary<string, (int sinkouiKbn, string itemName, List<TenItemModel>)> GetConversionItem(List<(string itemCd, int sinKouiKbn, string itemName)> expiredItems, int sinDate, int hpId)
        {
            Dictionary<string, (int sinkouiKbn, string itemName, List<TenItemModel>)> result = new();
            var expiredItemCds = expiredItems.Select(e => e.itemCd).Distinct().ToList();
            expiredItems = expiredItems.Where(e => expiredItemCds.Contains(e.Item1)).ToList();
            var conversionItemInfs = NoTrackingDataContext.ConversionItemInfs.Where(
                            c => expiredItemCds.Contains(c.SourceItemCd) && c.HpId == hpId
                           );
            var desItemCds = conversionItemInfs.Select(c => c.DestItemCd).Distinct().ToList();
            var desTenMstItems = NoTrackingDataContext.TenMsts.Where(t => desItemCds.Contains(t.ItemCd) && t.HpId == hpId && t.StartDate <= sinDate && sinDate <= t.EndDate).ToList();
            foreach (var expiredItem in expiredItems)
            {
                var conversionItemInfsOfOnes = conversionItemInfs.Where(c => c.SourceItemCd == expiredItem.Item1).Select(c => c.DestItemCd).Distinct().ToList();
                var tenMstItemsOfOne = desTenMstItems.Where(d => conversionItemInfsOfOnes.Contains(d.ItemCd)).Select(t => ConvertTenMstToModel(t)).ToList();
                result.Add(new(expiredItem.itemCd, new(expiredItem.sinKouiKbn, expiredItem.itemName, tenMstItemsOfOne)));
            }

            return result;
        }

        public bool ExceConversionItem(int hpId, int userId, Dictionary<string, List<TenItemModel>> values)
        {
            foreach (var value in values)
            {
                var maxSortNo = NoTrackingDataContext.ConversionItemInfs.Where(c => c.HpId == hpId && c.SourceItemCd == value.Key).AsEnumerable().Select(c => c.SortNo).DefaultIfEmpty(0).Max();
                foreach (var tenItem in value.Value)
                {
                    if (tenItem.ModeStatus == -1)
                    {
                        var conversionItem = TrackingDataContext.ConversionItemInfs.FirstOrDefault(c => c.HpId == hpId && c.SourceItemCd == value.Key && c.DestItemCd == tenItem.ItemCd);
                        if (conversionItem != null)
                        {
                            TrackingDataContext.ConversionItemInfs.Remove(conversionItem);
                        }
                    }
                    else if (tenItem.ModeStatus == 0)
                    {
                        var conversionItem = new ConversionItemInf
                        {
                            HpId = hpId,
                            SourceItemCd = value.Key,
                            DestItemCd = tenItem.ItemCd,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId,
                            SortNo = ++maxSortNo
                        };
                        TrackingDataContext.ConversionItemInfs.Add(conversionItem);
                    }
                }
            }

            return TrackingDataContext.SaveChanges() > 0;
        }

        public List<HolidayModel> FindHolidayMstList(int hpId, int fromDate, int toDate)
        {
            var holidayMsts = NoTrackingDataContext.HolidayMsts
                .Where(item =>
                    item.HpId == hpId &&
                    item.IsDeleted == 0 &&
                    item.SinDate >= fromDate &&
                    item.SinDate <= toDate &&
                    item.HolidayKbn > 0 &&
                    item.KyusinKbn > 0).AsEnumerable();

            return holidayMsts.Select(item => new HolidayModel(item.SinDate,
                                            item.HolidayKbn,
                                            item.HolidayName ?? string.Empty))
            .OrderBy(item => item.SinDate)
            .ToList();
        }

        public List<KensaCenterMstModel> GetListKensaCenterMst(int hpId)
        {
            var kensaCenterMstModels = NoTrackingDataContext.KensaCenterMsts.Where(u => u.HpId == hpId);
            if (!kensaCenterMstModels.Any())
            {
                return new();
            }

            return kensaCenterMstModels.Select(x => new KensaCenterMstModel(
                                                        x.Id,
                                                        x.HpId,
                                                        x.CenterCd ?? string.Empty,
                                                        x.CenterName ?? string.Empty,
                                                        x.PrimaryKbn,
                                                        x.SortNo
                                        )).ToList();
        }

        public List<TenMstOriginModel> GetGroupTenMst(string itemCd)
        {
            return NoTrackingDataContext.TenMsts.Where(item => item.ItemCd == itemCd)
                                                .OrderByDescending(item => item.StartDate)
                                                .Select(x => new TenMstOriginModel(x.HpId,
                                                                                   x.ItemCd,
                                                                                   x.StartDate,
                                                                                   x.EndDate,
                                                                                   x.MasterSbt ?? string.Empty,
                                                                                   x.SinKouiKbn,
                                                                                   x.Name ?? string.Empty,
                                                                                   x.KanaName1 ?? string.Empty,
                                                                                   x.KanaName2 ?? string.Empty,
                                                                                   x.KanaName3 ?? string.Empty,
                                                                                   x.KanaName4 ?? string.Empty,
                                                                                   x.KanaName5 ?? string.Empty,
                                                                                   x.KanaName6 ?? string.Empty,
                                                                                   x.KanaName7 ?? string.Empty,
                                                                                   x.RyosyuName ?? string.Empty,
                                                                                   x.ReceName ?? string.Empty,
                                                                                   x.TenId,
                                                                                   x.Ten,
                                                                                   x.ReceUnitCd ?? string.Empty,
                                                                                   x.ReceUnitName ?? string.Empty,
                                                                                   x.OdrUnitName ?? string.Empty,
                                                                                   x.CnvUnitName ?? string.Empty,
                                                                                   x.OdrTermVal,
                                                                                   x.CnvTermVal,
                                                                                   x.DefaultVal,
                                                                                   x.IsAdopted,
                                                                                   x.KoukiKbn,
                                                                                   x.HokatuKensa,
                                                                                   x.ByomeiKbn,
                                                                                   x.Igakukanri,
                                                                                   x.JitudayCount,
                                                                                   x.Jituday,
                                                                                   x.DayCount,
                                                                                   x.DrugKanrenKbn,
                                                                                   x.KizamiId,
                                                                                   x.KizamiMin,
                                                                                   x.KizamiMax,
                                                                                   x.KizamiVal,
                                                                                   x.KizamiTen,
                                                                                   x.KizamiErr,
                                                                                   x.MaxCount,
                                                                                   x.MaxCountErr,
                                                                                   x.TyuCd ?? string.Empty,
                                                                                   x.TyuSeq ?? string.Empty,
                                                                                   x.TusokuAge,
                                                                                   x.MinAge ?? string.Empty,
                                                                                   x.MaxAge ?? string.Empty,
                                                                                   x.AgeCheck,
                                                                                   x.TimeKasanKbn,
                                                                                   x.FutekiKbn,
                                                                                   x.FutekiSisetuKbn,
                                                                                   x.SyotiNyuyojiKbn,
                                                                                   x.LowWeightKbn,
                                                                                   x.HandanKbn,
                                                                                   x.HandanGrpKbn,
                                                                                   x.TeigenKbn,
                                                                                   x.SekituiKbn,
                                                                                   x.KeibuKbn,
                                                                                   x.AutoHougouKbn,
                                                                                   x.GairaiKanriKbn,
                                                                                   x.TusokuTargetKbn,
                                                                                   x.HokatuKbn,
                                                                                   x.TyoonpaNaisiKbn,
                                                                                   x.AutoFungoKbn,
                                                                                   x.TyoonpaGyokoKbn,
                                                                                   x.GazoKasan,
                                                                                   x.KansatuKbn,
                                                                                   x.MasuiKbn,
                                                                                   x.FukubikuNaisiKasan,
                                                                                   x.FukubikuKotunanKasan,
                                                                                   x.MasuiKasan,
                                                                                   x.MoniterKasan,
                                                                                   x.ToketuKasan,
                                                                                   x.TenKbnNo ?? string.Empty,
                                                                                   x.ShortstayOpe,
                                                                                   x.BuiKbn,
                                                                                   x.Sisetucd1,
                                                                                   x.Sisetucd2,
                                                                                   x.Sisetucd3,
                                                                                   x.Sisetucd4,
                                                                                   x.Sisetucd5,
                                                                                   x.Sisetucd6,
                                                                                   x.Sisetucd7,
                                                                                   x.Sisetucd8,
                                                                                   x.Sisetucd9,
                                                                                   x.Sisetucd10,
                                                                                   x.AgekasanMin1 ?? string.Empty,
                                                                                   x.AgekasanMax1 ?? string.Empty,
                                                                                   x.AgekasanCd1 ?? string.Empty,
                                                                                   x.AgekasanMin2 ?? string.Empty,
                                                                                   x.AgekasanMax2 ?? string.Empty,
                                                                                   x.AgekasanCd2 ?? string.Empty,
                                                                                   x.AgekasanMin3 ?? string.Empty,
                                                                                   x.AgekasanMax3 ?? string.Empty,
                                                                                   x.AgekasanCd3 ?? string.Empty,
                                                                                   x.AgekasanMin4 ?? string.Empty,
                                                                                   x.AgekasanMax4 ?? string.Empty,
                                                                                   x.AgekasanCd4 ?? string.Empty,
                                                                                   x.KensaCmt,
                                                                                   x.MadokuKbn,
                                                                                   x.SinkeiKbn,
                                                                                   x.SeibutuKbn,
                                                                                   x.ZoueiKbn,
                                                                                   x.DrugKbn,
                                                                                   x.ZaiKbn,
                                                                                   x.ZaikeiPoint,
                                                                                   x.Capacity,
                                                                                   x.KohatuKbn,
                                                                                   x.TokuzaiAgeKbn,
                                                                                   x.SansoKbn,
                                                                                   x.TokuzaiSbt,
                                                                                   x.MaxPrice,
                                                                                   x.MaxTen,
                                                                                   x.SyukeiSaki ?? string.Empty,
                                                                                   x.CdKbn ?? string.Empty,
                                                                                   x.CdSyo,
                                                                                   x.CdBu,
                                                                                   x.CdKbnno,
                                                                                   x.CdEdano,
                                                                                   x.CdKouno,
                                                                                   x.KokujiKbn ?? string.Empty,
                                                                                   x.KokujiSyo,
                                                                                   x.KokujiBu,
                                                                                   x.KokujiKbnNo,
                                                                                   x.KokujiEdaNo,
                                                                                   x.KokujiKouNo,
                                                                                   x.Kokuji1 ?? string.Empty,
                                                                                   x.Kokuji2 ?? string.Empty,
                                                                                   x.KohyoJun,
                                                                                   x.YjCd ?? string.Empty,
                                                                                   x.YakkaCd ?? string.Empty,
                                                                                   x.SyusaiSbt,
                                                                                   x.SyohinKanren ?? string.Empty,
                                                                                   x.UpdDate,
                                                                                   x.DelDate,
                                                                                   x.KeikaDate,
                                                                                   x.RousaiKbn,
                                                                                   x.SisiKbn,
                                                                                   x.ShotCnt,
                                                                                   x.IsNosearch,
                                                                                   x.IsNodspPaperRece,
                                                                                   x.IsNodspRece,
                                                                                   x.IsNodspRyosyu,
                                                                                   x.IsNodspKarte,
                                                                                   x.IsNodspYakutai,
                                                                                   x.JihiSbt,
                                                                                   x.KazeiKbn,
                                                                                   x.YohoKbn,
                                                                                   x.IpnNameCd ?? string.Empty,
                                                                                   x.FukuyoRise,
                                                                                   x.FukuyoMorning,
                                                                                   x.FukuyoDaytime,
                                                                                   x.FukuyoNight,
                                                                                   x.FukuyoSleep,
                                                                                   x.SuryoRoundupKbn,
                                                                                   x.KouseisinKbn,
                                                                                   x.ChusyaDrugSbt,
                                                                                   x.KensaFukusuSantei,
                                                                                   x.SanteiItemCd ?? string.Empty,
                                                                                   x.SanteigaiKbn,
                                                                                   x.KensaItemCd ?? string.Empty,
                                                                                   x.KensaItemSeqNo,
                                                                                   x.RenkeiCd1 ?? string.Empty,
                                                                                   x.RenkeiCd2 ?? string.Empty,
                                                                                   x.SaiketuKbn,
                                                                                   x.CmtKbn,
                                                                                   x.CmtCol1,
                                                                                   x.CmtColKeta1,
                                                                                   x.CmtCol2,
                                                                                   x.CmtColKeta2,
                                                                                   x.CmtCol3,
                                                                                   x.CmtColKeta3,
                                                                                   x.CmtCol4,
                                                                                   x.CmtColKeta4,
                                                                                   x.SelectCmtId,
                                                                                   x.KensaLabel,
                                                                                   false,
                                                                                   false,
                                                                                   x.IsDeleted,
                                                                                   false,
                                                                                   x.StartDate)).ToList();
        }

        public string GetMaxItemCdByTypeForAdd(string startWithstr)
        {
            var tenMstList = NoTrackingDataContext.TenMsts.Where(item => item.ItemCd.StartsWith(startWithstr)).ToList();

            var tenMst = tenMstList.Where(item => item.ItemCd.Substring(startWithstr.Length).AsInteger() != 0)
                                   .OrderByDescending(item => item.ItemCd.Replace(startWithstr, string.Empty).PadLeft(10, '0'))
                                   .FirstOrDefault();
            if (tenMst != null)
            {
                return startWithstr + (tenMst.ItemCd.Replace(startWithstr, "").AsInteger() + 1);
            }

            return startWithstr + "1";
        }


        public int GetMinJihiSbtMst(int hpId)
        {
            var jihiSbtMst = NoTrackingDataContext.JihiSbtMsts.Where(i => i.HpId == hpId && i.IsDeleted == DeleteStatus.None)
                                                            .OrderBy(item => item.JihiSbt)
                                                            .FirstOrDefault();
            if (jihiSbtMst != null)
            {
                return jihiSbtMst.JihiSbt;
            }
            return 0;
        }

        public bool SaveKensaCenterMst(int userId, List<KensaCenterMstModel> kensaCenterMstModels)
        {
            var addedModels = kensaCenterMstModels.Where(u => u.KensaCenterMstModelStatus == ModelStatus.Added);
            var updatedModels = kensaCenterMstModels.Where(u => u.KensaCenterMstModelStatus == ModelStatus.Modified);
            var deletedModels = kensaCenterMstModels.Where(u => u.KensaCenterMstModelStatus == ModelStatus.Deleted);

            if (!addedModels.Any() && !updatedModels.Any() && !deletedModels.Any()) return true;

            if (deletedModels.Any())
            {
                var modelsToDelete = TrackingDataContext.KensaCenterMsts.Where(u => deletedModels.Any(d => d.HpId == u.HpId && d.Id == u.Id));
                TrackingDataContext.KensaCenterMsts.RemoveRange(modelsToDelete);
            }

            if (updatedModels.Any())
            {
                foreach (var model in updatedModels)
                {
                    var kensaTracking = TrackingDataContext.KensaCenterMsts
                        .FirstOrDefault(x => x.HpId == model.HpId && x.Id == model.Id);

                    if (kensaTracking != null)
                    {
                        kensaTracking.CenterCd = model.CenterCd;
                        kensaTracking.CenterName = model.CenterName;
                        kensaTracking.PrimaryKbn = model.PrimaryKbn;
                        kensaTracking.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        kensaTracking.UpdateId = userId;
                        kensaTracking.SortNo = model.SortNo;
                    }
                }
            }

            if (addedModels.Any())
            {
                foreach (var model in addedModels)
                {
                    TrackingDataContext.KensaCenterMsts.Add(new KensaCenterMst()
                    {
                        HpId = model.HpId,
                        CenterCd = model.CenterCd,
                        CenterName = model.CenterName,
                        PrimaryKbn = model.PrimaryKbn,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        SortNo = model.SortNo
                    });
                }
            }

            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool IsTenMstItemCdUsed(int hpId, string itemCd)
        {
            return NoTrackingDataContext.OdrInfDetails.Any(x => x.HpId == hpId && x.ItemCd == itemCd);
        }


        public bool SaveDeleteOrRecoverTenMstOrigin(DeleteOrRecoverTenMstMode mode, string itemCd, int userId, List<TenMstOriginModel> tenMstModifieds)
        {
            var tenMstDatabases = TrackingDataContext.TenMsts.Where(item => item.ItemCd == itemCd).ToList();
            if (mode == DeleteOrRecoverTenMstMode.Delete)
            {
                tenMstDatabases.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.Deleted;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    x.UpdateId = userId;
                });
            }
            else
            {
                tenMstDatabases.ForEach(x =>
                {
                    x.IsDeleted = DeleteTypes.None;
                    x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    x.UpdateId = userId;
                });
            }

            if (tenMstModifieds.Any()) //Have Changes
            {
                foreach (var item in tenMstModifieds)
                {
                    if (item.IsStartDateKeyUpdated) //After change StartDate => IsStartDateKeyUpdated will is true
                    {
                        // remove old entity
                        var oldEntity = tenMstDatabases.FirstOrDefault(x => x.ItemCd == item.ItemCd && x.HpId == item.HpId && x.StartDate == item.OriginStartDate); // case customer update key.
                        if (oldEntity != null)
                        {
                            TrackingDataContext.TenMsts.Remove(oldEntity);
                        }
                        // Clone new entity with updated start date
                        TrackingDataContext.TenMsts.Add(Mapper.Map(item, new TenMst(), (src, dest) =>
                        {
                            dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            dest.UpdateId = userId;
                            dest.IsDeleted = mode == DeleteOrRecoverTenMstMode.Delete ? DeleteTypes.Deleted : DeleteTypes.None;
                            dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                            dest.CreateId = userId;
                            return dest;
                        }));
                    }
                    else
                    {
                        var update = tenMstDatabases.FirstOrDefault(x => x.ItemCd == item.ItemCd && x.HpId == item.HpId && x.StartDate == item.StartDate);
                        if (update != null)
                            Mapper.Map(item, update, (src, dest) =>
                            {
                                dest.IsDeleted = mode == DeleteOrRecoverTenMstMode.Delete ? DeleteTypes.Deleted : DeleteTypes.None;
                                return dest;
                            });
                    }
                }
            }
            return TrackingDataContext.SaveChanges() > 0;
        }

        public List<CmtKbnMstModel> GetListCmtKbnMstModelByItemCd(int hpId, string itemCd)
        {
            return NoTrackingDataContext.CmtKbnMsts
                                        .Where(item => item.HpId == hpId && item.ItemCd == itemCd)
                                        .OrderByDescending(item => item.StartDate)
                                        .Select(x => new CmtKbnMstModel(x.Id, x.StartDate, x.EndDate, x.CmtKbn, x.ItemCd, false))
                                        .ToList();
        }

        public TenMstOriginModel GetTenMstOriginModel(int hpId, string itemCd, int sinDate)
        {
            var model = NoTrackingDataContext.TenMsts.FirstOrDefault(
                x => x.HpId == hpId &&
                     x.StartDate <= sinDate &&
                     x.EndDate >= sinDate &&
                     x.ItemCd == itemCd &&
                     x.IsDeleted == DeleteTypes.None);

            if (model == null)
                return new TenMstOriginModel();
            else
            {
                return new TenMstOriginModel(model.HpId,
                                             model.ItemCd,
                                             model.StartDate,
                                             model.EndDate,
                                             model.MasterSbt ?? string.Empty,
                                             model.SinKouiKbn,
                                             model.Name ?? string.Empty,
                                             model.KanaName1 ?? string.Empty,
                                             model.KanaName2 ?? string.Empty,
                                             model.KanaName3 ?? string.Empty,
                                             model.KanaName4 ?? string.Empty,
                                             model.KanaName5 ?? string.Empty,
                                             model.KanaName6 ?? string.Empty,
                                             model.KanaName7 ?? string.Empty,
                                             model.RyosyuName ?? string.Empty,
                                             model.ReceName ?? string.Empty,
                                             model.TenId,
                                             model.Ten,
                                             model.ReceUnitCd ?? string.Empty,
                                             model.ReceUnitName ?? string.Empty,
                                             model.OdrUnitName ?? string.Empty,
                                             model.CnvUnitName ?? string.Empty,
                                             model.OdrTermVal,
                                             model.CnvTermVal,
                                             model.DefaultVal,
                                             model.IsAdopted,
                                             model.KoukiKbn,
                                             model.HokatuKensa,
                                             model.ByomeiKbn,
                                             model.Igakukanri,
                                             model.JitudayCount,
                                             model.Jituday,
                                             model.DayCount,
                                             model.DrugKanrenKbn,
                                             model.KizamiId,
                                             model.KizamiMin,
                                             model.KizamiMax,
                                             model.KizamiVal,
                                             model.KizamiTen,
                                             model.KizamiErr,
                                             model.MaxCount,
                                             model.MaxCountErr,
                                             model.TyuCd ?? string.Empty,
                                             model.TyuSeq ?? string.Empty,
                                             model.TusokuAge,
                                             model.MinAge ?? string.Empty,
                                             model.MaxAge ?? string.Empty,
                                             model.AgeCheck,
                                             model.TimeKasanKbn,
                                             model.FutekiKbn,
                                             model.FutekiSisetuKbn,
                                             model.SyotiNyuyojiKbn,
                                             model.LowWeightKbn,
                                             model.HandanKbn,
                                             model.HandanGrpKbn,
                                             model.TeigenKbn,
                                             model.SekituiKbn,
                                             model.KeibuKbn,
                                             model.AutoHougouKbn,
                                             model.GairaiKanriKbn,
                                             model.TusokuTargetKbn,
                                             model.HokatuKbn,
                                             model.TyoonpaNaisiKbn,
                                             model.AutoFungoKbn,
                                             model.TyoonpaGyokoKbn,
                                             model.GazoKasan,
                                             model.KansatuKbn,
                                             model.MasuiKbn,
                                             model.FukubikuNaisiKasan,
                                             model.FukubikuKotunanKasan,
                                             model.MasuiKasan,
                                             model.MoniterKasan,
                                             model.ToketuKasan,
                                             model.TenKbnNo ?? string.Empty,
                                             model.ShortstayOpe,
                                             model.BuiKbn,
                                             model.Sisetucd1,
                                             model.Sisetucd2,
                                             model.Sisetucd3,
                                             model.Sisetucd4,
                                             model.Sisetucd5,
                                             model.Sisetucd6,
                                             model.Sisetucd7,
                                             model.Sisetucd8,
                                             model.Sisetucd9,
                                             model.Sisetucd10,
                                             model.AgekasanMin1 ?? string.Empty,
                                             model.AgekasanMax1 ?? string.Empty,
                                             model.AgekasanCd1 ?? string.Empty,
                                             model.AgekasanMin2 ?? string.Empty,
                                             model.AgekasanMax2 ?? string.Empty,
                                             model.AgekasanCd2 ?? string.Empty,
                                             model.AgekasanMin3 ?? string.Empty,
                                             model.AgekasanMax3 ?? string.Empty,
                                             model.AgekasanCd3 ?? string.Empty,
                                             model.AgekasanMin4 ?? string.Empty,
                                             model.AgekasanMax4 ?? string.Empty,
                                             model.AgekasanCd4 ?? string.Empty,
                                             model.KensaCmt,
                                             model.MadokuKbn,
                                             model.SinkeiKbn,
                                             model.SeibutuKbn,
                                             model.ZoueiKbn,
                                             model.DrugKbn,
                                             model.ZaiKbn,
                                             model.ZaikeiPoint,
                                             model.Capacity,
                                             model.KohatuKbn,
                                             model.TokuzaiAgeKbn,
                                             model.SansoKbn,
                                             model.TokuzaiSbt,
                                             model.MaxPrice,
                                             model.MaxTen,
                                             model.SyukeiSaki ?? string.Empty,
                                             model.CdKbn ?? string.Empty,
                                             model.CdSyo,
                                             model.CdBu,
                                             model.CdKbnno,
                                             model.CdEdano,
                                             model.CdKouno,
                                             model.KokujiKbn ?? string.Empty,
                                             model.KokujiSyo,
                                             model.KokujiBu,
                                             model.KokujiKbnNo,
                                             model.KokujiEdaNo,
                                             model.KokujiKouNo,
                                             model.Kokuji1 ?? string.Empty,
                                             model.Kokuji2 ?? string.Empty,
                                             model.KohyoJun,
                                             model.YjCd ?? string.Empty,
                                             model.YakkaCd ?? string.Empty,
                                             model.SyusaiSbt,
                                             model.SyohinKanren ?? string.Empty,
                                             model.UpdDate,
                                             model.DelDate,
                                             model.KeikaDate,
                                             model.RousaiKbn,
                                             model.SisiKbn,
                                             model.ShotCnt,
                                             model.IsNosearch,
                                             model.IsNodspPaperRece,
                                             model.IsNodspRece,
                                             model.IsNodspRyosyu,
                                             model.IsNodspKarte,
                                             model.IsNodspYakutai,
                                             model.JihiSbt,
                                             model.KazeiKbn,
                                             model.YohoKbn,
                                             model.IpnNameCd ?? string.Empty,
                                             model.FukuyoRise,
                                             model.FukuyoMorning,
                                             model.FukuyoDaytime,
                                             model.FukuyoNight,
                                             model.FukuyoSleep,
                                             model.SuryoRoundupKbn,
                                             model.KouseisinKbn,
                                             model.ChusyaDrugSbt,
                                             model.KensaFukusuSantei,
                                             model.SanteiItemCd ?? string.Empty,
                                             model.SanteigaiKbn,
                                             model.KensaItemCd ?? string.Empty,
                                             model.KensaItemSeqNo,
                                             model.RenkeiCd1 ?? string.Empty,
                                             model.RenkeiCd2 ?? string.Empty,
                                             model.SaiketuKbn,
                                             model.CmtKbn,
                                             model.CmtCol1,
                                             model.CmtColKeta1,
                                             model.CmtCol2,
                                             model.CmtColKeta2,
                                             model.CmtCol3,
                                             model.CmtColKeta3,
                                             model.CmtCol4,
                                             model.CmtColKeta4,
                                             model.SelectCmtId,
                                             model.KensaLabel,
                                             false,
                                             false,
                                             model.IsDeleted,
                                             false,
                                             model.StartDate);
            }

        }


        public string GetTenMstName(int hpId, string santeiItemCd)
        {
            var model = NoTrackingDataContext.TenMsts.Where(
                x => x.HpId == hpId &&
                     x.ItemCd == santeiItemCd)
                .OrderByDescending(item => item.StartDate)
                .FirstOrDefault();
            if (model == null)
                return string.Empty;
            else
                return model.Name ?? string.Empty;
        }

        public List<M10DayLimitModel> GetM10DayLimitModels(string yjCdItem)
        {
            return NoTrackingDataContext.M10DayLimit.Where(
                    x => x.YjCd == yjCdItem)
                    .Select(x => new M10DayLimitModel(x.YjCd,
                                                      x.SeqNo,
                                                      x.LimitDay,
                                                      x.StDate ?? string.Empty,
                                                      x.EdDate ?? string.Empty,
                                                      x.Cmt ?? string.Empty))
                    .ToList();
        }


        public List<IpnMinYakkaMstModel> GetIpnMinYakkaMstModels(int hpId, string IpnNameCd)
        {
            return NoTrackingDataContext.IpnMinYakkaMsts.Where(
                    x => x.HpId == hpId &&
                         x.IsDeleted == 0 &&
                         x.IpnNameCd == IpnNameCd)
                    .Select(x => new IpnMinYakkaMstModel(x.Id, x.HpId, x.IpnNameCd, x.StartDate, x.EndDate, x.Yakka, x.SeqNo, x.IsDeleted))
                    .ToList();
        }


        public List<DrugDayLimitModel> GetDrugDayLimitModels(int hpId, string ItemCd)
        {
            return NoTrackingDataContext.DrugDayLimits.Where(
                    x => x.HpId == hpId &&
                         x.IsDeleted == 0 &&
                         x.ItemCd == ItemCd)
                    .AsEnumerable()
                    .Select(x => new DrugDayLimitModel(x.Id, x.HpId, x.ItemCd, x.SeqNo, x.LimitDay, x.StartDate, x.EndDate, x.IsDeleted, false))
                    .ToList();
        }

        public DosageMstModel GetDosageMstModel(int hpId, string ItemCd)
        {
            var model = NoTrackingDataContext.DosageMsts.FirstOrDefault(x =>
                        x.HpId == hpId &&
                        x.IsDeleted == 0 &&
                        x.ItemCd == ItemCd);

            if (model == null)
                return new DosageMstModel(hpId, ItemCd);
            else
            {
                return Mapper.Map(model, new DosageMstModel());
            }
        }

        public IpnNameMstModel GetIpnNameMstModel(int hpId, string ipnNameCd, int sinDate)
        {
            var model = NoTrackingDataContext.IpnNameMsts.FirstOrDefault(x =>
                        x.HpId == hpId &&
                        x.IsDeleted == 0 &&
                        x.IpnNameCd == ipnNameCd &&
                        x.StartDate <= sinDate &&
                        x.EndDate >= sinDate);

            if (model == null)
                return new IpnNameMstModel(hpId);
            else
            {
                return Mapper.Map(model, new IpnNameMstModel());
            }
        }

        public string GetYohoInfMstPrefixByItemCd(string itemCd)
        {
            var model = NoTrackingDataContext.YohoInfMsts.FirstOrDefault(x => x.ItemCd == itemCd);
            if (model == null)
                return string.Empty;
            else
                return model.YohoSuffix ?? string.Empty;
        }

        public List<DrugInfModel> GetDrugInfByItemCd(int hpId, string itemCd)
        {
            List<DrugInfModel> result = new List<DrugInfModel>();
            var listDrugInf = NoTrackingDataContext.DrugInfs.Where(u => u.HpId == hpId &&
                                                                   u.ItemCd == itemCd &&
                                                                   u.IsDeleted == 0)
                                                .OrderByDescending(u => u.UpdateDate).ToList();

            DrugInf description = listDrugInf.FirstOrDefault(u => u.InfKbn == 0) ?? new DrugInf() { InfKbn = 0, ItemCd = itemCd };
            DrugInf drugAction = listDrugInf.FirstOrDefault(u => u.InfKbn == 1) ?? new DrugInf() { InfKbn = 1, ItemCd = itemCd };
            DrugInf precautions = listDrugInf.FirstOrDefault(u => u.InfKbn == 2) ?? new DrugInf() { InfKbn = 2, ItemCd = itemCd };

            result.Add(new DrugInfModel(description.HpId, description.ItemCd, description.InfKbn, description.SeqNo, description.DrugInfo ?? string.Empty, description.IsDeleted, false));
            result.Add(new DrugInfModel(drugAction.HpId, drugAction.ItemCd, drugAction.InfKbn, drugAction.SeqNo, drugAction.DrugInfo ?? string.Empty, drugAction.IsDeleted, false));
            result.Add(new DrugInfModel(precautions.HpId, precautions.ItemCd, precautions.InfKbn, precautions.SeqNo, precautions.DrugInfo ?? string.Empty, precautions.IsDeleted, false));
            return result;
        }


        public PiImageModel GetImagePiByItemCd(int hpId, string itemCd, int imageType)
        {
            var piImage = NoTrackingDataContext.PiImages.FirstOrDefault(u => u.HpId == hpId && u.ItemCd == itemCd && u.ImageType == imageType);
            if (piImage != null)
            {
                return new PiImageModel(piImage.HpId, piImage.ImageType, piImage.ItemCd, piImage.FileName ?? string.Empty, false, false);
            }
            else
            {
                return new PiImageModel(hpId, itemCd, imageType);
            }
        }


        public List<TeikyoByomeiModel> GetTeikyoByomeiModel(int hpId, string itemCd, bool isFromCheckingView = false)
        {
            var result = new List<TeikyoByomeiModel>();

            var teikyoByomeis = NoTrackingDataContext.TekiouByomeiMsts.Where(
                    (x) => x.HpId == hpId && x.ItemCd == itemCd && (!isFromCheckingView || x.IsInvalidTokusyo != 1));


            var byomeiMsts = NoTrackingDataContext.ByomeiMsts.Where((x) => x.HpId == Session.HospitalID);

            var query = from teikyoByomei in teikyoByomeis
                        join byomeiMst in byomeiMsts on
                        teikyoByomei.ByomeiCd equals byomeiMst.ByomeiCd
                        select new
                        {
                            TeikyoByomei = teikyoByomei,
                            ByomeiMst = byomeiMst
                        };

            result = query.AsEnumerable()
                          .Select(x => new TeikyoByomeiModel(x.ByomeiMst.SikkanCd,
                                                             x.TeikyoByomei.HpId,
                                                             x.TeikyoByomei.ItemCd,
                                                             x.TeikyoByomei.ByomeiCd,
                                                             x.TeikyoByomei.StartYM,
                                                             x.TeikyoByomei.EndYM,
                                                             x.TeikyoByomei.IsInvalid,
                                                             x.TeikyoByomei.IsInvalidTokusyo,
                                                             x.TeikyoByomei.EditKbn,
                                                             x.TeikyoByomei.SystemData,
                                                             x.ByomeiMst.Byomei ?? string.Empty,
                                                             x.ByomeiMst.KanaName1 ?? string.Empty,
                                                             false,
                                                             false))
                          .OrderByDescending(x => x.SystemData)
                          .ThenBy(x => x.KanaName)
                          .ToList();
            return result;
        }

        public TekiouByomeiMstExcludedModel GetTekiouByomeiMstExcludedModelByItemCd(int hpId, string itemCd)
        {
            var model = NoTrackingDataContext.TekiouByomeiMstExcludeds.FirstOrDefault(
               x => x.HpId == hpId && x.IsDeleted == 0 && x.ItemCd == itemCd);

            if (model is null)
                return new TekiouByomeiMstExcludedModel(hpId, "", 0, 1);

            else
                return new TekiouByomeiMstExcludedModel(model.HpId, model.ItemCd, model.SeqNo, model.IsDeleted);
        }

        public List<DensiSanteiKaisuModel> GetDensiSanteiKaisuByItemCd(int hpId, string itemCd)
        {
            return NoTrackingDataContext.DensiSanteiKaisus.Where(
                     x => x.HpId == hpId &&
                          x.ItemCd == itemCd
                    )
                    .Select(x => new DensiSanteiKaisuModel(x.Id,
                                                           x.HpId,
                                                           x.ItemCd,
                                                           x.UnitCd,
                                                           x.MaxCount,
                                                           x.SpJyoken,
                                                           x.StartDate,
                                                           x.EndDate,
                                                           x.SeqNo,
                                                           x.UserSetting,
                                                           x.TargetKbn,
                                                           x.TermCount,
                                                           x.TermSbt,
                                                           x.IsInvalid,
                                                           x.ItemGrpCd,
                                                           false,
                                                           false))
                    .ToList();
        }


        public List<DensiHaihanModel> GetDensiHaihans(int hpId, string itemCd, int haihanKbn)
        {
            List<DensiHaihanModel> result = new List<DensiHaihanModel>();

            var tenMsts = NoTrackingDataContext.TenMsts.Where(x => x.HpId == hpId && x.IsDeleted == DeleteTypes.None)
                                                        .OrderByDescending(item => item.StartDate);

            var densiHaihanCustomEntities = NoTrackingDataContext.DensiHaihanCustoms.Where(x => x.HpId == hpId &&
                                                                                             x.ItemCd1 == itemCd &&
                                                                                             x.HaihanKbn == haihanKbn);

            var densiHaihanCustoms = (from densiHaihanCustomEntity in densiHaihanCustomEntities
                                      select new
                                      {
                                          densiHaihanCustomEntity,
                                          Name = (from tenMst in tenMsts
                                                  where densiHaihanCustomEntity.ItemCd2 == tenMst.ItemCd
                                                  select tenMst.Name).FirstOrDefault()
                                      })
                                    .AsEnumerable()
                                    .Select(x => new DensiHaihanModel((int)HaiHanModelType.DENSI_HAIHAN_CUSTOM,
                                                                    (int)HaiHanModelType.DENSI_HAIHAN_CUSTOM,
                                                                    x.densiHaihanCustomEntity.Id,
                                                                    x.densiHaihanCustomEntity.HpId, x.densiHaihanCustomEntity.ItemCd1,
                                                                    x.densiHaihanCustomEntity.ItemCd2 ?? string.Empty,
                                                                    x.densiHaihanCustomEntity.ItemCd2 ?? string.Empty,
                                                                    x.densiHaihanCustomEntity.ItemCd2 ?? string.Empty,
                                                                    x.Name ?? string.Empty,
                                                                    x.densiHaihanCustomEntity.HaihanKbn,
                                                                    x.densiHaihanCustomEntity.SpJyoken,
                                                                    x.densiHaihanCustomEntity.StartDate,
                                                                    x.densiHaihanCustomEntity.EndDate,
                                                                    x.densiHaihanCustomEntity.SeqNo,
                                                                    x.densiHaihanCustomEntity.UserSetting,
                                                                    x.densiHaihanCustomEntity.TargetKbn,
                                                                    x.densiHaihanCustomEntity.TermCnt,
                                                                    x.densiHaihanCustomEntity.TermSbt,
                                                                    x.densiHaihanCustomEntity.IsInvalid,
                                                                    x.densiHaihanCustomEntity.UserSetting == 0 || x.densiHaihanCustomEntity.UserSetting == 1,
                                                                    false,
                                                                    false))
                                    .ToList();

            if (densiHaihanCustoms != null)
            {
                result.AddRange(densiHaihanCustoms);
            }

            var densiHaihanDayEntites = NoTrackingDataContext.DensiHaihanDays.Where(
                x => x.HpId == hpId &&
                     x.ItemCd1 == itemCd &&
                     x.HaihanKbn == haihanKbn);

            var densiHaihanDays = (from densiHaihanDayEntity in densiHaihanDayEntites
                                   select new
                                   {
                                       densiHaihanDayEntity,
                                       Name = (from tenMst in tenMsts
                                               where densiHaihanDayEntity.ItemCd2 == tenMst.ItemCd
                                               select tenMst.Name).FirstOrDefault()
                                   })
                                    .AsEnumerable()
                                    .Select(x => new DensiHaihanModel((int)HaiHanModelType.DENSI_HAIHAN_DAY,
                                                                    (int)HaiHanModelType.DENSI_HAIHAN_DAY,
                                                                    x.densiHaihanDayEntity.Id,
                                                                    x.densiHaihanDayEntity.HpId,
                                                                    x.densiHaihanDayEntity.ItemCd1,
                                                                    x.densiHaihanDayEntity.ItemCd2 ?? string.Empty,
                                                                    x.densiHaihanDayEntity.ItemCd2 ?? string.Empty,
                                                                    x.densiHaihanDayEntity.ItemCd2 ?? string.Empty,
                                                                    x.Name ?? string.Empty,
                                                                    x.densiHaihanDayEntity.HaihanKbn,
                                                                    x.densiHaihanDayEntity.SpJyoken,
                                                                    x.densiHaihanDayEntity.StartDate,
                                                                    x.densiHaihanDayEntity.EndDate,
                                                                    x.densiHaihanDayEntity.SeqNo,
                                                                    x.densiHaihanDayEntity.UserSetting,
                                                                    x.densiHaihanDayEntity.TargetKbn,
                                                                    0,
                                                                    0,
                                                                    x.densiHaihanDayEntity.IsInvalid,
                                                                    x.densiHaihanDayEntity.UserSetting == 0 || x.densiHaihanDayEntity.UserSetting == 1,
                                                                    false,
                                                                    false))
                                    .ToList();

            if (densiHaihanDays != null)
            {
                result.AddRange(densiHaihanDays);
            }

            var densiHaihanKarteEntites = NoTrackingDataContext.DensiHaihanKartes.Where(
                 x => x.HpId == hpId &&
                      x.ItemCd1 == itemCd &&
                      x.HaihanKbn == haihanKbn);

            var densiHaihanKartes = (from densiHaihanKarteEntity in densiHaihanKarteEntites
                                     select new
                                     {
                                         densiHaihanKarteEntity,
                                         Name = (from tenMst in tenMsts
                                                 where densiHaihanKarteEntity.ItemCd2 == tenMst.ItemCd
                                                 select tenMst.Name).FirstOrDefault()
                                     })
                                    .AsEnumerable()
                                    .Select(x => new DensiHaihanModel((int)HaiHanModelType.DENSI_HAIHAN_KARTE,
                                                                    (int)HaiHanModelType.DENSI_HAIHAN_KARTE,
                                                                    x.densiHaihanKarteEntity.Id,
                                                                    x.densiHaihanKarteEntity.HpId,
                                                                    x.densiHaihanKarteEntity.ItemCd1,
                                                                    x.densiHaihanKarteEntity.ItemCd2 ?? string.Empty,
                                                                    x.densiHaihanKarteEntity.ItemCd2 ?? string.Empty,
                                                                    x.densiHaihanKarteEntity.ItemCd2 ?? string.Empty,
                                                                    x.Name ?? string.Empty,
                                                                    x.densiHaihanKarteEntity.HaihanKbn,
                                                                    x.densiHaihanKarteEntity.SpJyoken,
                                                                    x.densiHaihanKarteEntity.StartDate,
                                                                    x.densiHaihanKarteEntity.EndDate,
                                                                    x.densiHaihanKarteEntity.SeqNo,
                                                                    x.densiHaihanKarteEntity.UserSetting,
                                                                    x.densiHaihanKarteEntity.TargetKbn,
                                                                    0,
                                                                    0,
                                                                    x.densiHaihanKarteEntity.IsInvalid,
                                                                    x.densiHaihanKarteEntity.UserSetting == 0 || x.densiHaihanKarteEntity.UserSetting == 1,
                                                                    false,
                                                                    false))
                                    .ToList();

            if (densiHaihanKartes != null)
            {
                result.AddRange(densiHaihanKartes);
            }

            var densiHaihanMonthEntites = NoTrackingDataContext.DensiHaihanMonths.Where(x => x.HpId == hpId &&
                                                                                        x.ItemCd1 == itemCd &&
                                                                                        x.HaihanKbn == haihanKbn);

            var densiHaihanMonths = (from densiHaihanMonthEntity in densiHaihanMonthEntites
                                     select new
                                     {
                                         densiHaihanMonthEntity,
                                         Name = (from tenMst in tenMsts
                                                 where densiHaihanMonthEntity.ItemCd2 == tenMst.ItemCd
                                                 select tenMst.Name).FirstOrDefault()
                                     })
                                    .AsEnumerable()
                                    .Select(x => new DensiHaihanModel((int)HaiHanModelType.DENSI_HAIHAN_MONTH,
                                                                    (int)HaiHanModelType.DENSI_HAIHAN_MONTH,
                                                                    x.densiHaihanMonthEntity.Id,
                                                                    x.densiHaihanMonthEntity.HpId,
                                                                    x.densiHaihanMonthEntity.ItemCd1,
                                                                    x.densiHaihanMonthEntity.ItemCd2 ?? string.Empty,
                                                                    x.densiHaihanMonthEntity.ItemCd2 ?? string.Empty,
                                                                    x.densiHaihanMonthEntity.ItemCd2 ?? string.Empty,
                                                                    x.Name ?? string.Empty,
                                                                    x.densiHaihanMonthEntity.HaihanKbn,
                                                                    x.densiHaihanMonthEntity.SpJyoken,
                                                                    x.densiHaihanMonthEntity.StartDate,
                                                                    x.densiHaihanMonthEntity.EndDate,
                                                                    x.densiHaihanMonthEntity.SeqNo,
                                                                    x.densiHaihanMonthEntity.UserSetting,
                                                                    x.densiHaihanMonthEntity.TargetKbn,
                                                                    0,
                                                                    0,
                                                                    x.densiHaihanMonthEntity.IsInvalid,
                                                                    x.densiHaihanMonthEntity.UserSetting == 0 || x.densiHaihanMonthEntity.UserSetting == 1,
                                                                    false,
                                                                    false))
                                    .ToList();

            if (densiHaihanMonths != null)
            {
                result.AddRange(densiHaihanMonths);
            }

            var densiHaihanWeekEntites = NoTrackingDataContext.DensiHaihanWeeks.Where(x => x.HpId == hpId &&
                                                                                          x.ItemCd1 == itemCd &&
                                                                                          x.HaihanKbn == haihanKbn);

            var densiHaihanWeeks = (from densiHaihanWeekEntity in densiHaihanWeekEntites
                                    select new
                                    {
                                        densiHaihanWeekEntity,
                                        Name = (from tenMst in tenMsts
                                                where densiHaihanWeekEntity.ItemCd2 == tenMst.ItemCd
                                                select tenMst.Name).FirstOrDefault()
                                    })
                                    .AsEnumerable()
                                    .Select(x => new DensiHaihanModel((int)HaiHanModelType.DENSI_HAIHAN_WEEK,
                                                                    (int)HaiHanModelType.DENSI_HAIHAN_WEEK,
                                                                    x.densiHaihanWeekEntity.Id,
                                                                    x.densiHaihanWeekEntity.HpId,
                                                                    x.densiHaihanWeekEntity.ItemCd1,
                                                                    x.densiHaihanWeekEntity.ItemCd2 ?? string.Empty,
                                                                    x.densiHaihanWeekEntity.ItemCd2 ?? string.Empty,
                                                                    x.densiHaihanWeekEntity.ItemCd2 ?? string.Empty,
                                                                    x.Name ?? string.Empty,
                                                                    x.densiHaihanWeekEntity.HaihanKbn,
                                                                    x.densiHaihanWeekEntity.SpJyoken,
                                                                    x.densiHaihanWeekEntity.StartDate,
                                                                    x.densiHaihanWeekEntity.EndDate,
                                                                    x.densiHaihanWeekEntity.SeqNo,
                                                                    x.densiHaihanWeekEntity.UserSetting,
                                                                    x.densiHaihanWeekEntity.TargetKbn,
                                                                    0,
                                                                    0,
                                                                    x.densiHaihanWeekEntity.IsInvalid,
                                                                    x.densiHaihanWeekEntity.UserSetting == 0 || x.densiHaihanWeekEntity.UserSetting == 1,
                                                                    false,
                                                                    false))
                                    .ToList();
            if (densiHaihanWeeks != null)
            {
                result.AddRange(densiHaihanWeeks);
            }

            return result
                    .OrderBy(x => (int)x.ModelType)
                    .ThenByDescending(x => x.StartDate)
                    .ThenBy(x => x.ItemCd2)
                    .ThenBy(x => x.UserSetting)
                    .ToList();
        }

        public List<DensiHoukatuModel> GetListDensiHoukatuByItemCd(int hpId, string itemCd, int sinDate)
        {
            List<DensiHoukatuModel> result = new List<DensiHoukatuModel>();
            var listHoukatu = NoTrackingDataContext.DensiHoukatus.Where(u => u.HpId == hpId &&
                                                                                          u.ItemCd == itemCd &&
                                                                                          u.StartDate <= sinDate &&
                                                                                          u.EndDate >= sinDate &&
                                                                                          !_HoukatuTermExclude.Contains(u.HoukatuTerm));

            var listHoukatuGrp = NoTrackingDataContext.DensiHoukatuGrps.Where(u => u.HpId == hpId &&
                                                                                            u.StartDate <= sinDate &&
                                                                                            u.EndDate >= sinDate);

            var tenMstQuery = NoTrackingDataContext.TenMsts.Where(e => e.StartDate <= sinDate && e.EndDate >= sinDate && e.IsDeleted == DeleteTypes.None);

            var query = from hokatu in listHoukatu
                        join hokatuGrp in listHoukatuGrp on hokatu.HoukatuGrpNo equals hokatuGrp.HoukatuGrpNo
                        join tenMst in tenMstQuery on hokatuGrp.ItemCd equals tenMst.ItemCd into ListTenMst
                        from tenMstItem in ListTenMst.DefaultIfEmpty()
                        select new
                        {
                            Hokatu = hokatu,
                            HokatuGbp = hokatuGrp,
                            ItemName = tenMstItem.Name,
                        };
            result = query.AsEnumerable().Where(data => !string.IsNullOrEmpty(data.ItemName))
                                        .Select(x => new DensiHoukatuModel(x.Hokatu.HpId,
                                                                           x.Hokatu.ItemCd,
                                                                           x.Hokatu.StartDate,
                                                                           x.Hokatu.EndDate,
                                                                           x.Hokatu.TargetKbn,
                                                                           x.Hokatu.SeqNo,
                                                                           x.Hokatu.HoukatuTerm,
                                                                           x.Hokatu.HoukatuGrpNo ?? string.Empty,
                                                                           x.Hokatu.UserSetting,
                                                                           x.Hokatu.IsInvalid,
                                                                           x.ItemName,
                                                                           x.HokatuGbp.ItemCd,
                                                                           x.HokatuGbp.SpJyoken,
                                                                           false,
                                                                           false))
                                        .OrderBy(u => u.HoukatuGrpNo)
                                        .ThenBy(u => u.ItemCd)
                                        .ThenBy(u => u.SeqNo)
                                        .ToList();
            return result;
        }

        public List<DensiHoukatuGrpModel> GetListDensiHoukatuGrpByItemCd(int hpId, string itemCd, int sinDate)
        {
            List<DensiHoukatuGrpModel> result = new List<DensiHoukatuGrpModel>();
            var listHoukatuGrp = NoTrackingDataContext.DensiHoukatuGrps.Where(u => u.HpId == hpId &&
                                                                                             u.ItemCd == itemCd &&
                                                                                             u.StartDate <= sinDate &&
                                                                                             u.EndDate >= sinDate);

            var listHoukatu = NoTrackingDataContext.DensiHoukatus.Where(u => u.HpId == hpId &&
                                                                  u.StartDate <= sinDate &&
                                                                  u.EndDate >= sinDate &&
                                                                  !_HoukatuTermExclude.Contains(u.HoukatuTerm));

            var tenMstQuery = NoTrackingDataContext.TenMsts.Where(e => e.StartDate <= sinDate && e.EndDate >= sinDate && e.IsDeleted == DeleteTypes.None);

            var query = from hokatuGrp in listHoukatuGrp
                        join hokatu in listHoukatu on hokatuGrp.HoukatuGrpNo equals hokatu.HoukatuGrpNo
                        join tenMst in tenMstQuery on hokatu.ItemCd equals tenMst.ItemCd into ListTenMst
                        from tenMstItem in ListTenMst.DefaultIfEmpty()
                        select new
                        {
                            HokatuGrp = hokatuGrp,
                            Hokatu = hokatu,
                            ItemName = tenMstItem.Name,
                        };

            result = query.AsEnumerable().Where(data => !string.IsNullOrEmpty(data.ItemName))
                                         .Select(x => new DensiHoukatuGrpModel(x.HokatuGrp.HpId,
                                                                              x.HokatuGrp.HoukatuGrpNo,
                                                                              x.HokatuGrp.ItemCd,
                                                                              x.HokatuGrp.SpJyoken,
                                                                              x.HokatuGrp.StartDate,
                                                                              x.HokatuGrp.EndDate,
                                                                              x.HokatuGrp.SeqNo,
                                                                              x.HokatuGrp.UserSetting,
                                                                              x.HokatuGrp.TargetKbn,
                                                                              x.HokatuGrp.IsInvalid,
                                                                              x.Hokatu.HoukatuTerm,
                                                                              x.ItemName,
                                                                              x.Hokatu.ItemCd,
                                                                              false,
                                                                              false))
                                         .OrderBy(u => u.HoukatuGrpNo)
                                         .ThenBy(u => u.HoukatuItemCd)
                                         .ThenBy(u => u.SeqNo)
                                         .ToList();
            return result;
        }

        public List<DensiHoukatuModel> GetListDensiHoukatuMaster(int hpId, List<string> listGrpNo)
        {
            List<DensiHoukatuModel> result = new List<DensiHoukatuModel>();
            var listHoukatu = NoTrackingDataContext.DensiHoukatus.Where(u => u.HpId == hpId &&
                                                                        u.HoukatuGrpNo != null && listGrpNo.Contains(u.HoukatuGrpNo));

            var tenMstQuery = NoTrackingDataContext.TenMsts.Where(e => e.HpId == hpId && e.IsDeleted == DeleteTypes.None);

            var query = from hokatu in listHoukatu
                        join tenMst in tenMstQuery on hokatu.ItemCd equals tenMst.ItemCd into ListTenMstLeft
                        from tenMst in ListTenMstLeft.Where(item => hokatu.StartDate <= item.StartDate && hokatu.EndDate >= item.EndDate).OrderByDescending(item => item.StartDate).Take(1)
                        select new
                        {
                            Hokatu = hokatu,
                            ItemName = tenMst.Name,
                        };
            result = query.AsEnumerable().Where(data => !string.IsNullOrEmpty(data.ItemName))
                                         .Select(x => new DensiHoukatuModel(x.Hokatu.HpId,
                                                                           x.Hokatu.ItemCd,
                                                                           x.Hokatu.StartDate,
                                                                           x.Hokatu.EndDate,
                                                                           x.Hokatu.TargetKbn,
                                                                           x.Hokatu.SeqNo,
                                                                           x.Hokatu.HoukatuTerm,
                                                                           x.Hokatu.HoukatuGrpNo ?? string.Empty,
                                                                           x.Hokatu.UserSetting,
                                                                           x.Hokatu.IsInvalid,
                                                                           x.ItemName,
                                                                           string.Empty,
                                                                           0,
                                                                           false,
                                                                           false))
                                         .GroupBy(x => new { x.ItemCd, x.StartDate })
                                         .Select(x => x.First()).ToList();

            return result;
        }

        public List<CombinedContraindicationModel> GetContraindicationModelList(int sinDate, string itemCd)
        {
            var kinkiQuery = NoTrackingDataContext.KinkiMsts.Where(item => item.ACd == itemCd);
            var itemMstQuery = NoTrackingDataContext.TenMsts.Where(item => item.StartDate <= sinDate && item.EndDate >= sinDate && item.IsDeleted == DeleteTypes.None);

            var query = from kinki in kinkiQuery
                        join tenMst in itemMstQuery on new { kinki.HpId, ItemCd = kinki.BCd }
                                                equals new { tenMst.HpId, tenMst.ItemCd } into tenMstLeft
                        from tMst in tenMstLeft.DefaultIfEmpty()
                        select new
                        {
                            Kinki = kinki,
                            TenMst = tMst ?? new TenMst()
                        };

            return query.AsEnumerable().Select(
                   data => new CombinedContraindicationModel(data.Kinki.HpId,
                                                             data.Kinki.ACd,
                                                             data.Kinki.BCd ?? string.Empty,
                                                             data.Kinki.SeqNo,
                                                             false,
                                                             data.TenMst?.Name ?? string.Empty,
                                                             false,
                                                             false,
                                                             data.Kinki.BCd ?? string.Empty)).ToList();
        }
    }
}
