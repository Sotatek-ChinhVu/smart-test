using Domain.Models.RaiinListMst;
using Domain.Models.RaiinListSetting;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class RaiinListSettingRepository : RepositoryBase, IRaiinListSettingRepository
    {
        public RaiinListSettingRepository(ITenantProvider tenantProvider) : base(tenantProvider) { }

        public List<FilingCategoryModel> GetFilingcategoryCollection(int hpId)
        {
            return NoTrackingDataContext.FilingCategoryMst.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None)
                        .OrderBy(f => f.SortNo)
                        .Select(x=> new FilingCategoryModel(x.HpId, x.SortNo, x.CategoryCd, x.CategoryName ?? string.Empty, x.DspKanzok)).ToList();
        }

        public List<RaiinListMstModel> GetRaiiinListSetting(int hpId)
        {
            var list = NoTrackingDataContext.RaiinListMsts.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None).ToList();

            var listDetail = GetActionGroupValueCollection(hpId);

            var memJoin = from mst in list
                          join detail in listDetail
                          on mst.GrpId equals detail.GrpId into details
                          select new { 
                              Mst = mst, 
                              Detals = details 
                          };

            return memJoin.Select(item => new RaiinListMstModel(item.Mst.GrpId, item.Mst.GrpName ?? string.Empty, item.Mst.SortNo, item.Detals.ToList())).ToList();
        }

        private List<RaiinListDetailModel> GetActionGroupValueCollection(int hpId)
        {
            var details = NoTrackingDataContext.RaiinListDetails.Where(item => item.HpId == hpId).ToList();

            var raiinListKouiCollection = NoTrackingDataContext.RaiinListKouis.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None).ToList();

            var raiinListItemCollection = GetRaiinListItemCollection(hpId);

            var raiinListDocCollection = GetRaiinListDocCollection(hpId);

            var raiinListFileCollection = GetRaiinListFileCollection(hpId);

            List<RaiinListDetailModel> result = new List<RaiinListDetailModel>();
            foreach (var item in details)
            {
                //MedicalSupervision
                RaiinListKouiModel iKanModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.IKanId, item.GrpId, item.KbnCd);

                //AtHome
                RaiinListKouiModel zaitakuModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.ZaitakuId, item.GrpId, item.KbnCd);

                //Internal
                RaiinListKouiModel naifukuModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.NaifukulId, item.GrpId, item.KbnCd);

                //Clothing
                RaiinListKouiModel clothingModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.TonpukuId, item.GrpId, item.KbnCd);

                //External
                RaiinListKouiModel gaiyoModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.GaiyoId, item.GrpId, item.KbnCd);

                //SelfNote
                RaiinListKouiModel selfNoteModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.JikochuId, item.GrpId, item.KbnCd);

                //Injection1
                RaiinListKouiModel hikaKinchuModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.HikaKinchuId, item.GrpId, item.KbnCd);

                //Injection2
                RaiinListKouiModel jochuModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.JochuId, item.GrpId, item.KbnCd);

                //Injection3
                RaiinListKouiModel tentekiModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.TentekiId, item.GrpId, item.KbnCd);

                //Injection4
                RaiinListKouiModel tachuModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.TachuId, item.GrpId, item.KbnCd);

                //treatment
                RaiinListKouiModel shochiModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.ShochiId, item.GrpId, item.KbnCd);

                //Surgery
                RaiinListKouiModel shujutsuModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.ShujutsuId, item.GrpId, item.KbnCd);

                //Anesthesia
                RaiinListKouiModel masuiModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.MasuiId, item.GrpId, item.KbnCd);

                //Inspection1
                RaiinListKouiModel kentaiModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.KentaiId, item.GrpId, item.KbnCd);

                //Inspection2
                RaiinListKouiModel seitaiModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.Seitai2Id, item.GrpId, item.KbnCd);

                //Inspection3
                RaiinListKouiModel sonohokaModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.SonohokaId, item.GrpId, item.KbnCd);

                //Image
                RaiinListKouiModel gazoModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.GazoId, item.GrpId, item.KbnCd);

                //Rehaha
                RaiinListKouiModel rihaModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.RihaId, item.GrpId, item.KbnCd);

                //Spirit
                RaiinListKouiModel seishinModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.SeishinId, item.GrpId, item.KbnCd);

                //Radiation
                RaiinListKouiModel hoshaModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.HoshaId, item.GrpId, item.KbnCd);

                //Pathology
                RaiinListKouiModel byoriModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.ByoriId, item.GrpId, item.KbnCd);

                //OwnCost
                RaiinListKouiModel jihiModel = CreateRaiinListKouiModel(hpId, raiinListKouiCollection, KouiKbnIdConst.JihiId, item.GrpId, item.KbnCd);

                result.Add(new RaiinListDetailModel(item.GrpId, item.KbnCd, item.SortNo, item.KbnName ?? string.Empty, item.ColorCd ?? string.Empty, item.IsDeleted, raiinListDocCollection.Where(x => x.GrpId == item.GrpId && x.KbnCd == item.KbnCd).ToList(),
                                                                                                                                    raiinListItemCollection.Where(x => x.GrpId == item.GrpId && x.KbnCd == item.KbnCd).ToList(),
                                                                                                                                    raiinListFileCollection.Where(x => x.GrpId == item.GrpId && x.KbnCd == item.KbnCd).ToList(),
                                                                                                                                    new KouiKbnCollectionModel(iKanModel,
                                                                                                                                                               zaitakuModel,
                                                                                                                                                               naifukuModel,
                                                                                                                                                               clothingModel,
                                                                                                                                                               gaiyoModel,
                                                                                                                                                               hikaKinchuModel,
                                                                                                                                                               jochuModel,
                                                                                                                                                               tentekiModel,
                                                                                                                                                               tachuModel,
                                                                                                                                                               selfNoteModel,
                                                                                                                                                               shochiModel,
                                                                                                                                                               shujutsuModel,
                                                                                                                                                               masuiModel,
                                                                                                                                                               kentaiModel,
                                                                                                                                                               seitaiModel,
                                                                                                                                                               sonohokaModel,
                                                                                                                                                               gazoModel,
                                                                                                                                                               rihaModel,
                                                                                                                                                               seishinModel,
                                                                                                                                                               hoshaModel,
                                                                                                                                                               byoriModel,
                                                                                                                                                               jihiModel)));
            }
            return result;
        }

        private List<RaiinListItemModel> GetRaiinListItemCollection(int hpId)
        {
            int stDate = CIUtil.DateTimeToInt(DateTime.Now);

            var items = NoTrackingDataContext.RaiinListItems.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None);

            var joinQuerry = from raiinkbItem in items
                             select new {
                                 RaiinkbItem = raiinkbItem, 
                                 TenMst = NoTrackingDataContext.TenMsts.FirstOrDefault(t => t.StartDate <= stDate && t.EndDate >= stDate && t.ItemCd == raiinkbItem.ItemCd)
                             };

            return joinQuerry.AsEnumerable().Select(x => new RaiinListItemModel(x.RaiinkbItem.HpId,
                                                                                x.RaiinkbItem.GrpId,
                                                                                x.RaiinkbItem.KbnCd,
                                                                                x.RaiinkbItem.ItemCd ?? string.Empty,
                                                                                x.RaiinkbItem.SeqNo,
                                                                                x.TenMst == null ? string.Empty : x.TenMst.Name ?? string.Empty,
                                                                                x.RaiinkbItem.IsExclude,
                                                                                false,
                                                                                x.RaiinkbItem.IsDeleted,
                                                                                false
                                                                                )).ToList();
        }
        public List<RaiinListDocModel> GetRaiinListDocCollection(int hpId)
        {
            var raiinListDocs = NoTrackingDataContext.RaiinListDocs.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None);

            var joinQuerry = from raiinListDoc in raiinListDocs
                             select new { 
                                 RaiinListDoc = raiinListDoc, 
                                 DocCategory = NoTrackingDataContext.DocCategoryMsts.FirstOrDefault(item => item.HpId == hpId && item.CategoryCd == raiinListDoc.CategoryCd)  
                             };

            return joinQuerry.AsEnumerable().Select(x => new RaiinListDocModel(x.RaiinListDoc.HpId,
                                                                               x.RaiinListDoc.GrpId,
                                                                               x.RaiinListDoc.KbnCd,
                                                                               x.RaiinListDoc.SeqNo,
                                                                               x.RaiinListDoc.CategoryCd,
                                                                               x.DocCategory == null ? string.Empty : x.DocCategory.CategoryName ?? string.Empty,
                                                                               x.RaiinListDoc.IsDeleted,
                                                                               false)).ToList();
        }
        public List<RaiinListFileModel> GetRaiinListFileCollection(int hpId)
        {
            var raiinListFiles = NoTrackingDataContext.RaiinListFile.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None);

            var joinQuerry = from raiinListFile in raiinListFiles
                             select new { 
                                 RaiinListFile = raiinListFile, 
                                 FilingCategory = NoTrackingDataContext.FilingCategoryMst.FirstOrDefault(x => x.HpId == hpId && x.CategoryCd == raiinListFile.CategoryCd)
                             };

            return joinQuerry.AsEnumerable().Select(x => new RaiinListFileModel(x.RaiinListFile.HpId,
                                                                                x.RaiinListFile.GrpId,
                                                                                x.RaiinListFile.KbnCd,
                                                                                x.RaiinListFile.CategoryCd,
                                                                                x.FilingCategory == null ? string.Empty : x.FilingCategory.CategoryName ?? string.Empty,
                                                                                x.RaiinListFile.SeqNo,
                                                                                x.RaiinListFile.IsDeleted,
                                                                                false)).ToList();
        }

        private RaiinListKouiModel CreateRaiinListKouiModel(int hpId, List<RaiinListKoui> raiinListKouiCollection, int kouiKbnId, int grpId, int KbnCd)
        {
            var raiinListKoui = raiinListKouiCollection.FirstOrDefault(r => r.KouiKbnId == kouiKbnId && r.GrpId == grpId && r.KbnCd == KbnCd);

            if (raiinListKoui == null)
            {
                raiinListKoui = new RaiinListKoui()
                {
                    KouiKbnId = kouiKbnId,
                    KbnCd = KbnCd,
                    HpId = hpId,
                    IsDeleted = DeleteTypes.Deleted
                };
            }
            return new RaiinListKouiModel(raiinListKoui.HpId, raiinListKoui.GrpId, raiinListKoui.KbnCd, raiinListKoui.SeqNo, raiinListKoui.KouiKbnId, raiinListKoui.IsDeleted);
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
