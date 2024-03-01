using Domain.Models.ReceSeikyu;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Mapping;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
using System.Linq.Dynamic.Core.Tokenizer;

namespace Infrastructure.Repositories
{
    public class ReceSeikyuRepository : RepositoryBase, IReceSeikyuRepository
    {
        public ReceSeikyuRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<ReceSeikyuModel> GetListReceSeikyModel(int hpId, int sinDate, int sinYm, bool isIncludingUnConfirmed, long ptNumSearch, bool noFilter, bool isFilterMonthlyDelay, bool isFilterReturn, bool isFilterOnlineReturn, bool isGetDataPending)
        {
            var ptInfo = NoTrackingDataContext.PtInfs.Where(u => u.HpId == hpId &&
                                                                                u.IsDelete == 0);

            var listReceSeikyu = NoTrackingDataContext.ReceSeikyus.Where(u => u.HpId == hpId &&
                                                                                       u.IsDeleted == 0);

            var ptHoken = NoTrackingDataContext.PtHokenInfs.Where(u => u.HpId == hpId && u.IsDeleted == 0);


            var listRecedenHenjiyuu = NoTrackingDataContext.RecedenHenJiyuus.Where(u => u.HpId == hpId &&
                                                                                                 u.IsDeleted == 0);

            var recedenHenjiyuuInfo = from recedenHenjiyuu in listRecedenHenjiyuu
                                      join ptHokenInf in ptHoken on
                                           new { recedenHenjiyuu.PtId, recedenHenjiyuu.HokenId } equals
                                           new { ptHokenInf.PtId, ptHokenInf.HokenId } into lj
                                      from item in lj.DefaultIfEmpty()
                                      select new
                                      {
                                          RecedenHenJiyuu = recedenHenjiyuu,
                                          PtHokenInfItem = item
                                      };
            if (isGetDataPending)
            {
                var receSeikyuInf = from receSeikyu in listReceSeikyu.AsEnumerable()
                                    join ptHokenInf in ptHoken on
                                      new { receSeikyu.PtId, receSeikyu.HokenId } equals
                                      new { ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenList
                                    join ptInf in ptInfo on receSeikyu.PtId equals ptInf.PtId
                                    select new
                                    {
                                        PtInfo = ptInf,
                                        ReceSeikyu = receSeikyu,
                                        PtHokenInfItem = ptHokenList?.FirstOrDefault() ?? new()
                                    };

                var query = from receSeikyu in receSeikyuInf
                            join recedenHenjiyuu in recedenHenjiyuuInfo on
                            new { receSeikyu.ReceSeikyu.PtId, receSeikyu.ReceSeikyu.SinYm, HokenId = receSeikyu.ReceSeikyu.PreHokenId } equals
                            new { recedenHenjiyuu.RecedenHenJiyuu.PtId, recedenHenjiyuu.RecedenHenJiyuu.SinYm, HokenId = recedenHenjiyuu.RecedenHenJiyuu.HokenId }
                            into recedenHenjiyuuList
                            select new
                            {
                                recedenHenjiyuuList = recedenHenjiyuuList,
                                ReceSeikyuPending = receSeikyu,
                            };

                var result = query.AsEnumerable().Select(u => new ReceSeikyuModel(sinDate,
                                                                              u.ReceSeikyuPending?.PtInfo?.HpId ?? 0,
                                                                              u.ReceSeikyuPending?.PtInfo?.PtId ?? 0,
                                                                              u.ReceSeikyuPending?.PtInfo?.Name ?? string.Empty,
                                                                              sinYm,
                                                                              u.ReceSeikyuPending?.ReceSeikyu?.SinYm ?? 0,
                                                                              u.ReceSeikyuPending?.PtHokenInfItem?.HokenId ?? 0,
                                                                              u.ReceSeikyuPending?.PtHokenInfItem?.HokensyaNo ?? string.Empty,
                                                                              u.ReceSeikyuPending?.ReceSeikyu?.SeqNo ?? 0,
                                                                              u.ReceSeikyuPending?.ReceSeikyu?.SeikyuYm ?? 0,
                                                                              u.ReceSeikyuPending?.ReceSeikyu?.SeikyuKbn ?? 0,
                                                                              u.ReceSeikyuPending?.ReceSeikyu?.PreHokenId ?? 0,
                                                                              u.ReceSeikyuPending?.ReceSeikyu?.Cmt ?? string.Empty,
                                                                              u.ReceSeikyuPending?.PtInfo?.PtNum ?? 0,
                                                                              u.ReceSeikyuPending?.PtHokenInfItem?.HokenKbn ?? 0,
                                                                              u.ReceSeikyuPending?.PtHokenInfItem?.Houbetu ?? string.Empty,
                                                                              u.ReceSeikyuPending?.PtHokenInfItem?.StartDate ?? 0,
                                                                              u.ReceSeikyuPending?.PtHokenInfItem?.EndDate ?? 0,
                                                                              false,
                                                                              u.ReceSeikyuPending?.ReceSeikyu?.SeikyuYm ?? 0,
                                                                              u.ReceSeikyuPending?.ReceSeikyu?.SinYm ?? 0,
                                                                              false,
                                                                              DeleteTypes.None,
                                                                              u.ReceSeikyuPending?.ReceSeikyu?.SeikyuYm != 999999,
                                                                              u.recedenHenjiyuuList?.AsEnumerable().Select(p => new RecedenHenJiyuuModel(p.RecedenHenJiyuu?.HpId ?? 0,
                                                                              p.RecedenHenJiyuu?.PtId ?? 0,
                                                                              p.RecedenHenJiyuu?.HokenId ?? 0,
                                                                              p.RecedenHenJiyuu?.SinYm ?? 0,
                                                                              p.RecedenHenJiyuu?.SeqNo ?? 0,
                                                                              p.RecedenHenJiyuu?.HenreiJiyuuCd ?? string.Empty,
                                                                              p.RecedenHenJiyuu?.HenreiJiyuu ?? string.Empty,
                                                                              p.RecedenHenJiyuu?.Hosoku ?? string.Empty,
                                                                              p.RecedenHenJiyuu?.IsDeleted ?? 0,
                                                                              p.PtHokenInfItem?.HokenKbn ?? 0,
                                                                              p.PtHokenInfItem?.Houbetu ?? string.Empty,
                                                                              p.PtHokenInfItem?.StartDate ?? 0,
                                                                              p.PtHokenInfItem?.EndDate ?? 0,
                                                                              p.PtHokenInfItem?.HokensyaNo ?? string.Empty)).ToList() ?? new())
                {
                    OriginSeikyuYm = u.ReceSeikyuPending?.ReceSeikyu?.SeikyuYm ?? 0,
                    OriginSinYm = u.ReceSeikyuPending?.ReceSeikyu?.SinYm ?? 0,
                    IsChecked = u.ReceSeikyuPending?.ReceSeikyu?.SeikyuYm != 999999
                }
                ).OrderByDescending(u => u.SeikyuYm).ThenBy(u => u.SinYm).ThenBy(u => u.PtNum).ToList();

                return result;
            }
            else
            {
                var receSeikyuInf = from receSeikyu in listReceSeikyu
                                    join ptHokenInf in ptHoken on
                                      new { receSeikyu.PtId, receSeikyu.HokenId } equals
                                      new { ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenList
                                    from item in ptHokenList.DefaultIfEmpty()
                                    join ptInf in ptInfo on receSeikyu.PtId equals ptInf.PtId
                                    where receSeikyu.IsDeleted == DeleteTypes.None
                                          && !(!isIncludingUnConfirmed && receSeikyu.SeikyuYm == 999999)
                                          && !(sinYm > 0 && receSeikyu.SeikyuYm != 999999 && receSeikyu.SeikyuYm != sinYm)

                                          && (ptNumSearch == 0 || ptInf.PtNum == ptNumSearch)
                                          && (noFilter ||
                                                (isFilterMonthlyDelay && receSeikyu.SeikyuKbn == 1) ||
                                                (isFilterReturn && receSeikyu.SeikyuKbn == 2) ||
                                                (isFilterOnlineReturn && receSeikyu.SeikyuKbn == 3)
                                              )
                                    select new
                                    {
                                        PtInfo = ptInf,
                                        ReceSeikyu = receSeikyu,
                                        PtHokenInfItem = item ?? new PtHokenInf()
                                    };

                var query = from receSeikyu in receSeikyuInf
                            select new
                            {
                                recedenHenjiyuuList = recedenHenjiyuuInfo.Where(x => x.RecedenHenJiyuu.PtId == receSeikyu.ReceSeikyu.PtId
                                                                                    && x.RecedenHenJiyuu.SinYm == receSeikyu.ReceSeikyu.SinYm
                                                                                    && x.RecedenHenJiyuu.HokenId == receSeikyu.ReceSeikyu.HokenId).AsEnumerable(),
                                ReceSeikyuPending = receSeikyu,
                            };

                return query.AsEnumerable().Select(x => new ReceSeikyuModel(sinDate,
                                                              x.ReceSeikyuPending.PtInfo.HpId,
                                                              x.ReceSeikyuPending.PtInfo.PtId,
                                                              x.ReceSeikyuPending.PtInfo.Name ?? string.Empty,
                                                              x.ReceSeikyuPending.ReceSeikyu.SinYm,
                                                              sinYm,
                                                              x.ReceSeikyuPending.PtHokenInfItem.HokenId,
                                                              x.ReceSeikyuPending.PtHokenInfItem.HokensyaNo ?? string.Empty,
                                                              x.ReceSeikyuPending.ReceSeikyu.SeqNo,
                                                              x.ReceSeikyuPending.ReceSeikyu.SeikyuYm,
                                                              x.ReceSeikyuPending.ReceSeikyu.SeikyuKbn,
                                                              x.ReceSeikyuPending.ReceSeikyu.PreHokenId,
                                                              x.ReceSeikyuPending.ReceSeikyu.Cmt ?? string.Empty,
                                                              x.ReceSeikyuPending.PtInfo.PtNum,
                                                              x.ReceSeikyuPending.PtHokenInfItem.HokenKbn,
                                                              x.ReceSeikyuPending.PtHokenInfItem.Houbetu ?? string.Empty,
                                                              x.ReceSeikyuPending.PtHokenInfItem.StartDate,
                                                              x.ReceSeikyuPending.PtHokenInfItem.EndDate,
                                                              false,
                                                              x.ReceSeikyuPending.ReceSeikyu.SeikyuYm,
                                                              x.ReceSeikyuPending.ReceSeikyu.SinYm,
                                                              false,
                                                              DeleteTypes.None,
                                                              x.ReceSeikyuPending.ReceSeikyu.SeikyuYm != 999999,
                                                              x.recedenHenjiyuuList.Select(m => new RecedenHenJiyuuModel(hpId,
                                                                                                                        m.RecedenHenJiyuu?.PtId ?? 0,
                                                                                                                        m.PtHokenInfItem?.HokenId ?? 0,
                                                                                                                        sinYm,
                                                                                                                        m.RecedenHenJiyuu?.SeqNo ?? 0,
                                                                                                                        m.RecedenHenJiyuu?.HenreiJiyuuCd ?? string.Empty,
                                                                                                                        m.RecedenHenJiyuu?.HenreiJiyuu ?? string.Empty,
                                                                                                                        m.RecedenHenJiyuu?.Hosoku ?? string.Empty,
                                                                                                                        0,
                                                                                                                        m.PtHokenInfItem?.HokenKbn ?? 0,
                                                                                                                        m.PtHokenInfItem?.Houbetu ?? string.Empty,
                                                                                                                        m.PtHokenInfItem?.SikakuDate ?? 0,
                                                                                                                        m.PtHokenInfItem?.EndDate ?? 0,
                                                                                                                        m.PtHokenInfItem?.HokensyaNo ?? string.Empty)).ToList()
                                                              )).OrderByDescending(o => o.SeikyuKbn).ThenBy(u => u.SinYm).ThenBy(i => i.PtNum).ToList();
            }

        }

        public List<ReceSeikyuModel> GetListReceSeikyModel(int hpId, int seikyuYm, List<long> ptIdList)
        {
            List<ReceSeikyuModel> result = new();
            var ptInfList = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                    && item.IsDelete == DeleteTypes.None
                                                                    && (ptIdList.Count <= 0 || ptIdList.Contains(item.PtId)))
                                                     .ToList();

            ptIdList = ptInfList.Select(item => item.PtId).Distinct().ToList();
            var receSeikyus = NoTrackingDataContext.ReceSeikyus.Where(item => item.HpId == hpId
                                                                              && item.SeikyuYm == seikyuYm
                                                                              && item.IsDeleted == DeleteTypes.None
                                                                              && ptIdList.Contains(item.PtId))
                                                                .ToList();

            foreach (var ptInf in ptInfList)
            {
                var receSeikyu = receSeikyus.FirstOrDefault(item => item.PtId == ptInf.PtId);
                if (receSeikyu == null)
                {
                    continue;
                }
                result.Add(new ReceSeikyuModel(
                               ptInf.PtId,
                               receSeikyu?.SinYm ?? 0,
                               receSeikyu?.HokenId ?? 0,
                               ptInf.PtNum,
                               receSeikyu?.SeikyuKbn ?? 0
                          ));
            }
            return result;
        }

        public ReceSeikyuModel GetReceSeikyModelByPtNum(int hpId, int sinDate, int sinYm, long ptNum)
        {
            ReceSeikyuModel? result = null;

            var ptInfo = NoTrackingDataContext.PtInfs.Where(u => u.HpId == hpId &&
                                                                                       u.IsDelete == 0 &&
                                                                                       u.PtNum == ptNum).FirstOrDefault() ?? new();
            var raiinInf = NoTrackingDataContext.RaiinInfs.Where(u => u.HpId == hpId &&
                                                                                      u.IsDeleted == DeleteTypes.None &&
                                                                                      u.Status >= RaiinState.Calculate &&
                                                                                      u.PtId == ptInfo.PtId &&
                                                                                      ((sinYm == 999999 || sinYm == 0) || (u.SinDate / 100) == sinYm));
            var ptHokenPattentInfo = NoTrackingDataContext.PtHokenPatterns.Where(u => u.HpId == hpId &&
                                                                                                      u.IsDeleted == 0 &&
                                                                                                      u.PtId == ptInfo.PtId);
            var ptHokenInfo = NoTrackingDataContext.PtHokenInfs.Where(u => u.HpId == hpId &&
                                                                                           u.PtId == ptInfo.PtId &&
                                                                                           u.IsDeleted == 0);
            var listRecedenHenjiyuu = NoTrackingDataContext.RecedenHenJiyuus.Where(u => u.HpId == hpId &&
                                                                                                        u.PtId == ptInfo.PtId &&
                                                                                                        u.IsDeleted == 0 &&
                                                                                                        ((sinYm == 999999 || sinYm == 0) || u.SinYm == sinYm));

            var ptInformation = (from raiin in raiinInf
                                 join hokenpattent in ptHokenPattentInfo on raiin.HokenPid equals hokenpattent.HokenPid
                                 join pthoken in ptHokenInfo on hokenpattent.HokenId equals pthoken.HokenId
                                 select new
                                 {
                                     PtHokenInfo = pthoken,
                                 }).FirstOrDefault();
            var recedenHenjiyuuInfo = (from recedenHenjiyuu in listRecedenHenjiyuu
                                       join ptHokenInf in ptHokenInfo on
                                            new { recedenHenjiyuu.PtId, recedenHenjiyuu.HokenId } equals
                                            new { ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenList
                                       from ptHokenInfItem in ptHokenList
                                       select new
                                       {
                                           RecedenHenJiyuu = recedenHenjiyuu,
                                           PtHokenInfItem = ptHokenInfItem,
                                       }).ToList();
            if (ptInformation != null)
            {
                result = new ReceSeikyuModel(sinDate,
                                            ptInformation?.PtHokenInfo?.HpId ?? 0,
                                            ptInformation?.PtHokenInfo?.PtId ?? 0,
                                            ptInfo?.Name ?? string.Empty,
                                            sinYm,
                                            0,
                                            ptInformation?.PtHokenInfo?.HokenId ?? 0,
                                            ptInformation?.PtHokenInfo?.HokensyaNo ?? string.Empty,
                                            0,
                                            0,
                                            0,
                                            0,
                                            string.Empty,
                                            ptInfo?.PtNum ?? 0,
                                            ptInformation?.PtHokenInfo?.HokenKbn ?? 0,
                                            ptInformation?.PtHokenInfo?.Houbetu ?? string.Empty,
                                            ptInformation?.PtHokenInfo?.StartDate ?? 0,
                                            ptInformation?.PtHokenInfo?.EndDate ?? 0,
                                            false,
                                            0,
                                            0,
                                            false,
                                            DeleteTypes.None,
                                            false,
                                            recedenHenjiyuuInfo.Select(p => new RecedenHenJiyuuModel(p.RecedenHenJiyuu?.HpId ?? 0,
                                            p.RecedenHenJiyuu?.PtId ?? 0,
                                            p.RecedenHenJiyuu?.HokenId ?? 0,
                                            p.RecedenHenJiyuu?.SinYm ?? 0,
                                            p.RecedenHenJiyuu?.SeqNo ?? 0,
                                            p.RecedenHenJiyuu?.HenreiJiyuuCd ?? string.Empty,
                                            p.RecedenHenJiyuu?.HenreiJiyuu ?? string.Empty,
                                            p.RecedenHenJiyuu?.Hosoku ?? string.Empty,
                                            p.RecedenHenJiyuu?.IsDeleted ?? 0,
                                            p.PtHokenInfItem?.HokenKbn ?? 0,
                                            p.PtHokenInfItem?.Houbetu ?? string.Empty,
                                            p.PtHokenInfItem?.StartDate ?? 0,
                                            p.PtHokenInfItem?.EndDate ?? 0,
                                            p.PtHokenInfItem?.HokensyaNo ?? string.Empty)).ToList() ?? new());
            }

            return result ?? new();
        }

        public IEnumerable<RegisterSeikyuModel> SearchReceInf(int hpId, long ptNum, int sinYm)
        {
            PtInf? ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(u => u.HpId == hpId && u.PtNum == ptNum && u.IsDelete == 0);
            if (ptInf == null) return new List<RegisterSeikyuModel>();

            var listPtHokenInf = NoTrackingDataContext.PtHokenInfs.Where(u => u.HpId == hpId && u.PtId == ptInf.PtId && u.IsDeleted == 0);
            var listReceInf = NoTrackingDataContext.ReceInfs.Where(u => u.HpId == hpId && u.PtId == ptInf.PtId && (sinYm <= 0 || u.SinYm <= sinYm)).OrderBy(u => u.SinYm);
            var query = from receInf in listReceInf
                        join ptHoken in listPtHokenInf on receInf.HokenId equals ptHoken.HokenId
                        select new
                        {
                            ReceInf = receInf,
                            PtHokenInfo = ptHoken
                        };

            return query.AsEnumerable().Select(u => new RegisterSeikyuModel(ptInf.PtId,
                                                                            ptInf.Name ?? string.Empty,
                                                                            u.ReceInf.SinYm,
                                                                            u.ReceInf.SeikyuYm,
                                                                            u.ReceInf.SeikyuKbn,
                                                                            0,
                                                                            u.ReceInf.HokenId,
                                                                            u.ReceInf.HokensyaNo ?? string.Empty,
                                                                            u.ReceInf.HokenKbn,
                                                                            u.ReceInf.Houbetu ?? string.Empty,
                                                                            u.ReceInf.HonkeKbn,
                                                                            u.PtHokenInfo.StartDate,
                                                                            u.PtHokenInfo.EndDate,
                                                                            false)).OrderByDescending(u => u.SeikyuYm).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public void EntryDeleteHenJiyuu(int hpId, long ptId, int sinYm, int preHokenId, int userId)
        {
            var henJiyuuList = TrackingDataContext.RecedenHenJiyuus.Where(item => item.HpId == hpId
                                                                              && item.PtId == ptId
                                                                              && item.SinYm == sinYm
                                                                              && (item.HokenId == preHokenId || preHokenId == 0)
                                                                              && item.IsDeleted == 0).ToList();
            foreach (var henJiyuu in henJiyuuList)
            {
                henJiyuu.IsDeleted = DeleteTypes.Deleted;
                henJiyuu.UpdateId = userId;
                henJiyuu.UpdateDate = CIUtil.GetJapanDateTimeNow();
            }
        }

        public bool InsertNewReceSeikyu(List<ReceSeikyuModel> listInsert, int userId, int hpId)
        {
            var addedList = listInsert.Select(item => Mapper.Map(item, new ReceSeikyu(), (src, dest) =>
            {
                dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                dest.CreateId = userId;
                dest.UpdateId = userId;
                dest.SeqNo = 0;
                dest.HpId = hpId;
                if (src.HpId == 0 && src.Cmt == "返戻ファイルより登録")
                {
                    dest.SeikyuKbn = 3;
                }
                return dest;
            })).ToList();
            TrackingDataContext.ReceSeikyus.AddRange(addedList);
            return TrackingDataContext.SaveChanges() > 0;
        }

        public int InsertNewReceSeikyu(ReceSeikyuModel model, int userId, int hpId)
        {
            ReceSeikyu entity = new();
            entity.PtId = model.PtId;
            entity.SeikyuYm = model.SeikyuYm;
            entity.SinYm = model.SinYm;
            entity.HokenId = model.HokenId;
            entity.SeikyuKbn = model.SeikyuKbn;
            entity.IsDeleted = 0;
            entity.CreateDate = CIUtil.GetJapanDateTimeNow();
            entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
            entity.CreateId = userId;
            entity.UpdateId = userId;
            entity.SeqNo = 0;
            entity.HpId = hpId;
            TrackingDataContext.ReceSeikyus.Add(entity);
            TrackingDataContext.SaveChanges();
            return entity.SeqNo;
        }

        public bool UpdateReceSeikyu(List<ReceSeikyuModel> receSeikyuList, int userId, int hpId)
        {
            var seqNoList = receSeikyuList.Select(item => item.SeqNo).Distinct().ToList();
            var receSeikyuDB = TrackingDataContext.ReceSeikyus.Where(item => item.HpId == hpId
                                                                             && seqNoList.Contains(item.SeqNo)
                                                                             && item.IsDeleted == 0)
                                                              .ToList();
            foreach (var model in receSeikyuList)
            {
                var updateItem = receSeikyuDB.FirstOrDefault(item => item.SinYm == model.SinYm && item.SeqNo == model.SeqNo);
                if (updateItem == null)
                {
                    updateItem = new ReceSeikyu();
                    updateItem.SeqNo = 0;
                    updateItem.HpId = hpId;
                    updateItem.CreateDate = CIUtil.GetJapanDateTimeNow();
                    updateItem.CreateId = userId;
                }
                updateItem.PtId = model.PtId;
                updateItem.SinYm = model.SinYm;
                updateItem.HokenId = model.HokenId;
                updateItem.SeikyuKbn = model.SeikyuKbn;
                updateItem.PreHokenId = model.PreHokenId;
                updateItem.Cmt = model.Cmt;
                updateItem.IsDeleted = model.IsDeleted;
                updateItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                updateItem.UpdateId = userId;
                if (updateItem.SeqNo == 0)
                {
                    updateItem.IsDeleted = 0;
                    TrackingDataContext.Add(updateItem);
                }
            }
            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool SaveReceSeiKyu(int hpId, int userId, List<ReceSeikyuModel> data)
        {
            var addedList = data.FindAll(item => item.SeqNo == 0 && item.OriginSinYm != item.SinYm).Select(item => Mapper.Map(item, new ReceSeikyu(), (src, dest) =>
            {
                dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                dest.CreateId = userId;
                dest.UpdateId = userId;
                dest.HpId = hpId;
                if (src.HpId == 0 && src.Cmt == "返戻ファイルより登録")
                {
                    dest.SeikyuKbn = 3;
                }
                return dest;
            })).ToList();
            TrackingDataContext.ReceSeikyus.AddRange(addedList);

            foreach (var item in data.Where(x => x.SeqNo != 0 && x.OriginSinYm == x.SinYm))
            {
                var update = TrackingDataContext.ReceSeikyus.FirstOrDefault(x => x.HpId == hpId && x.SeqNo == item.SeqNo);
                if (update != null)
                {
                    if (item.IsDeleted == DeleteTypes.Deleted)
                    {
                        update.IsDeleted = DeleteTypes.Deleted;
                        update.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        update.UpdateId = userId;
                    }
                    else
                    {
                        update.SeikyuYm = item.SeikyuYm;
                        update.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        update.UpdateId = userId;
                        update.SeikyuKbn = item.SeikyuKbn;
                        update.PreHokenId = item.PreHokenId;
                        update.Cmt = item.Cmt;
                    }
                }
            }
            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool RemoveReceSeikyuDuplicateIfExist(long ptId, int sinYm, int hokenId, int userId, int hpId)
        {
            var model = TrackingDataContext.ReceSeikyus.FirstOrDefault(u => u.HpId == hpId &&
                                                                             u.PtId == ptId &&
                                                                             u.SinYm == sinYm &&
                                                                             u.HokenId == hokenId &&
                                                                             u.SeikyuYm != 999999 &&
                                                                             u.IsDeleted == 0);

            if (model is null)
                return true;
            else
            {
                model.IsDeleted = DeleteTypes.Deleted;
                return TrackingDataContext.SaveChanges() > 0;
            }
        }

        public bool UpdateSeikyuYmReceipSeikyuIfExist(long ptId, int sinYm, int hokenId, int seikyuYm, int userId, int hpId)
        {
            var model = TrackingDataContext.ReceSeikyus.FirstOrDefault(u => u.HpId == hpId &&
                                                                             u.PtId == ptId &&
                                                                             u.SinYm == sinYm &&
                                                                             u.HokenId == hokenId &&
                                                                             u.SeikyuYm == 999999 &&
                                                                             u.IsDeleted == 0);
            if (model is null)
                return true;
            else
            {
                model.SeikyuYm = seikyuYm;
                return TrackingDataContext.SaveChanges() > 0;
            }
        }

        public bool IsReceSeikyuExisted(int hpId, long ptId, int sinYm, int hokenId)
        {
            return NoTrackingDataContext.ReceSeikyus.FirstOrDefault(item => item.HpId == hpId
                                                                                            && item.PtId == ptId
                                                                                            && item.HokenId != hokenId
                                                                                            && item.PreHokenId == hokenId
                                                                                            && item.SinYm == sinYm
                                                                                            && item.IsDeleted == 0) != null;
        }

        public int GetReceSeikyuPreHoken(int hpId, long ptId, int sinYm, int hokenId)
        {
            var receSeikyu = NoTrackingDataContext.ReceSeikyus.FirstOrDefault(item => item.HpId == hpId
                                                                                            && item.PtId == ptId
                                                                                            && item.HokenId == hokenId
                                                                                            && item.SinYm == sinYm
                                                                                            && item.SeikyuKbn == 3
                                                                                            && item.IsDeleted == 0);
            if (receSeikyu != null)
            {
                return receSeikyu.PreHokenId;
            }
            return 0;
        }

        public void DeleteReceSeikyu(int hpId, long ptId, int sinYm, int hokenId)
        {
            var receSeikyuList = TrackingDataContext.ReceSeikyus.Where(item => item.HpId == hpId
                                                                                    && item.PtId == ptId
                                                                                    && item.SinYm == sinYm
                                                                                    && item.HokenId == hokenId
                                                                                    && item.IsDeleted == 0);
            foreach (var receSeikyu in receSeikyuList)
            {
                receSeikyu.IsDeleted = 1;
            }
        }

        public void DeleteHenJiyuuRireki(int hpId, long ptId, int sinYm, int preHokenId)
        {
            // RECEDEN_HEN_JIYUU
            var henJiyuuList = TrackingDataContext.RecedenHenJiyuus.Where(item => item.HpId == hpId
                                                                              && item.PtId == ptId
                                                                              && item.SinYm == sinYm
                                                                              && item.HokenId == preHokenId
                                                                              && item.IsDeleted == 0);
            foreach (var henJiyuu in henJiyuuList)
            {
                henJiyuu.IsDeleted = 1;
            }

            // RECEDEN_RIREKI_INF
            var rirekiInfList = TrackingDataContext.RecedenRirekiInfs.Where(item => item.HpId == hpId
                                                                              && item.PtId == ptId
                                                                              && item.SinYm == sinYm
                                                                              && item.HokenId == preHokenId
                                                                              && item.IsDeleted == 0);
            foreach (var rirekiInf in rirekiInfList)
            {
                rirekiInf.IsDeleted = 1;
            }
        }

        public void InsertSingleReceSeikyu(int hpId, long ptId, int sinYm, int hokenId, int userId)
        {
            ReceSeikyu receSeikyu = new ReceSeikyu()
            {
                HpId = hpId,
                PtId = ptId,
                SinYm = sinYm,
                HokenId = hokenId,
                SeikyuYm = 999999,
                SeikyuKbn = 3,
                PreHokenId = hokenId,
                Cmt = "返戻ファイルより登録",
                IsDeleted = 0,
                CreateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId,
                UpdateDate = CIUtil.GetJapanDateTimeNow()
            };
            TrackingDataContext.ReceSeikyus.Add(receSeikyu);
        }

        public void InsertSingleRerikiInf(int hpId, long ptId, int sinYm, int hokenId, string searchNo, string rireki, int userId)
        {
            RecedenRirekiInf recedenRirekiInf = new RecedenRirekiInf()
            {
                HpId = hpId,
                PtId = ptId,
                SinYm = sinYm,
                HokenId = hokenId,
                SearchNo = searchNo,
                Rireki = rireki,
                IsDeleted = 0,
                CreateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId,
                UpdateDate = CIUtil.GetJapanDateTimeNow()
            };
            TrackingDataContext.RecedenRirekiInfs.Add(recedenRirekiInf);
        }

        public void InsertSingleHenJiyuu(int hpId, long ptId, int sinYm, int hokenId, string hosoku, string henreiJiyuuCd, string henreiJiyuu, int userId)
        {
            RecedenHenJiyuu recedenHen = new RecedenHenJiyuu()
            {
                HpId = hpId,
                PtId = ptId,
                SinYm = sinYm,
                HokenId = hokenId,
                Hosoku = hosoku,
                HenreiJiyuu = henreiJiyuu,
                HenreiJiyuuCd = henreiJiyuuCd,
                IsDeleted = 0,
                CreateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId,
                UpdateDate = CIUtil.GetJapanDateTimeNow()
            };
            TrackingDataContext.RecedenHenJiyuus.Add(recedenHen);
        }

        /// <summary>
        /// Save changes all entities tracking by this TennantDataContext
        /// </summary>
        /// <returns></returns>
        public bool SaveChangeImportFileRececeikyus()
        {
            int changeDatas = TrackingDataContext.ChangeTracker.Entries().Count(x => x.State == EntityState.Modified || x.State == EntityState.Added);
            if (changeDatas == 0) return true; //case nochanges
            return TrackingDataContext.SaveChanges() > 0;
        }

        public ReceSeikyuModel GetReceSeikyuDuplicate(int hpId, long ptId, int sinYm, int hokenId)
        {
            var result = NoTrackingDataContext.ReceSeikyus.FirstOrDefault(item => item.HpId == hpId &&
                                                                                  item.PtId == ptId &&
                                                                                  item.SinYm == sinYm &&
                                                                                  item.HokenId == hokenId &&
                                                                                  item.SeikyuYm != 999999 &&
                                                                                  item.IsDeleted == 0);
            if (result != null)
            {
                return new ReceSeikyuModel(0, result.HpId, result.PtId, string.Empty, result.SinYm, 0, result.HokenId, string.Empty, result.SeqNo, result.SeikyuYm, result.SeikyuKbn, result.PreHokenId, result.Cmt ?? string.Empty, 0, 0, string.Empty, 0, 0, false, 0, 0, false, 0, false, new());
            }
            return new();
        }

        public List<RecedenHenJiyuuModel> GetRecedenHenJiyuuModels(int hpId, long ptId, int sinYm)
        {
            var recedenHenjiyuus = NoTrackingDataContext.RecedenHenJiyuus.Where(item => item.HpId == hpId
                                                                                        && item.PtId == ptId
                                                                                        && item.IsDeleted == 0
                                                                                        && item.SinYm == sinYm);
            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(item => item.HpId == hpId
                                                                              && item.PtId == ptId
                                                                              && item.IsDeleted == 0);

            var result = (from recedenHenjiyuu in recedenHenjiyuus
                          join ptHokenInf in ptHokenInfs on
                          new { recedenHenjiyuu.HpId, recedenHenjiyuu.PtId, recedenHenjiyuu.HokenId } equals
                          new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
                          select new RecedenHenJiyuuModel(
                                     hpId,
                                     recedenHenjiyuu.PtId,
                                     recedenHenjiyuu.HokenId,
                                     recedenHenjiyuu.SinYm,
                                     recedenHenjiyuu.SeqNo,
                                     recedenHenjiyuu.HenreiJiyuuCd ?? string.Empty,
                                     recedenHenjiyuu.HenreiJiyuu ?? string.Empty,
                                     recedenHenjiyuu.Hosoku ?? string.Empty,
                                     recedenHenjiyuu.IsDeleted,
                                     ptHokenInf.HokenKbn,
                                     ptHokenInf.Houbetu ?? string.Empty,
                                     ptHokenInf.StartDate,
                                     ptHokenInf.EndDate,
                                     ptHokenInf.HokensyaNo ?? string.Empty
                          )).ToList();

            return result;
        }
    }
}
