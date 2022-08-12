using Domain.Constant;
using Domain.Models.InputItem;
using Domain.Models.Reception;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class InputItemRepository : IInputItemRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public InputItemRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public IEnumerable<InputItemModel> SearchDataInputItem(string keyword, int KouiKbn, int sinDate, string ItemCodeStartWith, int pageIndex, int pageCount)
        {
            if (String.IsNullOrEmpty(keyword))
            {
                return new List<InputItemModel>();
            }

            string sBigKeyword = keyword.ToUpper()
              .Replace("ｧ", "ｱ")
              .Replace("ｨ", "ｲ")
              .Replace("ｩ", "ｳ")
              .Replace("ｪ", "ｴ")
              .Replace("ｫ", "ｵ")
              .Replace("ｬ", "ﾔ")
              .Replace("ｭ", "ﾕ")
              .Replace("ｮ", "ﾖ")
              .Replace("ｯ", "ﾂ");

            var yakkaSyusaiMstList = _tenantDataContext.YakkaSyusaiMsts.ToList();

            var queryResult = _tenantDataContext.TenMsts
                    .Where(t =>
                        t.ItemCd.StartsWith(keyword)
                        || t.KanaName1.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || t.KanaName2.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || t.KanaName3.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || t.KanaName4.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || t.KanaName5.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || t.KanaName6.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || t.KanaName7.ToUpper()
                          .Replace("ｧ", "ｱ")
                          .Replace("ｨ", "ｲ")
                          .Replace("ｩ", "ｳ")
                          .Replace("ｪ", "ｴ")
                          .Replace("ｫ", "ｵ")
                          .Replace("ｬ", "ﾔ")
                          .Replace("ｭ", "ﾕ")
                          .Replace("ｮ", "ﾖ")
                          .Replace("ｯ", "ﾂ").StartsWith(sBigKeyword)
                        || t.Name.Contains(keyword));

            if (KouiKbn > 0)
            {
                //2019-12-04 @duong.vu said: this is a self injection -> search items relate to injection only
                var SELF_INJECTION_KOUIKBN = 28;
                if (KouiKbn == SELF_INJECTION_KOUIKBN) {
                    KouiKbn = 30;
                } 

                switch (KouiKbn)
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

                if (KouiKbn >= 20 && KouiKbn <= 27 || KouiKbn >= 30 && KouiKbn <= 39)
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

            //var DrugKbns = new List<int>();
            //if (DrugKbns != null)
            //{
            //    queryResult = queryResult.Where(p => DrugKbns.Contains(p.DrugKbn));
            //}

            //var IsIncludeUsage = true;
            //if (!IsIncludeUsage)
            //{
            //    queryResult = queryResult.Where(t => t.YohoKbn == 0);
            //}

            if (sinDate > 0)
            {
                queryResult = queryResult.Where(t => t.StartDate <= sinDate && t.EndDate >= sinDate);

                yakkaSyusaiMstList = yakkaSyusaiMstList.Where(t => t.StartDate <= sinDate && t.EndDate >= sinDate).ToList();
            }
            else
            {
                if (queryResult != null && queryResult.ToList().Count > 0)
                {
                    queryResult = queryResult.GroupBy(item => item.ItemCd, (key, group) => group.OrderByDescending(item => item.EndDate).FirstOrDefault());
                }
            }

            if (!string.IsNullOrEmpty(ItemCodeStartWith))
            {
                queryResult = queryResult.Where(t => t.ItemCd.StartsWith(ItemCodeStartWith));
            }

            queryResult = queryResult.Where(t => t.IsNosearch == 0);

            var tenJoinYakkaSyusai = from ten in queryResult
                                     join yakkaSyusaiMstItem in yakkaSyusaiMstList
                                     on new { ten.HpId, ten.YakkaCd, ten.ItemCd, ten.StartDate }
                                     equals new { yakkaSyusaiMstItem.HpId, yakkaSyusaiMstItem.YakkaCd, yakkaSyusaiMstItem.ItemCd, yakkaSyusaiMstItem.StartDate } into yakkaSyusaiMstItems
                                     from yakkaSyusaiItem in yakkaSyusaiMstItems.DefaultIfEmpty()
                                     select new { TenMst = ten, YakkaSyusaiItem = yakkaSyusaiItem };

            var sinKouiCollection = new SinkouiCollection();

            var queryFinal = from ten in tenJoinYakkaSyusai.AsEnumerable()
                             join kouiKbnItem in sinKouiCollection.AsEnumerable()
                             on ten.TenMst.SinKouiKbn equals kouiKbnItem.SinKouiCd into tenKouiKbns
                             from tenKouiKbn in tenKouiKbns.DefaultIfEmpty()
                             select new { TenMst = ten.TenMst, KouiName = tenKouiKbn.SinkouiName, ten.YakkaSyusaiItem };


            var entities = queryFinal.OrderBy(item => item.TenMst.KanaName1).ThenBy(item => item.TenMst.Name).Skip(pageIndex).Take(pageCount);

            var listTenMst = entities.AsEnumerable().Select( x => new { TenMst =  x.TenMst == null ? new TenMst() : x.TenMst }).ToList();
            var listTenMstModels = new List<InputItemModel>();
            if (listTenMst != null)
            {
                foreach (var item in listTenMst)
                {
                    if (item.TenMst != null)
                    {
                        var newItemModel = new InputItemModel(
                                                item.TenMst.HpId,
                                                item.TenMst.ItemCd,
                                                "",
                                                item.TenMst.KanaName1,
                                                item.TenMst.Name,
                                                "",
                                                "",
                                                "",
                                                "",
                                                "",
                                                "",
                                                0,
                                                item.TenMst.DrugKbn,
                                                item.TenMst.MasterSbt,
                                                item.TenMst.BuiKbn
                                            );
                        listTenMstModels.Add(newItemModel);
                    }
                }
            }
            return listTenMstModels;
        }
    }
}
