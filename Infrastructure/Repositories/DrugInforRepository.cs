using Domain.Constant;
using Domain.Models.DrugInfor;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class DrugInforRepository : RepositoryBase, IDrugInforRepository
    {
        private readonly IConfiguration _configuration;

        public DrugInforRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
        {
            _configuration = configuration;
        }

        public DrugInforModel GetDrugInfor(int hpId, int sinDate, string itemCd)
        {
            var queryItems = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId && new[] { 20, 30 }.Contains(item.SinKouiKbn)
                                                                && item.StartDate <= sinDate && item.EndDate >= sinDate && item.IsDeleted == DeleteTypes.None);

            ////Join
            var joinQuery = from m28DrugMst in NoTrackingDataContext.M28DrugMst.Where(m => m.HpId == hpId)
                            join tenItem in queryItems
                            on m28DrugMst.KikinCd equals tenItem.ItemCd
                            join m34DrugInfoMain in NoTrackingDataContext.M34DrugInfoMains.Where(m => m.HpId == hpId)
                            on m28DrugMst.YjCd equals m34DrugInfoMain.YjCd
                            join tekiouByomei in NoTrackingDataContext.TekiouByomeiMsts.Where(m => m.HpId == hpId)
                               on tenItem.ItemCd equals tekiouByomei.ItemCd into listtekiouByomeis
                            where string.IsNullOrEmpty(itemCd) || tenItem.ItemCd == itemCd
                            select new
                            {
                                m28DrugMst,
                                tenItem,
                                m34DrugInfoMain,
                                TekiouByomei = listtekiouByomeis.FirstOrDefault()
                            };

            // piczai pichou
            string pathServerDefault = _configuration["PathImageDrugFolder"] ?? string.Empty;

            var pathConfDb = NoTrackingDataContext.PathConfs.Where(p => p.HpId == hpId && p.GrpCd == PicImageConstant.GrpCodeDefault || p.GrpCd == PicImageConstant.GrpCodeCustomDefault).ToList();

            var pathConfDf = pathConfDb.FirstOrDefault(p => p.GrpCd == PicImageConstant.GrpCodeDefault);

            // PicZai
            string defaultPicZai = "";
            string otherPicZai = "";
            string defaultPicHou = "";
            string otherPicHou = "";
            if (pathConfDf != null)
            {
                defaultPicZai = pathConfDf.Path + "zaikei/";
                defaultPicHou = pathConfDf.Path + "housou/";
            }
            else
            {
                defaultPicZai = pathServerDefault + "zaikei/";
                defaultPicHou = pathServerDefault + "housou/";
            }
            string customPathPicZai = "";
            string customPathPicHou = "";
            var customPathConf = pathConfDb.FirstOrDefault(p => p.GrpCd == PicImageConstant.GrpCodeCustomDefault);
            if (customPathConf != null)
            {
                customPathPicZai = customPathConf.Path + "zaikei/";
                customPathPicHou = customPathConf.Path + "housou/";
            }
            else
            {
                customPathPicZai = pathServerDefault + "zaikei/";
                customPathPicHou = pathServerDefault + "housou/";
            }

            var imagePics = NoTrackingDataContext.PiImages.Where(pi => pi.ItemCd == itemCd && (pi.ImageType == PicImageConstant.PicZaikei || pi.ImageType == PicImageConstant.PicHousou)).ToList();

            var otherImagePicZai = imagePics.FirstOrDefault(pi => pi.ImageType == PicImageConstant.PicZaikei);
            if (otherImagePicZai != null)
            {
                otherPicZai = defaultPicZai + otherImagePicZai.FileName ?? string.Empty;
            }

            var otherImagePicHou = imagePics.FirstOrDefault(pi => pi.ImageType == PicImageConstant.PicHousou);
            if (otherImagePicHou != null)
            {
                otherPicHou = defaultPicHou + otherImagePicHou.FileName ?? string.Empty;
            }

            var rs = joinQuery.FirstOrDefault();
            var yjCd = rs?.m28DrugMst?.YjCd;
            var drugInf = NoTrackingDataContext.PiProductInfs.FirstOrDefault(i => i.HpId == hpId && i.YjCd == yjCd);
            if (rs != null)
            {
                return new DrugInforModel(rs.tenItem != null ? (rs.tenItem.Name ?? string.Empty) : string.Empty,
                                          drugInf != null ? (drugInf.GenericName ?? string.Empty) : string.Empty,
                                          drugInf != null ? (drugInf.Unit ?? string.Empty) : string.Empty,
                                          drugInf != null ? (drugInf.Maker ?? string.Empty) : string.Empty,
                                          drugInf != null ? (drugInf.Vender ?? string.Empty) : string.Empty,
                                          rs.tenItem != null ? rs.tenItem.KohatuKbn : 0,
                                          rs.tenItem != null ? rs.tenItem.Ten : 0,
                                          rs.tenItem != null ? (rs.tenItem.ReceUnitName ?? string.Empty) : string.Empty,
                                          rs.m34DrugInfoMain != null ? (rs.m34DrugInfoMain.Mark ?? string.Empty) : string.Empty,
                                          rs.tenItem != null ? rs.tenItem.YjCd ?? string.Empty : string.Empty,
                                          "",
                                          "",
                                          defaultPicZai,
                                          customPathPicZai,
                                          otherPicZai,
                                          defaultPicHou,
                                          customPathPicHou,
                                          otherPicHou,
                                          new List<string>(),
                                          new List<string>());
            }
            else
            {
                return new DrugInforModel();
            }
        }

        public List<SinrekiFilterMstModel> GetSinrekiFilterMstList(int hpId)
        {
            List<SinrekiFilterMstModel> result = new();
            var sinrekiMstList = NoTrackingDataContext.SinrekiFilterMsts.Where(item => item.HpId == hpId
                                                                                       && item.IsDeleted == 0)
                                                                        .ToList();
            var grpCdList = sinrekiMstList.Select(item => item.GrpCd).Distinct().ToList();
            var detailList = NoTrackingDataContext.SinrekiFilterMstDetails.Where(item => item.HpId == hpId
                                                                                         && item.IsDeleted == 0
                                                                                         && grpCdList.Contains(item.GrpCd))
                                                                          .ToList();
            var itemCdList = detailList.Select(item => item.ItemCd).Distinct().ToList();
            var tenMstList = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                         && item.IsDeleted == 0
                                                                         && item.ItemCd != null
                                                                         && itemCdList.Contains(item.ItemCd))
                                                          .GroupBy(item => item.ItemCd)
                                                          .Select(item => item.OrderByDescending(t => t.EndDate).FirstOrDefault())
                                                          .ToList();
            var kouiList = NoTrackingDataContext.SinrekiFilterMstKouis.Where(item => item.HpId == hpId
                                                                                     && grpCdList.Contains(item.GrpCd)
                                                                                     && item.IsDeleted == 0)
                                                                      .ToList();
            foreach (var mst in sinrekiMstList)
            {
                var sinrekiFilterMstKouiList = kouiList.Where(item => item.GrpCd == mst.GrpCd)
                                                       .Select(item => new SinrekiFilterMstKouiModel(
                                                                           item.GrpCd,
                                                                           item.SeqNo,
                                                                           item.KouiKbnId,
                                                                           item.IsDeleted == 0))
                                                       .ToList();

                var sinrekiFilterMstDetailList = (from detail in detailList.Where(item => item.GrpCd == mst.GrpCd)
                                                  join ten in tenMstList on detail.ItemCd equals ten.ItemCd
                                                  select new SinrekiFilterMstDetailModel(
                                                             detail.Id,
                                                             detail.GrpCd,
                                                             detail.ItemCd ?? string.Empty,
                                                             ten.Name ?? string.Empty,
                                                             detail.SortNo,
                                                             detail.IsExclude == 1
                                                  )).OrderBy(item => item.SortNo)
                                                  .ToList();

                var mstModel = new SinrekiFilterMstModel(
                                   mst.GrpCd,
                                   mst.Name ?? string.Empty,
                                   mst.SortNo,
                                   sinrekiFilterMstKouiList,
                                   sinrekiFilterMstDetailList);
                result.Add(mstModel);
            }
            return result.OrderBy(item => item.SortNo).ToList();
        }

        public SinrekiFilterMstModel GetSinrekiFilterMst(int hpId, int grpCd)
        {
            var sinrekiMst = NoTrackingDataContext.SinrekiFilterMsts.FirstOrDefault(item => item.HpId == hpId
                                                                                            && item.IsDeleted == 0
                                                                                            && item.GrpCd == grpCd);
            if (sinrekiMst == null)
            {
                return new();
            }
            var detailList = NoTrackingDataContext.SinrekiFilterMstDetails.Where(item => item.HpId == hpId
                                                                                         && item.IsDeleted == 0
                                                                                         && item.GrpCd == grpCd)
                                                                          .ToList();
            var itemCdList = detailList.Select(item => item.ItemCd).Distinct().ToList();
            var tenMstList = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                         && item.IsDeleted == 0
                                                                         && item.ItemCd != null
                                                                         && itemCdList.Contains(item.ItemCd))
                                                          .GroupBy(item => item.ItemCd)
                                                          .Select(item => item.OrderByDescending(t => t.EndDate).FirstOrDefault())
                                                          .ToList();
            var kouiList = NoTrackingDataContext.SinrekiFilterMstKouis.Where(item => item.HpId == hpId
                                                                                     && item.GrpCd == grpCd
                                                                                     && item.IsDeleted == 0)
                                                                      .ToList();

            var sinrekiFilterMstKouiList = kouiList.Where(item => item.GrpCd == sinrekiMst.GrpCd)
                                                   .Select(item => new SinrekiFilterMstKouiModel(
                                                                       item.GrpCd,
                                                                       item.SeqNo,
                                                                       item.KouiKbnId,
                                                                       item.IsDeleted == 0))
                                                   .ToList();

            var sinrekiFilterMstDetailList = (from detail in detailList.Where(item => item.GrpCd == sinrekiMst.GrpCd)
                                              join ten in tenMstList on detail.ItemCd equals ten.ItemCd
                                              select new SinrekiFilterMstDetailModel(
                                                         detail.Id,
                                                         detail.GrpCd,
                                                         detail.ItemCd ?? string.Empty,
                                                         ten.Name ?? string.Empty,
                                                         detail.SortNo,
                                                         detail.IsExclude == 1
                                              )).OrderBy(item => item.SortNo)
                                              .ToList();

            return new SinrekiFilterMstModel(
                       sinrekiMst.GrpCd,
                       sinrekiMst.Name ?? string.Empty,
                       sinrekiMst.SortNo,
                       sinrekiFilterMstKouiList,
                       sinrekiFilterMstDetailList);
        }

        public bool SaveSinrekiFilterMstList(int hpId, int userId, List<SinrekiFilterMstModel> sinrekiFilterMstList)
        {
            var dateTimeUpdate = CIUtil.GetJapanDateTimeNow();
            var grpCdList = sinrekiFilterMstList.Where(item => item.GrpCd > 0).Select(item => item.GrpCd).Distinct().ToList();
            var sinrekiFilterMstDbList = TrackingDataContext.SinrekiFilterMsts.Where(item => item.HpId == hpId
                                                                                             && grpCdList.Contains(item.GrpCd)
                                                                                             && item.IsDeleted == 0)
                                                                              .ToList();
            var sinrekiFilterMstKouiDbList = TrackingDataContext.SinrekiFilterMstKouis.Where(item => item.HpId == hpId
                                                                                                     && grpCdList.Contains(item.GrpCd)
                                                                                                     && item.IsDeleted == 0)
                                                                                      .ToList();
            var sinrekiFilterMstDetailDbList = TrackingDataContext.SinrekiFilterMstDetails.Where(item => item.HpId == hpId
                                                                                                         && grpCdList.Contains(item.GrpCd)
                                                                                                         && item.IsDeleted == 0)
                                                                                          .ToList();
            var allGrpCd = NoTrackingDataContext.SinrekiFilterMsts.Where(i => i.HpId == hpId).Select(item => item.GrpCd).ToList();
            int maxGrpCd = allGrpCd != null && allGrpCd.Any() ? allGrpCd.Max() : 0;
            bool saveSuccess = false;

            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
            executionStrategy.Execute(
                () =>
                {
                    using var transaction = TrackingDataContext.Database.BeginTransaction();
                    try
                    {
                        foreach (var mstModel in sinrekiFilterMstList)
                        {
                            bool isAddNew = false;
                            var mstEntity = sinrekiFilterMstDbList.FirstOrDefault(item => item.GrpCd == mstModel.GrpCd);
                            if (mstEntity == null)
                            {
                                if (mstModel.GrpCd != 0)
                                {
                                    continue;
                                }
                                else if (mstModel.GrpCd == 0 && mstModel.IsDeleted)
                                {
                                    continue;
                                }
                                // Add new SinrekiFilterMst
                                mstEntity = new();
                                mstEntity.HpId = hpId;
                                mstEntity.GrpCd = maxGrpCd + 1;
                                mstEntity.CreateDate = dateTimeUpdate;
                                mstEntity.CreateId = userId;
                                maxGrpCd += 1;
                                isAddNew = true;
                            }
                            mstEntity.UpdateDate = dateTimeUpdate;
                            mstEntity.UpdateId = userId;
                            if (mstModel.IsDeleted)
                            {
                                mstEntity.IsDeleted = 1;
                            }
                            mstEntity.Name = mstModel.Name;
                            mstEntity.SortNo = mstModel.SortNo;
                            if (isAddNew)
                            {
                                TrackingDataContext.SinrekiFilterMsts.Add(mstEntity);
                                TrackingDataContext.SaveChanges();
                            }

                            // Update SinrekiFilterMstDetail
                            foreach (var detailModel in mstModel.SinrekiFilterMstDetailList)
                            {
                                var detailEntity = sinrekiFilterMstDetailDbList.FirstOrDefault(item => item.Id == detailModel.Id);
                                if (detailEntity == null)
                                {
                                    if (detailModel.Id != 0)
                                    {
                                        continue;
                                    }
                                    else if (detailModel.Id == 0 && detailModel.IsDeleted)
                                    {
                                        continue;
                                    }

                                    // add new FilterMstDetail
                                    detailEntity = new();
                                    detailEntity.HpId = hpId;
                                    detailEntity.GrpCd = mstEntity.GrpCd;
                                    detailEntity.Id = 0;
                                    detailEntity.CreateDate = dateTimeUpdate;
                                    detailEntity.CreateId = userId;
                                }
                                detailEntity.UpdateId = userId;
                                detailEntity.UpdateDate = dateTimeUpdate;
                                if (detailModel.IsDeleted)
                                {
                                    detailEntity.IsDeleted = 1;
                                    continue;
                                }
                                detailEntity.ItemCd = detailModel.ItemCd;
                                detailEntity.SortNo = detailModel.SortNo;
                                detailEntity.IsExclude = detailModel.IsExclude ? 1 : 0;
                                if (detailEntity.Id == 0)
                                {
                                    TrackingDataContext.SinrekiFilterMstDetails.Add(detailEntity);
                                }
                            }

                            // Update SinrekiFilterMstKoui
                            foreach (var kouiModel in mstModel.SinrekiFilterMstKouiList)
                            {
                                var kouiEntity = sinrekiFilterMstKouiDbList.FirstOrDefault(item => item.SeqNo == kouiModel.SeqNo);
                                if (kouiEntity == null)
                                {
                                    if (kouiModel.SeqNo != 0)
                                    {
                                        continue;
                                    }
                                    kouiEntity = new();
                                    kouiEntity.HpId = hpId;
                                    kouiEntity.GrpCd = mstEntity.GrpCd;
                                    kouiEntity.SeqNo = 0;
                                    kouiEntity.CreateId = userId;
                                    kouiEntity.CreateDate = dateTimeUpdate;
                                    kouiEntity.KouiKbnId = kouiModel.KouiKbnId;
                                }
                                kouiEntity.UpdateDate = dateTimeUpdate;
                                kouiEntity.UpdateId = userId;
                                if (kouiModel.IsChecked)
                                {
                                    kouiEntity.IsDeleted = 0;
                                }
                                else
                                {
                                    kouiEntity.IsDeleted = 1;
                                    continue;
                                }
                                if (kouiEntity.SeqNo == 0)
                                {
                                    TrackingDataContext.SinrekiFilterMstKouis.Add(kouiEntity);
                                }
                            }
                        }
                        TrackingDataContext.SaveChanges();
                        saveSuccess = true;
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                });
            return saveSuccess;
        }

        public bool CheckExistGrpCd(int hpId, List<int> grpCdList)
        {
            grpCdList = grpCdList.Distinct().ToList();
            return NoTrackingDataContext.SinrekiFilterMsts.Count(item => item.HpId == hpId && grpCdList.Contains(item.GrpCd)) == grpCdList.Count;
        }

        public bool CheckExistKouiKbn(List<int> kouiKbnIdList)
        {
            kouiKbnIdList = kouiKbnIdList.Distinct().ToList();
            return NoTrackingDataContext.KouiKbnMsts.Count(item => kouiKbnIdList.Contains(item.KouiKbnId)) == kouiKbnIdList.Count;
        }

        public bool CheckExistSinrekiFilterMstKoui(int hpId, List<long> kouiSeqNoList)
        {
            kouiSeqNoList = kouiSeqNoList.Distinct().ToList();
            return NoTrackingDataContext.SinrekiFilterMstKouis.Count(item => item.HpId == hpId && item.IsDeleted == 0 && kouiSeqNoList.Contains(item.SeqNo)) == kouiSeqNoList.Count;
        }

        public bool CheckExistSinrekiFilterMstDetail(int hpId, List<long> detailIdList)
        {
            detailIdList = detailIdList.Distinct().ToList();
            return NoTrackingDataContext.SinrekiFilterMstDetails.Count(item => item.HpId == hpId && item.IsDeleted == 0 && detailIdList.Contains(item.Id)) == detailIdList.Count;
        }

        public List<DrugUsageHistoryModel> GetDrugUsageHistoryList(int hpId, long ptId)
        {
            var odrInfRepos = NoTrackingDataContext.OdrInfs.Where(item => item.HpId == hpId
                                                                          && item.PtId == ptId
                                                                          && item.IsDeleted == 0)
                                                           .ToList();
            var rpNoList = odrInfRepos.Select(item => item.RpNo).Distinct().ToList();
            var rpEdaNoList = odrInfRepos.Select(item => item.RpEdaNo).Distinct().ToList();
            var raiinNoList = odrInfRepos.Select(item => item.RaiinNo).Distinct().ToList();
            var odrInfDetailRepos = NoTrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                                      && item.PtId == ptId
                                                                                      && item.ItemCd != null
                                                                                      && item.ItemCd != ""
                                                                                      && rpNoList.Contains(item.RpNo)
                                                                                      && rpEdaNoList.Contains(item.RpEdaNo)
                                                                                      && raiinNoList.Contains(item.RaiinNo))
                                                                       .ToList();

            var odrInfDetailUsage = odrInfDetailRepos.Where(item => item.YohoKbn > 0).ToList();

            var odrInfDetailNotUsage = odrInfDetailRepos.Where(item => item.YohoKbn == 0 &&
                                                                       item.ItemCd != ItemCdConst.ChusyaJikocyu)
                                                        .ToList();

            var queryUsage = (from odrInf in odrInfRepos
                              join odrInfDetail in odrInfDetailUsage on
                                  new { odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                                  new { odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
                              select new
                              {
                                  OdrInf = odrInf,
                                  odrInfDetail.SinKouiKbn
                              }).ToList();

            var query = from odrInf in odrInfRepos
                        join notUsage in odrInfDetailNotUsage on
                            new { odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                            new { notUsage.RaiinNo, notUsage.RpNo, notUsage.RpEdaNo }
                        join usage in queryUsage on
                            new { odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                            new { usage.OdrInf.RaiinNo, usage.OdrInf.RpNo, usage.OdrInf.RpEdaNo } into usageList
                        from usageItem in usageList.DefaultIfEmpty()
                        select new
                        {
                            OdrInf = odrInf,
                            SinKouiKbn = usageItem == null ? odrInf.OdrKouiKbn : usageItem.SinKouiKbn,
                            OdrInfDetail = notUsage,
                        };

            return query.Select(item => new DrugUsageHistoryModel(
                                            item.SinKouiKbn,
                                            item.OdrInf.SinDate,
                                            item.OdrInf.RaiinNo,
                                            item.OdrInf.RpNo,
                                            item.OdrInf.RpEdaNo,
                                            item.OdrInf.OdrKouiKbn,
                                            item.OdrInf.DaysCnt,
                                            item.OdrInfDetail.RowNo,
                                            item.OdrInfDetail.SinKouiKbn,
                                            item.OdrInfDetail.ItemCd ?? string.Empty,
                                            item.OdrInfDetail.ItemName ?? string.Empty,
                                            item.OdrInfDetail.Suryo,
                                            item.OdrInfDetail.UnitSBT,
                                            item.OdrInfDetail.UnitName ?? string.Empty))
                .Where(item => !(item.ItemCd == ItemCdConst.JikanKihon && item.Quantity == JikanConst.JikanNai ||
                                 item.ItemCd == ItemCdConst.SyosaiKihon && (item.Quantity == SyosaiConst.None || item.Quantity == SyosaiConst.Jihi)))
                .ToList();
        }

        public List<KouiKbnMstModel> GetKouiKbnMstList()
        {
            var result = NoTrackingDataContext.KouiKbnMsts.Select(item => new KouiKbnMstModel(
                                                                              item.KouiKbnId,
                                                                              item.KouiKbn1,
                                                                              item.KouiKbn2,
                                                                              item.KouiName ?? string.Empty,
                                                                              item.ExcKouiKbn,
                                                                              item.OyaKouiKbnId))
                                                          .ToList();
            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
