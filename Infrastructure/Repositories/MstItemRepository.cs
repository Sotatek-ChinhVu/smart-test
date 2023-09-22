﻿using Amazon.Runtime.Internal.Transform;
using Domain.Constant;
using Domain.Enum;
using Domain.Models.ContainerMaster;
using Domain.Models.FlowSheet;
using Domain.Models.KensaIrai;
using Domain.Models.MaterialMaster;
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
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using System.Text;
using KensaCenterMstModel = Domain.Models.MstItem.KensaCenterMstModel;

namespace Infrastructure.Repositories
{
    public class MstItemRepository : RepositoryBase, IMstItemRepository
    {

        private readonly AmazonS3Options _options;

        public MstItemRepository(ITenantProvider tenantProvider, IOptions<AmazonS3Options> optionsAccessor) : base(tenantProvider)
        {
            _options = optionsAccessor.Value;
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
            var query = from main in OtcMains.AsEnumerable()
                        join classcode in OtcClassCodes on main.ClassCd equals classcode.ClassCd into classLeft
                        from clas in classLeft.DefaultIfEmpty()
                        join makercode in OtcMakerCodes on main.CompanyCd equals makercode.MakerCd into makerLeft
                        from maker in makerLeft.DefaultIfEmpty()
                        join formcode in OtcFormCodes on main.DrugFormCd equals formcode.FormCd into formLeft
                        from form in formLeft.DefaultIfEmpty()
                        join usagecode in UsageCodes on main.YohoCd equals usagecode.YohoCd into usageLeft
                        from usage in usageLeft.DefaultIfEmpty()
                        where ((main.TradeKana ?? string.Empty).StartsWith(searchValue)
                                || (main.TradeName ?? string.Empty).Contains(searchValue)
                                || (maker.MakerKana ?? string.Empty).StartsWith(searchValue)
                                || (maker.MakerName ?? string.Empty).Contains(searchValue)
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

        public List<SearchSupplementModel> GetListSupplement(string searchValue)
        {
            searchValue = searchValue.Trim();

            var listSuppleIndexCode = NoTrackingDataContext.M41SuppleIndexcodes.AsQueryable();
            var listSuppleIndexDef = NoTrackingDataContext.M41SuppleIndexdefs.Where(u => string.IsNullOrEmpty(searchValue.Trim()) ? true : u.IndexWord.Contains(searchValue.Trim())).OrderBy(u => u.SeibunCd).ThenBy(u => u.IndexWord).ThenBy(u => u.TokuhoFlg);
            var listSuppleIngre = NoTrackingDataContext.M41SuppleIngres.AsQueryable();
            var indexDefJoinIngreQueryList = from indexCode in listSuppleIndexCode
                                             join ingre in listSuppleIngre on indexCode.SeibunCd equals ingre.SeibunCd into suppleIngreList
                                             from ingreItem in suppleIngreList.DefaultIfEmpty()
                                             select new
                                             {
                                                 IndexCode = indexCode,
                                                 Ingre = ingreItem,
                                             };

            var query = from indexDef in listSuppleIndexDef
                        join indexDefJoinIngreQuery in indexDefJoinIngreQueryList on indexDef.SeibunCd equals indexDefJoinIngreQuery.IndexCode.IndexCd into supplementList
                        from supplementItem in supplementList.DefaultIfEmpty()
                        select new
                        {
                            IndexDef = indexDef,
                            IndexCode = supplementItem.IndexCode,
                            Ingre = supplementItem.Ingre,
                        };

            var result = query
                  .AsEnumerable()
                  .Select(data => new SearchSupplementModel(data.Ingre.SeibunCd, data.Ingre.Seibun ?? string.Empty, data.IndexDef.IndexWord ?? string.Empty, data.IndexDef.TokuhoFlg ?? string.Empty, data.IndexCode.IndexCd, string.Empty))
                  .OrderBy(data => data.IndexWord)
                  .ThenBy(data => data.SeibunCd)
                  .ToList();

            return result;
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
                string.Empty,
                0,
                0,
                true
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
                string.Empty,
                0,
                0,
                true
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
        public (List<TenItemModel> tenItemModels, int totalCount) SearchTenMst(string keyword, int kouiKbn, int sinDate, int pageIndex, int pageCount, int genericOrSameItem, string yjCd, int hpId, double pointFrom, double pointTo, bool isRosai, bool isMirai, bool isExpired, string itemCodeStartWith, bool isMasterSearch, bool isSearch831SuffixOnly, bool isSearchSanteiItem, byte searchFollowUsage, bool isDeleted, List<int> kouiKbns, List<int> drugKbns, string masterSBT)
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

            string sSmallKeyword = kanaKeyword.ToUpper()
                                    .Replace("ｱ", "ｧ")
                                    .Replace("ｲ", "ｨ")
                                    .Replace("ｳ", "ｩ")
                                    .Replace("ｴ", "ｪ")
                                    .Replace("ｵ", "ｫ")
                                    .Replace("ﾔ", "ｬ")
                                    .Replace("ﾕ", "ｭ")
                                    .Replace("ﾖ", "ｮ")
                                    .Replace("ﾂ", "ｯ");

            var queryResult = NoTrackingDataContext.TenMsts.Where(t =>
                                t.ItemCd.StartsWith(keyword)
                                || (t.SanteiItemCd != null && t.SanteiItemCd.StartsWith(keyword))
                                || (t.KanaName1 != null && t.KanaName1 != "" && (t.KanaName1.StartsWith(sBigKeyword) || t.KanaName1.StartsWith(sSmallKeyword)))
                                ||
                                  (t.KanaName2 != null && t.KanaName2 != "" && (t.KanaName2.StartsWith(sBigKeyword) || t.KanaName2.StartsWith(sSmallKeyword)))

                                || (t.KanaName3 != null && t.KanaName3 != "" && (t.KanaName3.StartsWith(sBigKeyword) || t.KanaName3.StartsWith(sSmallKeyword)))
                                || (t.KanaName4 != null && t.KanaName4 != "" && (t.KanaName4.StartsWith(sBigKeyword) || t.KanaName4.StartsWith(sSmallKeyword)))
                                ||
                                (t.KanaName5 != null && t.KanaName5 != "" && (t.KanaName5.StartsWith(sBigKeyword) || t.KanaName5.StartsWith(sSmallKeyword)))
                                ||
                                (t.KanaName6 != null && t.KanaName6 != "" && (t.KanaName6.StartsWith(sBigKeyword) || t.KanaName6.StartsWith(sSmallKeyword)))
                                || (
                                  t.KanaName7 != null && t.KanaName7 != "" && (t.KanaName7.StartsWith(sBigKeyword) || t.KanaName7.StartsWith(sSmallKeyword)))
                                ||
                                (t.Name != null && t.Name != "" && t.Name.Contains(keyword)));

            if (masterSBT.ToLower() != "all")
            {
                queryResult = queryResult.Where(t => t.MasterSbt == masterSBT);
            }

            if (kouiKbns.Count == 0)
            {
                if (kouiKbn > 0)
                {
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

            if (drugKbns.Any())
            {
                queryResult = queryResult.Where(p => drugKbns.Contains(p.DrugKbn));
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

            if (!masterSBT.Equals("all"))
            {
                if (isDeleted)
                {
                    queryResult = queryResult.Where(t => t.IsDeleted == DeleteTypes.Deleted || t.IsDeleted == DeleteTypes.None);
                }
                else
                {
                    queryResult = queryResult.Where(t => t.IsDeleted == DeleteTypes.None);
                }
            }
            else
            {
                queryResult = queryResult.Where(t => t.IsDeleted == DeleteTypes.None);
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

            var ipnKasanExclude = NoTrackingDataContext.ipnKasanExcludes.Where(u => u.HpId == hpId && u.StartDate <= sinDate && u.EndDate >= sinDate);
            var ipnKasanExcludeItem = NoTrackingDataContext.ipnKasanExcludeItems.Where(u => u.HpId == hpId && u.StartDate <= sinDate && u.EndDate >= sinDate);

            var ipnMinYakka = NoTrackingDataContext.IpnMinYakkaMsts.Where(p =>
                                                                           p.HpId == hpId &&
                                                                           p.StartDate <= sinDate &&
                                                                           p.EndDate >= sinDate);

            var queryJoinWithKensaIpnName = from q in queryFinal
                                            join k in kensaMstList
                                            on q.TenMst.KensaItemCd equals k.KensaItemCd into kensaMsts
                                            from kensaMst in kensaMsts.DefaultIfEmpty()
                                            join i in ipnNameMstList
                                            on q.TenMst.IpnNameCd equals i.IpnNameCd into ipnNameMsts
                                            from ipnNameMst in ipnNameMsts.DefaultIfEmpty()
                                            join i in ipnKasanExclude on q.TenMst.IpnNameCd equals i.IpnNameCd into ipnExcludes
                                            from ipnExclude in ipnExcludes.DefaultIfEmpty()
                                            join ipnItem in ipnKasanExcludeItem on q.TenMst.ItemCd equals ipnItem.ItemCd into ipnExcludesItems
                                            from ipnExcludesItem in ipnExcludesItems.DefaultIfEmpty()
                                            join yakka in ipnMinYakka on q.TenMst.IpnNameCd equals yakka.IpnNameCd into ipnYakkas
                                            from ipnYakka in ipnYakkas.DefaultIfEmpty()
                                            select new
                                            {
                                                q.TenMst,
                                                q.tenKN,
                                                KensaMst = kensaMst,
                                                IpnName = ipnNameMst?.IpnName ?? string.Empty,
                                                IsGetYakkaPrice = ipnExcludes.FirstOrDefault() == null && ipnExcludesItems.FirstOrDefault() == null,
                                                Yakka = ipnYakkas.FirstOrDefault() == null ? 0 : ipnYakkas.FirstOrDefault()?.Yakka
                                            };
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
                                                           item.IpnName,
                                                           item.TenMst?.IsDeleted ?? 0,
                                                           item.TenMst?.HandanGrpKbn ?? 0,
                                                           item.KensaMst == null,
                                                           item.Yakka == null ? 0 : item.Yakka ?? 0,
                                                           item.IsGetYakkaPrice
                                                            )).ToList();
            }
            return (listTenMstModels, totalCount);
        }

        public (List<TenItemModel> tenItemModels, int totalCount) SearchTenMasterItem(int hpId, int pageIndex, int pageCount, string keyword, double? pointFrom, double? pointTo, int kouiKbn, int oriKouiKbn,
            List<int> kouiKbns, bool includeRosai, bool includeMisai, int sTDDate, string itemCodeStartWith, bool isIncludeUsage,
            bool onlyUsage, string yJCode, bool isMasterSearch, bool isExpiredSearchIfNoData, bool isAllowSearchDeletedItem,
            bool isExpired, bool isDeleted, List<int> drugKbns, bool isSearchSanteiItem, bool isSearchKenSaItem, List<ItemTypeEnums> itemFilter,
            bool isSearch831SuffixOnly, bool isSearchGazoDensibaitaiHozon, SortType sortType, FilterTenMstEnum sortCol)
        {
            string kanaKeyword = keyword;
            if (WanaKana.IsKana(keyword) && WanaKana.IsRomaji(keyword))
            {
                var inputKeyword = keyword;
                kanaKeyword = CIUtil.ToHalfsize(keyword);
                if (WanaKana.IsRomaji(kanaKeyword)) //If after convert to kana. type still is IsRomaji, back to base input keyword
                    kanaKeyword = inputKeyword;
            }

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

            var queryResult = NoTrackingDataContext.TenMsts
                    .Where(t =>
                        t.ItemCd.StartsWith(keyword)
                        || !string.IsNullOrEmpty(t.SanteiItemCd) && t.SanteiItemCd.StartsWith(keyword)
                        || !string.IsNullOrEmpty(t.KanaName1) && t.KanaName1.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || !string.IsNullOrEmpty(t.KanaName2) && t.KanaName2.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || !string.IsNullOrEmpty(t.KanaName3) && t.KanaName3.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || !string.IsNullOrEmpty(t.KanaName4) && t.KanaName4.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || !string.IsNullOrEmpty(t.KanaName5) && t.KanaName5.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || !string.IsNullOrEmpty(t.KanaName6) && t.KanaName6.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || !string.IsNullOrEmpty(t.KanaName7) && t.KanaName7.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || !string.IsNullOrEmpty(t.Name) && t.Name.Contains(keyword));

            if (isAllowSearchDeletedItem)
            {
                if (isDeleted)
                {
                    queryResult = queryResult.Where(t => t.IsDeleted == DeleteTypes.Deleted || t.IsDeleted == DeleteTypes.None);
                }
                else
                {
                    queryResult = queryResult.Where(t => t.IsDeleted == DeleteTypes.None);
                }
            }
            else
            {
                queryResult = queryResult.Where(t => t.IsDeleted == DeleteTypes.None);
            }
            if (!kouiKbns.Any())
            {
                switch (kouiKbn)
                {
                    case 11:
                        queryResult = queryResult.Where(t => new[] { 11, 99 }.Contains(t.SinKouiKbn) || new[] { 1, 3, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
                        break;
                    case 12:
                        queryResult = queryResult.Where(t => new[] { 12, 99 }.Contains(t.SinKouiKbn) || new[] { 1, 3, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
                        break;
                    case 13:
                        queryResult = queryResult.Where(t => new[] { 13, 99 }.Contains(t.SinKouiKbn) || new[] { 1, 3, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
                        break;
                    case 14:
                        queryResult = queryResult.Where(t => new[] { 14, 99 }.Contains(t.SinKouiKbn) || new[] { 1, 3, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
                        break;
                    case 21:
                    case 22:
                    case 23:
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.YohoKbn > 0 || new[] { 1, 3, 6 }.Contains(t.DrugKbn) || t.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu || t.ItemCd == ItemCdConst.Con_Refill);
                        break;
                    case 20:
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || (t.SinKouiKbn >= 20 && t.SinKouiKbn <= 29) || t.DrugKbn == 3 || t.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu || t.ItemCd == ItemCdConst.Con_Refill);
                        break;
                    case 24:
                    case 25:
                    case 26:
                    case 27:
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || (t.SinKouiKbn >= 20 && t.SinKouiKbn <= 29) || t.DrugKbn == 3);
                        break;
                    case 28:
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U" || t.ItemCd == ItemCdConst.Con_Refill);
                        break;
                    case 30:
                        if (oriKouiKbn == 28)
                        {
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || (t.SinKouiKbn >= 30 && t.SinKouiKbn <= 39) || t.MasterSbt == "T" || t.MasterSbt == "U" || new[] { 4, 6 }.Contains(t.DrugKbn) || t.ItemCd == ItemCdConst.Con_Refill);
                        }
                        else
                        {
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || (t.SinKouiKbn >= 30 && t.SinKouiKbn <= 39) || t.MasterSbt == "T" || t.MasterSbt == "U" || new[] { 4, 6 }.Contains(t.DrugKbn));
                        }
                        break;
                    case 31:
                    case 32:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                    case 37:
                    case 38:
                    case 39:
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || (t.SinKouiKbn >= 30 && t.SinKouiKbn <= 39) || t.MasterSbt == "T" || t.MasterSbt == "U" || new[] { 4, 6 }.Contains(t.DrugKbn));
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
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 40 && t.SinKouiKbn <= 49) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
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
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 50 && t.SinKouiKbn <= 59) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
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
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 60 && t.SinKouiKbn <= 69) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
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
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 70 && t.SinKouiKbn <= 79) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
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
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 80 && t.SinKouiKbn <= 89) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
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
            else
            {
                queryResult = queryResult.Where(t => kouiKbns.Contains(t.SinKouiKbn));
            }

            if (drugKbns.Any())
            {
                queryResult = queryResult.Where(p => drugKbns.Contains(p.DrugKbn));
            }


            if (!isIncludeUsage)
            {
                queryResult = queryResult.Where(t => t.YohoKbn == 0);
            }

            if (onlyUsage)
            {
                queryResult = queryResult.Where(t => t.YohoKbn != 0);
            }

            if (!includeRosai)
            {
                queryResult = queryResult.Where(t => t.RousaiKbn != 1);
            }

            if (!includeMisai)
            {
                queryResult = queryResult.Where(t => t.IsAdopted == 1);
            }

            if (pointFrom != null)
            {
                queryResult = queryResult.Where(t => t.Ten >= pointFrom);
            }

            if (pointTo != null)
            {
                queryResult = queryResult.Where(t => t.Ten <= pointTo);
            }

            //special item GazoDensibaitaiHozon SMAR-4289
            if (!isSearchGazoDensibaitaiHozon)
            {
                queryResult = queryResult.Where(t => t.ItemCd != "840000100");
            }

            var yakkaSyusaiMstList = NoTrackingDataContext.YakkaSyusaiMsts.AsQueryable();

            if (isExpiredSearchIfNoData)
            {
                if (!isExpired && sTDDate > 0)
                {
                    queryResult = queryResult.Where(t => t.StartDate <= sTDDate && t.EndDate >= sTDDate);
                    yakkaSyusaiMstList = yakkaSyusaiMstList.Where(t => t.StartDate <= sTDDate && t.EndDate >= sTDDate);
                }
            }
            else if (!isMasterSearch || !isExpired)
            {
                if (sTDDate > 0)
                {
                    queryResult = queryResult.Where(t => t.StartDate <= sTDDate && t.EndDate >= sTDDate);

                    yakkaSyusaiMstList = yakkaSyusaiMstList.Where(t => t.StartDate <= sTDDate && t.EndDate >= sTDDate);
                }
            }

            if (!string.IsNullOrEmpty(itemCodeStartWith))
            {
                queryResult = queryResult.Where(t => t.ItemCd.StartsWith(itemCodeStartWith));
            }

            if (!string.IsNullOrEmpty(yJCode))
            {
                queryResult = queryResult.Where(t => (t.YjCd != null) && t.YjCd.StartsWith(yJCode));
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
            // Search kensa item 
            if (isSearchKenSaItem)
            {
                queryResult = queryResult.Where(t => t.ItemCd.StartsWith("16") && t.ItemCd == t.SanteiItemCd &&
                                                     t.SinKouiKbn >= 60 && t.SinKouiKbn <= 69);
            }


            if (itemFilter.Any())
            {
                if (itemFilter.Count == 1)
                {
                    if (itemFilter.FirstOrDefault() == ItemTypeEnums.Tokuzai)
                    {
                        queryResult = queryResult.Where(t => t.ItemCd.StartsWith("7") && t.ItemCd.Length == 9);
                    }
                    else if (itemFilter.FirstOrDefault() == ItemTypeEnums.KensaItem)
                    {
                        queryResult = queryResult.Where(t => t.ItemCd.Length == 9 && t.SinKouiKbn == 61);
                    }
                }
                else
                {
                    // In case search item teikyo byomei in tenmst screen
                    queryResult = queryResult.Where(t => (itemFilter.Contains(ItemTypeEnums.Tokuzai) ? (t.ItemCd.StartsWith("7") && t.ItemCd.Length == 9) : false) ||
                                                         (itemFilter.Contains(ItemTypeEnums.Yakuzai) ? (t.ItemCd.StartsWith("6") && t.ItemCd.Length == 9) : false) ||
                                                         (itemFilter.Contains(ItemTypeEnums.ShinryoKoi) ? (t.ItemCd.StartsWith("1") && t.ItemCd.Length == 9) : false) ||
                                                         (itemFilter.Contains(ItemTypeEnums.JihiItem) ? (t.ItemCd.StartsWith("J")) : false) ||
                                                         (itemFilter.Contains(ItemTypeEnums.SpecificMedicalMeterialItem) ? (t.ItemCd.StartsWith("Z")) : false) ||
                                                         (itemFilter.Contains(ItemTypeEnums.CommentItem) ? (t.ItemCd.StartsWith("8") && t.ItemCd.Length == 9) : false) ||
                                                         (itemFilter.Contains(ItemTypeEnums.COCommentItem) ? (t.ItemCd.StartsWith("CO")) : false) ||
                                                         (itemFilter.Contains(ItemTypeEnums.Bui) ? (t.ItemCd.Length == 4) : false) ||
                                                         (itemFilter.Contains(ItemTypeEnums.KensaItem) ? (t.ItemCd.StartsWith("KN")) : false) ||
                                                         (itemFilter.Contains(ItemTypeEnums.Kogai) ? (t.ItemCd.Length >= 2 && t.ItemCd.StartsWith("K")) : false));
                }
            }

            // Query 点数 for KN% item
            var tenMstQuery = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                                       && item.StartDate <= sTDDate
                                                                                       && item.EndDate >= sTDDate);
            if (isAllowSearchDeletedItem)
            {
                if (isDeleted)
                {
                    tenMstQuery = tenMstQuery.Where(t => t.IsDeleted == DeleteTypes.Deleted || t.IsDeleted == DeleteTypes.None);
                }
                else
                {
                    tenMstQuery = tenMstQuery.Where(t => t.IsDeleted == DeleteTypes.None);
                }
            }
            else
            {
                tenMstQuery = tenMstQuery.Where(t => t.IsDeleted == DeleteTypes.None);
            }

            var tenMstQueryForGetlastDate = queryResult.AsNoTracking();
            if (isAllowSearchDeletedItem)
            {
                if (isDeleted)
                {
                    tenMstQueryForGetlastDate = tenMstQueryForGetlastDate.Where(t => t.IsDeleted == DeleteTypes.None || t.IsDeleted == DeleteTypes.Deleted);
                }
                else
                {
                    tenMstQueryForGetlastDate = tenMstQueryForGetlastDate.Where(t => t.IsDeleted == DeleteTypes.None);
                }
            }
            else
            {
                tenMstQueryForGetlastDate = tenMstQueryForGetlastDate.Where(t => t.IsDeleted == DeleteTypes.None);
            }
            var tenMstGetLastDateQuery = tenMstQueryForGetlastDate.Select(x => new { ItemCd = x.ItemCd, EndDate = x.EndDate })
                                                                  .GroupBy(x => x.ItemCd)
                                                                  .Select(x => new { ItemCd = x.Key, EndDate = x.Max(y => y.EndDate) })
                                                                  .ToList();

            var totalCount = queryResult.Count();

            var queryKNTensu = (from tenKN in queryResult
                                join ten in tenMstQuery on new { tenKN.SanteiItemCd } equals new { SanteiItemCd = ten.ItemCd }
                                where tenKN.ItemCd.StartsWith("KN")
                                select new { tenKN.ItemCd, ten.Ten }).ToList();

            //tenJoinYakkaSyusai thấy đang ko dùng đến nên comment tạm lại
            //var tenJoinYakkaSyusai = (from ten in queryResult
            //                          join yakkaSyusaiMstItem in yakkaSyusaiMstList
            //                          on new { ten.YakkaCd, ten.ItemCd } equals new { yakkaSyusaiMstItem.YakkaCd, yakkaSyusaiMstItem.ItemCd } into yakkaSyusaiMstItems
            //                          from yakkaSyusaiItem in yakkaSyusaiMstItems.DefaultIfEmpty()
            //                          select new { TenMst = ten, YakkaSyusaiItem = yakkaSyusaiItem }).ToList();

            var sinKouiCollection = new SinkouiCollection();

            var queryFinal = (from ten in queryResult.AsEnumerable()
                              join tenLastDate in tenMstGetLastDateQuery
                              on ten.ItemCd equals tenLastDate.ItemCd
                              join kouiKbnItem in sinKouiCollection
                              on ten.SinKouiKbn equals kouiKbnItem.SinKouiCd into tenKouiKbns
                              from tenKouiKbn in tenKouiKbns.DefaultIfEmpty()
                              join tenKN in queryKNTensu
                              on ten.ItemCd equals tenKN.ItemCd into tenKNLeft
                              from tenKN in tenKNLeft.DefaultIfEmpty()
                              select new
                              {
                                  TenMst = ten,
                                  KouiName = tenKouiKbn.SinkouiName,
                                  //ten.YakkaSyusaiItem,
                                  tenKN,
                                  LastEndDate = tenLastDate.EndDate
                              }).ToList();

            var kensaMstQuery = NoTrackingDataContext.KensaMsts.Where(x => x.HpId == hpId);

            var queryJoinWithKensa = from q in queryFinal
                                     join k in kensaMstQuery
                                     on q.TenMst.KensaItemCd equals k.KensaItemCd into kensaMsts
                                     from kensaMst in kensaMsts.DefaultIfEmpty()
                                     select new
                                     {
                                         q.TenMst,
                                         q.KouiName,
                                         //q.YakkaSyusaiItem, 
                                         q.tenKN,
                                         KensaMst = kensaMst,
                                         LastEndDate = q.LastEndDate
                                     };

            var ipnCdList = queryJoinWithKensa.Select(q => q.TenMst.IpnNameCd).ToList();
            var ipnNameMstList = NoTrackingDataContext.IpnNameMsts.Where(i => ipnCdList.Contains(i.IpnNameCd)).ToList();

            var ipnKasanExclude = NoTrackingDataContext.ipnKasanExcludes.Where(u =>
                                                                                u.HpId == hpId &&
                                                                                u.StartDate <= sTDDate &&
                                                                                u.EndDate >= sTDDate).ToList();
            var ipnKasanExcludeItem = NoTrackingDataContext.ipnKasanExcludeItems.Where(u =>
                                                                                        u.HpId == hpId &&
                                                                                        u.StartDate <= sTDDate &&
                                                                                        u.EndDate >= sTDDate).ToList();

            var ipnMinYakka = NoTrackingDataContext.IpnMinYakkaMsts.Where(p =>
                                                                           p.HpId == hpId &&
                                                                           p.StartDate <= sTDDate &&
                                                                           p.EndDate >= sTDDate).ToList();

            var joinedQuery = from q in queryJoinWithKensa
                              join i in ipnKasanExclude on q.TenMst.IpnNameCd equals i.IpnNameCd into ipnExcludes
                              from ipnExclude in ipnExcludes.DefaultIfEmpty()
                              join ipnItem in ipnKasanExcludeItem on q.TenMst.ItemCd equals ipnItem.ItemCd into ipnExcludesItems
                              from ipnExcludesItem in ipnExcludesItems.DefaultIfEmpty()
                              join yakka in ipnMinYakka on q.TenMst.IpnNameCd equals yakka.IpnNameCd into ipnYakkas
                              from ipnYakka in ipnYakkas.DefaultIfEmpty()
                              join i in ipnNameMstList on q.TenMst.IpnNameCd equals i.IpnNameCd into ipnNameMsts
                              from ipnNameMst in ipnNameMsts.DefaultIfEmpty()
                              select new
                              {
                                  q.TenMst,
                                  q.KouiName,
                                  //q.YakkaSyusaiItem,
                                  q.tenKN,
                                  q.KensaMst,
                                  IpnName = ipnNameMst?.IpnName ?? string.Empty,
                                  IsGetYakkaPrice = ipnExcludes.FirstOrDefault() == null && ipnExcludesItems.FirstOrDefault() == null,
                                  Yakka = ipnYakkas.FirstOrDefault() == null ? 0 : ipnYakkas.FirstOrDefault()?.Yakka
                              };

            var tenMstModels = joinedQuery.Select(item => new TenItemModel(
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
                                                           item.IpnName,
                                                           item.TenMst?.IsDeleted ?? 0,
                                                           item.TenMst?.HandanGrpKbn ?? 0,
                                                           item.KensaMst == null,
                                                           item.Yakka == null ? 0 : item.Yakka ?? 0,
                                                           item.IsGetYakkaPrice
                                                            )).ToList();

            if (itemFilter.Any() && itemFilter.Contains(ItemTypeEnums.Kogai))
            {
                tenMstModels = tenMstModels.Where(t => (t.ItemCd.Length >= 2 && t.ItemCd.StartsWith("K") && Char.IsDigit(t.ItemCd, 1)) || t.ItemCd.StartsWith("KN") || !t.ItemCd.StartsWith("K")).ToList();
                totalCount = tenMstModels.Count();
            }
            // Get Master search result
            if (isMasterSearch || isExpiredSearchIfNoData)
            {
                tenMstModels = tenMstModels.GroupBy(item => item.ItemCd, (key, group) => group.OrderByDescending(item => item.EndDate)?.FirstOrDefault() ?? new()).ToList();
                totalCount = tenMstModels.Count();
            }

            var orderedQuery = (sortCol, sortType) switch
            {
                (FilterTenMstEnum.RousaiKbn, SortType.Asc) => tenMstModels.OrderBy(item => item.RousaiKbnDisplay),
                (FilterTenMstEnum.RousaiKbn, SortType.Desc) => tenMstModels.OrderByDescending(item => item.RousaiKbnDisplay),
                (FilterTenMstEnum.KanaName1, SortType.Asc) => tenMstModels.OrderBy(item => item.KanaName1),
                (FilterTenMstEnum.KanaName1, SortType.Desc) => tenMstModels.OrderByDescending(item => item.KanaName1),
                (FilterTenMstEnum.KouiName, SortType.Asc) => tenMstModels.OrderBy(item => item.KouiName),
                (FilterTenMstEnum.KouiName, SortType.Desc) => tenMstModels.OrderByDescending(item => item.KouiName),
                (FilterTenMstEnum.Name, SortType.Asc) => tenMstModels.OrderBy(item => item.Name),
                (FilterTenMstEnum.Name, SortType.Desc) => tenMstModels.OrderByDescending(item => item.Name),
                (FilterTenMstEnum.KohatuKbn, SortType.Asc) => tenMstModels.OrderBy(item => item.KohatuKbnDisplay),
                (FilterTenMstEnum.KohatuKbn, SortType.Desc) => tenMstModels.OrderByDescending(item => item.KohatuKbnDisplay),
                (FilterTenMstEnum.KubunTo, SortType.Asc) => tenMstModels.OrderBy(item => item.KubunToDisplay),
                (FilterTenMstEnum.KubunTo, SortType.Desc) => tenMstModels.OrderByDescending(item => item.KouseisinKbnDisplay),
                (FilterTenMstEnum.KouseisinKbn, SortType.Asc) => tenMstModels.OrderBy(item => item.KouseisinKbnDisplay),
                (FilterTenMstEnum.KouseisinKbn, SortType.Desc) => tenMstModels.OrderByDescending(item => item.KubunToDisplay),
                (FilterTenMstEnum.Ten, SortType.Asc) => tenMstModels.OrderBy(item => item.Ten),
                (FilterTenMstEnum.Ten, SortType.Desc) => tenMstModels.OrderByDescending(item => item.Ten),
                (FilterTenMstEnum.OdrUnitName, SortType.Asc) => tenMstModels.OrderBy(item => item.OdrUnitName),
                (FilterTenMstEnum.OdrUnitName, SortType.Desc) => tenMstModels.OrderByDescending(item => item.OdrUnitName),
                (FilterTenMstEnum.ItemCd, SortType.Asc) => tenMstModels.OrderBy(item => item.ItemCd),
                (FilterTenMstEnum.ItemCd, SortType.Desc) => tenMstModels.OrderByDescending(item => item.ItemCd),
                (FilterTenMstEnum.KensaCenterItemCD, SortType.Asc) => tenMstModels.OrderBy(item => item.KensaCenterItemCDDisplay),
                (FilterTenMstEnum.KensaCenterItemCD, SortType.Desc) => tenMstModels.OrderByDescending(item => item.KensaCenterItemCDDisplay),
                (FilterTenMstEnum.EndDate, SortType.Asc) => tenMstModels.OrderBy(item => item.EndDate),
                (FilterTenMstEnum.EndDate, SortType.Desc) => tenMstModels.OrderByDescending(item => item.EndDate),
                (FilterTenMstEnum.IsDeleted, SortType.Asc) => tenMstModels.OrderBy(item => item.IsDeleted),
                (FilterTenMstEnum.IsDeleted, SortType.Desc) => tenMstModels.OrderByDescending(item => item.IsDeleted),
                _ => tenMstModels.OrderBy(item => item.KanaName1).ThenBy(item => item.Name)
            };

            tenMstModels = orderedQuery.Skip((pageIndex - 1) * pageCount)
                                           .Take(pageCount).ToList();

            return (tenMstModels, totalCount);
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
                                                           string.Empty,
                                                           0,
                                                           0,
                                                           true
                                                           )).ToList();
            }

