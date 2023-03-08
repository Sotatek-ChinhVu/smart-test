using Domain.Models.DrugDetail;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class DrugDetailRepository : RepositoryBase, IDrugDetailRepository
    {
        public DrugDetailRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public IEnumerable<DrugMenuItemModel> GetDrugMenu(int hpId, int sinDate, string itemCd)
        {
            var yjCode = "";
            var drugName = "";
            var quetyTenMsts = NoTrackingDataContext.TenMsts.Where(
                     item => item.HpId == hpId && new[] { 20, 30 }.Contains(item.SinKouiKbn)
                     && item.StartDate <= sinDate && item.EndDate >= sinDate && item.ItemCd == itemCd
                     );

            var queryDrugInfs = NoTrackingDataContext.PiProductInfs.AsQueryable();
            var queryM28DrugMsts = NoTrackingDataContext.M28DrugMst.AsQueryable();
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
            DrugMenuItemModel rootMenu = new DrugMenuItemModel("医薬品情報", "", 0, 0, 0, "", 0, yjCode);
            drugMenuItems.Add(rootMenu);

            var piInfDetailCollection = NoTrackingDataContext.PiInfDetails.AsQueryable(); //PI_INF_DETAIL

            var piProductInfCollections = queryDrugInfs.Where(pi => pi.YjCd == yjCode).AsQueryable();

            //Kikaku
            var kikakuCollection = GetKikakuCollectionOrTenpuCollection(yjCode, piInfDetailCollection, piProductInfCollections, 1);
            if (kikakuCollection.Count > 0)
            {
                foreach (var kikakuItem in kikakuCollection)
                {
                    if (kikakuItem.DbLevel == 0)
                    {
                        CreateSubMenu(kikakuItem, 1, rootMenu, yjCode);
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
                    if ((tenpuItem.DbLevel == 0) || (siyoFlag && tenpuItem.DbLevel == 1))
                    {
                        if (currentMenu == null || (currentMenu.DrugMenuName != tenpuItem.DrugMenuName))
                        {
                            CreateSubMenu(tenpuItem, tenpuItem.DbLevel + 1, rootMenu, yjCode);
                            if (tenpuItem.DbLevel == 0)
                            {
                                siyoFlag = tenpuItem.DrugMenuName == "【使用上の注意】" || tenpuItem.DrugMenuName == "【使用上注意】";
                            }
                        }
                        currentMenu = tenpuItem;
                    }
                }
            }

            //Last
            DrugMenuItemModel lastMenu = new DrugMenuItemModel("患者向け情報", "", 0, 0, 0, "", 1, yjCode);
            drugMenuItems.Add(lastMenu);

            //適応病名
            DrugMenuItemModel tekyoByomeiMenu = new DrugMenuItemModel("適応病名", "", 0, 0, 0, "", 2, yjCode);
            drugMenuItems.Add(tekyoByomeiMenu);
            return drugMenuItems;
        }

        public DrugDetailModel GetDataDrugSeletedTree(int selectedIndexOfMenuLevel, int level, string drugName, string itemCd, string yjCode)
        {
            var piInfDetailCollection = NoTrackingDataContext.PiInfDetails.AsQueryable();
            var queryDrugInfs = NoTrackingDataContext.PiProductInfs.AsQueryable();
            var piProductInfCollections = queryDrugInfs.Where(pi => pi.YjCd == yjCode).AsQueryable();
            var kikakuCollection = GetKikakuCollectionOrTenpuCollection(yjCode, piInfDetailCollection, piProductInfCollections, 1);
            var tenpuCollection = GetKikakuCollectionOrTenpuCollection(yjCode, piInfDetailCollection, piProductInfCollections, 2);
            return GetDetail(selectedIndexOfMenuLevel, level, drugName, itemCd, yjCode, piProductInfCollections, kikakuCollection, tenpuCollection, piInfDetailCollection);
        }

        private DrugDetailModel GetDetail(int selectedIndex, int level, string drugName, string itemCd, string yjCode, IQueryable<PiProductInf> piProductInfCollections, List<DrugMenuItemModel> kikakuCollection, List<DrugMenuItemModel> tenpuCollection, IQueryable<PiInfDetail> piInfDetailCollection)
        {
            if (level == 0)
            {
                if (selectedIndex == 0)
                {
                    // Show Product Infor
                    var piInfCollection = NoTrackingDataContext.PiInfs.AsQueryable();
                    var joinQuery = from piInf in piInfCollection
                                    join piProduct in piProductInfCollections
                                    on piInf.PiId equals piProduct.PiId
                                    select new
                                    {
                                        ProductInfo = piProduct,
                                        PiInfo = piInf
                                    };
                    var syohinData = joinQuery.AsEnumerable().Select(j => new SyohinModel(
                                                                     j.ProductInfo.ProductName ?? string.Empty,
                                                                     j.PiInfo.PreparationName ?? string.Empty,
                                                                     j.ProductInfo.Unit ?? string.Empty,
                                                                     j.ProductInfo.Maker ?? string.Empty,
                                                                     j.ProductInfo.Vender ?? string.Empty)).FirstOrDefault();
                    var maxLevel = GetMaxLevel(piInfDetailCollection, piProductInfCollections);
                    return new DrugDetailModel(1, maxLevel, drugName, syohinData ?? new SyohinModel(), kikakuCollection, tenpuCollection, 0, new YakuModel(), new List<FukuModel>(), new SyokiModel(), new List<SougoModel>(), new List<ChuiModel>(), 0, new List<TenMstByomeiModel>());
                }
                else if (selectedIndex == 1)
                {
                    // show kajamuke

                    var m34DrugInfMainRepo = NoTrackingDataContext.M34DrugInfoMains.Where(m => m.YjCd == yjCode).AsQueryable();

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
            else if (level > 0 && selectedIndex >= 0)
            {
                // Show Product Infor
                var piInfCollection = NoTrackingDataContext.PiInfs.AsQueryable();
                var joinQuery = from piInf in piInfCollection
                                join piProduct in piProductInfCollections
                                on piInf.PiId equals piProduct.PiId
                                select new
                                {
                                    ProductInfo = piProduct,
                                    PiInfo = piInf
                                };
                var syohinData = joinQuery.AsEnumerable().Select(j => new SyohinModel(
                                                                 j.ProductInfo.ProductName ?? string.Empty,
                                                                 j.PiInfo.PreparationName ?? string.Empty,
                                                                 j.ProductInfo.Unit ?? string.Empty,
                                                                 j.ProductInfo.Maker ?? string.Empty,
                                                                 j.ProductInfo.Vender ?? string.Empty)).FirstOrDefault();
                var maxLevel = GetMaxLevel(piInfDetailCollection, piProductInfCollections);
                return new DrugDetailModel(1, maxLevel, drugName, syohinData ?? new SyohinModel(), kikakuCollection, tenpuCollection, 0, new YakuModel(), new List<FukuModel>(), new SyokiModel(), new List<SougoModel>(), new List<ChuiModel>(), 0, new List<TenMstByomeiModel>());
            }
            return new DrugDetailModel();
        }

        private YakuModel? GetYakuModel(IQueryable<M34DrugInfoMain> m34DrugInfMainRepo)
        {
            var m34FormCodeRepo = NoTrackingDataContext.M34FormCodes.AsQueryable();
            var m34IndicationCodeRepo = NoTrackingDataContext.M34IndicationCodes.AsQueryable();
            var m34ArCodeRepo = NoTrackingDataContext.M34ArCodes.AsQueryable();

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
            var m34ArDisconCodeRepo = NoTrackingDataContext.M34ArDisconCodes.AsQueryable();
            var m34ArDisconRepo = NoTrackingDataContext.M34ArDiscons.AsQueryable();

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
            var m34SarSymptomRepo = NoTrackingDataContext.M34SarSymptomCodes.AsQueryable();
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
            var m34InteractionPatCodeRepo = NoTrackingDataContext.M34InteractionPatCodes.AsQueryable();
            var m34InteractionPatRepo = NoTrackingDataContext.M34InteractionPats.AsQueryable();
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
            var m34PrecautionRepo = NoTrackingDataContext.M34Precautions.AsQueryable();
            var m34PrecautionCodeRepo = NoTrackingDataContext.M34PrecautionCodes.AsQueryable();
            var m34PropertyCodeRepo = NoTrackingDataContext.M34PropertyCodes.ToList();

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
            var tekioByomeiMsts = NoTrackingDataContext.TekiouByomeiMsts.Where(t => t.ItemCd == itemCd && t.IsInvalid == 0).AsQueryable();
            var byomeiMsts = NoTrackingDataContext.ByomeiMsts.AsQueryable();
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
            var minPiProductInfCollection = NoTrackingDataContext.PiProductInfs.Where(pi => pi.YjCd == diCode)
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
                                    .Select(mn => new DrugMenuItemModel(mn.Text ?? string.Empty, "", 0, mn.SeqNo, mn.Level, "", 0, diCode))
                                    .OrderBy(mn => mn.SeqNo).ToList();
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
                                    .Select(mn => new DrugMenuItemModel(mn.Text ?? string.Empty, "", 0, mn.SeqNo, mn.Level, "", 0, diCode))
                                    .OrderBy(mn => mn.SeqNo).ToList();
                return listMenuItem;
            }
        }


        private void CreateSubMenu(DrugMenuItemModel menuItem, int level, DrugMenuItemModel rootItem, string yjcode)
        {
            var lStartComma = new List<string>() { "＜", "［", "〈", "〔", "（" };

            string title = menuItem.DrugMenuName;
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
            if (level == 2)
            {
                // children-level2
                var menuItemChildren = new DrugMenuItemModel(title, menuItem.DrugMenuName, 2, 0, level - 1, (rootItem.Children.Count + 1).ToString(), rootItem.Children.Count, yjcode);
                rootItem.Children[rootItem.Children.Count - 1].Children.Add(menuItemChildren);
            }
            else
            {
                var menuItemChildren = new DrugMenuItemModel(title, menuItem.DrugMenuName, 1, 0, level - 1, (rootItem.Children.Count + 1).ToString(), rootItem.Children.Count, yjcode);
                rootItem.Children.Add(menuItemChildren);
            }
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public List<TenMstByomeiModel> GetZaiganIsoItems(int hpId, int seikyuYm)
        {
            int firstDateOfMonth = seikyuYm * 100 + 1;
            int lastDateOfMonth = seikyuYm * 100 + 31;
            return NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                               && item.StartDate <= lastDateOfMonth
                                                               && item.EndDate >= firstDateOfMonth
                                                               && item.CdKbn == "C"
                                                               && item.CdKbnno == 3
                                                               && item.CdEdano == 0
                                                               && (item.CdKouno == 1 || item.CdKouno == 2 || item.CdKouno == 4)
                                                               && item.IsDeleted == DeleteTypes.None)
                                                .Select(item => new TenMstByomeiModel(item.ItemCd, item.Name ?? string.Empty))
                                                .ToList();
        }
    }
}
