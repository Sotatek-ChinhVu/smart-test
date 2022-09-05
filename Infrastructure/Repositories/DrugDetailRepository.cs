using Domain.Models.DrugDetail;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Diagnostics;

namespace Infrastructure.Repositories
{
    public class DrugDetailRepository : IDrugDetailRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public DrugDetailRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public IEnumerable<DrugMenuItemModel> GetDrugMenu(int hpId, int sinDate, string itemCd)
        {
            var yjCode = "";
            var drugName = "";
            var quetyTenMsts = _tenantDataContext.TenMsts.Where(
                     item => item.HpId == hpId && new[] { 20, 30 }.Contains(item.SinKouiKbn)
                     && item.StartDate <= sinDate && item.EndDate >= sinDate && item.ItemCd == itemCd
                     );

            var queryDrugInfs = _tenantDataContext.PiProductInfs.AsQueryable();
            var queryM28DrugMsts = _tenantDataContext.M28DrugMst.AsQueryable();
            //Join
            var joinQueryDrugInf = from m28DrugMst in queryM28DrugMsts
                                   join tenItem in quetyTenMsts
                                   on m28DrugMst.KikinCd equals tenItem.ItemCd
                                   join drugInfor in queryDrugInfs
                                   on m28DrugMst.YjCd equals drugInfor.YjCd
                                   select new { drugInfor, tenItem };
            var drugInf = joinQueryDrugInf.FirstOrDefault();
            if (drugInf != null && drugInf.drugInfor != null)
            {
                yjCode = drugInf.drugInfor != null ? drugInf.drugInfor.YjCd ?? string.Empty : string.Empty;
                drugName = drugInf.tenItem != null ? drugInf.tenItem.Name ?? string.Empty : string.Empty;
            }
            // create drug item

            List<DrugMenuItemModel> drugMenuItems = new List<DrugMenuItemModel>();
            //Root
            var newModelInfFirst = new MenuInfModel("医薬品情報", "", 0, 0, 0, "", new DrugDetailModel());
            var newModelItem = new MenuItemModel(newModelInfFirst, new List<MenuInfModel>());
            DrugMenuItemModel rootMenu = new DrugMenuItemModel(newModelItem, new DrugDetailModel());
            drugMenuItems.Add(rootMenu);

            var piInfDetailCollection = _tenantDataContext.PiInfDetails.AsQueryable(); //PI_INF_DETAIL

            var piProductInfCollections = queryDrugInfs.Where(pi => pi.YjCd == yjCode).AsQueryable();

            //Kikaku
            var kikakuCollection = GetKikakuCollectionOrTenpuCollection(yjCode, piInfDetailCollection, piProductInfCollections, 1);
            if (kikakuCollection.Count > 0)
            {
                foreach (var kikakuItem in kikakuCollection)
                {
                    if (kikakuItem.MenuItem.Menu.DbLevel == 0)
                    {
                        CreateSubMenu(kikakuItem, 1, rootMenu);
                    }

                }
            }

            //Tenpu
            var tenpuCollection = GetKikakuCollectionOrTenpuCollection(yjCode, piInfDetailCollection, piProductInfCollections, 2);

            bool siyoFlag = false;
            var currentMenu = new DrugMenuItemModel();
            if (tenpuCollection.Count > 0)
            {
                foreach (var tenpuItem in tenpuCollection)
                {
                    if ((tenpuItem.MenuItem.Menu.DbLevel == 0) || (siyoFlag && tenpuItem.MenuItem.Menu.DbLevel == 1))
                    {
                        if (currentMenu == null || (currentMenu.MenuItem.Menu.DrugMenuName != tenpuItem.MenuItem.Menu.DrugMenuName))
                        {
                            CreateSubMenu(tenpuItem, tenpuItem.MenuItem.Menu.DbLevel + 1, rootMenu);
                            if (tenpuItem.MenuItem.Menu.DbLevel == 0)
                            {
                                siyoFlag = tenpuItem.MenuItem.Menu.DrugMenuName == "【使用上の注意】" || tenpuItem.MenuItem.Menu.DrugMenuName == "【使用上注意】";
                            }
                        }
                        currentMenu = tenpuItem;
                    }
                }
            }

