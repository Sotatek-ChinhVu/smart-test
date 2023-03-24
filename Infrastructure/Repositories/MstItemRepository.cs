﻿using Amazon.Runtime.Internal.Transform;
using Domain.Constant;
using Domain.Models.FlowSheet;
using Domain.Models.MstItem;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MstItemRepository : RepositoryBase, IMstItemRepository
    {
        public MstItemRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

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
                tenMst?.Kokuji2 ?? string.Empty
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
                tenMst.Kokuji2 ?? string.Empty
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
        public (List<TenItemModel> tenItemModels, int totalCount) SearchTenMst(string keyword, int kouiKbn, int sinDate, int pageIndex, int pageCount, int genericOrSameItem, string yjCd, int hpId, double pointFrom, double pointTo, bool isRosai, bool isMirai, bool isExpired, string itemCodeStartWith, bool isMasterSearch, bool isSearch831SuffixOnly, bool isSearchSanteiItem, byte searchFollowUsage, List<int> kouiKbns)
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

            var queryJoinWithKensa = from q in queryFinal
                                     join k in kensaMstList
                                     on q.TenMst.KensaItemCd equals k.KensaItemCd into kensaMsts
                                     from kensaMst in kensaMsts.DefaultIfEmpty()
                                     select new { q.TenMst, q.tenKN, KensaMst = kensaMst };
            var totalCount = queryJoinWithKensa.Count(item => item.TenMst != null);

            var listTenMst = queryJoinWithKensa.Where(item => item.TenMst != null).OrderBy(item => item.TenMst.KanaName1).ThenBy(item => item.TenMst.Name).Skip((pageIndex - 1) * pageCount);
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
                                                           item.TenMst?.Kokuji2 ?? string.Empty
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
                                                           item.Kokuji2 ?? string.Empty
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
                    entity?.Kokuji2 ?? string.Empty
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
                    entity.Kokuji2 ?? string.Empty
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
                    entity.Kokuji2 ?? string.Empty
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
                    entity.Kokuji2 ?? string.Empty
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
                    mst.IsAdopted == 1
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
                        tenMst?.Kokuji2 ?? string.Empty
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
    }
}
