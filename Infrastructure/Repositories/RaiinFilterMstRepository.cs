using Domain.Models.RaiinFilterMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RaiinFilterMstRepository : RepositoryBase, IRaiinFilterMstRepository
{
    public RaiinFilterMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public int GetLastTimeDate(int hpId, long ptId, int sinDate)
    {
        var result = NoTrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId
                                                        && x.PtId == ptId
                                                        && x.SinDate < sinDate
                                                        && x.Status >= RaiinState.TempSave
                                                        && x.IsDeleted == DeleteTypes.None
                                                    )
                                                    .OrderByDescending(x => x.SinDate)
                                                    .FirstOrDefault();
        return result != null ? result.SinDate : 0;
    }

    public List<RaiinFilterMstModel> GetList()
    {
        var query =
            from mst in NoTrackingDataContext.RaiinFilterMsts
            where mst.IsDeleted == DeleteTypes.None
            select new
            {
                mst,
                sorts = NoTrackingDataContext.RaiinFilterSorts
                    .Where(s => s.FilterId == mst.FilterId && s.IsDeleted == DeleteTypes.None)
                    .ToList()
            };

        var mstWithSorts = query.ToList();
        return mstWithSorts.Select(x => new RaiinFilterMstModel(
            x.mst.FilterId,
            x.mst.SortNo,
            x.mst.FilterName ?? string.Empty,
            x.mst.SelectKbn,
            x.mst.Shortcut ?? string.Empty,
            columnSortInfos: x.sorts.Select(s => new RaiinFilterSortModel(
                s.Id,
                s.FilterId,
                s.SeqNo,
                s.Priority,
                s.ColumnName ?? string.Empty,
                s.KbnCd,
                s.SortKbn
            )).ToList()
        )).ToList();
    }

    public int GetTantoId(long ptId, int sinDate, long raiinNo)
    {
        var raiinInf = NoTrackingDataContext.RaiinInfs.FirstOrDefault(p => p.PtId == ptId
                                                                && p.IsDeleted == DeleteTypes.None
                                                                && p.SinDate == sinDate
                                                                && p.RaiinNo == raiinNo);
        return raiinInf != null ? raiinInf.TantoId : 0;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public void SaveList(List<RaiinFilterMstModel> mstModels, int hpId, int userId)
    {
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(() =>
        {
            using var transaction = TrackingDataContext.Database.BeginTransaction();

            var query =
                from mst in TrackingDataContext.RaiinFilterMsts.AsTracking()
                where mst.IsDeleted == DeleteTypes.None
                select new
                {
                    mst,
                    sorts = TrackingDataContext.RaiinFilterSorts.AsTracking()
                        .Where(s => s.FilterId == mst.FilterId && s.IsDeleted == DeleteTypes.None)
                        .ToList()
                };

            var existingEntities = query.ToList();
            var sortsToInsert = new List<RaiinFilterSort>();
            var mstWithSortsToInsert = new List<(RaiinFilterMst Mst, List<RaiinFilterSort> Sorts)>();

            foreach (var mstModel in mstModels)
            {
                var entityToUpdate = existingEntities.FirstOrDefault(m => m.mst.FilterId == mstModel.FilterId);
                if (entityToUpdate is not null)
                {
                    var mstToUpdate = entityToUpdate.mst;
                    UpdateMstIfChanged(mstToUpdate, mstModel, userId);

                    foreach (var sortModel in mstModel.ColumnSortInfos)
                    {
                        var sortToUpdate = entityToUpdate.sorts.FirstOrDefault(s => s.Id == sortModel.Id);
                        if (sortToUpdate is not null)
                        {
                            UpdateSortIfChanged(sortToUpdate, sortModel, userId);
                        }
                        else
                        {
                            sortsToInsert.Add(CreateSortEntity(mstToUpdate.FilterId, sortModel, hpId, userId));
                        }
                    }
                }
                else
                {
                    var tempFilterId = 0;
                    var mst = new RaiinFilterMst
                    {
                        HpId = hpId,
                        FilterId = tempFilterId,
                        SortNo = mstModel.SortNo,
                        FilterName = mstModel.FilterName,
                        SelectKbn = mstModel.SelectKbn,
                        Shortcut = mstModel.Shortcut,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId
                    };
                    // Create RaiinFilterSort entities with temporary FilterId = 0
                    var sorts = mstModel.ColumnSortInfos.Select(sortModel => CreateSortEntity(tempFilterId, sortModel, hpId, userId)).ToList();

                    mstWithSortsToInsert.Add(new(mst, sorts));
                }
            }

            // Insert msts
            var mstsToInsert = mstWithSortsToInsert.Select(x => x.Mst);
            TrackingDataContext.RaiinFilterMsts.AddRange(mstsToInsert);
            TrackingDataContext.SaveChanges();
            // Insert related sorts
            // After SaveChanges called FilterId of RaiinFilterMst is populated
            // so we can set FilterId for the related RaiinFilterSort entities here
            foreach (var (mst, sorts) in mstWithSortsToInsert)
            {
                foreach (var sort in sorts)
                {
                    sort.FilterId = mst.FilterId;
                    sortsToInsert.Add(sort);
                }
            }

            TrackingDataContext.RaiinFilterSorts.AddRange(sortsToInsert);

            // If entities don't exist in the models, we will delete them
            foreach (var item in existingEntities)
            {
                bool mstExistsInModels = mstModels.Exists(m => m.FilterId == item.mst.FilterId);
                if (!mstExistsInModels)
                {
                    item.mst.IsDeleted = DeleteTypes.Deleted;
                }

                foreach (var sort in item.sorts)
                {
                    if (!SortExistsInModels(sort.Id))
                    {
                        sort.IsDeleted = DeleteTypes.Deleted;
                    }
                }
            }

            TrackingDataContext.SaveChanges();

            transaction.Commit();
        });

        #region Helper methods

        void UpdateMstIfChanged(RaiinFilterMst entity, RaiinFilterMstModel model, int userId)
        {
            // Detect changes
            if (entity.SortNo != model.SortNo
                || entity.FilterName != model.FilterName
                || entity.SelectKbn != model.SelectKbn
                || entity.Shortcut != model.Shortcut)
            {
                entity.SortNo = model.SortNo;
                entity.FilterName = model.FilterName;
                entity.SelectKbn = model.SelectKbn;
                entity.Shortcut = model.Shortcut;
                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                entity.UpdateId = userId;
            }
        }

        void UpdateSortIfChanged(RaiinFilterSort entity, RaiinFilterSortModel model, int userId)
        {
            // Detect changes
            if (entity.SeqNo != model.SeqNo
                || entity.Priority != model.Priority
                || entity.ColumnName != model.ColumnName
                || entity.KbnCd != model.KbnCd
                || entity.SortKbn != model.SortKbn)
            {
                entity.SeqNo = model.SeqNo;
                entity.Priority = model.Priority;
                entity.ColumnName = model.ColumnName;
                entity.KbnCd = model.KbnCd;
                entity.SortKbn = model.SortKbn;
                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                entity.UpdateId = userId;
            }
        }

        RaiinFilterSort CreateSortEntity(int filterId, RaiinFilterSortModel sortModel, int hpId, int userId)
        {
            return new RaiinFilterSort
            {
                HpId = hpId,
                FilterId = filterId,
                SeqNo = sortModel.SeqNo,
                Priority = sortModel.Priority,
                ColumnName = sortModel.ColumnName,
                KbnCd = sortModel.KbnCd,
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId,
                SortKbn = sortModel.SortKbn,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                CreateId = userId
            };
        }

        bool SortExistsInModels(long id)
        {
            foreach (var mstModel in mstModels)
            {
                foreach (var sortModel in mstModel.ColumnSortInfos)
                {
                    if (sortModel.Id == id) return true;
                }
            }

            return false;
        }

        #endregion
    }
}