            //Last
            var newModelInfLast = new MenuInfModel("患者向け情報", "", 0, 0, 0, "", new DrugDetailModel());
            var newModelItemLast = new MenuItemModel(newModelInfLast, new List<MenuInfModel>());
            DrugMenuItemModel lastMenu = new DrugMenuItemModel(newModelItemLast, new DrugDetailModel());
            drugMenuItems.Add(lastMenu);




            //適応病名
            var newModelInfTekyoByomeiMenu = new MenuInfModel("適応病名", "", 0, 0, 0, "", new DrugDetailModel());
            var newModelItemTekyoByomeiMenu = new MenuItemModel(newModelInfTekyoByomeiMenu, new List<MenuInfModel>());
            DrugMenuItemModel tekyoByomeiMenu = new DrugMenuItemModel(newModelItemTekyoByomeiMenu, new DrugDetailModel());
            drugMenuItems.Add(tekyoByomeiMenu);

            if (drugMenuItems.Count > 0)
            {
                for (int i = 0; i < drugMenuItems.Count; i++)
                {

                    int selectedIndex = drugMenuItems[0].MenuItem.Childrens.IndexOf(drugMenuItems[i].MenuItem.Menu);
                    var itemDetail = GetDetail(selectedIndex, drugName, itemCd, yjCode, drugMenuItems, drugMenuItems[i], piProductInfCollections, kikakuCollection, tenpuCollection, piInfDetailCollection);
                    drugMenuItems[i].DetailInfor = itemDetail;

                    if (drugMenuItems[i].MenuItem.Childrens.Count > 0)
                    {
                        foreach (var item in drugMenuItems[i].MenuItem.Childrens)
                        {
                            var itemChildrentDetail = GetDetail(selectedIndex, drugName, itemCd, yjCode, drugMenuItems, drugMenuItems[i], piProductInfCollections, kikakuCollection, tenpuCollection, piInfDetailCollection);
                            item.DetailInfor = itemChildrentDetail;
                        }
                    }
                }
            }
            return drugMenuItems;
            }