            return tenMstModels;
        }

        public List<ByomeiMstModel> DiseaseSearch(bool isPrefix, bool isByomei, bool isSuffix, bool isMisaiyou, string keyword, int sindate, int pageIndex, int pageSize, bool isHasFreeByomei = true)
        {
            var keywordHalfSize = keyword != string.Empty ? CIUtil.ToHalfsize(keyword) : "";

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

            if (!isHasFreeByomei)
            {
                query = query.Where(item => item.ByomeiCd != "0000999");
            }

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
                    string.Empty,
                    0,
                    0,
                    true
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
                    string.Empty,
                    0,
                    0,
                    true
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
                    string.Empty,
                    entity.IsDeleted,
                    0,
                    true
               )).ToList();
        }

        public (int, List<PostCodeMstModel>) SearchAddress(int hpId, string postCode1, string postCode2, string address, int pageIndex, int pageSize)
        {
            var listPostCode = GetPostCodeMsts(hpId, postCode1, postCode2, address, pageIndex, pageSize);

            if (listPostCode.Item1 == 0)
            {
                while (address.Length > 3)
                {
                    address = address.Substring(0, address.Length - 3);

                    listPostCode = GetPostCodeMsts(hpId, postCode1, postCode2, address, pageIndex, pageSize);
                    if (listPostCode.Item1 > 0)
                    {
                        break;
                    }
                }
            }

            return (listPostCode.Item1, listPostCode.Item2);
        }

        private (int, List<PostCodeMstModel>) GetPostCodeMsts(int hpId, string postCode1, string postCode2, string address, int pageIndex, int pageSize)
        {
            var entities = NoTrackingDataContext.PostCodeMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0);

            if (!string.IsNullOrEmpty(postCode1) && !string.IsNullOrEmpty(postCode2))
            {
                entities = entities.Where(e => e.PostCd != null && e.PostCd.Contains(postCode1 + postCode2));
            }
            if (!string.IsNullOrEmpty(postCode1))
            {
                entities = entities.Where(e => e.PostCd != null && e.PostCd.StartsWith(postCode1));
            }
            if (!string.IsNullOrEmpty(postCode2))
            {
                entities = entities.Where(e => e.PostCd != null && e.PostCd.EndsWith(postCode2));
            }

            if (!string.IsNullOrEmpty(address))
            {
                entities = entities.Where(e => (e.PrefName + e.CityName + e.Banti).Contains(address)
                                                || (e.PrefName + e.CityName).Contains(address)
                                                || (e.PrefName != null && e.PrefName.Contains(address)));
            }

            var query = entities;

            if (!string.IsNullOrEmpty(address))
            {
                query = entities.Where(e =>
                    (e.PrefName + e.CityName + e.Banti).Contains(address) ||
                    (e.PrefName + e.CityName).Contains(address) ||
                    (e.PrefName != null && e.PrefName.Contains(address)));
            }

            var totalCount = query.Count();

            var result = new List<PostCodeMstModel>();
            if (pageIndex == -1 && pageSize == -1)
            {
                result = query.OrderBy(x => x.PostCd)
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
                                      x.IsDeleted)).ToList();
            }
            else
            {
                result = query.OrderBy(x => x.PostCd)
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
            }

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
                    result.Add(new ItemCmtModel(itemCd, entity.HpId, entity.SeqNo, entity.Cmt ?? string.Empty, entity.SortNo));
                }
            }
            return result;
        }

        public List<CommentCheckMstModel> GetAllCmtCheckMst(int hpId, int sinDay)
        {
            var cmtCheckMsts = NoTrackingDataContext.CmtCheckMsts.Where(
                    (x) => x.HpId == hpId && x.IsDeleted == 0);
            var tenMsts = NoTrackingDataContext.TenMsts.Where(
                    (x) => x.HpId == Session.HospitalID && x.StartDate <= sinDay && x.EndDate >= sinDay);

            var sinKouiCollection = new SinkouiCollection();

            var tenJoinYakkaSyusai = from cmtCheckMst in cmtCheckMsts
                                     join tenMst in tenMsts on
                                         new { cmtCheckMst.HpId, cmtCheckMst.ItemCd } equals
                                         new { tenMst.HpId, tenMst.ItemCd }
                                     select new
                                     {
                                         CmtCheckMst = cmtCheckMst,
                                         TenMstName = tenMst.Name,
                                         KanaName1 = tenMst.KanaName1,
                                         KanaName2 = tenMst.KanaName2,
                                         KanaName3 = tenMst.KanaName3,
                                         KohatuKbn = tenMst.KohatuKbn,
                                         ItemCd = tenMst.ItemCd,
                                         Ten = tenMst.Ten,
                                         TenId = tenMst.TenId,
                                         BuiKbn = tenMst.BuiKbn,
                                         SinKouiKbn = tenMst.SinKouiKbn,
                                         DrugKbn = tenMst.DrugKbn,
                                         MasterSbt = tenMst.MasterSbt
                                     };

            var queryFinal = from ten in tenJoinYakkaSyusai.AsEnumerable()
                             join kouiKbnItem in sinKouiCollection.AsEnumerable()
                             on ten.SinKouiKbn equals kouiKbnItem.SinKouiCd into tenKouiKbns
                             from tenKouiKbn in tenKouiKbns.DefaultIfEmpty()
                             select new
                             {
                                 CmtCheckMst = ten.CmtCheckMst,
                                 TenMstName = ten.TenMstName,
                                 KanaName1 = ten.KanaName1,
                                 KanaName2 = ten.KanaName2,
                                 KanaName3 = ten.KanaName3,
                                 KohatuKbn = ten.KohatuKbn,
                                 ItemCd = ten.ItemCd,
                                 Ten = ten.Ten,
                                 TenId = ten.TenId,
                                 BuiKbn = ten.BuiKbn,
                                 SinKouiKbn = ten.SinKouiKbn,
                                 KouiName = tenKouiKbn.SinkouiName,
                                 DrugKbn = ten.DrugKbn,
                                 MasterSbt = ten.MasterSbt
                             };

            var result = queryFinal.AsEnumerable()
                              .Select((x) => new CommentCheckMstModel(x.ItemCd, x.TenMstName, x.KanaName1, x.KanaName2, x.KanaName3,
                              x.KouiName, x.KohatuKbn, x.Ten, x.TenId)).DistinctBy(x => x.ItemCd)
                              .ToList();
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
                if (listCommentWithCode.Count == 0)
                {
                    inputCodeItem.SetData(listCommentWithCode);
                    continue;
                }

                if (!isRecalculation)
                {
                    var itemNo = listCommentWithCode[0].ItemNo;

                    listCommentWithCode.AddRange(listComment.Where(item =>
                            item.ItemCd == "199999999" && item.ItemNo == itemNo)
                        .ToList());
                }

                listCommentWithCode = listCommentWithCode.OrderBy(item => item.ItemNo)
                    .ThenBy(item => item.EdaNo)
                    .ThenBy(item => item.SortNo)
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

        private static ByomeiMstModel ConvertToByomeiMstModelInDeseaseNameMst(ByomeiMst mst)
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
                    mst.NanbyoCd,
                    mst.KanaName2 ?? string.Empty
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
                        string.Empty,
                        0,
                        0,
                        true
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
                        var checkConversionItem = TrackingDataContext.ConversionItemInfs.FirstOrDefault(c => c.HpId == hpId && c.SourceItemCd == value.Key && c.DestItemCd == tenItem.ItemCd);
                        if (checkConversionItem == null)
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
            }

            TrackingDataContext.SaveChanges();

            return true;
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
                                                                                   x.StartDate,
                                                                                   false)).ToList();
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
                var modelsToDelete = TrackingDataContext.KensaCenterMsts.AsEnumerable().Where(u => deletedModels.Any(d => d.HpId == u.HpId && d.Id == u.Id));
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
                                             model.StartDate,
                                             false);
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
                    .Select(x => new IpnMinYakkaMstModel(x.Id, x.HpId, x.IpnNameCd, x.StartDate, x.EndDate, x.Yakka, x.SeqNo, x.IsDeleted, false))
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

            result.Add(new DrugInfModel(description.HpId, description.ItemCd, description.InfKbn, description.SeqNo, description.DrugInfo ?? string.Empty, description.IsDeleted, false, description.DrugInfo ?? string.Empty));
            result.Add(new DrugInfModel(drugAction.HpId, drugAction.ItemCd, drugAction.InfKbn, drugAction.SeqNo, drugAction.DrugInfo ?? string.Empty, drugAction.IsDeleted, false, drugAction.DrugInfo ?? string.Empty));
            result.Add(new DrugInfModel(precautions.HpId, precautions.ItemCd, precautions.InfKbn, precautions.SeqNo, precautions.DrugInfo ?? string.Empty, precautions.IsDeleted, false, precautions.DrugInfo ?? string.Empty));
            return result;
        }


        public PiImageModel GetImagePiByItemCd(int hpId, string itemCd, int imageType)
        {
            List<string> folderPaths = new List<string>() { CommonConstants.Image, CommonConstants.Reference, CommonConstants.DrugPhoto };
            if (imageType == (int)ImageTypeDrug.HouImage)
            {
                folderPaths.Add(CommonConstants.HouSou);
            }
            else if (imageType == (int)ImageTypeDrug.ZaiImage)
            {
                folderPaths.Add(CommonConstants.ZaiKei);
            }
            string path = BuildPathAws(folderPaths);

            var pathFull = new StringBuilder();
            pathFull.Append(_options.BaseAccessUrl);
            pathFull.Append("/");
            pathFull.Append(path);

            var piImage = NoTrackingDataContext.PiImages.FirstOrDefault(u => u.HpId == hpId && u.ItemCd == itemCd && u.ImageType == imageType);
            if (piImage != null)
            {
                return new PiImageModel(piImage.HpId, piImage.ImageType, piImage.ItemCd, piImage.FileName ?? string.Empty, false, false, pathFull + piImage.FileName ?? string.Empty);
            }
            else
            {
                return new PiImageModel(0, itemCd, imageType);
            }
        }


        public List<TeikyoByomeiModel> GetTeikyoByomeiModel(int hpId, string itemCd, bool isFromCheckingView = false)
        {
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

            var result = query.AsEnumerable()
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
                                                                 x.ByomeiMst.Sbyomei ?? string.Empty,
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
                                                                           x.Hokatu.IsInvalid == 1 ? true : false,
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
            var result = query.AsEnumerable().Where(data => !string.IsNullOrEmpty(data.ItemName))
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
                                                                               x.Hokatu.IsInvalid == 1,
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
                   data => new CombinedContraindicationModel(data.Kinki.Id,
                                                             data.Kinki.HpId,
                                                             data.Kinki.ACd,
                                                             data.Kinki.BCd ?? string.Empty,
                                                             data.Kinki.SeqNo,
                                                             data.Kinki.IsDeleted == 1,
                                                             data.TenMst?.Name ?? string.Empty,
                                                             false,
                                                             false,
                                                             data.Kinki.BCd ?? string.Empty)).ToList();
        }

        public bool SaveTenMstOriginSetData(IEnumerable<CategoryItemEnums> tabActs, string itemCd, List<TenMstOriginModel> tenMstGrigins, SetDataTenMstOriginModel setDataTen, int userId, int hpId)
        {
            #region handler tenMstOrigins
            List<TenMst> tenMstDatabase = TrackingDataContext.TenMsts.Where(item => item.ItemCd == itemCd).ToList();
            tenMstGrigins.ForEach(x =>
            {
                if (x.IsAddNew && x.IsDeleted == DeleteTypes.None)
                {
                    TrackingDataContext.TenMsts.Add(Mapper.Map(x, new TenMst(), (src, dest) =>
                    {
                        dest.HpId = hpId;
                        dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                        dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        dest.CreateId = userId;
                        dest.UpdateId = userId;
                        dest.ItemCd = itemCd;
                        return dest;
                    }));

                    if (x.GetItemType() == ItemTypeEnums.SpecialMedicineCommentItem)
                    {
                        TrackingDataContext.CmtKbnMsts.Add(new CmtKbnMst()
                        {
                            HpId = hpId,
                            CmtKbn = 5,
                            StartDate = 0,
                            EndDate = 99999999,
                            ItemCd = itemCd,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId
                        });
                    }

                }
                else //Modified
                {
                    var updateTenMst = tenMstDatabase.FirstOrDefault(it => it.HpId == x.HpId && it.ItemCd == x.ItemCd && it.StartDate == x.StartDate);
                    if (updateTenMst != null)
                    {
                        Mapper.Map(x, updateTenMst, (src, dest) =>
                        {
                            dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            dest.UpdateId = userId;
                            return dest;
                        });
                    }
                }
            });
            #endregion

            foreach (CategoryItemEnums category in tabActs)
            {
                switch (category)
                {
                    case CategoryItemEnums.BasicSetting:
                        BasicSettingUpdate();
                        break;
                    case CategoryItemEnums.IjiSetting:
                        break;
                    case CategoryItemEnums.PrecriptionSetting:
                        PrecriptionSettingUpdate();
                        break;
                    case CategoryItemEnums.UsageSetting:
                        UsageSettingUpdate();
                        break;
                    case CategoryItemEnums.SpecialMaterialSetting:
                        break;
                    case CategoryItemEnums.DrugInfomation:
                        DrugInfomationUpdate();
                        break;
                    case CategoryItemEnums.TeikyoByomei:
                        TeikyoByomeiUpdate();
                        break;
                    case CategoryItemEnums.SanteiKaishu:
                        SanteiKaishuUpdate();
                        break;
                    case CategoryItemEnums.Haihan:
                        HaihanUpdate();
                        break;
                    case CategoryItemEnums.Houkatsu:
                        HoukatsuUpdate();
                        break;
                    case CategoryItemEnums.CombinedContraindication:
                        CombinedContraindicationUpdate();
                        break;
                    case CategoryItemEnums.RenkeiSetting:
                        break;
                }
            }

            return TrackingDataContext.SaveChanges() > 0;

            #region functions update tabs
            void BasicSettingUpdate()
            {
                List<CmtKbnMstModel> listSource = setDataTen.BasicSettingTab.CmtKbnMstModels.Where(u => !u.CheckDefaultValue()).ToList();
                if (!listSource.Any())
                    return;

                var databaseList = TrackingDataContext.CmtKbnMsts.Where(item => item.HpId == hpId && item.ItemCd == itemCd).ToList();

                listSource.ForEach(x =>
                {
                    if (x.Id == 0) //Create
                    {
                        TrackingDataContext.CmtKbnMsts.Add(new CmtKbnMst()
                        {
                            HpId = hpId,
                            ItemCd = itemCd,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            CmtKbn = x.CmtKbn,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId
                        });
                    }
                    else if (x.IsDeleted) //Delete
                    {
                        var removeModel = databaseList.FirstOrDefault(k => k.Id == x.Id);
                        if (removeModel != null)
                            TrackingDataContext.CmtKbnMsts.Remove(removeModel);
                    }
                    else //Update
                    {
                        var updateModel = databaseList.FirstOrDefault(k => k.Id == x.Id);
                        if (updateModel != null)
                        {
                            updateModel.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            updateModel.UpdateId = userId;
                            updateModel.CmtKbn = x.CmtKbn;
                            updateModel.StartDate = x.StartDate;
                            updateModel.EndDate = x.EndDate;
                        }
                    }
                });
            }

            void PrecriptionSettingUpdate()
            {
                #region UpdateDosageMstModel
                DosageMstModel model = setDataTen.PrecriptionSettingTab.DosageMst;
                if (model.ModelModified)
                {
                    if (model.Id == 0)
                    {
                        TrackingDataContext.DosageMsts.Add(new DosageMst()
                        {
                            HpId = hpId,
                            ItemCd = itemCd,
                            SeqNo = model.SeqNo,
                            OnceMin = model.OnceMin,
                            OnceMax = model.OnceMax,
                            OnceLimit = model.OnceLimit,
                            OnceUnit = model.OnceUnit,
                            DayMin = model.DayMin,
                            DayMax = model.DayMax,
                            DayLimit = model.DayLimit,
                            DayUnit = model.DayUnit,
                            IsDeleted = model.IsDeleted,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId
                        });
                    }
                    else
                    {
                        if (model.CheckDefaultValue())
                        {
                            var dosageMsts = TrackingDataContext.DosageMsts.Where(x =>
                                                x.HpId == Session.HospitalID &&
                                                x.IsDeleted == 0 &&
                                                x.ItemCd == model.ItemCd).ToList();

                            dosageMsts.ForEach(x =>
                            {
                                x.IsDeleted = DeleteTypes.Deleted;
                                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                x.UpdateId = userId;
                            });
                        }
                        else
                        {
                            var modelDb = TrackingDataContext.DosageMsts.FirstOrDefault(x =>
                                                x.HpId == hpId &&
                                                x.IsDeleted == 0 &&
                                                x.ItemCd == itemCd &&
                                                x.Id == model.Id);

                            if (modelDb != null)
                            {
                                Mapper.Map(model, modelDb, (src, dest) =>
                                {
                                    dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                    dest.UpdateId = userId;
                                    return dest;
                                });
                            }
                        }
                    }
                }
                #endregion

                #region UpdateIpnNameMstModel
                IpnNameMstModel ipnModel = setDataTen.PrecriptionSettingTab.IpnNameMst;
                if (ipnModel.ModelModified)
                {
                    var ipnDb = TrackingDataContext.IpnNameMsts.FirstOrDefault(x =>
                          x.HpId == hpId &&
                          x.IpnNameCd == ipnModel.IpnNameCd &&
                          x.StartDate == ipnModel.StartDate &&
                          x.EndDate == ipnModel.EndDate);

                    if (string.IsNullOrEmpty(ipnModel.IpnNameCd) && ipnDb == null)
                    {
                        if (!string.IsNullOrEmpty(ipnModel.IpnNameCdOrigin))
                        {
                            TrackingDataContext.IpnNameMsts.Add(new IpnNameMst()
                            {
                                IpnNameCd = ipnModel.IpnNameCdOrigin,
                                HpId = hpId,
                                StartDate = ipnModel.StartDate,
                                SeqNo = 1,
                                EndDate = 99999999,
                                IpnName = ipnModel.IpnName,
                                CreateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateId = userId,
                                IsDeleted = DeleteTypes.None,
                                UpdateDate = CIUtil.GetJapanDateTimeNow()
                            });
                        }
                    }
                    else
                    {
                        if (ipnDb != null)
                        {
                            Mapper.Map(ipnModel, ipnDb, (src, dest) =>
                            {
                                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                dest.UpdateId = userId;
                                return dest;
                            });
                        }
                    }
                }
                #endregion

                #region UpdateIpnMinYakkaMstModels
                #endregion
                List<IpnMinYakkaMstModel> srcIpnMins = setDataTen.PrecriptionSettingTab.IpnMinYakkaMsts;
                srcIpnMins.ForEach(x =>
                {
                    if (!x.CheckDefaultValue() && x.ModelModified)
                    {
                        if (x.Id == 0 && x.IsDeleted == 0)
                        {
                            TrackingDataContext.IpnMinYakkaMsts.Add(new IpnMinYakkaMst()
                            {
                                CreateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateId = userId,
                                IsDeleted = DeleteTypes.None,
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                EndDate = x.EndDate,
                                HpId = hpId,
                                IpnNameCd = x.IpnNameCd,
                                SeqNo = 1,
                                StartDate = x.StartDate,
                                Yakka = x.Yakka
                            });
                        }
                        else
                        {
                            var updateIpnYk = TrackingDataContext.IpnMinYakkaMsts.FirstOrDefault(ip => ip.Id == x.Id && x.HpId == ip.HpId);
                            if (updateIpnYk != null)
                            {
                                updateIpnYk.UpdateId = userId;
                                updateIpnYk.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                updateIpnYk.StartDate = x.StartDate;
                                updateIpnYk.EndDate = x.EndDate;
                                updateIpnYk.Yakka = x.Yakka;
                                updateIpnYk.IsDeleted = x.IsDeleted;
                            }
                        }
                    }
                });

                #region UpdateDrugDayLimitModels
                List<DrugDayLimitModel> srcDrugDays = setDataTen.PrecriptionSettingTab.DrugDayLimits;
                var dbDrugLimitList = TrackingDataContext.DrugDayLimits.Where(x => x.HpId == hpId && x.IsDeleted == 0 && x.ItemCd == itemCd).ToList();
                srcDrugDays.ForEach(x =>
                {
                    if (!x.CheckDefaultValue() && x.ModelModified)
                    {
                        if (x.Id == 0 && x.IsDeleted == 0)
                        {
                            TrackingDataContext.DrugDayLimits.Add(Mapper.Map(x, new DrugDayLimit(), (src, dest) =>
                            {
                                dest.CreateId = userId;
                                dest.ItemCd = itemCd;
                                dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                                dest.UpdateId = userId;
                                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                return dest;
                            }));
                        }
                        else
                        {
                            var updateDrudDay = dbDrugLimitList.FirstOrDefault(dr => dr.Id == x.Id);
                            if (updateDrudDay != null)
                            {
                                updateDrudDay.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                updateDrudDay.UpdateId = userId;
                                updateDrudDay.LimitDay = x.LimitDay;
                                updateDrudDay.StartDate = x.StartDate;
                                updateDrudDay.EndDate = x.EndDate;
                                updateDrudDay.IsDeleted = x.IsDeleted;
                            }
                        }
                    }
                });
                #endregion

                #region TeikyoByomeiTabModel


                #endregion
            }

            void UsageSettingUpdate()
            {
                var yohoDb = TrackingDataContext.YohoInfMsts.FirstOrDefault(x => x.ItemCd == itemCd);
                if (yohoDb != null)
                {
                    yohoDb.YohoSuffix = setDataTen.UsageSettingTab.YohoInfMstPrefix;
                    yohoDb.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    yohoDb.UpdateId = userId;
                }
                else if (!string.IsNullOrEmpty(setDataTen.UsageSettingTab.YohoInfMstPrefix))
                {
                    TrackingDataContext.YohoInfMsts.Add(new YohoInfMst()
                    {
                        HpId = hpId,
                        ItemCd = itemCd,
                        YohoSuffix = setDataTen.UsageSettingTab.YohoInfMstPrefix,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                    });
                }
            }

            void DrugInfomationUpdate()
            {
                List<DrugInfModel> drugModels = setDataTen.DrugInfomationTab.DrugInfs;
                for (int i = 1; i < 3; i++)
                {
                    if (drugModels.ElementAtOrDefault(i) != null && string.IsNullOrEmpty(drugModels[i].DrugInfo) && !string.IsNullOrEmpty(drugModels[i].OldDrugInfo))
                    {
                        drugModels[i].SetDrugInfo(drugModels[i].OldDrugInfo);
                        drugModels[i].SetIsDeleted(DeleteTypes.Deleted);
                    }
                }

                var listDrugInfoModel = drugModels.Where(u => !u.IsDefaultModel);
                List<DrugInfModel> addedDrugInfModel = listDrugInfoModel.Where(k => k.IsNewModel && !k.IsDefaultModel).ToList();
                List<DrugInfModel> updatedDrugInfModel = listDrugInfoModel.Where(k => k.IsModified).ToList();

                TrackingDataContext.AddRange(Mapper.Map<DrugInfModel, DrugInf>(addedDrugInfModel, (src, dest) =>
                {
                    dest.HpId = hpId;
                    dest.UpdateId = userId;
                    dest.CreateId = userId;
                    dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                    dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    return dest;
                }));

                updatedDrugInfModel.ForEach(x =>
                {
                    var drDb = TrackingDataContext.DrugInfs.FirstOrDefault(d => d.HpId == x.HpId && d.InfKbn == x.InfKbn && d.SeqNo == x.SeqNo && x.ItemCd == x.ItemCd);
                    if (drDb != null)
                        Mapper.Map(x, drDb, (src, dest) =>
                        {
                            dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            dest.UpdateId = userId;
                            return dest;
                        });
                });

                List<PiImageModel> listImages = new List<PiImageModel>() {
                    setDataTen.DrugInfomationTab.ZaiImage,
                    setDataTen.DrugInfomationTab.HouImage
                };

                var listPiImage = listImages.Where(u => !u.IsDefaultModel).ToList();
                if (listPiImage.Count == 0)
                    return;

                IEnumerable<PiImageModel> addedImageModel = listPiImage.Where(k => k.IsNewModel && !k.IsDefaultModel);
                TrackingDataContext.PiImages.AddRange(Mapper.Map<PiImageModel, PiImage>(addedImageModel, (src, dest) =>
                {
                    dest.HpId = hpId;
                    dest.UpdateId = userId;
                    dest.CreateId = userId;
                    dest.ItemCd = itemCd;
                    dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                    dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    return dest;
                }));

                var updatedImageModel = listPiImage.Where(k => k.IsModified).ToList();
                updatedImageModel.ForEach(x =>
                {
                    var upIm = TrackingDataContext.PiImages.FirstOrDefault(i => i.HpId == x.HpId && i.ImageType == x.ImageType && i.ItemCd == x.ItemCd);
                    if (upIm != null)
                    {
                        upIm.UpdateId = userId;
                        upIm.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        upIm.FileName = x.FileName;
                    }
                });
            }

            void TeikyoByomeiUpdate()
            {
                List<TeikyoByomeiModel> teikyoByomeiModels = setDataTen.TeikyoByomeiTab.TeikyoByomeis;
                foreach (var model in teikyoByomeiModels)
                {
                    if (model.CheckDefaultValue()) continue;
                    if (model.IsModified)
                    {
                        var entity = TrackingDataContext.TekiouByomeiMsts.FirstOrDefault(
                                                x => x.HpId == hpId &&
                                                     x.ItemCd == model.ItemCd &&
                                                     x.ByomeiCd == model.ByomeiCd &&
                                                     x.SystemData == model.SystemData);

                        if (entity != null)
                        {
                            if (!model.IsDeleted)
                            {
                                //	ユーザーが何らか変更した場合、TEKIOU_BYOMEI_MST.EDIT_KBNを1にする	
                                if (model.SystemData == 1)
                                {
                                    entity.EditKbn = 1;
                                }
                                entity.StartYM = model.StartYM;
                                entity.EndYM = model.EndYM;
                                entity.IsInvalid = model.IsInvalid;
                                entity.IsInvalidTokusyo = model.IsInvalidTokusyo;
                                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                entity.UpdateId = userId;
                            }
                            else
                            {
                                TrackingDataContext.TekiouByomeiMsts.Remove(entity);
                            }
                        }
                        else if (!model.IsDeleted)
                        {
                            TrackingDataContext.TekiouByomeiMsts.Add(Mapper.Map(model, new TekiouByomeiMst(), (src, dest) =>
                            {
                                dest.HpId = hpId;
                                dest.CreateId = userId;
                                dest.UpdateId = userId;
                                dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                return dest;
                            }));
                        }
                    }
                }

                var tekiouByomeiMstExcluded = setDataTen.TeikyoByomeiTab.TekiouByomeiMstExcluded;
                var entityTekiouByomeiMstExcluded = TrackingDataContext.TekiouByomeiMstExcludeds.FirstOrDefault(
                                                x => x.HpId == hpId &&
                                                     x.ItemCd == tekiouByomeiMstExcluded.ItemCd &&
                                                     x.SeqNo == tekiouByomeiMstExcluded.SeqNo);

                if (entityTekiouByomeiMstExcluded != null)
                {
                    if (tekiouByomeiMstExcluded.IsDeleted == DeleteTypes.Deleted)
                    {
                        entityTekiouByomeiMstExcluded.IsDeleted = DeleteTypes.Deleted;
                        entityTekiouByomeiMstExcluded.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        entityTekiouByomeiMstExcluded.UpdateId = userId;
                    }
                    else
                    {
                        entityTekiouByomeiMstExcluded.ItemCd = tekiouByomeiMstExcluded.ItemCd;
                        entityTekiouByomeiMstExcluded.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        entityTekiouByomeiMstExcluded.UpdateId = userId;
                    }
                }
                else
                {
                    TrackingDataContext.Add(new TekiouByomeiMstExcluded { HpId = hpId, CreateDate = CIUtil.GetJapanDateTimeNow(), CreateId = userId, UpdateDate = CIUtil.GetJapanDateTimeNow(), UpdateId = userId, ItemCd = tekiouByomeiMstExcluded.ItemCd });
                }

            }

            void SanteiKaishuUpdate()
            {
                List<DensiSanteiKaisuModel> models = setDataTen.SanteiKaishuTab.DensiSanteiKaisus;

                var databases = TrackingDataContext.DensiSanteiKaisus.Where(x => x.HpId == hpId &&
                                                                                x.ItemCd == itemCd).ToList();

                foreach (var model in models)
                {
                    if (model.Id == 0 && !model.IsDeleted)
                    {
                        TrackingDataContext.DensiSanteiKaisus.Add(Mapper.Map(model, new DensiSanteiKaisu(), (src, dest) =>
                        {
                            dest.HpId = hpId;
                            dest.CreateId = userId;
                            dest.UpdateId = userId;
                            dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                            dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            return dest;
                        }));
                    }
                    else
                    {
                        var modelInDb = databases.FirstOrDefault(x => x.Id == model.Id);
                        if (modelInDb != null)
                        {
                            if (model.IsDeleted)
                            {
                                if (model.UserSetting == 0 || model.UserSetting == 1)
                                {
                                    modelInDb.IsInvalid = 1;
                                }
                                else if (model.UserSetting == 2)
                                {
                                    TrackingDataContext.DensiSanteiKaisus.Remove(modelInDb);
                                }
                            }
                            else
                            {
                                Mapper.Map(model, modelInDb, (src, dest) =>
                                {
                                    dest.UpdateId = userId;
                                    dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                    return dest;
                                });
                            }
                        }
                    }
                }
            }

            void HaihanUpdate()
            {
                var source = new List<DensiHaihanModel>();
                source.AddRange(setDataTen.HaihanTab.DensiHaihanModel1s);
                source.AddRange(setDataTen.HaihanTab.DensiHaihanModel2s);
                source.AddRange(setDataTen.HaihanTab.DensiHaihanModel3s);

                foreach (var model in source)
                {
                    if (!model.CheckDefaultValue() && model.IsModified && model.IsValidValue())
                    {
                        model.SetEndDate(model.EndDate);
                        switch (model.ModelType)
                        {
                            case (int)HaiHanModelType.DENSI_HAIHAN_DAY:
                                CRUDDensiHaihanDay(model);
                                break;
                            case (int)HaiHanModelType.DENSI_HAIHAN_MONTH:
                                CRUDDensiHaihanMonth(model);
                                break;
                            case (int)HaiHanModelType.DENSI_HAIHAN_KARTE:
                                CRUDDensiHaihanKarte(model);
                                break;
                            case (int)HaiHanModelType.DENSI_HAIHAN_WEEK:
                                CRUDDensiHaihanWeek(model);
                                break;
                            case (int)HaiHanModelType.DENSI_HAIHAN_CUSTOM:
                                CRUDDensiHaihanCustom(model);
                                break;
                        }
                    }
                }

                void CRUDDensiHaihanDay(DensiHaihanModel model)
                {
                    ///・DENSI_HAIHA_*は、同一の項目組み合わせでITEM_CD1, ITEM_CD2が逆のレコードが必ず作成される
                    ///ただし、HAIHAN_KBNが1,2の場合、逆になる
                    ///HAIHAN_KBN = 3のものはどちらも3になる
                    DensiHaihanDay? originOppositionEntity = TrackingDataContext.DensiHaihanDays.FirstOrDefault(
                                        x => x.HpId == model.HpId &&
                                             x.ItemCd1 == (model.OriginItemCd2 == "" ? model.ItemCd2 : model.OriginItemCd2) &&
                                             x.ItemCd2 == model.ItemCd1 &&
                                             x.HaihanKbn == (
                                                model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1))
                                             );
                    if (originOppositionEntity != null)
                    {
                        if (model.OriginItemCd2 != "" && model.OriginItemCd2 != model.ItemCd2)
                        {
                            DensiHaihanDay densiHaihanDay = CreateDensiHaihanDay(model);
                            densiHaihanDay.CreateDate = CIUtil.GetJapanDateTimeNow();
                            densiHaihanDay.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            densiHaihanDay.CreateId = userId;
                            densiHaihanDay.UpdateId = userId;
                            densiHaihanDay.ItemCd1 = model.ItemCd2;
                            densiHaihanDay.ItemCd2 = model.ItemCd1;
                            densiHaihanDay.HaihanKbn = model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1);
                            TrackingDataContext.DensiHaihanDays.Add(densiHaihanDay);
                        }
                        else
                        {
                            originOppositionEntity.SpJyoken = model.SpJyoken;
                            originOppositionEntity.StartDate = model.StartDate;
                            originOppositionEntity.EndDate = model.EndDate;
                            originOppositionEntity.IsInvalid = model.IsInvalid;
                            originOppositionEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            originOppositionEntity.UpdateId = userId;
                        }
                        if (model.Id != 0 && model.IsDeleted || model.InitModelType != model.ModelType || model.OriginItemCd2 != "" && model.OriginItemCd2 != model.ItemCd2)
                        {
                            TrackingDataContext.DensiHaihanDays.Remove(originOppositionEntity);
                        }
                    }
                    else
                    {
                        if (model.ItemCd1 != model.ItemCd2)
                        {
                            DensiHaihanDay densiHaihanDay = CreateDensiHaihanDay(model);
                            densiHaihanDay.CreateDate = CIUtil.GetJapanDateTimeNow();
                            densiHaihanDay.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            densiHaihanDay.CreateId = userId;
                            densiHaihanDay.UpdateId = userId;
                            densiHaihanDay.ItemCd1 = model.ItemCd2;
                            densiHaihanDay.ItemCd2 = model.ItemCd1;
                            densiHaihanDay.HaihanKbn = model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1);
                            TrackingDataContext.DensiHaihanDays.Add(densiHaihanDay);
                        }
                    }
                    if (model.Id == 0 && !model.IsDeleted)
                    {
                        var create = CreateDensiHaihanDay(model);
                        create.CreateDate = CIUtil.GetJapanDateTimeNow();
                        create.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        create.CreateId = userId;
                        create.UpdateId = userId;
                        TrackingDataContext.DensiHaihanDays.Add(create);
                    }
                    else if (model.Id != 0 && !model.IsDeleted)
                    {
                        if (model.InitModelType != model.ModelType)
                        {
                            //In case the user updates the model type of the model
                            DeleteDensiHaihan(model);

                            var create = CreateDensiHaihanDay(model);
                            create.CreateDate = CIUtil.GetJapanDateTimeNow();
                            create.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            create.CreateId = userId;
                            create.UpdateId = userId;
                            TrackingDataContext.DensiHaihanDays.Add(create);
                        }
                        else
                        {
                            DensiHaihanDay? entity = TrackingDataContext.DensiHaihanDays.FirstOrDefault(
                                x => x.HpId == model.HpId &&
                                        x.ItemCd1 == model.ItemCd1 &&
                                        x.Id == model.Id);

                            if (entity != null)
                            {
                                entity.ItemCd2 = model.ItemCd2;
                                entity.HaihanKbn = model.HaihanKbn;
                                entity.SpJyoken = model.SpJyoken;
                                entity.StartDate = model.StartDate;
                                entity.EndDate = model.EndDate;
                                entity.IsInvalid = model.IsInvalid;
                                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                entity.UpdateId = userId;
                            }
                        }
                    }
                    else if (model.Id != 0 && model.IsDeleted)
                    {
                        DeleteDensiHaihan(model);
                    }
                }

                void CRUDDensiHaihanMonth(DensiHaihanModel model)
                {
                    ///・DENSI_HAIHA_*は、同一の項目組み合わせでITEM_CD1, ITEM_CD2が逆のレコードが必ず作成される
                    ///ただし、HAIHAN_KBNが1,2の場合、逆になる
                    ///HAIHAN_KBN = 3のものはどちらも3になる
                    DensiHaihanMonth? originOppositionEntity = TrackingDataContext.DensiHaihanMonths.FirstOrDefault(
                                        x => x.HpId == model.HpId &&
                                             x.ItemCd1 == (model.OriginItemCd2 == "" ? model.ItemCd2 : model.OriginItemCd2) &&
                                             x.ItemCd2 == model.ItemCd1 &&
                                             x.HaihanKbn == (
                                                model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1))
                                             );
                    if (originOppositionEntity != null)
                    {
                        if (model.OriginItemCd2 != "" && model.OriginItemCd2 != model.ItemCd2)
                        {
                            DensiHaihanMonth densiHaihanMonth = CreateDensiHaihanMonth(model);
                            densiHaihanMonth.CreateDate = CIUtil.GetJapanDateTimeNow();
                            densiHaihanMonth.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            densiHaihanMonth.CreateId = userId;
                            densiHaihanMonth.UpdateId = userId;
                            densiHaihanMonth.ItemCd1 = model.ItemCd2;
                            densiHaihanMonth.ItemCd2 = model.ItemCd1;
                            densiHaihanMonth.HaihanKbn = model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1);
                            TrackingDataContext.DensiHaihanMonths.Add(densiHaihanMonth);
                        }
                        else
                        {
                            originOppositionEntity.SpJyoken = model.SpJyoken;
                            originOppositionEntity.StartDate = model.StartDate;
                            originOppositionEntity.EndDate = model.EndDate;
                            originOppositionEntity.IsInvalid = model.IsInvalid;
                            originOppositionEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            originOppositionEntity.UpdateId = userId;
                        }
                        if (model.Id != 0 && model.IsDeleted || model.InitModelType != model.ModelType || model.OriginItemCd2 != "" && model.OriginItemCd2 != model.ItemCd2)
                        {
                            TrackingDataContext.DensiHaihanMonths.Remove(originOppositionEntity);
                        }
                    }
                    else
                    {
                        DensiHaihanMonth densiHaihanMonth = CreateDensiHaihanMonth(model);
                        densiHaihanMonth.CreateDate = CIUtil.GetJapanDateTimeNow();
                        densiHaihanMonth.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        densiHaihanMonth.CreateId = userId;
                        densiHaihanMonth.UpdateId = userId;
                        densiHaihanMonth.ItemCd1 = model.ItemCd2;
                        densiHaihanMonth.ItemCd2 = model.ItemCd1;
                        densiHaihanMonth.HaihanKbn = model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1);
                        TrackingDataContext.DensiHaihanMonths.Add(densiHaihanMonth);
                    }
                    if (model.Id == 0 && !model.IsDeleted)
                    {
                        var create = CreateDensiHaihanMonth(model);
                        create.CreateDate = CIUtil.GetJapanDateTimeNow();
                        create.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        create.CreateId = userId;
                        create.UpdateId = userId;
                        TrackingDataContext.DensiHaihanMonths.Add(create);
                    }
                    else if (model.Id != 0 && !model.IsDeleted)
                    {
                        if (model.InitModelType != model.ModelType)
                        {
                            DeleteDensiHaihan(model);

                            //In case the user updates the model type of the model
                            var create = CreateDensiHaihanMonth(model);
                            create.CreateDate = CIUtil.GetJapanDateTimeNow();
                            create.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            create.CreateId = userId;
                            create.UpdateId = userId;
                            TrackingDataContext.DensiHaihanMonths.Add(create);
                        }
                        else
                        {
                            DensiHaihanMonth? entity = TrackingDataContext.DensiHaihanMonths.FirstOrDefault(x => x.HpId == model.HpId &&
                                                                                                                 x.ItemCd1 == model.ItemCd1 &&
                                                                                                                 x.Id == model.Id);
                            if (entity != null)
                            {
                                //ApplyChange(model, entity);
                                entity.ItemCd2 = model.ItemCd2;
                                entity.SpJyoken = model.SpJyoken;
                                entity.StartDate = model.StartDate;
                                entity.EndDate = model.EndDate;
                                entity.IsInvalid = model.IsInvalid;
                                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                entity.UpdateId = userId;
                            }
                        }
                    }
                    else if (model.Id != 0 && model.IsDeleted)
                    {
                        DeleteDensiHaihan(model);
                    }
                }

                void CRUDDensiHaihanKarte(DensiHaihanModel model)
                {
                    ///・DENSI_HAIHA_*は、同一の項目組み合わせでITEM_CD1, ITEM_CD2が逆のレコードが必ず作成される
                    ///ただし、HAIHAN_KBNが1,2の場合、逆になる
                    ///HAIHAN_KBN = 3のものはどちらも3になる
                    DensiHaihanKarte? originOppositionEntity = TrackingDataContext.DensiHaihanKartes.FirstOrDefault(
                                        x => x.HpId == model.HpId &&
                                             x.ItemCd1 == (model.OriginItemCd2 == "" ? model.ItemCd2 : model.OriginItemCd2) &&
                                             x.ItemCd2 == model.ItemCd1 &&
                                             x.HaihanKbn == (
                                                model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1))
                                             );
                    if (originOppositionEntity != null)
                    {
                        if (model.OriginItemCd2 != "" && model.OriginItemCd2 != model.ItemCd2)
                        {
                            DensiHaihanKarte densiHaihanKarte = CreateDensiHaihanKarte(model);
                            densiHaihanKarte.CreateDate = CIUtil.GetJapanDateTimeNow();
                            densiHaihanKarte.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            densiHaihanKarte.CreateId = userId;
                            densiHaihanKarte.UpdateId = userId;
                            densiHaihanKarte.ItemCd1 = model.ItemCd2;
                            densiHaihanKarte.ItemCd2 = model.ItemCd1;
                            densiHaihanKarte.HaihanKbn = model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1);
                            TrackingDataContext.DensiHaihanKartes.Add(densiHaihanKarte);
                        }
                        else
                        {
                            originOppositionEntity.SpJyoken = model.SpJyoken;
                            originOppositionEntity.StartDate = model.StartDate;
                            originOppositionEntity.EndDate = model.EndDate;
                            originOppositionEntity.IsInvalid = model.IsInvalid;
                            originOppositionEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            originOppositionEntity.UpdateId = userId;
                        }
                        if (model.Id != 0 && model.IsDeleted || model.InitModelType != model.ModelType || model.OriginItemCd2 != "" && model.OriginItemCd2 != model.ItemCd2)
                        {
                            TrackingDataContext.DensiHaihanKartes.Remove(originOppositionEntity);
                        }
                    }
                    else
                    {
                        DensiHaihanKarte densiHaihanKarte = CreateDensiHaihanKarte(model);
                        densiHaihanKarte.CreateDate = CIUtil.GetJapanDateTimeNow();
                        densiHaihanKarte.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        densiHaihanKarte.CreateId = userId;
                        densiHaihanKarte.UpdateId = userId;
                        densiHaihanKarte.ItemCd1 = model.ItemCd2;
                        densiHaihanKarte.ItemCd2 = model.ItemCd1;
                        densiHaihanKarte.HaihanKbn = model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1);
                        TrackingDataContext.DensiHaihanKartes.Add(densiHaihanKarte);
                    }
                    if (model.Id == 0 && !model.IsDeleted)
                    {
                        var create = CreateDensiHaihanKarte(model);
                        create.CreateDate = CIUtil.GetJapanDateTimeNow();
                        create.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        create.CreateId = userId;
                        create.UpdateId = userId;
                        TrackingDataContext.DensiHaihanKartes.Add(create);
                    }
                    else if (model.Id != 0 && !model.IsDeleted)
                    {
                        if (model.InitModelType != model.ModelType)
                        {
                            DeleteDensiHaihan(model);

                            //In case the user updates the model type of the model
                            var create = CreateDensiHaihanKarte(model);
                            create.CreateDate = CIUtil.GetJapanDateTimeNow();
                            create.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            create.CreateId = userId;
                            create.UpdateId = userId;
                            TrackingDataContext.DensiHaihanKartes.Add(create);
                        }
                        else
                        {
                            DensiHaihanKarte? entity = TrackingDataContext.DensiHaihanKartes.FirstOrDefault(
                                x => x.HpId == model.HpId &&
                                        x.ItemCd1 == model.ItemCd1 &&
                                        x.Id == model.Id);

                            if (entity != null)
                            {
                                entity.ItemCd2 = model.ItemCd2;
                                entity.SpJyoken = model.SpJyoken;
                                entity.StartDate = model.StartDate;
                                entity.EndDate = model.EndDate;
                                entity.IsInvalid = model.IsInvalid;
                                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                entity.UpdateId = userId;
                            }
                        }
                    }
                    else if (model.Id != 0 && model.IsDeleted)
                    {
                        DeleteDensiHaihan(model);
                    }
                }

                void CRUDDensiHaihanWeek(DensiHaihanModel model)
                {
                    ///・DENSI_HAIHA_*は、同一の項目組み合わせでITEM_CD1, ITEM_CD2が逆のレコードが必ず作成される
                    ///ただし、HAIHAN_KBNが1,2の場合、逆になる
                    ///HAIHAN_KBN = 3のものはどちらも3になる
                    DensiHaihanWeek? originOppositionEntity = TrackingDataContext.DensiHaihanWeeks.FirstOrDefault(
                                        x => x.HpId == model.HpId &&
                                             x.ItemCd1 == (model.OriginItemCd2 == "" ? model.ItemCd2 : model.OriginItemCd2) &&
                                             x.ItemCd2 == model.ItemCd1 &&
                                             x.HaihanKbn == (
                                                model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1))
                                             );
                    if (originOppositionEntity != null)
                    {
                        if (model.OriginItemCd2 != "" && model.OriginItemCd2 != model.ItemCd2)
                        {
                            DensiHaihanWeek densiHaihanWeek = CreateDensiHaihanWeek(model);
                            densiHaihanWeek.CreateDate = CIUtil.GetJapanDateTimeNow();
                            densiHaihanWeek.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            densiHaihanWeek.CreateId = userId;
                            densiHaihanWeek.UpdateId = userId;
                            densiHaihanWeek.ItemCd1 = model.ItemCd2;
                            densiHaihanWeek.ItemCd2 = model.ItemCd1;
                            densiHaihanWeek.HaihanKbn = model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1);
                            TrackingDataContext.DensiHaihanWeeks.Add(densiHaihanWeek);
                        }
                        else
                        {
                            originOppositionEntity.SpJyoken = model.SpJyoken;
                            originOppositionEntity.StartDate = model.StartDate;
                            originOppositionEntity.EndDate = model.EndDate;
                            originOppositionEntity.IsInvalid = model.IsInvalid;
                            originOppositionEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            originOppositionEntity.UpdateId = userId;
                        }
                        if (model.Id != 0 && model.IsDeleted || model.InitModelType != model.ModelType || model.OriginItemCd2 != "" && model.OriginItemCd2 != model.ItemCd2)
                        {
                            TrackingDataContext.DensiHaihanWeeks.Remove(originOppositionEntity);
                        }
                    }
                    else
                    {
                        DensiHaihanWeek densiHaihanWeek = CreateDensiHaihanWeek(model);
                        densiHaihanWeek.CreateDate = CIUtil.GetJapanDateTimeNow();
                        densiHaihanWeek.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        densiHaihanWeek.CreateId = userId;
                        densiHaihanWeek.UpdateId = userId;
                        densiHaihanWeek.ItemCd1 = model.ItemCd2;
                        densiHaihanWeek.ItemCd2 = model.ItemCd1;
                        densiHaihanWeek.HaihanKbn = model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1);
                        TrackingDataContext.DensiHaihanWeeks.Add(densiHaihanWeek);
                    }
                    if (model.Id == 0 && !model.IsDeleted)
                    {
                        var create = CreateDensiHaihanWeek(model);
                        create.CreateDate = CIUtil.GetJapanDateTimeNow();
                        create.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        create.CreateId = userId;
                        create.UpdateId = userId;
                        TrackingDataContext.DensiHaihanWeeks.Add(create);
                    }
                    else if (model.Id != 0 && !model.IsDeleted)
                    {
                        if (model.InitModelType != model.ModelType)
                        {
                            DeleteDensiHaihan(model);

                            //In case the user updates the model type of the model
                            var create = CreateDensiHaihanWeek(model);
                            create.CreateDate = CIUtil.GetJapanDateTimeNow();
                            create.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            create.CreateId = userId;
                            create.UpdateId = userId;
                            TrackingDataContext.DensiHaihanWeeks.Add(create);
                        }
                        else
                        {
                            DensiHaihanWeek? entity = TrackingDataContext.DensiHaihanWeeks.FirstOrDefault(x => x.HpId == model.HpId &&
                                                                                                              x.ItemCd1 == model.ItemCd1 &&
                                                                                                              x.Id == model.Id);
                            if (entity != null)
                            {
                                entity.ItemCd2 = model.ItemCd2;
                                entity.SpJyoken = model.SpJyoken;
                                entity.StartDate = model.StartDate;
                                entity.EndDate = model.EndDate;
                                entity.IsInvalid = model.IsInvalid;
                                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                entity.UpdateId = userId;
                            }
                        }
                    }
                    else if (model.Id != 0 && model.IsDeleted)
                    {
                        DeleteDensiHaihan(model);
                    }
                }

                void CRUDDensiHaihanCustom(DensiHaihanModel model)
                {
                    ///・DENSI_HAIHA_*は、同一の項目組み合わせでITEM_CD1, ITEM_CD2が逆のレコードが必ず作成される
                    ///ただし、HAIHAN_KBNが1,2の場合、逆になる
                    ///HAIHAN_KBN = 3のものはどちらも3になる
                    DensiHaihanCustom? originOppositionEntity = TrackingDataContext.DensiHaihanCustoms.FirstOrDefault(
                                        x => x.HpId == model.HpId &&
                                             x.ItemCd1 == (model.OriginItemCd2 == "" ? model.ItemCd2 : model.OriginItemCd2) &&
                                             x.ItemCd2 == model.ItemCd1 &&
                                             x.HaihanKbn == (
                                                model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1))
                                             );

                    if (originOppositionEntity != null)
                    {
                        if (model.OriginItemCd2 != "" && model.OriginItemCd2 != model.ItemCd2)
                        {
                            DensiHaihanCustom densiHaihanCustom = CreateDensiHaihanCustom(model);
                            densiHaihanCustom.CreateDate = CIUtil.GetJapanDateTimeNow();
                            densiHaihanCustom.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            densiHaihanCustom.CreateId = userId;
                            densiHaihanCustom.UpdateId = userId;
                            densiHaihanCustom.ItemCd1 = model.ItemCd2;
                            densiHaihanCustom.ItemCd2 = model.ItemCd1;
                            densiHaihanCustom.HaihanKbn = model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1);
                            TrackingDataContext.DensiHaihanCustoms.Add(densiHaihanCustom);
                        }
                        else
                        {
                            originOppositionEntity.SpJyoken = model.SpJyoken;
                            originOppositionEntity.StartDate = model.StartDate;
                            originOppositionEntity.EndDate = model.EndDate;
                            originOppositionEntity.TermCnt = model.TermCnt;
                            originOppositionEntity.TermSbt = model.TermSbt;
                            originOppositionEntity.IsInvalid = model.IsInvalid;
                            originOppositionEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            originOppositionEntity.UpdateId = userId;
                        }
                        if (model.Id != 0 && model.IsDeleted || model.InitModelType != model.ModelType || model.OriginItemCd2 != "" && model.OriginItemCd2 != model.ItemCd2)
                        {
                            TrackingDataContext.DensiHaihanCustoms.Remove(originOppositionEntity);
                        }
                    }
                    else
                    {
                        DensiHaihanCustom densiHaihanCustom = CreateDensiHaihanCustom(model);
                        densiHaihanCustom.CreateDate = CIUtil.GetJapanDateTimeNow();
                        densiHaihanCustom.CreateId = userId;
                        densiHaihanCustom.ItemCd1 = model.ItemCd2;
                        densiHaihanCustom.ItemCd2 = model.ItemCd1;
                        densiHaihanCustom.HaihanKbn = model.HaihanKbn == 3 ? 3 : (model.HaihanKbn == 1 ? 2 : 1);
                        TrackingDataContext.DensiHaihanCustoms.Add(densiHaihanCustom);
                    }
                    if (model.Id == 0 && !model.IsDeleted)
                    {
                        var create = CreateDensiHaihanCustom(model);
                        create.CreateDate = CIUtil.GetJapanDateTimeNow();
                        create.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        create.CreateId = userId;
                        create.UpdateId = userId;
                        TrackingDataContext.DensiHaihanCustoms.Add(create);
                    }
                    else if (model.Id != 0 && !model.IsDeleted)
                    {
                        if (model.InitModelType != model.ModelType)
                        {
                            DeleteDensiHaihan(model);

                            //In case the user updates the model type of the model
                            var create = CreateDensiHaihanCustom(model);
                            create.CreateDate = CIUtil.GetJapanDateTimeNow();
                            create.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            create.CreateId = userId;
                            create.UpdateId = userId;
                            TrackingDataContext.DensiHaihanCustoms.Add(create);
                        }
                        else
                        {
                            DensiHaihanCustom? entity = TrackingDataContext.DensiHaihanCustoms.FirstOrDefault(x => x.HpId == model.HpId &&
                                                                                                                    x.ItemCd1 == model.ItemCd1 &&
                                                                                                                    x.Id == model.Id);
                            if (entity != null)
                            {
                                entity.ItemCd2 = model.ItemCd2;
                                entity.SpJyoken = model.SpJyoken;
                                entity.StartDate = model.StartDate;
                                entity.EndDate = model.EndDate;
                                entity.IsInvalid = model.IsInvalid;
                                entity.TermCnt = model.TermCnt;
                                entity.TermSbt = model.TermSbt;
                                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                entity.UpdateId = userId;
                            }
                        }
                    }
                    else if (model.Id != 0 && model.IsDeleted)
                    {
                        DeleteDensiHaihan(model);
                    }
                }

                DensiHaihanDay CreateDensiHaihanDay(DensiHaihanModel x) => Mapper.Map(x, new DensiHaihanDay());

                DensiHaihanKarte CreateDensiHaihanKarte(DensiHaihanModel x) => Mapper.Map(x, new DensiHaihanKarte());

                DensiHaihanMonth CreateDensiHaihanMonth(DensiHaihanModel x) => Mapper.Map(x, new DensiHaihanMonth());

                DensiHaihanWeek CreateDensiHaihanWeek(DensiHaihanModel x) => Mapper.Map(x, new DensiHaihanWeek());

                DensiHaihanCustom CreateDensiHaihanCustom(DensiHaihanModel x) => Mapper.Map(x, new DensiHaihanCustom());

                void DeleteDensiHaihan(DensiHaihanModel model)
                {
                    switch (model.InitModelType)
                    {
                        case (int)HaiHanModelType.DENSI_HAIHAN_DAY:
                            {
                                DensiHaihanDay? entity = TrackingDataContext.DensiHaihanDays.FirstOrDefault(x => x.HpId == model.HpId &&
                                                                                                                 x.ItemCd1 == model.ItemCd1 &&
                                                                                                                 x.Id == model.Id);
                                if (entity != null)
                                    TrackingDataContext.DensiHaihanDays.Remove(entity);

                                DensiHaihanDay? originOppositionEntity = TrackingDataContext.DensiHaihanDays.FirstOrDefault(x => x.HpId == model.HpId &&
                                                                                                                                 x.ItemCd1 == model.OriginItemCd2 &&
                                                                                                                                 x.ItemCd2 == model.ItemCd1);

                                if (originOppositionEntity != null)
                                {
                                    TrackingDataContext.DensiHaihanDays.Remove(originOppositionEntity);
                                }
                                break;
                            }
                        case (int)HaiHanModelType.DENSI_HAIHAN_MONTH:
                            {
                                DensiHaihanMonth? entity = TrackingDataContext.DensiHaihanMonths.FirstOrDefault(x => x.HpId == model.HpId &&
                                                                                                                     x.ItemCd1 == model.ItemCd1 &&
                                                                                                                     x.Id == model.Id);

                                if (entity != null)
                                    TrackingDataContext.DensiHaihanMonths.Remove(entity);

                                DensiHaihanMonth? originOppositionEntity = TrackingDataContext.DensiHaihanMonths.FirstOrDefault(x => x.HpId == model.HpId &&
                                                                                                                                     x.ItemCd1 == model.OriginItemCd2 &&
                                                                                                                                     x.ItemCd2 == model.ItemCd1);
                                if (originOppositionEntity != null)
                                {
                                    TrackingDataContext.DensiHaihanMonths.Add(originOppositionEntity);
                                }
                                break;
                            }
                        case (int)HaiHanModelType.DENSI_HAIHAN_KARTE:
                            {
                                DensiHaihanKarte? entity = TrackingDataContext.DensiHaihanKartes.FirstOrDefault(x => x.HpId == model.HpId &&
                                                                                                                     x.ItemCd1 == model.ItemCd1 &&
                                                                                                                     x.Id == model.Id);

                                if (entity != null)
                                    TrackingDataContext.DensiHaihanKartes.Remove(entity);

                                DensiHaihanKarte? originOppositionEntity = TrackingDataContext.DensiHaihanKartes.FirstOrDefault(x => x.HpId == model.HpId &&
                                                                                                                                     x.ItemCd1 == model.OriginItemCd2 &&
                                                                                                                                     x.ItemCd2 == model.ItemCd1);

                                if (originOppositionEntity != null)
                                {
                                    TrackingDataContext.DensiHaihanKartes.Remove(originOppositionEntity);
                                }
                                break;
                            }
                        case (int)HaiHanModelType.DENSI_HAIHAN_WEEK:
                            {
                                DensiHaihanWeek? entity = TrackingDataContext.DensiHaihanWeeks.FirstOrDefault(x => x.HpId == model.HpId &&
                                                                                                                   x.ItemCd1 == model.ItemCd1 &&
                                                                                                                   x.Id == model.Id);

                                if (entity != null)
                                    TrackingDataContext.DensiHaihanWeeks.Remove(entity);

                                DensiHaihanWeek? originOppositionEntity = TrackingDataContext.DensiHaihanWeeks.FirstOrDefault(x => x.HpId == model.HpId &&
                                                                                                                                   x.ItemCd1 == model.OriginItemCd2 &&
                                                                                                                                   x.ItemCd2 == model.ItemCd1);
                                if (originOppositionEntity != null)
                                {
                                    TrackingDataContext.DensiHaihanWeeks.Remove(originOppositionEntity);
                                }
                                break;
                            }
                        case (int)HaiHanModelType.DENSI_HAIHAN_CUSTOM:
                            {
                                DensiHaihanCustom? entity = TrackingDataContext.DensiHaihanCustoms.FirstOrDefault(x => x.HpId == model.HpId &&
                                                                                                                       x.ItemCd1 == model.ItemCd1 &&
                                                                                                                       x.Id == model.Id);
                                if (entity != null)
                                    TrackingDataContext.DensiHaihanCustoms.Remove(entity);

                                DensiHaihanCustom? originOppositionEntity = TrackingDataContext.DensiHaihanCustoms.FirstOrDefault(x => x.HpId == model.HpId &&
                                                                                                                                       x.ItemCd1 == model.OriginItemCd2 &&
                                                                                                                                       x.ItemCd2 == model.ItemCd1);
                                if (originOppositionEntity != null)
                                {
                                    TrackingDataContext.DensiHaihanCustoms.Remove(originOppositionEntity);
                                }
                                break;
                            }
                    }
                }
            }

            void HoukatsuUpdate()
            {
                List<DensiHoukatuGrpModel> listDensiHoukatuGrp = setDataTen.HoukatsuTab.ListDensiHoukatuGrpModels.Where(x => !x.CheckDefaultValue() && x.IsUpdate).ToList();
                List<DensiHoukatuModel> listDensiHoukatu = setDataTen.HoukatsuTab.ListDensiHoukatuModels.Where(x => !x.CheckDefaultValue() && x.IsModified).ToList();
                List<string> listHoukatuGrpItemCd = listDensiHoukatu.GroupBy(x => x.HoukatuGrpItemCd).Select(x => x.Key).ToList();

                List<DensiHoukatuModel> listDensiHoukatuToUpdate = new List<DensiHoukatuModel>();
                foreach (var item in listHoukatuGrpItemCd)
                {
                    var listGrpItem = listDensiHoukatu.Where(x => x.HoukatuGrpItemCd == item).ToList();
                    int invalidCount = listGrpItem.Count(x => x.IsInvalidBinding);
                    int isvalidCount = listGrpItem.Count(x => !x.IsInvalidBinding);
                    if (invalidCount > isvalidCount)
                    {
                        var invalidItem = listGrpItem.FirstOrDefault(x => x.IsInvalidBinding);
                        if (invalidItem != null)
                        {
                            invalidItem.SetIsInvalid(1);
                            listDensiHoukatuToUpdate.Add(invalidItem);
                        }
                    }
                    else
                    {
                        var isvalidItem = listGrpItem.FirstOrDefault(x => !x.IsInvalidBinding);
                        if (isvalidItem != null)
                        {
                            isvalidItem.SetIsInvalid(0);
                            listDensiHoukatuToUpdate.Add(isvalidItem);
                        }
                    }
                }

                listDensiHoukatuGrp.ForEach(x =>
                {
                    DensiHoukatuGrp? update = TrackingDataContext.DensiHoukatuGrps.FirstOrDefault(f => f.SeqNo == x.SeqNo && f.HpId == x.HpId && f.HoukatuGrpNo == x.HoukatuGrpNo && f.ItemCd == x.ItemCd && f.StartDate == x.StartDate && f.UserSetting == x.UserSetting);
                    if (update != null)
                    {
                        update.SpJyoken = x.SpJyoken;
                        update.EndDate = x.EndDate;
                        update.TargetKbn = x.TargetKbn;
                        update.IsInvalid = x.IsInvalid;
                        update.UpdateId = userId;
                        update.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }
                });

                listDensiHoukatuToUpdate.ForEach(x =>
                {
                    DensiHoukatuGrp? update = TrackingDataContext.DensiHoukatuGrps.FirstOrDefault(f => f.SeqNo == x.SeqNo && f.HpId == x.HpId && f.HoukatuGrpNo == x.HoukatuGrpNo && f.ItemCd == x.ItemCd && f.StartDate == x.StartDate && f.UserSetting == x.UserSetting);
                    if (update != null)
                    {
                        update.SpJyoken = x.SpJyoken;
                        update.EndDate = x.EndDate;
                        update.TargetKbn = x.TargetKbn;
                        update.IsInvalid = x.IsInvalid;
                        update.UpdateId = userId;
                        update.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }
                });
            }

            void CombinedContraindicationUpdate()
            {
                List<CombinedContraindicationModel> source = setDataTen.CombinedContraindicationTab.CombinedContraindications;
                var kinkiQueryDb = TrackingDataContext.KinkiMsts.Where(item => item.ACd == itemCd).ToList();
                foreach (CombinedContraindicationModel model in source)
                {
                    if (model.CheckDefaultValue())
                    {
                        continue;
                    }
                    if (model.IsAddNew)
                    {
                        TrackingDataContext.KinkiMsts.Add(new KinkiMst()
                        {
                            HpId = hpId,
                            ACd = model.ACd,
                            BCd = model.BCd,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow()
                        });
                        continue;
                    }
                    if (model.IsUpdated)
                    {
                        var update = kinkiQueryDb.FirstOrDefault(x => x.Id == model.Id);
                        if (update != null)
                        {
                            update.UpdateId = userId;
                            update.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            update.BCd = model.BCd;
                            update.IsDeleted = model.IsDeleted ? 1 : 0;
                        }
                    }
                }
            }
            #endregion
        }

        public RenkeiMstModel GetRenkeiMst(int hpId, int renkeiId)
        {
            var renkei = NoTrackingDataContext.RenkeiMsts.FirstOrDefault(item => item.HpId == hpId && item.RenkeiId == renkeiId);
            if (renkei != null)
                return new RenkeiMstModel(renkei.HpId, renkei.RenkeiId, renkei.RenkeiName ?? string.Empty, renkei.RenkeiSbt, renkei.FunctionType, renkei.IsInvalid, renkei.SortNo);
            return ObjectExtension.CreateInstance<RenkeiMstModel>();
        }

        public bool IsTenMstUsed(int hpId, string itemCd, int startDate, int endDate)
        {
            return NoTrackingDataContext.OdrInfDetails.FirstOrDefault(
                x => x.HpId == hpId &&
                     x.ItemCd == itemCd &&
                     x.SinDate >= startDate &&
                     x.SinDate <= endDate) != null;
        }

        public List<JihiSbtMstModel> GetJihiSbtMstList(int hpId)
        {
            List<JihiSbtMstModel> result = new();
            result = NoTrackingDataContext.JihiSbtMsts
                .Where(item => item.IsDeleted == 0
                                                && item.HpId == hpId)
                .OrderBy(i => i.SortNo)
                .AsEnumerable().Select(i => new JihiSbtMstModel(i.HpId, i.JihiSbt, i.SortNo, i.Name ?? string.Empty, i.IsDeleted)).ToList();
            return result;
        }

        public List<TenMstMaintenanceModel> GetTenMstListByItemType(int hpId, ItemTypeEnums itemType, string startWithstr, int sinDate)
        {
            string GetJibaiItemType(TenMst tenmst)
            {
                switch (tenmst.SyukeiSaki)
                {
                    case "ZZ0":
                        return "診断書料";
                    case "ZZ1":
                        return "明細書料";
                    case "A18":
                        return "その他";
                }
                return string.Empty;
            }

            List<TenMstMaintenanceModel> result = new List<TenMstMaintenanceModel>();

            IQueryable<TenMst> listTenMst = NoTrackingDataContext.TenMsts.Where(item => item.ItemCd.StartsWith(startWithstr) &&
                                                                                item.StartDate <= sinDate &&
                                                                                item.EndDate >= sinDate &&
                                                                                item.IsDeleted == DeleteTypes.None);
            if (itemType == ItemTypeEnums.JihiItem)
            {
                var listJihiSbtMst = NoTrackingDataContext.JihiSbtMsts.Where(j => j.HpId == hpId &&
                                                                                   j.IsDeleted == DeleteStatus.None)
                                                                                   .Select(r => new
                                                                                   {
                                                                                       JihiSbt = r.JihiSbt,
                                                                                       Name = r.Name
                                                                                   });
                var queryTenMst = from tenMst in listTenMst
                                  join jihiSbtMst in listJihiSbtMst on
                                  tenMst.JihiSbt equals jihiSbtMst.JihiSbt into listJihiSbt
                                  from jihiSbt in listJihiSbt.DefaultIfEmpty()
                                  select new
                                  {
                                      TenMst = tenMst,
                                      JihiMst = jihiSbt
                                  };

                result = queryTenMst.AsEnumerable()
                          .Select(x => new TenMstMaintenanceModel(x.TenMst.HpId,
                                                                  x.TenMst.ItemCd,
                                                                  x.TenMst.StartDate,
                                                                  x.TenMst.EndDate,
                                                                  x.TenMst.MasterSbt ?? string.Empty,
                                                                  x.TenMst.SinKouiKbn,
                                                                  x.TenMst.Name ?? string.Empty,
                                                                  x.TenMst.KanaName1 ?? string.Empty,
                                                                  x.TenMst.KanaName2 ?? string.Empty,
                                                                  x.TenMst.KanaName3 ?? string.Empty,
                                                                  x.TenMst.KanaName4 ?? string.Empty,
                                                                  x.TenMst.KanaName5 ?? string.Empty,
                                                                  x.TenMst.KanaName6 ?? string.Empty,
                                                                  x.TenMst.KanaName7 ?? string.Empty,
                                                                  x.TenMst.RyosyuName ?? string.Empty,
                                                                  x.TenMst.ReceName ?? string.Empty,
                                                                  x.TenMst.TenId,
                                                                  x.TenMst.Ten,
                                                                  x.TenMst.ReceUnitCd ?? string.Empty,
                                                                  x.TenMst.ReceUnitName ?? string.Empty,
                                                                  x.TenMst.OdrUnitName ?? string.Empty,
                                                                  x.TenMst.CnvUnitName ?? string.Empty,
                                                                  x.TenMst.OdrTermVal,
                                                                  x.TenMst.CnvTermVal,
                                                                  x.TenMst.DefaultVal,
                                                                  x.TenMst.IsAdopted,
                                                                  x.TenMst.KoukiKbn,
                                                                  x.TenMst.SanteiItemCd ?? string.Empty,
                                                                  x.JihiMst == null ? string.Empty : x.JihiMst.Name))
                          .GroupBy(item => item.ItemCd, (key, group) => group.OrderByDescending(item => item.EndDate).First())
                          .OrderBy(item => item.KanaName1)
                          .ThenBy(item => item.Name)
                          .ToList();
            }
            else
            {
                // Check kikin_mst code for display CombinedContraindicationItem
                if (string.IsNullOrEmpty(startWithstr) && itemType == ItemTypeEnums.CombinedContraindicationItem)
                {
                    IQueryable<KinkiMst> kinki = NoTrackingDataContext.KinkiMsts.Where(item => item.HpId == hpId && item.IsDeleted == DeleteStatus.None);
                    listTenMst = listTenMst.Where(item => kinki.Any(k => k.ACd == item.ItemCd));
                    if (!listTenMst.Any())
                    {
                        return new List<TenMstMaintenanceModel>();
                    }
                }

                var sinKouiCollection = new SinkouiCollection();

                var query = from ten in listTenMst.AsEnumerable()
                            join kouiKbn in sinKouiCollection.AsEnumerable()
                                 on ten.SinKouiKbn equals kouiKbn.SinKouiCd into tenKouiKbns
                            from tenKouiKbn in tenKouiKbns.DefaultIfEmpty()
                            select new { TenMst = ten, KouiName = tenKouiKbn.SinkouiName };

                result = query.AsEnumerable()
                              .Select(x => new TenMstMaintenanceModel(x.TenMst.HpId,
                                                                      x.TenMst.ItemCd,
                                                                      x.TenMst.StartDate,
                                                                      x.TenMst.EndDate,
                                                                      x.TenMst.MasterSbt ?? string.Empty,
                                                                      x.TenMst.SinKouiKbn,
                                                                      x.TenMst.Name ?? string.Empty,
                                                                      x.TenMst.KanaName1 ?? string.Empty,
                                                                      x.TenMst.KanaName2 ?? string.Empty,
                                                                      x.TenMst.KanaName3 ?? string.Empty,
                                                                      x.TenMst.KanaName4 ?? string.Empty,
                                                                      x.TenMst.KanaName5 ?? string.Empty,
                                                                      x.TenMst.KanaName6 ?? string.Empty,
                                                                      x.TenMst.KanaName7 ?? string.Empty,
                                                                      x.TenMst.RyosyuName ?? string.Empty,
                                                                      x.TenMst.ReceName ?? string.Empty,
                                                                      x.TenMst.TenId,
                                                                      x.TenMst.Ten,
                                                                      x.TenMst.ReceUnitCd ?? string.Empty,
                                                                      x.TenMst.ReceUnitName ?? string.Empty,
                                                                      x.TenMst.OdrUnitName ?? string.Empty,
                                                                      x.TenMst.CnvUnitName ?? string.Empty,
                                                                      x.TenMst.OdrTermVal,
                                                                      x.TenMst.CnvTermVal,
                                                                      x.TenMst.DefaultVal,
                                                                      x.TenMst.IsAdopted,
                                                                      x.TenMst.KoukiKbn,
                                                                      x.TenMst.SanteiItemCd ?? string.Empty,
                                                                      itemType == ItemTypeEnums.Jibaiseki ? GetJibaiItemType(x.TenMst) : x.KouiName))
                              .GroupBy(item => item.ItemCd, (key, group) => group.OrderByDescending(item => item.EndDate).First())
                              .OrderBy(item => item.KanaName1)
                              .ThenBy(item => item.Name)
                              .ToList();
            }
            return result;
        }

        public (List<TenItemModel> tenItemModels, int totalCount) SearchSuggestionTenMstItem(int hpId, int pageIndex, int pageCount, string keyword, int kouiKbn, int oriKouiKbn, List<int> kouiKbns, bool includeMisai, bool includeRosai, int sTDDate, string itemCodeStartWith, bool isIncludeUsage, bool isDeleted, List<int> drugKbns, List<ItemTypeEnums> itemFilter, bool isSearch831SuffixOnly)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return new();
            }

            string kanaKeyword = keyword;
            if (WanaKana.IsKana(keyword) && WanaKana.IsRomaji(keyword))
            {
                var inputKeyword = keyword;
                kanaKeyword = CIUtil.ToHalfsize(keyword);
                if (WanaKana.IsRomaji(kanaKeyword)) //If after convert to kana. type still is IsRomaji, back to base input keyword
                    kanaKeyword = inputKeyword;
            }

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
            var tenMstModels = new List<TenItemModel>();

            var yakkaSyusaiMstList = NoTrackingDataContext.YakkaSyusaiMsts.AsQueryable();

            var queryResult = NoTrackingDataContext.TenMsts
                   .Where(t =>
                       t.ItemCd.StartsWith(keyword)
                       || !string.IsNullOrEmpty(t.SanteiItemCd) && t.SanteiItemCd.StartsWith(keyword)
                       || !string.IsNullOrEmpty(t.KanaName1) && t.KanaName1.ToUpper()
                         .Replace("ｧ", "ｱ")
                         .Replace("ｨ", "ｲ")
                         .Replace("ｩ", "ｳ")
                         .Replace("ｪ", "ｴ")
                         .Replace("ｫ", "ｵ")
                         .Replace("ｬ", "ﾔ")
                         .Replace("ｭ", "ﾕ")
                         .Replace("ｮ", "ﾖ")
                         .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                       || !string.IsNullOrEmpty(t.KanaName2) && t.KanaName2.ToUpper()
                         .Replace("ｧ", "ｱ")
                         .Replace("ｨ", "ｲ")
                         .Replace("ｩ", "ｳ")
                         .Replace("ｪ", "ｴ")
                         .Replace("ｫ", "ｵ")
                         .Replace("ｬ", "ﾔ")
                         .Replace("ｭ", "ﾕ")
                         .Replace("ｮ", "ﾖ")
                         .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                       || !string.IsNullOrEmpty(t.KanaName3) && t.KanaName3.ToUpper()
                         .Replace("ｧ", "ｱ")
                         .Replace("ｨ", "ｲ")
                         .Replace("ｩ", "ｳ")
                         .Replace("ｪ", "ｴ")
                         .Replace("ｫ", "ｵ")
                         .Replace("ｬ", "ﾔ")
                         .Replace("ｭ", "ﾕ")
                         .Replace("ｮ", "ﾖ")
                         .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                       || !string.IsNullOrEmpty(t.KanaName4) && t.KanaName4.ToUpper()
                         .Replace("ｧ", "ｱ")
                         .Replace("ｨ", "ｲ")
                         .Replace("ｩ", "ｳ")
                         .Replace("ｪ", "ｴ")
                         .Replace("ｫ", "ｵ")
                         .Replace("ｬ", "ﾔ")
                         .Replace("ｭ", "ﾕ")
                         .Replace("ｮ", "ﾖ")
                         .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                       || !string.IsNullOrEmpty(t.KanaName5) && t.KanaName5.ToUpper()
                         .Replace("ｧ", "ｱ")
                         .Replace("ｨ", "ｲ")
                         .Replace("ｩ", "ｳ")
                         .Replace("ｪ", "ｴ")
                         .Replace("ｫ", "ｵ")
                         .Replace("ｬ", "ﾔ")
                         .Replace("ｭ", "ﾕ")
                         .Replace("ｮ", "ﾖ")
                         .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                       || !string.IsNullOrEmpty(t.KanaName6) && t.KanaName6.ToUpper()
                         .Replace("ｧ", "ｱ")
                         .Replace("ｨ", "ｲ")
                         .Replace("ｩ", "ｳ")
                         .Replace("ｪ", "ｴ")
                         .Replace("ｫ", "ｵ")
                         .Replace("ｬ", "ﾔ")
                         .Replace("ｭ", "ﾕ")
                         .Replace("ｮ", "ﾖ")
                         .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                       || !string.IsNullOrEmpty(t.KanaName7) && t.KanaName7.ToUpper()
                         .Replace("ｧ", "ｱ")
                         .Replace("ｨ", "ｲ")
                         .Replace("ｩ", "ｳ")
                         .Replace("ｪ", "ｴ")
                         .Replace("ｫ", "ｵ")
                         .Replace("ｬ", "ﾔ")
                         .Replace("ｭ", "ﾕ")
                         .Replace("ｮ", "ﾖ")
                         .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                       || !string.IsNullOrEmpty(t.Name) && t.Name.Contains(keyword));

            if (kouiKbn > 0)
            {
                switch (kouiKbn)
                {
                    case 11:
                        queryResult = queryResult.Where(t => new[] { 11, 99 }.Contains(t.SinKouiKbn) || new[] { 1, 3, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
                        break;
                    case 12:
                        queryResult = queryResult.Where(t => new[] { 12, 99 }.Contains(t.SinKouiKbn) || new[] { 1, 3, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
                        break;
                    case 13:
                        queryResult = queryResult.Where(t => new[] { 13, 99 }.Contains(t.SinKouiKbn) || new[] { 1, 3, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
                        break;
                    case 14:
                        queryResult = queryResult.Where(t => new[] { 14, 99 }.Contains(t.SinKouiKbn) || new[] { 1, 3, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
                        break;
                    case 21:
                    case 22:
                    case 23:
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.YohoKbn > 0 || new[] { 1, 3, 6 }.Contains(t.DrugKbn) || t.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu || t.ItemCd == ItemCdConst.Con_Refill);
                        break;
                    case 20:
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || (t.SinKouiKbn >= 20 && t.SinKouiKbn <= 29) || t.DrugKbn == 3 || t.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu || t.ItemCd == ItemCdConst.Con_Refill);
                        break;
                    case 24:
                    case 25:
                    case 26:
                    case 27:
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || (t.SinKouiKbn >= 20 && t.SinKouiKbn <= 29) || t.DrugKbn == 3);
                        break;
                    case 28:
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U" || t.ItemCd == ItemCdConst.Con_Refill);
                        break;
                    case 30:
                        if (oriKouiKbn == 28)
                        {
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || (t.SinKouiKbn >= 30 && t.SinKouiKbn <= 39) || t.MasterSbt == "T" || t.MasterSbt == "U" || new[] { 4, 6 }.Contains(t.DrugKbn) || t.ItemCd == ItemCdConst.Con_Refill);
                        }
                        else
                        {
                            queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || (t.SinKouiKbn >= 30 && t.SinKouiKbn <= 39) || t.MasterSbt == "T" || t.MasterSbt == "U" || new[] { 4, 6 }.Contains(t.DrugKbn));
                        }
                        break;
                    case 31:
                    case 32:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                    case 37:
                    case 38:
                    case 39:
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || (t.SinKouiKbn >= 30 && t.SinKouiKbn <= 39) || t.MasterSbt == "T" || t.MasterSbt == "U" || new[] { 4, 6 }.Contains(t.DrugKbn));
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
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 40 && t.SinKouiKbn <= 49) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
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
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 50 && t.SinKouiKbn <= 59) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
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
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 60 && t.SinKouiKbn <= 69) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
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
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 70 && t.SinKouiKbn <= 79) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
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
                        queryResult = queryResult.Where(t => t.SinKouiKbn == 99 || t.BuiKbn > 0 || (t.SinKouiKbn >= 80 && t.SinKouiKbn <= 89) || new[] { 1, 4, 6 }.Contains(t.DrugKbn) || t.MasterSbt == "T" || t.MasterSbt == "U");
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

            if (drugKbns.Any())
            {
                queryResult = queryResult.Where(p => drugKbns.Contains(p.DrugKbn));
            }

            if (!isIncludeUsage)
            {
                queryResult = queryResult.Where(t => t.YohoKbn == 0);
            }

            if (sTDDate > 0)
            {
                queryResult = queryResult.Where(t => t.StartDate <= sTDDate && t.EndDate >= sTDDate);

                yakkaSyusaiMstList = yakkaSyusaiMstList.Where(t => t.StartDate <= sTDDate && t.EndDate >= sTDDate);
            }
            else
            {
                queryResult = queryResult.GroupBy(item => item.ItemCd, (key, group) => group.OrderByDescending(item => item.EndDate).FirstOrDefault() ?? new TenMst());
            }

            if (!string.IsNullOrEmpty(itemCodeStartWith))
            {
                queryResult = queryResult.Where(t => t.ItemCd.StartsWith(itemCodeStartWith));
            }

            if (itemFilter.Any())
            {
                queryResult = queryResult.Where(t => (itemFilter.Contains(ItemTypeEnums.CommentItem) ? (t.ItemCd.StartsWith("8") && t.ItemCd.Length == 9) : false) ||
                                                         (itemFilter.Contains(ItemTypeEnums.COCommentItem) ? (t.ItemCd.StartsWith("CO")) : false));
            }

            if (!includeMisai)
            {
                queryResult = queryResult.Where(t => t.IsAdopted == 1);
            }

            if (!includeRosai)
            {
                queryResult = queryResult.Where(t => t.RousaiKbn != 1);
            }

            queryResult = queryResult.Where(t => t.IsNosearch == 0);

            var tenJoinYakkaSyusai = (from ten in queryResult
                                      join yakkaSyusaiMstItem in yakkaSyusaiMstList
                                      on new { ten.HpId, ten.YakkaCd, ten.ItemCd, ten.StartDate }
                                      equals new { yakkaSyusaiMstItem.HpId, yakkaSyusaiMstItem.YakkaCd, yakkaSyusaiMstItem.ItemCd, yakkaSyusaiMstItem.StartDate }
                                      into yakkaSyusaiMstItems
                                      from yakkaSyusaiItem in yakkaSyusaiMstItems.DefaultIfEmpty()
                                      select new { TenMst = ten, YakkaSyusaiItem = yakkaSyusaiItem }).ToList();

            var queryKNTensu = (from tenKN in queryResult
                                join ten in queryResult on new { tenKN.SanteiItemCd } equals new { SanteiItemCd = ten.ItemCd }
                                where tenKN.ItemCd.StartsWith("KN")
                                select new { tenKN.ItemCd, ten.Ten }).ToList();

            var ipnKasanExclude = NoTrackingDataContext.ipnKasanExcludes.Where(u => u.HpId == hpId && u.StartDate <= sTDDate && u.EndDate >= sTDDate);
            var ipnKasanExcludeItem = NoTrackingDataContext.ipnKasanExcludeItems.Where(u => u.HpId == hpId && u.StartDate <= sTDDate && u.EndDate >= sTDDate);

            var ipnMinYakka = NoTrackingDataContext.IpnMinYakkaMsts.Where(p =>
                                                                           p.HpId == hpId &&
                                                                           p.StartDate <= sTDDate &&
                                                                           p.EndDate >= sTDDate).ToList();
            var sinKouiCollection = new SinkouiCollection();

            var queryFinal = (from ten in tenJoinYakkaSyusai
                              join kouiKbnItem in sinKouiCollection
                              on ten.TenMst.SinKouiKbn equals kouiKbnItem.SinKouiCd into tenKouiKbns
                              from tenKouiKbn in tenKouiKbns.DefaultIfEmpty()
                              join kensa in NoTrackingDataContext.KensaMsts
                              on ten.TenMst.KensaItemCd equals kensa.KensaItemCd into kensaMsts
                              from kensaMst in kensaMsts.DefaultIfEmpty()
                              join tenKN in queryKNTensu
                              on ten.TenMst.ItemCd equals tenKN.ItemCd into tenKNLeft
                              from tenKN in tenKNLeft.DefaultIfEmpty()
                              select new
                              {
                                  TenMst = ten.TenMst,
                                  KouiName = tenKouiKbn.SinkouiName,
                                  ten.YakkaSyusaiItem,
                                  KensaMst = kensaMst,
                                  TenKN = tenKN
                              }).ToList();

            var ipnCdList = queryFinal.Select(q => q.TenMst.IpnNameCd).ToList();
            var ipnNameMstList = NoTrackingDataContext.IpnNameMsts.Where(i => ipnCdList.Contains(i.IpnNameCd)).ToList();

            var joinedQuery = from q in queryFinal
                              join i in ipnKasanExclude on q.TenMst.IpnNameCd equals i.IpnNameCd into ipnExcludes
                              from ipnExclude in ipnExcludes.DefaultIfEmpty()
                              join ipnItem in ipnKasanExcludeItem on q.TenMst.ItemCd equals ipnItem.ItemCd into ipnExcludesItems
                              from ipnExcludesItem in ipnExcludesItems.DefaultIfEmpty()
                              join yakka in ipnMinYakka on q.TenMst.IpnNameCd equals yakka.IpnNameCd into ipnYakkas
                              from ipnYakka in ipnYakkas.DefaultIfEmpty()
                              join i in ipnNameMstList on q.TenMst.IpnNameCd equals i.IpnNameCd into ipnNameMsts
                              from ipnNameMst in ipnNameMsts.DefaultIfEmpty()
                              select new
                              {
                                  q.TenMst,
                                  q.KouiName,
                                  q.YakkaSyusaiItem,
                                  q.TenKN,
                                  KensaMst = q.KensaMst,
                                  IpnName = ipnNameMst?.IpnName ?? string.Empty,
                                  IsGetYakkaPrice = ipnExcludes.FirstOrDefault() == null && ipnExcludesItems.FirstOrDefault() == null,
                                  Yakka = ipnYakkas.FirstOrDefault() == null ? 0 : ipnYakkas.FirstOrDefault()?.Yakka
                              };

            var totalCount = joinedQuery.Count();

            var entities = joinedQuery.OrderBy(item => item.TenMst.KanaName1).ThenBy(item => item.TenMst.Name).Skip((pageIndex - 1) * pageCount).Take(pageCount);

            tenMstModels = entities.Select(item => new TenItemModel(
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
                                                           item.TenKN != null ? item.TenKN.Ten : (item.TenMst?.Ten ?? 0),
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
                                                           item.IpnName,
                                                           item.TenMst?.IsDeleted ?? 0,
                                                           item.TenMst?.HandanGrpKbn ?? 0,
                                                           item.KensaMst == null,
                                                           item.Yakka == null ? 0 : item.Yakka ?? 0,
                                                           item.IsGetYakkaPrice
                                                            )).ToList();

            return (tenMstModels, totalCount);
        }

        public string GetDrugAction(string yjCd)
        {
            var m34DrugInfMain = NoTrackingDataContext.M34DrugInfoMains.Where(drug => drug.YjCd == yjCd);
            var m34IndicationCode = NoTrackingDataContext.M34IndicationCodes.Where(ind => ind.KonoCd != null);
            var query = (from drug in m34DrugInfMain
                         join ind in m34IndicationCode on drug.KonoCd equals ind.KonoCd
                         select new
                         {
                             M34IndicationCode = ind
                         }).FirstOrDefault();

            var result = query != null ? query.M34IndicationCode.KonoSimpleCmt ?? string.Empty : string.Empty;

            return result;
        }

        public string GetPrecautions(string yjCd)
        {
            string result = string.Empty;

            var listPrecautionCode = NoTrackingDataContext.M34Precautions.Where(pre => pre.YjCd == yjCd).ToList().Select(u => u.PrecautionCd).ToList();
            if (listPrecautionCode.Count == 0) return string.Empty;
            var listPrecautionInf = NoTrackingDataContext.M34PrecautionCodes.Where(preCode => listPrecautionCode.Contains(preCode.PrecautionCd)).ToList();
            foreach (var item in listPrecautionInf)
            {
                result += item.PrecautionCmt + Environment.NewLine;
            }
            result?.TrimEnd(Environment.NewLine.ToCharArray());

            return result ?? string.Empty;
        }

        public List<SingleDoseMstModel> GetListSingleDoseModel(int hpId)
        {
            string functName = string.Empty;
            functName = nameof(GetListSingleDoseModel);
            List<SingleDoseMstModel> result = new List<SingleDoseMstModel>();
            var listSingleDoseMst = TrackingDataContext.SingleDoseMsts.Where(x => x.HpId == hpId).ToList();
            result = listSingleDoseMst.Select(i => new SingleDoseMstModel(ModelStatus.None, false, i.Id, i.HpId, i.UnitName)).ToList();
            return result;
        }

        public List<MedicineUnitModel> GetListMedicineUnitModel(int hpId, int today)
        {
            string functName = string.Empty;
            functName = nameof(GetListMedicineUnitModel);

            List<MedicineUnitModel> result = new List<MedicineUnitModel>();
            var listTenMstName = NoTrackingDataContext.TenMsts.Where(x => x.HpId == hpId && x.EndDate >= today && x.IsDeleted == DeleteTypes.None)
                                                           .OrderBy(x => x.OdrUnitName)
                                                           .Select(x => x.OdrUnitName)
                                                           .Distinct()
                                                           .ToList();
            var listSingleDoseMstName = NoTrackingDataContext.SingleDoseMsts.Where(x => x.HpId == hpId)
                                                                         .Select(s => s.UnitName)
                                                                         .ToList();

            foreach (var item in listTenMstName)
            {
                if (!listSingleDoseMstName.Contains(item) && !string.IsNullOrEmpty(item))
                {
                    result.Add(new MedicineUnitModel(item, false));
                }
            }
            return result;
        }
        public bool UpdateSingleDoseMst(int hpId, int userId, List<SingleDoseMstModel> listToSave)
        {
            string functName = string.Empty;
            functName = nameof(UpdateSingleDoseMst);
            List<SingleDoseMst> singleDoseAdded = new List<SingleDoseMst>();
            List<SingleDoseMst> singleDoseEdit = new List<SingleDoseMst>();
            List<SingleDoseMst> singleDoseDelete = new List<SingleDoseMst>();
            foreach (var item in listToSave)
            {
                if (item != null && !item.CheckDefaultValue())
                {
                    if (item.Status == ModelStatus.Modified)
                    {
                        var data = TrackingDataContext.SingleDoseMsts.FirstOrDefault(i => i.Id == item.Id);
                        if (data != null)
                        {
                            data.UnitName = item.UnitName;
                            _UpdateSingleDose(data, userId);
                            singleDoseEdit.Add(data);
                        }
                    }
                    if (item.Status == ModelStatus.Added && !item.IsDeleted && item.Id == 0)
                    {
                        var singleDoseMst = new SingleDoseMst();
                        singleDoseMst.UnitName = item.UnitName;
                        singleDoseMst.HpId = hpId;
                        _CreateSingleDose(singleDoseMst, userId);
                        singleDoseAdded.Add(singleDoseMst);
                    }
                    if (item.Status == ModelStatus.Deleted)
                    {
                        var data = TrackingDataContext.SingleDoseMsts.FirstOrDefault(i => i.Id == item.Id);
                        if (data != null)
                        {
                            singleDoseDelete.Add(data);
                        }
                    }
                }
            }
            TrackingDataContext.SingleDoseMsts.AddRange(singleDoseAdded);
            TrackingDataContext.SingleDoseMsts.UpdateRange(singleDoseEdit);
            TrackingDataContext.SingleDoseMsts.RemoveRange(singleDoseDelete);
            TrackingDataContext.SaveChanges();
            return TrackingDataContext.SaveChanges() > 0;

        }
        private void _UpdateSingleDose(SingleDoseMst singleDoseMst, int userId)
        {

            singleDoseMst.CreateDate = TimeZoneInfo.ConvertTimeToUtc(singleDoseMst.CreateDate);
            singleDoseMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
            singleDoseMst.UpdateId = userId;
        }

        private void _CreateSingleDose(SingleDoseMst singleDoseMst, int userId)
        {
            singleDoseMst.CreateDate = CIUtil.GetJapanDateTimeNow();
            singleDoseMst.CreateId = userId;
            singleDoseMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
            singleDoseMst.UpdateId = userId;
        }

        private string BuildPathAws(List<string> folders)
        {
            StringBuilder result = new();
            foreach (var item in folders)
            {
                result.Append(item);
                result.Append("/");
            }
            return result.ToString();
        }

        public List<ByomeiMstModel> DiseaseNameMstSearch(int hpId, string keyword, bool chkByoKbn0, bool chkByoKbn1, bool chkSaiKbn, bool chkMiSaiKbn, bool chkSidoKbn, bool chkToku, bool chkHiToku1, bool chkHiToku2, bool chkTenkan, bool chkTokuTenkan, bool chkNanbyo, int pageIndex, int pageSize, bool isCheckPage)
        {
            string kanaKeyword = keyword != string.Empty ? keyword : "";
            if (WanaKana.IsKana(keyword) && WanaKana.IsRomaji(keyword))
            {
                var inputKeyword = keyword;
                kanaKeyword = CIUtil.ToHalfsize(keyword);
                if (WanaKana.IsRomaji(kanaKeyword)) //If after convert to kana. type still is IsRomaji, back to base input keyword
                    kanaKeyword = inputKeyword;
            }

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
            var query = NoTrackingDataContext.ByomeiMsts.Where(item => item.HpId == hpId &&
                                    (!String.IsNullOrEmpty(keyword)
                                    &&
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
                                        .StartsWith(sBigKeyword))
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
                                        .StartsWith(sBigKeyword))
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
                                        .StartsWith(sBigKeyword))
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
                                        .StartsWith(sBigKeyword))
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
                                        .StartsWith(sBigKeyword))
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
                                        .StartsWith(sBigKeyword))
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
                                        .StartsWith(sBigKeyword))
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
                                        .Contains(sBigKeyword))
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
                                    item.Icd1022013.StartsWith(keyword)))
                                    &&
                                    (item.ByomeiCd.Length != 4 || (item.ByomeiCd.Length == 4 && string.Compare(item.ByomeiCd, "9000") < 0)))
                                    .OrderBy(item => item.KanaName1).ThenByDescending(item => item.IsAdopted).ToList();
            List<ByomeiMstModel> listByomeies = new();
            if (query != null)
            {
                foreach (var item in query)
                {
                    var itemCheck = checkIsMatch(item, chkByoKbn0, chkByoKbn1, chkSaiKbn, chkMiSaiKbn, chkSidoKbn, chkToku, chkHiToku1, chkHiToku2, chkTenkan, chkTokuTenkan, chkNanbyo);
                    if (itemCheck)
                    {
                        var itemAdd = ConvertToByomeiMstModelInDeseaseNameMst(item);
                        listByomeies.Add(itemAdd);
                    }
                }
            }

            if (isCheckPage)
            {
                listByomeies = listByomeies.OrderBy(x => x.DisplayedOrder).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            return listByomeies.OrderBy(x => x.DisplayedOrder).ToList();
        }

        private bool checkIsMatch(ByomeiMst? pt, bool chkByoKbn0, bool chkByoKbn1, bool chkSaiKbn, bool chkMiSaiKbn, bool chkSidoKbn, bool chkToku, bool chkHiToku1, bool chkHiToku2, bool chkTenkan, bool chkTokuTenkan, bool chkNanbyo)
        {
            if (pt == null)
                return false;
            bool isMatch = false;
            if (chkByoKbn0)
            {
                isMatch = pt.ByomeiCd.Length != 4;
            }
            if (chkByoKbn1)
            {
                isMatch = isMatch ? isMatch : pt.ByomeiCd.Length == 4;
            }

            if (!isMatch)
            {
                return isMatch;
            }

            isMatch = false;
            if (chkSaiKbn)
            {
                isMatch = pt.IsAdopted == 1;
            }
            if (chkMiSaiKbn)
            {
                isMatch = isMatch ? isMatch : pt.IsAdopted == 0;
            }

            if (!isMatch)
            {
                return isMatch;
            }

            isMatch = false;
            if (chkSidoKbn)
            {
                isMatch = pt.SikkanCd != 3 && pt.SikkanCd != 5 && pt.SikkanCd != 4 && pt.SikkanCd != 7 && pt.SikkanCd != 8;
            }
            if (chkToku)
            {
                isMatch = isMatch ? isMatch : pt.SikkanCd == 5;
            }
            if (chkHiToku1)
            {
                isMatch = isMatch ? isMatch : pt.SikkanCd == 3;
            }
            if (chkHiToku2)
            {
                isMatch = isMatch ? isMatch : pt.SikkanCd == 4;
            }
            if (chkTenkan)
            {
                isMatch = isMatch ? isMatch : pt.SikkanCd == 7;
            }
            if (chkTokuTenkan)
            {
                isMatch = isMatch ? isMatch : pt.SikkanCd == 8;
            }
            if (chkNanbyo)
            {
                isMatch = isMatch ? isMatch : pt.NanbyoCd == 9;
            }
            return isMatch;
        }

        public bool UpdateCmtCheckMst(int userId, int hpId, List<ItemCmtModel> listData)
        {
            foreach (var item in listData)
            {
                var itemUpdate = TrackingDataContext.CmtCheckMsts.FirstOrDefault(t => t.HpId == item.HpId && t.ItemCd == item.ItemCd && t.SeqNo == item.SeqNo);
                if (itemUpdate != null)
                {
                    itemUpdate.Cmt = item.Comment;
                    itemUpdate.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    itemUpdate.UpdateId = userId;
                    TrackingDataContext.SaveChanges();
                }
            }
            return true;
        }

        public bool SaveAddressMaster(List<PostCodeMstModel> postCodes, int hpId, int userId)
        {
            var addedModels = postCodes.Where(k => k.PostCodeStatus == ModelStatus.Added && k.Id == 0);
            var updatedModels = postCodes.Where(k => k.PostCodeStatus == ModelStatus.Modified);
            var deletedModels = postCodes.Where(k => k.PostCodeStatus == ModelStatus.Deleted);

            if (deletedModels.Any())
            {
                foreach (var model in deletedModels)
                {
                    var postCodeUpdate = TrackingDataContext.PostCodeMsts.FirstOrDefault(x => x.HpId == model.HpId && x.Id == model.Id);

                    if (postCodeUpdate != null)
                    {
                        postCodeUpdate.IsDeleted = 1;
                        postCodeUpdate.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        postCodeUpdate.UpdateId = userId;
                    }
                }
            }

            if (updatedModels.Any())
            {
                foreach (var model in updatedModels)
                {
                    var postCodeUpdate = TrackingDataContext.PostCodeMsts.FirstOrDefault(x => x.HpId == model.HpId && x.Id == model.Id);

                    if (postCodeUpdate != null)
                    {
                        postCodeUpdate.PostCd = model.PostCd;
                        postCodeUpdate.PrefName = model.PrefName;
                        postCodeUpdate.CityName = model.CityName;
                        postCodeUpdate.Banti = model.Banti;
                        postCodeUpdate.IsDeleted = model.IsDeleted;
                        postCodeUpdate.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        postCodeUpdate.UpdateId = userId;
                    }
                }
            }

            if (addedModels.Any())
            {
                foreach (var model in addedModels)
                {
                    TrackingDataContext.PostCodeMsts.Add(new PostCodeMst()
                    {
                        HpId = hpId,
                        PostCd = model.PostCd,
                        PrefName = model.PrefName,
                        CityName = model.CityName,
                        Banti = model.Banti,
                        IsDeleted = 0,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                    });
                }
            }

            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool CheckPostCodeExist(int hpId, string zipCD)
        {
            return NoTrackingDataContext.PostCodeMsts.Any(x => x.HpId == hpId &&
                                                               x.IsDeleted == 0 &&
                                                               x.PostCd == zipCD);
        }

        public bool UpsertMaterialMaster(int hpId, int userId, List<MaterialMasterModel> materialMasters)
        {
            foreach (var item in materialMasters)
            {
                if (item.MaterialModelStatus == ModelStatus.Deleted)
                {
                    var containerMaster = TrackingDataContext.MaterialMsts.Where(x => x.MaterialCd == item.MaterialCd);
                    if (containerMaster != null)
                    {
                        TrackingDataContext.MaterialMsts.RemoveRange(containerMaster);
                    }
                }
                else
                {
                    var materialMaster = TrackingDataContext.MaterialMsts.FirstOrDefault(x => x.MaterialCd == item.MaterialCd);
                    if (materialMaster != null)
                    {
                        materialMaster.MaterialCd = item.MaterialCd;
                        materialMaster.MaterialName = item.MaterialName;
                        materialMaster.UpdateId = userId;
                        materialMaster.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }
                    else
                    {
                        MaterialMst itemtest = ConvertContainerMasterList(item, userId, hpId);
                        TrackingDataContext.MaterialMsts.Add(itemtest);
                    }
                }
            }
            return TrackingDataContext.SaveChanges() >= 1;
        }

        private MaterialMst ConvertContainerMasterList(MaterialMasterModel u, int userId, int hpId)
        {
            return new MaterialMst
            {
                HpId = hpId,
                MaterialCd = u.MaterialCd,
                MaterialName = u.MaterialName,
                CreateId = userId,
                UpdateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateDate = CIUtil.GetJapanDateTimeNow()
            };
        }

        public List<KensaMstModel> GetParrentKensaMstModels(int hpId, string keyWord)
        {
            var result = new List<KensaMstModel>();
            string itemCd = "";
            var kensaInKensaMst = NoTrackingDataContext.KensaMsts.Where(x => x.HpId == hpId);
            var kensaInTenMst = NoTrackingDataContext.TenMsts.Where(x => x.HpId == hpId);

            if (string.IsNullOrEmpty(keyWord))
            {
                kensaInKensaMst = NoTrackingDataContext.KensaMsts.Where(p => p.HpId == hpId &&
                                                                                      p.IsDelete == DeleteTypes.None &&
                                                                                      (string.IsNullOrEmpty(p.OyaItemCd) || p.KensaItemCd == p.OyaItemCd));

                kensaInTenMst = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId &&
                                                                                  !string.IsNullOrEmpty(p.KensaItemCd) &&
                                                                                  p.IsDeleted == DeleteTypes.None);
            }
            else
            {
                string bigKeyWord = keyWord.ToUpper()
                                           .Replace("ｧ", "ｱ")
                                           .Replace("ｨ", "ｲ")
                                           .Replace("ｩ", "ｳ")
                                           .Replace("ｪ", "ｴ")
                                           .Replace("ｫ", "ｵ")
                                           .Replace("ｬ", "ﾔ")
                                           .Replace("ｭ", "ﾕ")
                                           .Replace("ｮ", "ﾖ")
                                           .Replace("ｯ", "ﾂ");

                //get kensa in KensaMst
                kensaInKensaMst = NoTrackingDataContext.KensaMsts.Where(p => p.HpId == hpId &&
                                                                                          p.IsDelete == DeleteTypes.None &&
                                                                                          (string.IsNullOrEmpty(p.OyaItemCd) || p.KensaItemCd == p.OyaItemCd) &&
                                                                                          (keyWord == "ﾊﾞｲﾀﾙ" ? p.KensaItemCd.Contains("V") :
                                                                                          (p.KensaName.ToUpper().Contains(bigKeyWord) ||
                                                                                           p.KensaKana.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                                .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ")
                                                                                                                .StartsWith(bigKeyWord))));

                //get kensa in TenMst
                kensaInTenMst = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId &&
                                                                                  p.IsDeleted == DeleteTypes.None &&
                                                                                      !string.IsNullOrEmpty(p.KensaItemCd) &&
                                                                                      (keyWord == "IGE" ? p.ItemCd.StartsWith("IGE") :
                                                                                      (p.Name.ToUpper().Contains(bigKeyWord)
                                                                                    || p.KanaName1.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord)
                                                                                    || p.KanaName2.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord)
                                                                                    || p.KanaName3.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord)
                                                                                    || p.KanaName4.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord)
                                                                                    || p.KanaName5.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord)
                                                                                    || p.KanaName6.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord)
                                                                                    || p.KanaName7.ToUpper().Replace("ｧ", "ｱ").Replace("ｨ", "ｲ").Replace("ｩ", "ｳ").Replace("ｪ", "ｴ").Replace("ｫ", "ｵ")
                                                                                                            .Replace("ｬ", "ﾔ").Replace("ｭ", "ﾕ").Replace("ｮ", "ﾖ").Replace("ｯ", "ﾂ").StartsWith(bigKeyWord))));
            }
            if (!string.IsNullOrEmpty(itemCd))
            {
                kensaInKensaMst = kensaInKensaMst.Where(u => u.KensaItemCd == itemCd);
            }
            var kensaMsts = NoTrackingDataContext.KensaMsts.Where(p => p.HpId == hpId && p.IsDelete == DeleteTypes.None);

            var tenMstJoinKensaMstQuery = from kensaTenMst in kensaInTenMst
                                          join kensaMst in kensaMsts.Where(p => string.IsNullOrEmpty(p.OyaItemCd) || p.KensaItemCd == p.OyaItemCd)
                                          on new { kensaTenMst.KensaItemCd, kensaTenMst.KensaItemSeqNo } equals new { kensaMst.KensaItemCd, kensaMst.KensaItemSeqNo }
                                          select new
                                          {
                                              KensaMst = kensaMst
                                          };

            var allParrentKensaMsts = tenMstJoinKensaMstQuery.Select(p => p.KensaMst).Union(kensaInKensaMst).OrderBy(p => p.KensaKana).Distinct().ToList();

            //get all child kensaMst 
            var allKensaMsts = from parrentKensaMst in allParrentKensaMsts
                               join kensaMst in kensaMsts.Where(p => !string.IsNullOrEmpty(p.OyaItemCd) && p.KensaItemCd != p.OyaItemCd)
                               on parrentKensaMst.KensaItemCd equals kensaMst.OyaItemCd into childKensaMsts
                               select new
                               {
                                   ParrentKensaMst = parrentKensaMst,
                                   ChildKensaMsts = childKensaMsts,
                               };

            var KensaItemCd = kensaMsts.Select(x => x.KensaItemCd).ToList();
            var KensaItemSeqNo = kensaMsts.Select(x => x.KensaItemSeqNo).ToList();

            var tenMsts = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None && KensaItemCd.Contains(p.KensaItemCd ?? string.Empty) && KensaItemSeqNo.Contains(p.KensaItemSeqNo));

            var query = from kensaMst in allKensaMsts
                        join tenMst in tenMsts
                        on new { kensaMst.ParrentKensaMst.KensaItemCd, kensaMst.ParrentKensaMst.KensaItemSeqNo }
                        equals new { tenMst.KensaItemCd, tenMst.KensaItemSeqNo } into tempTenMsts
                        select new
                        {
                            ParrentKensaMst = kensaMst.ParrentKensaMst,
                            ChildKensaMsts = kensaMst.ChildKensaMsts.Select(x => new KensaMstModel(
                                                                            x.KensaItemCd,
                                                                            x.KensaItemSeqNo,
                                                                            x.CenterCd ?? string.Empty,
                                                                            x.KensaName ?? string.Empty,
                                                                            x.KensaKana ?? string.Empty,
                                                                            x.Unit ?? string.Empty,
                                                                            x.MaterialCd,
                                                                            x.ContainerCd,
                                                                            x.MaleStd ?? string.Empty,
                                                                            x.MaleStdLow ?? string.Empty,
                                                                            x.MaleStdHigh ?? string.Empty,
                                                                            x.FemaleStd ?? string.Empty,
                                                                            x.FemaleStdLow ?? string.Empty,
                                                                            x.FemaleStdHigh ?? string.Empty,
                                                                            x.Formula ?? string.Empty,
                                                                            x.Digit,
                                                                            x.OyaItemCd ?? string.Empty,
                                                                            x.OyaItemSeqNo,
                                                                            x.SortNo,
                                                                            x.CenterItemCd1 ?? string.Empty,
                                                                            x.CenterItemCd2 ?? string.Empty)).ToList(),
                            TenMsts = tempTenMsts.Select(x => new TenItemModel(x.SinKouiKbn,
                                                                              x.MasterSbt ?? string.Empty,
                                                                              x.ItemCd,
                                                                              x.KensaItemCd ?? string.Empty,
                                                                              x.KensaItemSeqNo,
                                                                              x.Ten,
                                                                              x.Name ?? string.Empty,
                                                                              x.ReceName ?? string.Empty,
                                                                              x.KanaName1 ?? string.Empty,
                                                                              x.KanaName2 ?? string.Empty,
                                                                              x.KanaName3 ?? string.Empty,
                                                                              x.KanaName4 ?? string.Empty,
                                                                              x.KanaName5 ?? string.Empty,
                                                                              x.KanaName6 ?? string.Empty,
                                                                              x.KanaName7 ?? string.Empty,
                                                                              x.StartDate,
                                                                              x.EndDate,
                                                                              x.DefaultVal,
                                                                              x.OdrUnitName ?? string.Empty,
                                                                              x.SanteiItemCd ?? string.Empty,
                                                                              x.SanteigaiKbn,
                                                                              x.IsNosearch)).ToList()
                        };

            foreach (var entity in query)
            {
                var ChildKensaMsts = NoTrackingDataContext.KensaMsts.FirstOrDefault(x => x.KensaItemCd == entity.ParrentKensaMst.KensaItemCd);
                result.Add(new KensaMstModel(
                    entity.ParrentKensaMst.KensaItemCd,
                    entity.ParrentKensaMst.KensaItemSeqNo,
                    entity.ParrentKensaMst.CenterCd ?? string.Empty,
                    entity.ParrentKensaMst.KensaName ?? string.Empty,
                    entity.ParrentKensaMst.KensaKana ?? string.Empty,
                    entity.ParrentKensaMst.Unit ?? string.Empty,
                    entity.ParrentKensaMst.MaterialCd,
                    entity.ParrentKensaMst.ContainerCd,
                    entity.ParrentKensaMst.MaleStd ?? string.Empty,
                    entity.ParrentKensaMst.MaleStdLow ?? string.Empty,
                    entity.ParrentKensaMst.MaleStdHigh ?? string.Empty,
                    entity.ParrentKensaMst.FemaleStd ?? string.Empty,
                    entity.ParrentKensaMst.FemaleStdLow ?? string.Empty,
                    entity.ParrentKensaMst.FemaleStdHigh ?? string.Empty,
                    entity.ParrentKensaMst.Formula ?? string.Empty,
                    entity.ParrentKensaMst.Digit,
                    entity.ParrentKensaMst.OyaItemCd ?? string.Empty,
                    entity.ParrentKensaMst.OyaItemSeqNo,
                    entity.ParrentKensaMst.SortNo,
                    entity.ParrentKensaMst.CenterItemCd1 ?? string.Empty,
                    entity.ParrentKensaMst.CenterItemCd2 ?? string.Empty,
                    entity.TenMsts,
                    entity.ChildKensaMsts,
                    new()
                    ));
            }

            return result;
        }

        public bool ContainerMasterUpdate(int hpId, int userId, List<ContainerMasterModel> containerMasters)
        {
            foreach (var item in containerMasters)
            {
                if (item.ContainerModelStatus == ModelStatus.Deleted)
                {
                    var containerMaster = TrackingDataContext.ContainerMsts.Where(x => x.ContainerCd == item.ContainerCd);
                    if (containerMaster != null)
                    {
                        TrackingDataContext.ContainerMsts.RemoveRange(containerMaster);
                    }
                }
                else
                {
                    var containerMaster = TrackingDataContext.ContainerMsts.FirstOrDefault(x => x.ContainerCd == item.ContainerCd);
                    if (containerMaster != null)
                    {
                        containerMaster.ContainerCd = item.ContainerCd;
                        containerMaster.ContainerName = item.ContainerName;
                        containerMaster.UpdateId = userId;
                        containerMaster.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }
                    else
                    {
                        ContainerMst itemtest = ConvertContainerMasterList(item, userId, hpId);
                        TrackingDataContext.ContainerMsts.Add(itemtest);
                    }
                }
            }
            return TrackingDataContext.SaveChanges() >= 1;
        }

        private ContainerMst ConvertContainerMasterList(ContainerMasterModel u, int userId, int hpId)
        {
            return new ContainerMst
            {
                HpId = hpId,
                ContainerCd = u.ContainerCd,
                ContainerName = u.ContainerName,
                CreateId = userId,
                UpdateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateDate = CIUtil.GetJapanDateTimeNow()
            };
        }

        public bool UpdateKensaMst(int hpId, int userId, List<KensaMstModel> kensaMstModels, List<TenItemModel> tenMstModels)
        {
            List<KensaMst> newKensaMsts = new List<KensaMst>();
            List<TenMst> newTenMsts = new List<TenMst>();

            foreach (var item in kensaMstModels)
            {
                if (item.IsDeleted == 1)
                {
                    var listKensaMst = TrackingDataContext.KensaMsts.FirstOrDefault(x => x.KensaItemCd == item.KensaItemCd && x.KensaItemSeqNo == item.KensaItemSeqNo);
                    if (listKensaMst != null)
                    {
                        listKensaMst.IsDelete = 1;
                    }
                }
                else
                {
                    var listKensaMst = TrackingDataContext.KensaMsts.FirstOrDefault(x => x.KensaItemCd == item.KensaItemCd && x.KensaItemSeqNo == item.KensaItemSeqNo);
                    if (listKensaMst != null)
                    {
                        listKensaMst.CenterCd = item.CenterCd;
                        listKensaMst.KensaName = item.KensaName;
                        listKensaMst.KensaKana = item.KensaKana;
                        listKensaMst.Unit = item.Unit;
                        listKensaMst.MaterialCd = item.MaterialCd;
                        listKensaMst.MaleStd = item.MaleStd;
                        listKensaMst.MaleStdLow = item.MaleStdLow;
                        listKensaMst.MaleStdHigh = item.MaleStdHigh;
                        listKensaMst.FemaleStd = item.FemaleStd;
                        listKensaMst.FemaleStdLow = item.FemaleStdLow;
                        listKensaMst.FemaleStdHigh = item.FemaleStdHigh;
                        listKensaMst.Formula = item.Formula;
                        listKensaMst.OyaItemCd = item.OyaItemCd;
                        listKensaMst.OyaItemSeqNo = item.OyaItemSeqNo;
                        listKensaMst.SortNo = item.SortNo;
                        listKensaMst.CenterItemCd1 = item.CenterItemCd1;
                        listKensaMst.CenterItemCd2 = item.CenterItemCd2;
                        listKensaMst.Digit = item.Digit;
                        listKensaMst.UpdateId = userId;
                        listKensaMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }
                    else
                    {
                        KensaMst itemtest = ConvertKensaMasterList(item, userId, hpId);
                        TrackingDataContext.KensaMsts.AddRange(itemtest);
                    }
                }
            }

            foreach (var item in tenMstModels)
            {
                if (item.IsDeleted == 1)
                {
                    var listTenMst = TrackingDataContext.TenMsts.FirstOrDefault(x => x.ItemCd == item.ItemCd && x.StartDate == item.StartDate);
                    if (listTenMst != null)
                    {
                        listTenMst.IsDeleted = 1;
                    }
                }
                else
                {
                    var listTenMst = TrackingDataContext.TenMsts.FirstOrDefault(x => x.ItemCd == item.ItemCd && x.StartDate == item.StartDate);
                    if (listTenMst != null)
                    {
                        listTenMst.ItemCd = item.ItemCd;
                        listTenMst.SinKouiKbn = item.SinKouiKbn;
                        listTenMst.Name = item.Name;
                        listTenMst.OdrUnitName = item.OdrUnitName;
                        listTenMst.IsNodspRece = item.IsNodspRece;
                        listTenMst.YohoKbn = item.YohoKbn;
                        listTenMst.OdrTermVal = item.OdrTermVal;
                        listTenMst.CnvTermVal = item.CnvTermVal;
                        listTenMst.YjCd = item.YjCd;
                        listTenMst.KensaItemCd = item.KensaItemCd;
                        listTenMst.KensaItemSeqNo = item.KensaItemSeqNo;
                        listTenMst.KohatuKbn = item.KohatuKbn;
                        listTenMst.Ten = item.Ten;
                        listTenMst.HandanGrpKbn = item.HandanGrpKbn;
                        listTenMst.IpnNameCd = item.IpnNameCd;
                        listTenMst.CmtCol1 = item.CmtCol1;
                        listTenMst.CmtCol2 = item.CmtCol2;
                        listTenMst.CmtCol3 = item.CmtCol3;
                        listTenMst.CmtCol4 = item.CmtCol4;
                        listTenMst.CmtColKeta1 = item.CmtColKeta1;
                        listTenMst.CmtColKeta2 = item.CmtColKeta2;
                        listTenMst.CmtColKeta3 = item.CmtColKeta3;
                        listTenMst.CmtColKeta4 = item.CmtColKeta4;
                        listTenMst.MinAge = item.MaxAge;
                        listTenMst.StartDate = item.StartDate;
                        listTenMst.EndDate = item.EndDate;
                        listTenMst.MasterSbt = item.MasterSbt;
                        listTenMst.BuiKbn = item.BuiKbn;
                        listTenMst.CdKbn = item.CdKbn;
                        listTenMst.CdKbnno = item.CdKbnno;
                        listTenMst.CdEdano = item.CdEdano;
                        listTenMst.Kokuji1 = item.Kokuji1;
                        listTenMst.Kokuji2 = item.Kokuji2;
                        listTenMst.DrugKbn = item.DrugKbn;
                        listTenMst.ReceName = item.ReceName;
                        listTenMst.SanteiItemCd = item.SanteiItemCd;
                        listTenMst.JihiSbt = item.JihiSbt;
                        listTenMst.IsDeleted = item.IsDeleted;
                        listTenMst.UpdateId = userId;
                        listTenMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }
                    else
                    {
                        TenMst itemtest = ConvertTenMasterList(item, userId, hpId);
                        newTenMsts.Add(itemtest);
                        TrackingDataContext.TenMsts.AddRange(newTenMsts);
                    }
                }
            }

            return TrackingDataContext.SaveChanges() >= 1;
        }

        private TenMst ConvertTenMasterList(TenItemModel u, int userId, int hpId)
        {
            return new TenMst
            {
                HpId = hpId,
                ItemCd = u.ItemCd,
                SinKouiKbn = u.SinKouiKbn,
                Name = u.Name,
                OdrUnitName = u.OdrUnitName,
                CnvUnitName = u.CnvUnitName,
                IsNodspRece = u.IsNodspRece,
                YohoKbn = u.YohoKbn,
                OdrTermVal = u.OdrTermVal,
                CnvTermVal = u.CnvTermVal,
                YjCd = u.YjCd,
                KensaItemCd = u.KensaItemCd,
                KensaItemSeqNo = u.KensaItemSeqNo,
                KohatuKbn = u.KohatuKbn,
                Ten = u.Ten,
                HandanGrpKbn = u.HandanGrpKbn,
                IpnNameCd = u.IpnNameCd,
                CmtCol1 = u.CmtCol1,
                CmtCol2 = u.CmtCol2,
                CmtCol3 = u.CmtCol3,
                CmtCol4 = u.CmtCol4,
                CmtColKeta1 = u.CmtColKeta1,
                CmtColKeta2 = u.CmtColKeta2,
                CmtColKeta3 = u.CmtColKeta3,
                CmtColKeta4 = u.CmtColKeta4,
                MinAge = u.MinAge,
                MaxAge = u.MaxAge,
                StartDate = u.StartDate,
                EndDate = u.EndDate,
                MasterSbt = u.MasterSbt,
                BuiKbn = u.BuiKbn,
                CdKbn = u.CdKbn,
                CdKbnno = u.CdKbnno,
                CdEdano = u.CdEdano,
                Kokuji1 = u.Kokuji1,
                Kokuji2 = u.Kokuji2,
                DrugKbn = u.DrugKbn,
                ReceName = u.ReceName,
                SanteiItemCd = u.SanteiItemCd,
                JihiSbt = u.JihiSbt,
                IsDeleted = u.IsDeleted,
                CreateId = userId,
                UpdateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateDate = CIUtil.GetJapanDateTimeNow()
            };
        }

        private KensaMst ConvertKensaMasterList(KensaMstModel u, int userId, int hpId)
        {
            return new KensaMst
            {
                HpId = hpId,
                KensaItemCd = u.KensaItemCd,
                KensaItemSeqNo = u.KensaItemSeqNo,
                CenterCd = u.CenterCd,
                KensaName = u.KensaName,
                KensaKana = u.KensaKana,
                Unit = u.Unit,
                MaterialCd = u.MaterialCd,
                MaleStd = u.MaleStd,
                MaleStdLow = u.MaleStdLow,
                MaleStdHigh = u.MaleStdHigh,
                FemaleStd = u.FemaleStd,
                FemaleStdLow = u.FemaleStdLow,
                FemaleStdHigh = u.FemaleStdHigh,
                Formula = u.Formula,
                OyaItemCd = u.OyaItemCd,
                OyaItemSeqNo = u.OyaItemSeqNo,
                SortNo = u.SortNo,
                CenterItemCd1 = u.CenterItemCd1,
                CenterItemCd2 = u.CenterItemCd2,
                Digit = u.Digit,
                IsDelete = u.IsDeleted,
                CreateId = userId,
                UpdateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateDate = CIUtil.GetJapanDateTimeNow()
            };
        }

        public bool IsUsingKensa(int hpId, string kensaItemCd, List<string> itemCds)
        {
            bool result = NoTrackingDataContext.KensaInfDetails.Where(p => p.HpId == hpId && p.KensaItemCd == kensaItemCd).Any();

            if (itemCds?.Count > 0)
            {
                result = result || NoTrackingDataContext.OdrInfDetails.Where(p => p.HpId == hpId && itemCds.Contains(p.ItemCd ?? string.Empty)).Any();
            }
            return result;
        }

        public bool UpdateKensaStdMst(int hpId, int userId, List<KensaStdMstModel> kensaStdMstModels)
        {
            foreach (var item in kensaStdMstModels)
            {
                if (item.IsDefault) continue;

                if (item.IsDeleted)
                {
                    var kensaStdMaster = TrackingDataContext.KensaStdMsts.Where(x => x.KensaItemCd == item.KensaItemcd && x.StartDate == item.StartDate);
                    if (kensaStdMaster != null)
                    {
                        TrackingDataContext.KensaStdMsts.RemoveRange(kensaStdMaster);
                    }
                }
                else
                {
                    var kensaStdMaster = TrackingDataContext.KensaStdMsts.FirstOrDefault(x => x.KensaItemCd == item.KensaItemcd && x.StartDate == item.StartDate);
                    if (kensaStdMaster != null)
                    {
                        kensaStdMaster.MaleStd = item.MaleStd;
                        kensaStdMaster.MaleStdLow = item.MaleStdLow;
                        kensaStdMaster.MaleStdHigh = item.MaleStdHigh;
                        kensaStdMaster.FemaleStd = item.FemaleStd;
                        kensaStdMaster.FemaleStdLow = item.FemaleStdLow;
                        kensaStdMaster.FemaleStdHigh = item.FemaleStdHigh;
                        kensaStdMaster.UpdateId = userId;
                        kensaStdMaster.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }
                    else
                    {
                        KensaStdMst itemtest = ConvertKensaStdMstList(item, userId, hpId);
                        TrackingDataContext.KensaStdMsts.Add(itemtest);
                    }
                }
            }
            return TrackingDataContext.SaveChanges() >= 1;
        }

        private KensaStdMst ConvertKensaStdMstList(KensaStdMstModel u, int userId, int hpId)
        {
            return new KensaStdMst
            {
                HpId = hpId,
                KensaItemCd = u.KensaItemcd,
                StartDate = u.StartDate,
                MaleStd = u.MaleStd,
                MaleStdLow = u.MaleStdLow,
                MaleStdHigh = u.MaleStdHigh,
                FemaleStd = u.FemaleStd,
                FemaleStdLow = u.FemaleStdLow,
                FemaleStdHigh = u.FemaleStdHigh,
                CreateId = userId,
                UpdateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateDate = CIUtil.GetJapanDateTimeNow()
            };
        }

        public bool UpdateByomeiMst(int userId, int hpId, List<UpdateByomeiMstModel> listData)
        {
            // Update IsAdopted Item TenMst
            foreach (var item in listData)
            {
                var itemUpdate = TrackingDataContext.ByomeiMsts.FirstOrDefault(t => t.HpId == hpId && t.ByomeiCd == item.ByomeiCd);
                if (itemUpdate != null)
                {
                    itemUpdate.KanaName2 = item.KanaName2;
                    itemUpdate.SikkanCd = item.SikkanCd;
                    itemUpdate.NanbyoCd = item.NanbyoCd;
                    itemUpdate.IsAdopted = item.IsAdopted ? 1 : 0;
                    itemUpdate.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    itemUpdate.UpdateId = userId;
                    TrackingDataContext.SaveChanges();
                }
            }
            return true;
        }

        public List<KensaStdMstModel> GetKensaStdMstModels(int hpId, string kensaItemCd)
        {
            var kensaStdMsts = NoTrackingDataContext.KensaStdMsts.Where(p => p.HpId == hpId && p.KensaItemCd == kensaItemCd);
            return kensaStdMsts.Select(p => new KensaStdMstModel(p.KensaItemCd,
                                                                 p.MaleStd ?? string.Empty,
                                                                 p.MaleStdLow ?? string.Empty,
                                                                 p.MaleStdHigh ?? string.Empty,
                                                                 p.FemaleStd ?? string.Empty,
                                                                 p.FemaleStdLow ?? string.Empty,
                                                                 p.MaleStdHigh ?? string.Empty,
                                                                 p.StartDate,
                                                                 p.CreateId)).ToList();
        }

        public List<string> GetUsedKensaItemCds(int hpId)
        {
            List<string> result = new();
            var itemCdsFromKensaMst = NoTrackingDataContext.KensaMsts
                                                           .Where(p => p.HpId == hpId)
                                                           .Select(p => p.KensaItemCd).ToList();
            var itemCdsFromTenMst = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId && !string.IsNullOrEmpty(p.KensaItemCd) && p.IsDeleted == DeleteTypes.None).Select(p => p.KensaItemCd).ToList();

            foreach (var item in itemCdsFromKensaMst.Union(itemCdsFromTenMst).Distinct().ToList())
            {
                result.Add(item ?? string.Empty);
            }

            return result;
        }

        public List<string> GetTenItemCds(int hpId)
        {
            return NoTrackingDataContext.TenMsts
                            .Where(p => (p.ItemCd.StartsWith("KN") || p.ItemCd.StartsWith("IGE")) && p.IsDeleted == DeleteTypes.None)
                            .Select(p => p.ItemCd).Distinct().ToList();
        }

        public Dictionary<int, string> GetMaterialMsts(int hpId)
        {
            var result = new Dictionary<int, string>();
            result.Add(0, string.Empty);

            var materialMsts = NoTrackingDataContext.MaterialMsts.Where(p => p.HpId == hpId);
            foreach (var materialMst in materialMsts)
            {
                result.Add(materialMst.MaterialCd.AsInteger(), materialMst.MaterialName ?? string.Empty);
            }
            return result;
        }

        public Dictionary<int, string> GetContainerMsts(int hpId)
        {
            var result = new Dictionary<int, string>();
            result.Add(0, string.Empty);

            var containerMsts = NoTrackingDataContext.ContainerMsts.Where(p => p.HpId == hpId);
            foreach (var containerMst in containerMsts)
            {
                result.Add(containerMst.ContainerCd.AsInteger(), containerMst.ContainerName ?? string.Empty);
            }
            return result;
        }

        public Dictionary<string, string> GetKensaCenterMsts(int hpId)
        {
            var result = new Dictionary<string, string>();
            result.Add(string.Empty, string.Empty);
            var kensaCenterMsts = NoTrackingDataContext.KensaCenterMsts.Where(p => p.HpId == hpId);

            foreach (var kensaCenterMst in kensaCenterMsts)
            {
                result.Add(kensaCenterMst.CenterCd ?? string.Empty, kensaCenterMst.CenterName ?? string.Empty);
            }

            return result;
        }

        public Dictionary<string, double> GetTenOfItem(int hpId)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            var tenOfHRTItem = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId && p.ItemCd == "160162950" && p.IsDeleted == DeleteTypes.None)
                                                        .OrderByDescending(p => p.StartDate).FirstOrDefault();

            var a = tenOfHRTItem != null ? tenOfHRTItem.Ten : 0;

            var tenOfIGEItem = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId && p.ItemCd == "160056110" && p.IsDeleted == DeleteTypes.None)
                                                        .OrderByDescending(p => p.StartDate).FirstOrDefault();

            var b = tenOfIGEItem != null ? tenOfIGEItem.Ten : 0;

            result.Add("TenOfHRTItem", a);
            result.Add("TenOfIGEItem", b);

            return result;
        }

        public List<KensaMstModel> GetListKensaMst(int hpId, string keyWord)
        {
            var result = new List<KensaMstModel>();
            var allkensaKensaMst = NoTrackingDataContext.KensaMsts.Where(x => x.HpId == hpId && x.IsDelete == DeleteTypes.None).ToList();

            if (allkensaKensaMst == null)
            {
                return result;
            }

            string bigKeyWord = keyWord.ToUpper()
                                       .Replace("ｧ", "ｱ")
                                       .Replace("ｨ", "ｲ")
                                       .Replace("ｩ", "ｳ")
                                       .Replace("ｪ", "ｴ")
                                       .Replace("ｫ", "ｵ")
                                       .Replace("ｬ", "ﾔ")
                                       .Replace("ｭ", "ﾕ")
                                       .Replace("ｮ", "ﾖ")
                                       .Replace("ｯ", "ﾂ");

            //get kensa in KensaMst
            var kensaInKensaMst = allkensaKensaMst.Where(p => p.HpId == hpId &&
                    p.IsDelete == DeleteTypes.None &&
                        (p.KensaItemCd.Contains(bigKeyWord) ||
                        (keyWord == "ﾊﾞｲﾀﾙ" ? p.KensaItemCd.Contains("V") :
                        (p.KensaName != null && p.KensaKana != null &&
                         (p.KensaName.ToUpper().Contains(bigKeyWord) ||
                          p.KensaKana.ToUpper()
                                  .Replace("ｧ", "ｱ")
                                  .Replace("ｨ", "ｲ")
                                  .Replace("ｩ", "ｳ")
                                  .Replace("ｪ", "ｴ")
                                  .Replace("ｫ", "ｵ")
                                  .Replace("ｬ", "ﾔ")
                                  .Replace("ｭ", "ﾕ")
                                  .Replace("ｮ", "ﾖ")
                                  .Replace("ｯ", "ﾂ")
                                  .StartsWith(bigKeyWord)))))).ToList(); ;


            foreach (var entity in kensaInKensaMst)
            {
                var parentItem = allkensaKensaMst.Where(x => !string.IsNullOrEmpty(entity.OyaItemCd) && x.KensaItemCd == entity.OyaItemCd).Select(x => new KensaMstModel(
                          entity.KensaItemCd,
                          entity.KensaItemSeqNo,
                          entity.CenterCd ?? string.Empty,
                          entity.KensaName ?? string.Empty,
                          entity.KensaKana ?? string.Empty,
                          entity.Unit ?? string.Empty,
                          entity.MaterialCd,
                          entity.ContainerCd,
                          entity.MaleStd ?? string.Empty,
                          entity.MaleStdLow ?? string.Empty,
                          entity.MaleStdHigh ?? string.Empty,
                          entity.FemaleStd ?? string.Empty,
                          entity.FemaleStdLow ?? string.Empty,
                          entity.FemaleStdHigh ?? string.Empty,
                          entity.Formula ?? string.Empty,
                          entity.Digit,
                          entity.OyaItemCd ?? string.Empty,
                          entity.OyaItemSeqNo,
                          entity.SortNo,
                          entity.CenterItemCd1 ?? string.Empty,
                          entity.CenterItemCd2 ?? string.Empty,
                          null,
                          null,
                          null
                        )).FirstOrDefault();

                var chilrenItems = allkensaKensaMst.Where(x => !string.IsNullOrEmpty(x.OyaItemCd) && x.OyaItemCd == entity.KensaItemCd).Select(x => new KensaMstModel(
                          entity.KensaItemCd,
                          entity.KensaItemSeqNo,
                          entity.CenterCd ?? string.Empty,
                          entity.KensaName ?? string.Empty,
                          entity.KensaKana ?? string.Empty,
                          entity.Unit ?? string.Empty,
                          entity.MaterialCd,
                          entity.ContainerCd,
                          entity.MaleStd ?? string.Empty,
                          entity.MaleStdLow ?? string.Empty,
                          entity.MaleStdHigh ?? string.Empty,
                          entity.FemaleStd ?? string.Empty,
                          entity.FemaleStdLow ?? string.Empty,
                          entity.FemaleStdHigh ?? string.Empty,
                          entity.Formula ?? string.Empty,
                          entity.Digit,
                          entity.OyaItemCd ?? string.Empty,
                          entity.OyaItemSeqNo,
                          entity.SortNo,
                          entity.CenterItemCd1 ?? string.Empty,
                          entity.CenterItemCd2 ?? string.Empty,
                          null,
                          null,
                          null
                        )).ToList();

                result.Add(new KensaMstModel(
                  entity.KensaItemCd,
                  entity.KensaItemSeqNo,
                  entity.CenterCd ?? string.Empty,
                  entity.KensaName ?? string.Empty,
                  entity.KensaKana ?? string.Empty,
                  entity.Unit ?? string.Empty,
                  entity.MaterialCd,
                  entity.ContainerCd,
                  entity.MaleStd ?? string.Empty,
                  entity.MaleStdLow ?? string.Empty,
                  entity.MaleStdHigh ?? string.Empty,
                  entity.FemaleStd ?? string.Empty,
                  entity.FemaleStdLow ?? string.Empty,
                  entity.FemaleStdHigh ?? string.Empty,
                  entity.Formula ?? string.Empty,
                  entity.Digit,
                  entity.OyaItemCd ?? string.Empty,
                  entity.OyaItemSeqNo,
                  entity.SortNo,
                  entity.CenterItemCd1 ?? string.Empty,
                  entity.CenterItemCd2 ?? string.Empty,
                  null,
                  chilrenItems,
                  parentItem
                  ));


            }
            return result;
        }
    }
}
