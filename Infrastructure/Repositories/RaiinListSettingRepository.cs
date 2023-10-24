using Domain.Models.RaiinListMst;
using Domain.Models.RaiinListSetting;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories
{
    public class RaiinListSettingRepository : RepositoryBase, IRaiinListSettingRepository
    {
        private readonly StackExchange.Redis.IDatabase _cache;
        private string key;
        private string RaiinListMstCacheKey
        {
            get => $"{key}-RaiinListMstCacheKey";
        }

        public RaiinListSettingRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
            key = GetCacheKey().Replace(this.GetType().Name, typeof(FlowSheetRepository).Name);
            _cache = RedisConnectorHelper.Connection.GetDatabase();
        }

        public List<FilingCategoryModel> GetFilingcategoryCollection(int hpId)
        {
            return NoTrackingDataContext.FilingCategoryMst.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None)
                        .OrderBy(f => f.SortNo)
                        .Select(x => new FilingCategoryModel(x.HpId, x.SortNo, x.CategoryCd, x.CategoryName ?? string.Empty, x.DspKanzok)).ToList();
        }

        public (List<RaiinListMstModel> raiinListMsts, int grpIdMax, int sortNoMax) GetRaiiinListSetting(int hpId)
        {
            var list = NoTrackingDataContext.RaiinListMsts.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None).ToList();
            var listRaiinMstAll = NoTrackingDataContext.RaiinListMsts.Where(x => x.HpId == hpId).ToList();
            var grpIdMstMax = listRaiinMstAll.Select(x => x.GrpId).Max();
            var sortNoMstMax = list.Select(x => x.SortNo).Max();

            var listDetail = GetActionGroupValueCollection(hpId);

            var memJoin = from mst in list
                          join detail in listDetail
                          on mst.GrpId equals detail.GrpId into details
                          select new
                          {
                              Mst = mst,
                              Detals = details.OrderBy(x => x.SortNo).ToList(),
                              SortNoDetailMax = details.Select(x => x.SortNoDetailMax).FirstOrDefault(),
                              KbnCdDetailMax = details.Select(x => x.KbnCdDetailMax).FirstOrDefault()
                          };

            return (memJoin.Select(item => new RaiinListMstModel(item.Mst.GrpId, item.Mst.GrpName ?? string.Empty, item.Mst.SortNo, item.Mst.IsDeleted, item.SortNoDetailMax, item.KbnCdDetailMax, item.Detals.ToList())).OrderBy(x => x.SortNo).ToList(), grpIdMstMax, sortNoMstMax);
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
                var sortNoDetailMax = details.Where(x => x.GrpId == item.GrpId && x.IsDeleted == 0).OrderByDescending(x => x.SortNo).Select(x => x.SortNo).FirstOrDefault();
                var kbnCdDetailMax = details.Where(x => x.GrpId == item.GrpId).OrderByDescending(x => x.KbnCd).Select(x => x.KbnCd).FirstOrDefault();
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

                result.Add(new RaiinListDetailModel(item.GrpId, item.KbnCd, item.SortNo, item.KbnName ?? string.Empty, item.ColorCd ?? string.Empty, item.IsDeleted, false, sortNoDetailMax, kbnCdDetailMax,
                                                                                                                                    raiinListDocCollection.Where(x => x.GrpId == item.GrpId && x.KbnCd == item.KbnCd).ToList(),
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
            int stDate = CIUtil.DateTimeToInt(CIUtil.GetJapanDateTimeNow());

            var items = NoTrackingDataContext.RaiinListItems.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None);

            var joinQuerry = from raiinkbItem in items
                             select new
                             {
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
                                                                                x.RaiinkbItem.IsDeleted
                                                                                )).ToList();
        }
        public List<RaiinListDocModel> GetRaiinListDocCollection(int hpId)
        {
            var raiinListDocs = NoTrackingDataContext.RaiinListDocs.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None);

            var joinQuerry = from raiinListDoc in raiinListDocs
                             select new
                             {
                                 RaiinListDoc = raiinListDoc,
                                 DocCategory = NoTrackingDataContext.DocCategoryMsts.FirstOrDefault(item => item.HpId == hpId && item.CategoryCd == raiinListDoc.CategoryCd)
                             };

            return joinQuerry.AsEnumerable().Select(x => new RaiinListDocModel(x.RaiinListDoc.HpId,
                                                                               x.RaiinListDoc.GrpId,
                                                                               x.RaiinListDoc.KbnCd,
                                                                               x.RaiinListDoc.SeqNo,
                                                                               x.RaiinListDoc.CategoryCd,
                                                                               x.DocCategory == null ? string.Empty : x.DocCategory.CategoryName ?? string.Empty,
                                                                               x.RaiinListDoc.IsDeleted)).ToList();
        }
        public List<RaiinListFileModel> GetRaiinListFileCollection(int hpId)
        {
            var raiinListFiles = NoTrackingDataContext.RaiinListFile.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None);

            var joinQuerry = from raiinListFile in raiinListFiles
                             select new
                             {
                                 RaiinListFile = raiinListFile,
                                 FilingCategory = NoTrackingDataContext.FilingCategoryMst.FirstOrDefault(x => x.HpId == hpId && x.CategoryCd == raiinListFile.CategoryCd)
                             };

            return joinQuerry.AsEnumerable().Select(x => new RaiinListFileModel(x.RaiinListFile.HpId,
                                                                                x.RaiinListFile.GrpId,
                                                                                x.RaiinListFile.KbnCd,
                                                                                x.RaiinListFile.CategoryCd,
                                                                                x.FilingCategory == null ? string.Empty : x.FilingCategory.CategoryName ?? string.Empty,
                                                                                x.RaiinListFile.SeqNo,
                                                                                x.RaiinListFile.IsDeleted)).ToList();
        }

        private RaiinListKouiModel CreateRaiinListKouiModel(int hpId, List<RaiinListKoui> raiinListKouiCollection, int kouiKbnId, int grpId, int KbnCd)
        {
            var raiinListKoui = raiinListKouiCollection.FirstOrDefault(r => r.KouiKbnId == kouiKbnId && r.GrpId == grpId && r.KbnCd == KbnCd);

            if (raiinListKoui == null)
            {
                raiinListKoui = new RaiinListKoui()
                {
                    KouiKbnId = kouiKbnId,
                    GrpId = grpId,
                    KbnCd = KbnCd,
                    HpId = hpId,
                    IsDeleted = DeleteTypes.Deleted
                };
            }
            return new RaiinListKouiModel(raiinListKoui.HpId, raiinListKoui.GrpId, raiinListKoui.KbnCd, raiinListKoui.SeqNo, raiinListKoui.KouiKbnId, raiinListKoui.IsDeleted);
        }

        public bool SaveRaiinListSetting(int hpId, List<RaiinListMstModel> raiinListMstModels, int userId)
        {
            bool resultSave = false;

            IQueryable<RaiinListMst> databaseRaiinMst = TrackingDataContext.RaiinListMsts.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None);
            IQueryable<RaiinListDetail> databaseRaiinDetails = TrackingDataContext.RaiinListDetails.Where(item => item.HpId == hpId);

            IQueryable<RaiinListItem> databaseRaiinListItems = TrackingDataContext.RaiinListItems.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None);
            IQueryable<RaiinListDoc> databaseRaiinListDocs = TrackingDataContext.RaiinListDocs.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None);
            IQueryable<RaiinListFile> databaseRaiinListFiles = TrackingDataContext.RaiinListFile.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None);
            IQueryable<RaiinListKoui> databaseRaiinListKoui = TrackingDataContext.RaiinListKouis.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None);

            List<RaiinListDetailModel> detailDeletedList = new List<RaiinListDetailModel>();
            List<RaiinListDetailModel> detailDeletedCurrentList = new List<RaiinListDetailModel>();
            List<RaiinListKouiModel> kouiDeleteList = new List<RaiinListKouiModel>();
            List<RaiinListKouiModel> kouiAddList = new List<RaiinListKouiModel>();
            List<RaiinListItemModel> itemDeleteList = new List<RaiinListItemModel>();
            List<RaiinListItemModel> itemAddList = new List<RaiinListItemModel>();
            List<RaiinListDocModel> docDeleteList = new List<RaiinListDocModel>();
            List<RaiinListDocModel> docAddList = new List<RaiinListDocModel>();
            List<RaiinListFileModel> fileDeleteList = new List<RaiinListFileModel>();
            List<RaiinListFileModel> fileAddList = new List<RaiinListFileModel>();

            // For only update sortno at the raiinlist detail.
            List<RaiinListDetailModel> detailDeleteds = new List<RaiinListDetailModel>();
            List<RaiinListKouiModel> kouiAdds = new List<RaiinListKouiModel>();
            List<RaiinListItemModel> itemAdds = new List<RaiinListItemModel>();
            List<RaiinListDocModel> docAdds = new List<RaiinListDocModel>();
            List<RaiinListFileModel> fileAdds = new List<RaiinListFileModel>();

            void SaveKouKbnCollection(KouiKbnCollectionModel kouCollection, int grpId, int kbnCd, bool isOnlySwapSortNo = false)
            {
                if (kouCollection.IKanModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.IKanModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.IKanModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.IKanModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.IKanModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.IKanModel.SeqNo != 0 && kouCollection.IKanModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.IKanModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.IKanModel.IsDeleted == DeleteTypes.None && kouCollection.IKanModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.IKanModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.IKanModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.IKanModel);
                            kouiAddList.Add(kouCollection.IKanModel);
                        }
                    }
                }

                if (kouCollection.ZaitakuModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.ZaitakuModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.ZaitakuModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.ZaitakuModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.ZaitakuModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.ZaitakuModel.SeqNo != 0 && kouCollection.ZaitakuModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.ZaitakuModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.ZaitakuModel.IsDeleted == DeleteTypes.None && kouCollection.ZaitakuModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.ZaitakuModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.ZaitakuModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.ZaitakuModel);
                            kouiAddList.Add(kouCollection.ZaitakuModel);
                        }
                    }
                }

                if (kouCollection.NaifukuModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.NaifukuModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.NaifukuModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.NaifukuModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.NaifukuModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.NaifukuModel.SeqNo != 0 && kouCollection.NaifukuModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.NaifukuModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.NaifukuModel.IsDeleted == DeleteTypes.None && kouCollection.NaifukuModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.NaifukuModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.NaifukuModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.NaifukuModel);
                            kouiAddList.Add(kouCollection.NaifukuModel);
                        }
                    }
                }

                if (kouCollection.TonpukuModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.TonpukuModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.TonpukuModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.TonpukuModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.TonpukuModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.TonpukuModel.SeqNo != 0 && kouCollection.TonpukuModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.TonpukuModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.TonpukuModel.IsDeleted == DeleteTypes.None && kouCollection.TonpukuModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.TonpukuModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.TonpukuModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.TonpukuModel);
                            kouiAddList.Add(kouCollection.TonpukuModel);
                        }
                    }
                }

                if (kouCollection.GaiyoModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.GaiyoModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.GaiyoModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.GaiyoModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.GaiyoModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.GaiyoModel.SeqNo != 0 && kouCollection.GaiyoModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.GaiyoModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.GaiyoModel.IsDeleted == DeleteTypes.None && kouCollection.GaiyoModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.GaiyoModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.GaiyoModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.GaiyoModel);
                            kouiAddList.Add(kouCollection.GaiyoModel);
                        }
                    }
                }

                if (kouCollection.HikaKinchuModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.HikaKinchuModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.HikaKinchuModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.HikaKinchuModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.HikaKinchuModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.HikaKinchuModel.SeqNo != 0 && kouCollection.HikaKinchuModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.HikaKinchuModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.HikaKinchuModel.IsDeleted == DeleteTypes.None && kouCollection.HikaKinchuModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.HikaKinchuModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.HikaKinchuModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.HikaKinchuModel);
                            kouiAddList.Add(kouCollection.HikaKinchuModel);
                        }
                    }
                }

                if (kouCollection.JochuModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.JochuModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.JochuModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.JochuModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.JochuModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.JochuModel.SeqNo != 0 && kouCollection.JochuModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.JochuModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.JochuModel.IsDeleted == DeleteTypes.None && kouCollection.JochuModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.JochuModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.JochuModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.JochuModel);
                            kouiAddList.Add(kouCollection.JochuModel);
                        }
                    }
                }

                if (kouCollection.TentekiModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.TentekiModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.TentekiModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.TentekiModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.TentekiModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.TentekiModel.SeqNo != 0 && kouCollection.TentekiModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.TentekiModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.TentekiModel.IsDeleted == DeleteTypes.None && kouCollection.TentekiModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.TentekiModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.TentekiModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.TentekiModel);
                            kouiAddList.Add(kouCollection.TentekiModel);
                        }
                    }
                }

                if (kouCollection.TachuModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.TachuModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.TachuModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.TachuModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.TachuModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.TachuModel.SeqNo != 0 && kouCollection.TachuModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.TachuModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.TachuModel.IsDeleted == DeleteTypes.None && kouCollection.TachuModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.TachuModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.TachuModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.TachuModel);
                            kouiAddList.Add(kouCollection.TachuModel);
                        }
                    }
                }

                if (kouCollection.JikochuModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.JikochuModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.JikochuModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.JikochuModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.JikochuModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.JikochuModel.SeqNo != 0 && kouCollection.JikochuModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.JikochuModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.JikochuModel.IsDeleted == DeleteTypes.None && kouCollection.JikochuModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.JikochuModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.JikochuModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.JikochuModel);
                            kouiAddList.Add(kouCollection.JikochuModel);
                        }
                    }
                }

                if (kouCollection.ShochiModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.ShochiModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.ShochiModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.ShochiModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.ShochiModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.ShochiModel.SeqNo != 0 && kouCollection.ShochiModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.ShochiModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.ShochiModel.IsDeleted == DeleteTypes.None && kouCollection.ShochiModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.ShochiModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.ShochiModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.ShochiModel);
                            kouiAddList.Add(kouCollection.ShochiModel);
                        }
                    }
                }

                if (kouCollection.ShujutsuModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.ShujutsuModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.ShujutsuModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.ShujutsuModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.ShujutsuModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.ShujutsuModel.SeqNo != 0 && kouCollection.ShujutsuModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.ShujutsuModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.ShujutsuModel.IsDeleted == DeleteTypes.None && kouCollection.ShujutsuModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.ShujutsuModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.ShujutsuModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.ShujutsuModel);
                            kouiAddList.Add(kouCollection.ShujutsuModel);
                        }
                    }
                }

                if (kouCollection.MasuiModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.MasuiModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.MasuiModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.MasuiModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.MasuiModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.MasuiModel.SeqNo != 0 && kouCollection.MasuiModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.MasuiModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.MasuiModel.IsDeleted == DeleteTypes.None && kouCollection.MasuiModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.MasuiModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.MasuiModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.MasuiModel);
                            kouiAddList.Add(kouCollection.MasuiModel);
                        }
                    }
                }

                if (kouCollection.KentaiModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.KentaiModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.KentaiModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.KentaiModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.KentaiModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.KentaiModel.SeqNo != 0 && kouCollection.KentaiModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.KentaiModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.KentaiModel.IsDeleted == DeleteTypes.None && kouCollection.KentaiModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.KentaiModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.KentaiModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.KentaiModel);
                            kouiAddList.Add(kouCollection.KentaiModel);
                        }
                    }
                }

                if (kouCollection.SeitaiModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.SeitaiModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.SeitaiModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.SeitaiModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.SeitaiModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.SeitaiModel.SeqNo != 0 && kouCollection.SeitaiModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.SeitaiModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.SeitaiModel.IsDeleted == DeleteTypes.None && kouCollection.SeitaiModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.SeitaiModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.SeitaiModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.SeitaiModel);
                            kouiAddList.Add(kouCollection.SeitaiModel);
                        }
                    }
                }

                if (kouCollection.SonohokaModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.SonohokaModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.SonohokaModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.SonohokaModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.SonohokaModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.SonohokaModel.SeqNo != 0 && kouCollection.SonohokaModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.SonohokaModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.SonohokaModel.IsDeleted == DeleteTypes.None && kouCollection.SonohokaModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.SonohokaModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.SonohokaModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.SonohokaModel);
                            kouiAddList.Add(kouCollection.SonohokaModel);
                        }
                    }
                }

                if (kouCollection.GazoModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.GazoModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.GazoModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.GazoModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.GazoModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.GazoModel.SeqNo != 0 && kouCollection.GazoModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.GazoModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.GazoModel.IsDeleted == DeleteTypes.None && kouCollection.GazoModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.GazoModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.GazoModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.GazoModel);
                            kouiAddList.Add(kouCollection.GazoModel);
                        }
                    }
                }

                if (kouCollection.RihaModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.RihaModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.RihaModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.RihaModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.RihaModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.RihaModel.SeqNo != 0 && kouCollection.RihaModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.RihaModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.RihaModel.IsDeleted == DeleteTypes.None && kouCollection.RihaModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.RihaModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.RihaModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.RihaModel);
                            kouiAddList.Add(kouCollection.RihaModel);
                        }
                    }
                }

                if (kouCollection.SeishinModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.SeishinModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.SeishinModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.SeishinModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.SeishinModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.SeishinModel.SeqNo != 0 && kouCollection.SeishinModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.SeishinModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.SeishinModel.IsDeleted == DeleteTypes.None && kouCollection.SeishinModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.SeishinModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.SeishinModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.SeishinModel);
                            kouiAddList.Add(kouCollection.SeishinModel);
                        }
                    }
                }

                if (kouCollection.HoshaModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.HoshaModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.HoshaModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.HoshaModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.HoshaModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.HoshaModel.SeqNo != 0 && kouCollection.HoshaModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.HoshaModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.HoshaModel.IsDeleted == DeleteTypes.None && kouCollection.HoshaModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.HoshaModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.HoshaModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.HoshaModel);
                            kouiAddList.Add(kouCollection.HoshaModel);
                        }
                    }
                }

                if (kouCollection.ByoriModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.ByoriModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.ByoriModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.ByoriModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.ByoriModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.ByoriModel.SeqNo != 0 && kouCollection.ByoriModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.ByoriModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.ByoriModel.IsDeleted == DeleteTypes.None && kouCollection.ByoriModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.ByoriModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.ByoriModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.ByoriModel);
                            kouiAddList.Add(kouCollection.ByoriModel);
                        }
                    }
                }

                if (kouCollection.JihiModel != null)
                {
                    RaiinListKoui? updateKoui = databaseRaiinListKoui.FirstOrDefault(x => x.KouiKbnId == kouCollection.JihiModel.KouiKbnId && x.KbnCd == kbnCd && x.GrpId == grpId && x.SeqNo == kouCollection.JihiModel.SeqNo);
                    if (updateKoui != null)
                    {
                        updateKoui.IsDeleted = kouCollection.JihiModel.IsDeleted;
                        if (updateKoui.IsDeleted == DeleteTypes.Deleted)
                        {
                            kouiDeleteList.Add(kouCollection.JihiModel);
                        }

                        if (isOnlySwapSortNo && kouCollection.JihiModel.SeqNo != 0 && kouCollection.JihiModel.IsDeleted == DeleteTypes.None)
                        {
                            kouiAdds.Add(kouCollection.JihiModel);
                        }
                    }
                    else
                    {
                        if (kouCollection.JihiModel.IsDeleted == DeleteTypes.None && kouCollection.JihiModel.SeqNo == 0)
                        {
                            TrackingDataContext.RaiinListKouis.Add(new RaiinListKoui()
                            {
                                IsDeleted = kouCollection.JihiModel.IsDeleted,
                                HpId = hpId,
                                GrpId = grpId,
                                KbnCd = kbnCd,
                                UpdateId = userId,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                KouiKbnId = kouCollection.JihiModel.KouiKbnId,
                            });

                            kouiDeleteList.Add(kouCollection.JihiModel);
                            kouiAddList.Add(kouCollection.JihiModel);
                        }
                    }
                }
            }

            void ExceuteCommandRawAddModel()
            {
                // Add by koui
                kouiAddList.AddRange(kouiAdds);
                if (kouiAddList.Any())
                {
                    var groupKoui = kouiAddList.GroupBy(item => new { item.GrpId, item.KbnCd })
                                               .Select(item => new { item.Key.GrpId, item.Key.KbnCd, ListKoui = item.Select(x => x.KouiKbnId) })
                                               .ToList();

                    foreach (var koui in groupKoui)
                    {
                        var kouiList = NoTrackingDataContext.KouiKbnMsts.Where(item => koui.ListKoui.Contains(item.KouiKbnId)).Select(item => item.KouiKbn1).ToList();
                        if (kouiList == null || !kouiList.Any()) continue;

                        string kouiInCondition = string.Join(",", kouiList);
                        string addByKouiQuery = "INSERT INTO \"public\".\"RAIIN_LIST_INF\""
                                           + " ("
                                           + " SELECT \"HP_ID\", \"PT_ID\", \"SIN_DATE\", \"RAIIN_NO\""
                                           + $" , {koui.GrpId}, {koui.KbnCd}, current_timestamp, {userId}, '',{RaiinListKbnConstants.KOUI_KBN}"
                                           + " FROM \"public\".\"ODR_INF\""
                                           + " WHERE \"HP_ID\" = 1 AND \"IS_DELETED\" = 0"
                                           + $" AND \"ODR_KOUI_KBN\" IN ({kouiInCondition})"
                                           + " GROUP BY \"HP_ID\", \"SIN_DATE\", \"RAIIN_NO\", \"PT_ID\" "
                                           + " )  ON CONFLICT DO NOTHING;";
                        TrackingDataContext.Database.SetCommandTimeout(1200);
                        TrackingDataContext.Database.ExecuteSqlRaw(addByKouiQuery);
                        TrackingDataContext.SaveChanges();
                    }
                }

                //// Add by item
                itemAddList.AddRange(itemAdds);
                if (itemAddList.Any())
                {
                    var groupItem = itemAddList.GroupBy(item => new { item.GrpId, item.KbnCd })
                                               .Select(item => new { item.Key.GrpId, item.Key.KbnCd, ListItem = item.Select(x => string.Format("'{0}'", x.ItemCd)) })
                                               .ToList();
                    foreach (var item in groupItem)
                    {
                        string itemInCondition = string.Join(",", item.ListItem);
                        string addByItemQuery = "INSERT INTO \"public\".\"RAIIN_LIST_INF\""
                                           + " ("
                                           + " SELECT ODR.\"HP_ID\", ODR.\"PT_ID\", ODR.\"SIN_DATE\", ODR.\"RAIIN_NO\""
                                           + $" , {item.GrpId}, {item.KbnCd}, current_timestamp, {userId}, '',{RaiinListKbnConstants.ITEM_KBN}"
                                           + " FROM \"public\".\"ODR_INF\" ODR"
                                           + " JOIN \"public\".\"ODR_INF_DETAIL\" DETAIL"
                                           + " ON ODR.\"RAIIN_NO\"  = DETAIL.\"RAIIN_NO\"  "
                                           + " AND ODR.\"RP_NO\" = DETAIL.\"RP_NO\" and ODR.\"RP_EDA_NO\" = DETAIL.\"RP_EDA_NO\" "
                                           + " WHERE ODR.\"HP_ID\" = 1 AND ODR.\"IS_DELETED\" = 0"
                                           + $" AND DETAIL.\"ITEM_CD\" IN ({itemInCondition})"
                                           + " GROUP BY ODR.\"HP_ID\", ODR.\"SIN_DATE\", ODR.\"RAIIN_NO\", ODR.\"PT_ID\" "
                                           + " )  ON CONFLICT DO NOTHING;";
                        TrackingDataContext.Database.SetCommandTimeout(1200);
                        TrackingDataContext.Database.ExecuteSqlRaw(addByItemQuery);
                        TrackingDataContext.SaveChanges();
                    }
                }

                // Add by doc
                docAddList.AddRange(docAdds);
                if (docAddList.Any())
                {
                    var groupDoc = docAddList.GroupBy(item => new { item.GrpId, item.KbnCd })
                                               .Select(item => new { item.Key.GrpId, item.Key.KbnCd, ListCategory = item.Select(x => x.CategoryCd) })
                                               .ToList();
                    foreach (var doc in groupDoc)
                    {
                        string docInCondition = string.Join(",", doc.ListCategory);
                        string addByDocQuery = "INSERT INTO \"public\".\"RAIIN_LIST_INF\""
                                            + " ("
                                            + " SELECT DOC.\"HP_ID\", DOC.\"PT_ID\", DOC.\"SIN_DATE\", DOC.\"RAIIN_NO\""
                                            + $" , {doc.GrpId}, {doc.KbnCd}, current_timestamp, {userId}, '',{RaiinListKbnConstants.DOCUMENT_KBN}"
                                            + " FROM \"public\".\"DOC_INF\" DOC"
                                            + " WHERE DOC.\"HP_ID\" = 1 AND DOC.\"IS_DELETED\" = 0"
                                            + $" AND DOC.\"CATEGORY_CD\" IN ({docInCondition})"
                                            + " GROUP BY DOC.\"HP_ID\", DOC.\"SIN_DATE\", DOC.\"RAIIN_NO\", DOC.\"PT_ID\" "
                                            + " )  ON CONFLICT DO NOTHING;";
                        TrackingDataContext.Database.SetCommandTimeout(1200);
                        TrackingDataContext.Database.ExecuteSqlRaw(addByDocQuery);
                        TrackingDataContext.SaveChanges();
                    }
                }

                // Add by file
                fileAddList.AddRange(fileAdds);
                if (fileAddList.Any())
                {
                    var groupFile = fileAddList.GroupBy(item => new { item.GrpId, item.KbnCd })
                                               .Select(item => new { item.Key.GrpId, item.Key.KbnCd, ListCategory = item.Select(x => x.CategoryCd) })
                                               .ToList();
                    foreach (var file in groupFile)
                    {
                        string fileInCondition = string.Join(",", file.ListCategory);
                        string addByFileQuery = "INSERT INTO \"public\".\"RAIIN_LIST_INF\""
                                            + " ("
                                            + " SELECT FILE.\"HP_ID\", FILE.\"PT_ID\", FILE.\"GET_DATE\""
                                            + $", 0, {file.GrpId}, {file.KbnCd}, current_timestamp, {userId}, '',{RaiinListKbnConstants.FILE_KBN}"
                                            + " FROM \"public\".\"FILING_INF\" FILE"
                                            + " WHERE FILE.\"HP_ID\" = 1 AND FILE.\"IS_DELETED\" = 0"
                                            + $" AND FILE.\"CATEGORY_CD\" IN ({fileInCondition})"
                                            + " GROUP BY FILE.\"HP_ID\", FILE.\"GET_DATE\", FILE.\"PT_ID\" "
                                            + " )  ON CONFLICT DO NOTHING;";
                        TrackingDataContext.Database.SetCommandTimeout(1200);
                        TrackingDataContext.Database.ExecuteSqlRaw(addByFileQuery);
                        TrackingDataContext.SaveChanges();
                    }
                }
            }

            void ReCalculationAfterDelete()
            {
                // Recalculated after delete
                foreach (var deleteDetailModel in detailDeletedCurrentList)
                {
                    var detailModel = NoTrackingDataContext.RaiinListDetails.Where(detail => detail.HpId == hpId
                                                                                              && detail.GrpId == deleteDetailModel.GrpId
                                                                                              && detail.IsDeleted == 0)
                                                                        .OrderBy(detail => detail.SortNo)
                                                                        .FirstOrDefault();
                    if (detailModel == null) continue;
                    kouiDeleteList.Add(new RaiinListKouiModel(hpId, deleteDetailModel.GrpId, detailModel.KbnCd, 0, 0, 0));
                    itemDeleteList.Add(new RaiinListItemModel(hpId, deleteDetailModel.GrpId, detailModel.KbnCd, string.Empty, 0, string.Empty, 0, false, 0));
                    docDeleteList.Add(new RaiinListDocModel(hpId, deleteDetailModel.GrpId, detailModel.KbnCd, 0, 0, string.Empty, 0));
                    fileDeleteList.Add(new RaiinListFileModel(hpId, deleteDetailModel.GrpId, deleteDetailModel.KbnCd, 0, string.Empty, 0, 0));
                }

                foreach (var kouiModel in kouiDeleteList)
                {
                    var raiinListKoui = NoTrackingDataContext.RaiinListKouis.Where(item => item.HpId == kouiModel.HpId
                                                                                      && item.GrpId == kouiModel.GrpId
                                                                                      && item.KbnCd == kouiModel.KbnCd
                                                                                      && item.IsDeleted == 0)
                                                                     .Select(x => new RaiinListKouiModel(x.HpId,
                                                                                                         x.GrpId,
                                                                                                         x.KbnCd,
                                                                                                         x.SeqNo,
                                                                                                         x.KouiKbnId,
                                                                                                         x.IsDeleted)).ToList();
                    if (raiinListKoui != null && raiinListKoui.Count() > 0)
                    {
                        kouiAddList.AddRange(raiinListKoui);
                    }
                }

                foreach (var itemModel in itemDeleteList)
                {
                    var raiinListItem = NoTrackingDataContext.RaiinListItems.Where(item => item.HpId == itemModel.HpId
                                                                                      && item.GrpId == itemModel.GrpId
                                                                                      && item.KbnCd == itemModel.KbnCd
                                                                                      && item.IsDeleted == 0)
                                                                     .Select(x => new RaiinListItemModel(x.HpId,
                                                                                                         x.GrpId,
                                                                                                         x.KbnCd,
                                                                                                         x.ItemCd ?? string.Empty,
                                                                                                         x.SeqNo,
                                                                                                         string.Empty,
                                                                                                         0,
                                                                                                         false,
                                                                                                         0)).ToList();
                    if (raiinListItem != null && raiinListItem.Count() > 0)
                    {
                        itemAddList.AddRange(raiinListItem);
                    }
                }

                foreach (var docModel in docDeleteList)
                {
                    var raiinListDoc = NoTrackingDataContext.RaiinListDocs.Where(item => item.HpId == docModel.HpId
                                                                                      && item.GrpId == docModel.GrpId
                                                                                      && item.KbnCd == docModel.KbnCd
                                                                                      && item.IsDeleted == 0).Select(x => new RaiinListDocModel(x.HpId,
                                                                                                                                                x.GrpId,
                                                                                                                                                x.KbnCd,
                                                                                                                                                x.SeqNo,
                                                                                                                                                x.CategoryCd,
                                                                                                                                                string.Empty,
                                                                                                                                                0)).ToList();
                    if (raiinListDoc != null && raiinListDoc.Count() > 0)
                    {
                        docAddList.AddRange(raiinListDoc);
                    }
                }

                foreach (var fileModel in fileDeleteList)
                {
                    var raiinListFile = NoTrackingDataContext.RaiinListFile.Where(item => item.HpId == fileModel.HpId
                                                                                      && item.GrpId == fileModel.GrpId
                                                                                      && item.KbnCd == fileModel.KbnCd
                                                                                      && item.IsDeleted == 0).Select(x => new RaiinListFileModel(x.HpId,
                                                                                                                                                x.GrpId,
                                                                                                                                                x.KbnCd,
                                                                                                                                                x.CategoryCd,
                                                                                                                                                string.Empty,
                                                                                                                                                x.SeqNo,
                                                                                                                                                x.IsDeleted)).ToList();
                    if (raiinListFile != null && raiinListFile.Count() > 0)
                    {
                        fileAddList.AddRange(raiinListFile);
                    }
                }
            }

            IExecutionStrategy strategy = TrackingDataContext.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using (var transaction = TrackingDataContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var raiinMst in raiinListMstModels)
                        {
                            RaiinListMst? mstInDb = databaseRaiinMst.FirstOrDefault(x => x.GrpId == raiinMst.GrpId);
                            if (mstInDb is null)
                            {
                                if (raiinMst.IsDeleted == DeleteTypes.Deleted) continue;

                                RaiinListMst addNewModel = new RaiinListMst()
                                {
                                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                                    GrpName = raiinMst.GrpName,
                                    SortNo = raiinMst.SortNo,
                                    CreateId = userId,
                                    IsDeleted = DeleteTypes.None,
                                    HpId = hpId,
                                    UpdateId = userId,
                                    UpdateDate = CIUtil.GetJapanDateTimeNow()
                                };

                                //Master
                                TrackingDataContext.RaiinListMsts.Add(addNewModel);
                                TrackingDataContext.SaveChanges(); //Get Id AI master

                                //Details
                                foreach (RaiinListDetailModel detail in raiinMst.RaiinListDetailsList)
                                {
                                    if (detail.IsDeleted == DeleteTypes.Deleted) continue;

                                    TrackingDataContext.RaiinListDetails.Add(new RaiinListDetail()
                                    {
                                        HpId = hpId,
                                        CreateId = userId,
                                        IsDeleted = DeleteTypes.None,
                                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                        UpdateId = userId,
                                        ColorCd = detail.ColorCd,
                                        GrpId = addNewModel.GrpId,
                                        KbnCd = detail.KbnCd,
                                        KbnName = detail.KbnName,
                                        SortNo = detail.SortNo
                                    });

                                    //raiinListDoc
                                    TrackingDataContext.RaiinListDocs.AddRange(detail.RaiinListDoc.Where(x => x.IsDeleted == DeleteTypes.None).Select(x => new RaiinListDoc()
                                    {
                                        IsDeleted = DeleteTypes.None,
                                        CategoryCd = x.CategoryCd,
                                        UpdateId = userId,
                                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                        GrpId = addNewModel.GrpId,
                                        KbnCd = detail.KbnCd,
                                        CreateId = userId,
                                        HpId = hpId
                                    }));
                                    docAddList.AddRange(detail.RaiinListDoc.Where(x => x.IsDeleted == DeleteTypes.None));

                                    //RaiinListFile
                                    TrackingDataContext.RaiinListFile.AddRange(detail.RaiinListFile.Where(x => x.IsDeleted == DeleteTypes.None).Select(x => new RaiinListFile()
                                    {
                                        IsDeleted = DeleteTypes.None,
                                        UpdateId = userId,
                                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                        CreateId = userId,
                                        CategoryCd = x.CategoryCd,
                                        GrpId = addNewModel.GrpId,
                                        HpId = hpId,
                                        KbnCd = detail.KbnCd
                                    }));
                                    fileAddList.AddRange(detail.RaiinListFile.Where(x => x.IsDeleted == DeleteTypes.None));

                                    //RaiinListItem
                                    TrackingDataContext.RaiinListItems.AddRange(detail.RaiinListItem.Where(x => x.IsDeleted == DeleteTypes.None).Select(x => new RaiinListItem()
                                    {
                                        IsDeleted = DeleteTypes.None,
                                        UpdateId = userId,
                                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                        CreateId = userId,
                                        HpId = hpId,
                                        GrpId = addNewModel.GrpId,
                                        ItemCd = x.ItemCd,
                                        IsExclude = x.IsExclude,
                                        KbnCd = detail.KbnCd
                                    }));
                                    itemAddList.AddRange(detail.RaiinListItem.Where(x => x.IsDeleted == DeleteTypes.None));

                                    //KouKbnColeection
                                    SaveKouKbnCollection(detail.KouCollection, addNewModel.GrpId, detail.KbnCd);
                                }
                            }
                            else
                            {
                                mstInDb.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                mstInDb.UpdateId = userId;
                                mstInDb.SortNo = raiinMst.SortNo;
                                mstInDb.GrpName = raiinMst.GrpName;
                                mstInDb.IsDeleted = raiinMst.IsDeleted;

                                if (mstInDb.IsDeleted == DeleteTypes.Deleted)
                                {
                                    var detailInDb = databaseRaiinDetails.Where(x => x.GrpId == mstInDb.GrpId && x.IsDeleted == DeleteTypes.None).ToList();
                                    detailInDb.ForEach(x =>
                                    {
                                        x.IsDeleted = DeleteTypes.Deleted;
                                        x.UpdateId = userId;
                                        x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                    });

                                    var detailDeleteMap = detailInDb.Select(x => new RaiinListDetailModel(x.GrpId, x.KbnCd, x.SortNo, x.KbnName ?? string.Empty, x.ColorCd ?? string.Empty, x.IsDeleted));
                                    detailDeletedList.AddRange(detailDeleteMap);
                                    detailDeletedCurrentList.AddRange(detailDeleteMap);

                                    var deleteListItems = databaseRaiinListItems.Where(x => x.GrpId == mstInDb.GrpId).ToList();
                                    deleteListItems.ForEach(x =>
                                    {
                                        x.IsDeleted = DeleteTypes.Deleted;
                                        x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                        x.UpdateId = userId;
                                    });
                                    itemDeleteList.AddRange(deleteListItems.Select(x => new RaiinListItemModel(x.HpId, x.GrpId, x.KbnCd, x.ItemCd ?? string.Empty, x.SeqNo, string.Empty, x.IsExclude, false, x.IsDeleted)));

                                    var deleteListDocs = databaseRaiinListDocs.Where(x => x.GrpId == mstInDb.GrpId).ToList();
                                    deleteListDocs.ForEach(x =>
                                    {
                                        x.IsDeleted = DeleteTypes.Deleted;
                                        x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                        x.UpdateId = userId;
                                    });
                                    docDeleteList.AddRange(deleteListDocs.Select(x => new RaiinListDocModel(x.HpId, x.GrpId, x.KbnCd, x.SeqNo, x.CategoryCd, string.Empty, x.IsDeleted)));

                                    var deleteListFiles = databaseRaiinListFiles.Where(x => x.GrpId == mstInDb.GrpId).ToList();
                                    deleteListFiles.ForEach(x =>
                                    {
                                        x.IsDeleted = DeleteTypes.Deleted;
                                        x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                        x.UpdateId = userId;
                                    });
                                    fileDeleteList.AddRange(deleteListFiles.Select(x => new RaiinListFileModel(x.HpId, x.GrpId, x.KbnCd, x.CategoryCd, string.Empty, x.SeqNo, x.IsDeleted)));

                                    var deleteListKoui = databaseRaiinListKoui.Where(x => x.GrpId == mstInDb.GrpId).ToList();
                                    deleteListKoui.ForEach(x =>
                                    {
                                        x.IsDeleted = DeleteTypes.Deleted;
                                        x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                        x.UpdateId = userId;
                                    });
                                    kouiDeleteList.AddRange(deleteListKoui.Select(x => new RaiinListKouiModel(x.HpId, x.GrpId, x.KbnCd, x.SeqNo, x.KouiKbnId, x.IsDeleted)));
                                }
                                else
                                {
                                    #region DeleteList
                                    var raiinDetailDeleteList = raiinMst.RaiinListDetailsList.Where(x => x.IsDeleted == DeleteTypes.Deleted).ToList();
                                    foreach (var detailDelete in raiinDetailDeleteList)
                                    {
                                        var detailDeleteExistInDb = databaseRaiinDetails.FirstOrDefault(x => x.GrpId == detailDelete.GrpId && x.KbnCd == detailDelete.KbnCd);
                                        if (detailDeleteExistInDb == null || detailDeleteExistInDb.IsDeleted == DeleteTypes.Deleted) continue;
                                        else
                                        {
                                            detailDeleteExistInDb.IsDeleted = DeleteTypes.Deleted;
                                            detailDeleteExistInDb.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                            detailDeleteExistInDb.UpdateId = userId;

                                            detailDeletedList.Add(detailDelete);
                                            detailDeletedCurrentList.Add(detailDelete);

                                            var deleteListItems = databaseRaiinListItems.Where(x => x.GrpId == detailDeleteExistInDb.GrpId && x.KbnCd == detailDeleteExistInDb.KbnCd).ToList();
                                            deleteListItems.ForEach(x =>
                                            {
                                                x.IsDeleted = DeleteTypes.Deleted;
                                                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                                x.UpdateId = userId;
                                            });
                                            itemDeleteList.AddRange(deleteListItems.Select(x => new RaiinListItemModel(x.HpId, x.GrpId, x.KbnCd, x.ItemCd ?? string.Empty, x.SeqNo, string.Empty, x.IsExclude, false, x.IsDeleted)));

                                            var deleteListDocs = databaseRaiinListDocs.Where(x => x.GrpId == detailDeleteExistInDb.GrpId && x.KbnCd == detailDeleteExistInDb.KbnCd).ToList();
                                            deleteListDocs.ForEach(x =>
                                            {
                                                x.IsDeleted = DeleteTypes.Deleted;
                                                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                                x.UpdateId = userId;
                                            });
                                            docDeleteList.AddRange(deleteListDocs.Select(x => new RaiinListDocModel(x.HpId, x.GrpId, x.KbnCd, x.SeqNo, x.CategoryCd, string.Empty, x.IsDeleted)));

                                            var deleteListFiles = databaseRaiinListFiles.Where(x => x.GrpId == detailDeleteExistInDb.GrpId && x.KbnCd == detailDeleteExistInDb.KbnCd).ToList();
                                            deleteListFiles.ForEach(x =>
                                            {
                                                x.IsDeleted = DeleteTypes.Deleted;
                                                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                                x.UpdateId = userId;
                                            });
                                            fileDeleteList.AddRange(deleteListFiles.Select(x => new RaiinListFileModel(x.HpId, x.GrpId, x.KbnCd, x.CategoryCd, string.Empty, x.SeqNo, x.IsDeleted)));

                                            var deleteListKoui = databaseRaiinListKoui.Where(x => x.GrpId == detailDeleteExistInDb.GrpId && x.KbnCd == detailDeleteExistInDb.KbnCd).ToList();
                                            deleteListKoui.ForEach(x =>
                                            {
                                                x.IsDeleted = DeleteTypes.Deleted;
                                                x.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                                x.UpdateId = userId;
                                            });
                                            kouiDeleteList.AddRange(deleteListKoui.Select(x => new RaiinListKouiModel(x.HpId, x.GrpId, x.KbnCd, x.SeqNo, x.KouiKbnId, x.IsDeleted)));
                                        }
                                    }
                                    #endregion DeleteList

                                    #region Update & Add
                                    var listRaiinAddAndUpdate = raiinMst.RaiinListDetailsList.Where(x => x.IsDeleted == DeleteTypes.None).ToList();
                                    foreach (var item in listRaiinAddAndUpdate)
                                    {
                                        var model = databaseRaiinDetails.FirstOrDefault(x => x.GrpId == raiinMst.GrpId && x.KbnCd == item.KbnCd);
                                        if (model is null)
                                        {
                                            TrackingDataContext.RaiinListDetails.Add(new RaiinListDetail()
                                            {
                                                HpId = hpId,
                                                CreateId = userId,
                                                IsDeleted = DeleteTypes.None,
                                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                                UpdateId = userId,
                                                ColorCd = item.ColorCd,
                                                GrpId = raiinMst.GrpId,
                                                KbnCd = item.KbnCd,
                                                KbnName = item.KbnName,
                                                SortNo = item.SortNo
                                            });

                                            //raiinListDoc
                                            TrackingDataContext.RaiinListDocs.AddRange(item.RaiinListDoc.Where(x => x.IsDeleted == DeleteTypes.None).Select(x => new RaiinListDoc()
                                            {
                                                IsDeleted = DeleteTypes.None,
                                                CategoryCd = x.CategoryCd,
                                                UpdateId = userId,
                                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                                GrpId = raiinMst.GrpId,
                                                KbnCd = item.KbnCd,
                                                CreateId = userId,
                                                HpId = hpId
                                            }));
                                            docAddList.AddRange(item.RaiinListDoc.Where(x => x.IsDeleted == DeleteTypes.None));

                                            //RaiinListFile
                                            TrackingDataContext.RaiinListFile.AddRange(item.RaiinListFile.Where(x => x.IsDeleted == DeleteTypes.None).Select(x => new RaiinListFile()
                                            {
                                                IsDeleted = DeleteTypes.None,
                                                UpdateId = userId,
                                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                                CreateId = userId,
                                                CategoryCd = x.CategoryCd,
                                                GrpId = raiinMst.GrpId,
                                                HpId = hpId,
                                                KbnCd = item.KbnCd
                                            }));
                                            fileAddList.AddRange(item.RaiinListFile.Where(x => x.IsDeleted == DeleteTypes.None));

                                            //RaiinListItem
                                            TrackingDataContext.RaiinListItems.AddRange(item.RaiinListItem.Where(x => x.IsDeleted == DeleteTypes.None).Select(x => new RaiinListItem()
                                            {
                                                IsDeleted = DeleteTypes.None,
                                                UpdateId = userId,
                                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                                CreateId = userId,
                                                HpId = hpId,
                                                GrpId = raiinMst.GrpId,
                                                ItemCd = x.ItemCd,
                                                IsExclude = x.IsExclude,
                                                KbnCd = item.KbnCd
                                            }));
                                            itemAddList.AddRange(item.RaiinListItem.Where(x => x.IsDeleted == DeleteTypes.None));

                                            //KouKbnColeection
                                            SaveKouKbnCollection(item.KouCollection, raiinMst.GrpId, item.KbnCd);
                                        }
                                        else
                                        {
                                            model.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                            model.UpdateId = userId;
                                            model.ColorCd = item.ColorCd;
                                            model.KbnName = item.KbnName;
                                            model.SortNo = item.SortNo;

                                            if (item.IsOnlySwapSortNo)
                                            {
                                                itemAdds.AddRange(item.RaiinListItem.Where(item => item.IsDeleted == 0 && !string.IsNullOrEmpty(item.ItemCd)));
                                                docAdds.AddRange(item.RaiinListDoc.Where(item => item.IsDeleted == 0 && item.CategoryCd > 0));
                                                fileAdds.AddRange(item.RaiinListFile.Where(item => item.IsDeleted == 0 && item.CategoryCd > 0));
                                                detailDeleteds.Add(item);
                                            }

                                            //raiinListDoc
                                            TrackingDataContext.RaiinListDocs.AddRange(item.RaiinListDoc.Where(x => x.IsDeleted == DeleteTypes.None && x.SeqNo == 0).Select(x => new RaiinListDoc()
                                            {
                                                IsDeleted = DeleteTypes.None,
                                                CategoryCd = x.CategoryCd,
                                                UpdateId = userId,
                                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                                GrpId = raiinMst.GrpId,
                                                KbnCd = item.KbnCd,
                                                CreateId = userId,
                                                HpId = hpId
                                            }));
                                            docAddList.AddRange(item.RaiinListDoc.Where(x => x.IsDeleted == DeleteTypes.None));

                                            foreach (var updateDoc in item.RaiinListDoc.Where(x => x.SeqNo != 0))
                                            {
                                                var updateDocModel = databaseRaiinListDocs.FirstOrDefault(x => x.SeqNo == updateDoc.SeqNo);
                                                if (updateDocModel != null)
                                                {
                                                    updateDocModel.CategoryCd = updateDoc.CategoryCd;
                                                    updateDocModel.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                                    updateDocModel.UpdateId = userId;
                                                    updateDocModel.IsDeleted = updateDoc.IsDeleted;
                                                    if (updateDocModel.IsDeleted == DeleteTypes.Deleted)
                                                    {
                                                        docDeleteList.Add(updateDoc);
                                                    }
                                                }
                                            }

                                            //RaiinListFile
                                            TrackingDataContext.RaiinListFile.AddRange(item.RaiinListFile.Where(x => x.IsDeleted == DeleteTypes.None && x.SeqNo == 0).Select(x => new RaiinListFile()
                                            {
                                                IsDeleted = DeleteTypes.None,
                                                CategoryCd = x.CategoryCd,
                                                UpdateId = userId,
                                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                                GrpId = raiinMst.GrpId,
                                                KbnCd = item.KbnCd,
                                                CreateId = userId,
                                                HpId = hpId
                                            }));
                                            fileAddList.AddRange(item.RaiinListFile.Where(x => x.IsDeleted == DeleteTypes.None));

                                            foreach (var updateFile in item.RaiinListFile.Where(x => x.SeqNo != 0))
                                            {
                                                var updateFileModel = databaseRaiinListFiles.FirstOrDefault(x => x.SeqNo == updateFile.SeqNo);
                                                if (updateFileModel != null)
                                                {
                                                    updateFileModel.CategoryCd = updateFile.CategoryCd;
                                                    updateFileModel.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                                    updateFileModel.UpdateId = userId;
                                                    updateFileModel.IsDeleted = updateFile.IsDeleted;
                                                    if (updateFileModel.IsDeleted == DeleteTypes.Deleted)
                                                    {
                                                        fileDeleteList.Add(updateFile);
                                                    }
                                                }
                                            }

                                            //RaiinListItem
                                            TrackingDataContext.RaiinListItems.AddRange(item.RaiinListItem.Where(x => x.IsDeleted == DeleteTypes.None && x.SeqNo == 0).Select(x => new RaiinListItem()
                                            {
                                                IsDeleted = DeleteTypes.None,
                                                ItemCd = x.ItemCd,
                                                IsExclude = x.IsExclude,
                                                UpdateId = userId,
                                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                                GrpId = raiinMst.GrpId,
                                                KbnCd = item.KbnCd,
                                                CreateId = userId,
                                                HpId = hpId
                                            }));

                                            itemAddList.AddRange(item.RaiinListItem.Where(x => x.IsDeleted == DeleteTypes.None));

                                            foreach (var updateItem in item.RaiinListItem.Where(x => x.SeqNo != 0))
                                            {
                                                var updateItemModel = databaseRaiinListItems.FirstOrDefault(x => x.SeqNo == updateItem.SeqNo);
                                                if (updateItemModel != null)
                                                {
                                                    updateItemModel.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                                    updateItemModel.UpdateId = userId;
                                                    updateItemModel.IsDeleted = updateItem.IsDeleted;
                                                    updateItemModel.ItemCd = updateItem.ItemCd;
                                                    updateItemModel.IsExclude = updateItem.IsExclude;
                                                    updateItemModel.IsDeleted = updateItem.IsDeleted;
                                                    if (updateItemModel.IsDeleted == DeleteTypes.Deleted)
                                                    {
                                                        itemDeleteList.Add(updateItem);
                                                    }
                                                }
                                            }
                                            //KouKbnColeection
                                            SaveKouKbnCollection(item.KouCollection, raiinMst.GrpId, item.KbnCd, item.IsOnlySwapSortNo);
                                        }
                                    }
                                    #endregion Update & Add
                                }
                            }
                        }
                        TrackingDataContext.SaveChanges();

                        // Delete by all detail
                        string queryDelete = "DELETE FROM \"public\".\"RAIIN_LIST_INF\" WHERE FALSE";
                        detailDeletedList.AddRange(detailDeleteds);
                        foreach (var deleteDetailModel in detailDeletedList)
                        {
                            queryDelete += " OR (\"GRP_ID\" = " + deleteDetailModel.GrpId + " AND \"KBN_CD\" = " + deleteDetailModel.KbnCd + ")";
                        }

                        foreach (var kouiModel in kouiDeleteList)
                        {
                            queryDelete += " OR (\"GRP_ID\" = " + kouiModel.GrpId + " AND \"KBN_CD\" = " + kouiModel.KbnCd + " AND  \"RAIIN_LIST_KBN\" = " + RaiinListKbnConstants.KOUI_KBN + ")";
                        }

                        foreach (var itemModel in itemDeleteList)
                        {
                            queryDelete += " OR (\"GRP_ID\" = " + itemModel.GrpId + " AND \"KBN_CD\" = " + itemModel.KbnCd + " AND  \"RAIIN_LIST_KBN\" = " + RaiinListKbnConstants.ITEM_KBN + ")";
                        }

                        foreach (var docModel in docDeleteList)
                        {
                            queryDelete += " OR (\"GRP_ID\" = " + docModel.GrpId + " AND \"KBN_CD\" = " + docModel.KbnCd + " AND  \"RAIIN_LIST_KBN\" = " + RaiinListKbnConstants.DOCUMENT_KBN + ")";
                        }

                        foreach (var fileModel in fileDeleteList)
                        {
                            queryDelete += " OR (\"GRP_ID\" = " + fileModel.GrpId + " AND \"KBN_CD\" = " + fileModel.KbnCd + " AND  \"RAIIN_LIST_KBN\" = " + RaiinListKbnConstants.FILE_KBN + ")";
                        }

                        TrackingDataContext.Database.ExecuteSqlRaw(queryDelete);
                        TrackingDataContext.SaveChanges();

                        ReCalculationAfterDelete();
                        ExceuteCommandRawAddModel();

                        // Commit transaction if all commands succeed, transaction will auto-rollback
                        // when disposed if either commands fails
                        transaction.Commit();
                        resultSave = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            });

            // Clear RaiinListMstCache
            if (resultSave)
            {
                _cache.KeyDelete(RaiinListMstCacheKey);
            }

            return resultSave;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