        private DrugDetailModel GetDetail(int selectedIndex, string drugName, string itemCd, string yjCode, List<DrugMenuItemModel> listDrugMenu, DrugMenuItemModel drugMenuItem, IQueryable<PiProductInf> piProductInfCollections, List<DrugMenuItemModel> kikakuCollection, List<DrugMenuItemModel> tenpuCollection, IQueryable<PiInfDetail> piInfDetailCollection)
        {
            if (selectedIndex >= 0)
            {
                // Show Product Infor
                var piInfCollection = _tenantDataContext.PiInfs.AsQueryable();
                var joinQuery = from piInf in piInfCollection
                                join piProduct in piProductInfCollections
                                on piInf.PiId equals piProduct.PiId
                                select new
                                {
                                    ProductInfo = piProduct,
                                    PiInfo = piInf
                                };
                var syohinData = joinQuery.AsEnumerable().Select(j => new SyohinModel(
                                                                 j.ProductInfo.ProductName,
                                                                 j.PiInfo.PreparationName ?? string.Empty,
                                                                 j.ProductInfo.Unit ?? string.Empty,
                                                                 j.ProductInfo.Maker,
                                                                 j.ProductInfo.Vender ?? string.Empty)).FirstOrDefault();
                var maxLevel = GetMaxLevel(piInfDetailCollection, piProductInfCollections);
                return new DrugDetailModel(1, maxLevel, drugName, syohinData ?? new SyohinModel(), kikakuCollection, tenpuCollection, 0, new YakuModel(), new List<FukuModel>(), new SyokiModel(), new List<SougoModel>(), new List<ChuiModel>(), 0, new List<TenMstByomeiModel>());
            }
            else
            {
                int indexInLevel0 = listDrugMenu.IndexOf(drugMenuItem);
                if (indexInLevel0 == 0)
                {
                    // Show Product Infor
                    var piInfCollection = _tenantDataContext.PiInfs.AsQueryable();
                    var joinQuery = from piInf in piInfCollection
                                    join piProduct in piProductInfCollections
                                    on piInf.PiId equals piProduct.PiId
                                    select new
                                    {
                                        ProductInfo = piProduct,
                                        PiInfo = piInf
                                    };
                    var syohinData = joinQuery.AsEnumerable().Select(j => new SyohinModel(
                                                                     j.ProductInfo.ProductName,
                                                                     j.PiInfo.PreparationName ?? string.Empty,
                                                                     j.ProductInfo.Unit ?? string.Empty,
                                                                     j.ProductInfo.Maker,
                                                                     j.ProductInfo.Vender ?? string.Empty)).FirstOrDefault();
                    var maxLevel = GetMaxLevel(piInfDetailCollection, piProductInfCollections);
                    return new DrugDetailModel(1, maxLevel, drugName, syohinData ?? new SyohinModel(), kikakuCollection, tenpuCollection, 0, new YakuModel(), new List<FukuModel>(), new SyokiModel(), new List<SougoModel>(), new List<ChuiModel>(), 0, new List<TenMstByomeiModel>());
                }
                else if (indexInLevel0 == 1)
                {
                    // show kajamuke

                    var m34DrugInfMainRepo = _tenantDataContext.M34DrugInfoMains.Where(m => m.YjCd == yjCode).AsQueryable();

                    //YakuInf 
                    var yakuInf = GetYakuModel(m34DrugInfMainRepo);

                    //FukuInf
                    var fukuInf = GetFukuModels(m34DrugInfMainRepo);

                    //Syoki
                    var syokiInf = GetSyokiModel(m34DrugInfMainRepo);

                    // Sougo 
                    var sougoInf = GetSougoModels(m34DrugInfMainRepo);

                    // ChuiInf
                    var chuiInf = GetChuiModels(m34DrugInfMainRepo);

                    return new DrugDetailModel(0, 0, "", new SyohinModel(), new List<DrugMenuItemModel>(), new List<DrugMenuItemModel>(), 1, yakuInf ?? new YakuModel(), fukuInf, syokiInf ?? new SyokiModel(), sougoInf, chuiInf, 0, new List<TenMstByomeiModel>());
                }
                else
                {
                    // MdbByomei
                    var mdbByomei = GetTenMstByomeiModels(itemCd);

                    return new DrugDetailModel(0, 0, "", new SyohinModel(), new List<DrugMenuItemModel>(), new List<DrugMenuItemModel>(), 0, new YakuModel(), new List<FukuModel>(), new SyokiModel(), new List<SougoModel>(), new List<ChuiModel>(), 1, mdbByomei);
                }
            }
        }

        private YakuModel? GetYakuModel(IQueryable<M34DrugInfoMain> m34DrugInfMainRepo)
        {
            var m34FormCodeRepo = _tenantDataContext.M34FormCodes.AsQueryable();
            var m34IndicationCodeRepo = _tenantDataContext.M34IndicationCodes.AsQueryable();
            var m34ArCodeRepo = _tenantDataContext.M34ArCodes.AsQueryable();

            var yakuJoin = from drugInfoMain in m34DrugInfMainRepo
                           join formCode in m34FormCodeRepo
                           on drugInfoMain.FormCd equals formCode.FormCd into NewFormCode
                           from newFormCode in NewFormCode.DefaultIfEmpty()
                           join indicationCode in m34IndicationCodeRepo
                           on drugInfoMain.KonoCd equals indicationCode.KonoCd into NewIndicationCode
                           from newIndicationCode in NewIndicationCode.DefaultIfEmpty()
                           join arCode in m34ArCodeRepo
                           on drugInfoMain.FukusayoCd equals arCode.FukusayoCd into NewArCode
                           from newArCode in NewArCode.DefaultIfEmpty()
                           select
                           new
                           {
                               newFormCode.Form,
                               drugInfoMain.Color,
                               drugInfoMain.Mark,
                               newIndicationCode.KonoDetailCmt,
                               newIndicationCode.KonoSimpleCmt,
                               newArCode.FukusayoCmt
                           };

            var model = yakuJoin.Take(1).AsEnumerable().Select(m => new YakuModel(
                                                                    m.Form,
                                                                    m.Color,
                                                                    m.Mark,
                                                                    m.KonoDetailCmt,
                                                                    m.KonoSimpleCmt,
                                                                    m.FukusayoCmt)).FirstOrDefault();
            return model;
        }

        private List<FukuModel> GetFukuModels(IQueryable<M34DrugInfoMain> m34DrugInfMainRepo)
        {
            var m34ArDisconCodeRepo = _tenantDataContext.M34ArDisconCodes.AsQueryable();
            var m34ArDisconRepo = _tenantDataContext.M34ArDiscons.AsQueryable();

            var fukuJoin = from drugInfoMain in m34DrugInfMainRepo
                           join m34ArDiscon in m34ArDisconRepo
                           on drugInfoMain.YjCd equals m34ArDiscon.YjCd
                           join m34ArDisconCode in m34ArDisconCodeRepo
                           on m34ArDiscon.FukusayoCd equals m34ArDisconCode.FukusayoCd
                           select
                           new
                           {
                               m34ArDisconCode.FukusayoCd
                           };

            var models = fukuJoin.AsEnumerable().Select(m => new FukuModel(m.FukusayoCd)).ToList();

            return models;
        }

        private SyokiModel? GetSyokiModel(IQueryable<M34DrugInfoMain> m34DrugInfMainRepo)
        {
            var m34SarSymptomRepo = _tenantDataContext.M34SarSymptomCodes.AsQueryable();
            var syokiJoin = from drugInfMain in m34DrugInfMainRepo
                            join sarSumptom in m34SarSymptomRepo
                            on drugInfMain.FukusayoInitCd equals sarSumptom.FukusayoInitCd
                            select new
                            {
                                sarSumptom.FukusayoInitCmt
                            };
            var model = syokiJoin.Take(1).AsEnumerable().Select(y => new SyokiModel(y.FukusayoInitCmt)).FirstOrDefault();

            return model;
        }

        private List<SougoModel> GetSougoModels(IQueryable<M34DrugInfoMain> m34DrugInfMainRepo)
        {
            var m34InteractionPatCodeRepo = _tenantDataContext.M34InteractionPatCodes.AsQueryable();
            var m34InteractionPatRepo = _tenantDataContext.M34InteractionPats.AsQueryable();
            var sougoJoin = from drugInfMain in m34DrugInfMainRepo
                            join interactionPat in m34InteractionPatRepo
                            on drugInfMain.YjCd equals interactionPat.YjCd
                            join interactionPatCode in m34InteractionPatCodeRepo
                            on interactionPat.InteractionPatCd equals interactionPatCode.InteractionPatCd
                            select new { interactionPatCode.InteractionPatCmt };
            var models = sougoJoin.AsEnumerable().Select(s => new SougoModel(s.InteractionPatCmt)).ToList();
            return models;

        }

        private List<ChuiModel> GetChuiModels(IQueryable<M34DrugInfoMain> m34DrugInfMainRepo)
        {
            var m34PrecautionRepo = _tenantDataContext.M34Precautions.AsQueryable();
            var m34PrecautionCodeRepo = _tenantDataContext.M34PrecautionCodes.AsQueryable();
            var m34PropertyCodeRepo = _tenantDataContext.M34PropertyCodes.ToList();

            var precautionAndPrecautionCodeJoin = from drugInfoMain in m34DrugInfMainRepo
                                                  join precaution in m34PrecautionRepo
                                                  on drugInfoMain.YjCd equals precaution.YjCd
                                                  join precautionCode in m34PrecautionCodeRepo
                                                  on precaution.PrecautionCd equals precautionCode.PrecautionCd
                                                  select new
                                                  {
                                                      precautionCode.PropertyCd,
                                                      precautionCode.PrecautionCmt
                                                  };
            var dataPrecautionAndPrecautionCodeJoin = precautionAndPrecautionCodeJoin.ToList();
            var chuiJoin = from propertyCode in m34PropertyCodeRepo
                           join precautionAndCode in dataPrecautionAndPrecautionCodeJoin
                           on propertyCode.PropertyCd equals precautionAndCode.PropertyCd into PrecautionAndPropertyCode
                           from newPrecaution in PrecautionAndPropertyCode.DefaultIfEmpty()
                           select new
                           {
                               propertyCode.PropertyCd,
                               Property = "(" + propertyCode.Property + ")",
                               PrecautionComment = newPrecaution != null ? (newPrecaution.PrecautionCmt ?? string.Empty) : string.Empty
                           };
            var models = chuiJoin.AsEnumerable().Select(c => new ChuiModel
            (
                 c.PropertyCd,
                 c.Property,
                 c.PrecautionComment
            )).ToList();

            return models;
        }

        private List<TenMstByomeiModel> GetTenMstByomeiModels(string itemCd)
        {
            var tekioByomeiMsts = _tenantDataContext.TekiouByomeiMsts.Where(t => t.ItemCd == itemCd && t.IsInvalid == 0).AsQueryable();
            var byomeiMsts = _tenantDataContext.ByomeiMsts.AsQueryable();
            var joinQuery = from tekioByomei in tekioByomeiMsts
                            join byomeiMst in byomeiMsts
                            on tekioByomei.ByomeiCd equals byomeiMst.ByomeiCd
                            select new
                            {
                                ItemCd = tekioByomei.ItemCd,
                                SByomei = byomeiMst.Sbyomei
                            };
            var list = joinQuery.AsEnumerable().Select(j => new TenMstByomeiModel(j.ItemCd, j.SByomei)).OrderBy(item => item.Byomei).ToList();
            return list;
        }


        private int GetMaxLevel(IQueryable<PiInfDetail> piInfDetailCollection, IQueryable<PiProductInf> piProductInfCollections)
        {
            int rs = 0;

            var joinInfDetailAndProductInf = from piInfDetail in piInfDetailCollection
                                             join piProductInf in piProductInfCollections
                                             on
                                             new
                                             {
                                                 piInfDetail.PiId
                                             }
                                             equals
                                             new
                                             {
                                                 piProductInf.PiId
                                             }
                                             select piInfDetail.Level;
            var dataJoinInfDetail = joinInfDetailAndProductInf.ToList();
            int maxKikaku = 0;
            if (dataJoinInfDetail.Count > 0)
            {
                maxKikaku = joinInfDetailAndProductInf.Max();
            }

            var piInfDetailMainCollection = piInfDetailCollection.Where(pi => pi.Branch == "999");

            var joinMainInfDetailAndProductInf = from piInfDetail in piInfDetailMainCollection
                                                 join piProductInf in piProductInfCollections
                                                 on
                                                 new
                                                 {
                                                     piInfDetail.PiId
                                                 }
                                                 equals
                                                 new
                                                 {
                                                     piProductInf.PiId
                                                 }
                                                 select piInfDetail.Level;
            var dataJoinInfMainCollection = joinMainInfDetailAndProductInf.ToList();
            int maxTenpu = 0;
            if (dataJoinInfMainCollection.Count > 0)
            {
                maxTenpu = joinInfDetailAndProductInf.Max();
            }
            rs = Math.Max(maxKikaku, maxTenpu);
            return rs;
        }


        private List<DrugMenuItemModel> GetKikakuCollectionOrTenpuCollection(string diCode, IQueryable<PiInfDetail> piInfDetailCollection, IQueryable<PiProductInf> piProductInfCollections, int IsKikakuOrTenpu)
        {
            var minPiProductInfCollection = _tenantDataContext.PiProductInfs.Where(pi => pi.YjCd == diCode)
                .GroupBy(pi => pi.YjCd)
                .Select(pi => new { YjCd = pi.Key, PiId = pi.Min(m => m.PiId) });

            var joinPiProductInfAndMinProductInfByJpn = from pi in piProductInfCollections
                                                        join minPi in minPiProductInfCollection
                                                        on
                                                        new
                                                        {
                                                            PiId = pi.PiId,
                                                            YjCd = pi.YjCd,
                                                        }
                                                        equals
                                                        new
                                                        {
                                                            PiId = minPi.PiId,
                                                            YjCd = minPi.YjCd,
                                                        }
                                                        select pi;

            if (IsKikakuOrTenpu == 1)
            {
                var joinInfDetailAndProductInf = from piInfDetail in piInfDetailCollection
                                                 join piProduct in joinPiProductInfAndMinProductInfByJpn
                                                 on
                                                 new
                                                 {
                                                     piInfDetail.PiId,
                                                     piInfDetail.Branch,
                                                     piInfDetail.Jpn,
                                                 }
                                                 equals
                                                 new
                                                 {
                                                     piProduct.PiId,
                                                     piProduct.Branch,
                                                     piProduct.Jpn
                                                 }
                                                 select piInfDetail;

                var listMenuItem = joinInfDetailAndProductInf.Where(mn => mn.Branch != "999").AsEnumerable()
                                    .Select(mn => new DrugMenuItemModel(new MenuItemModel(new MenuInfModel(mn.Text, "", 0, mn.SeqNo, mn.Level, "", new DrugDetailModel()), new List<MenuInfModel>()), new DrugDetailModel()))
                                    .OrderBy(mn => mn.MenuItem.Menu.SeqNo).ToList();
                return listMenuItem;

            }
            else
            {
                var joinInfDetailAndProductInf = from piInfDetail in piInfDetailCollection
                                                 join piProduct in joinPiProductInfAndMinProductInfByJpn
                                                 on
                                                 new
                                                 {
                                                     piInfDetail.PiId,
                                                 }
                                                 equals
                                                 new
                                                 {
                                                     piProduct.PiId,
                                                 }
                                                 select piInfDetail;

                var listMenuItem = joinInfDetailAndProductInf.Where(mn => mn.Branch == "999").AsEnumerable()
                                    .Select(mn => new DrugMenuItemModel(new MenuItemModel(new MenuInfModel(mn.Text, "", 0, mn.SeqNo, mn.Level, "", new DrugDetailModel()), new List<MenuInfModel>()), new DrugDetailModel()))
                                    .OrderBy(mn => mn.MenuItem.Menu.SeqNo).ToList();
                return listMenuItem;
            }
        }


        private void CreateSubMenu(DrugMenuItemModel menuItem, int level, DrugMenuItemModel rootItem)
        {
            var lStartComma = new List<string>() { "＜", "［", "〈", "〔", "（" };

            string title = menuItem.MenuItem.Menu.DrugMenuName;
            title = title
                .Replace("【", "")
                .Replace("】", "");

            string sSetTitle = title;

            for (int i = 0; i < lStartComma.Count; i++)
            {
                int findIndex = sSetTitle.IndexOf(lStartComma[i], StringComparison.OrdinalIgnoreCase);
                string sCut = CIUtil.Copy(sSetTitle, 1, findIndex);
                if (sCut.Length < 1)
                {
                    sSetTitle = sSetTitle.Replace(lStartComma[i], "");
                }
                else
                {
                    string sBuf = CIUtil.Copy(sSetTitle, i + 1, 1);
                    if (sBuf.IndexOf(lStartComma[i], StringComparison.OrdinalIgnoreCase) < 1)
                    {
                        sSetTitle = sCut;
                    }
                }
            }
            title = sSetTitle;

            for (int i = 0; i < level; i++)
            {
                title = "\u3000" + title;
            }
            var menuItemChildren = new MenuInfModel(title, menuItem.MenuItem.Menu.DrugMenuName, 1, 0, level - 1, (rootItem.MenuItem.Childrens.Count + 1).ToString(), new DrugDetailModel());

            rootItem.MenuItem.Childrens.Add(menuItemChildren);
        }
    }
}
