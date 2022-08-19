using Domain.Models.RaiinFilterMst;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class RaiinFilterMstRepository : IRaiinFilterMstRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public RaiinFilterMstRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<RaiinFilterMstModel> GetList()
    {
        var query =
            from mst in _tenantDataContext.RaiinFilterMsts
            where mst.IsDeleted == DeleteTypes.None
            select new
            {
                mst,
                sorts = _tenantDataContext.RaiinFilterSorts
                    .Where(s => s.FilterId == mst.FilterId && s.IsDeleted == DeleteTypes.None)
                    .ToList()
            };

        var mstWithSorts = query.ToList();
        return mstWithSorts.Select(x => new RaiinFilterMstModel(
            x.mst.FilterId,
            x.mst.SortNo,
            x.mst.FilterName,
            x.mst.SelectKbn,
            x.mst.Shortcut,
            columnSortInfos: x.sorts.Select(s => new RaiinFilterSortModel(
                s.Id,
                s.FilterId,
                s.SeqNo,
                s.Priority,
                s.ColumnName,
                s.KbnCd,
                s.SortKbn
            )).ToList()
        )).ToList();
    }

    public void SaveList(List<RaiinFilterMstModel> mstModels)
    {
        var executionStrategy = _tenantDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(() =>
        {
            using var transaction = _tenantDataContext.Database.BeginTransaction();

            var query =
                from mst in _tenantDataContext.RaiinFilterMsts.AsTracking()
                where mst.IsDeleted == DeleteTypes.None
                select new
                {
                    mst,
                    sorts = _tenantDataContext.RaiinFilterSorts.AsTracking()
                        .Where(s => s.FilterId == mst.FilterId && s.IsDeleted == DeleteTypes.None)
                        .ToList()
                };

            var existingEntities = query.ToList();
            var sortsToInsert = new List<RaiinFilterSort>();
            var mstVsSortsToInsert = new List<(RaiinFilterMst Mst, List<RaiinFilterSort> Sorts)>();

            foreach (var mstModel in mstModels)
            {
                var entityToUpdate = existingEntities.FirstOrDefault(m => m.mst.FilterId == mstModel.FilterId);
                if (entityToUpdate is not null)
                {
                    var mstToUpdate = entityToUpdate.mst;
                    UpdateMstIfChanged(mstToUpdate, mstModel);

                    foreach (var sortModel in mstModel.ColumnSortInfos)
                    {
                        var sortToUpdate = entityToUpdate.sorts.FirstOrDefault(s => s.Id == sortModel.Id);
                        if (sortToUpdate is not null)
                        {
                            UpdateSortIfChanged(sortToUpdate, sortModel);
                        }
                        else
                        {
                            sortsToInsert.Add(CreateSortEntity(mstToUpdate.FilterId, sortModel));
                        }
                    }
                }
                else
                {
                    var mst = new RaiinFilterMst
                    {
                        HpId = TempIdentity.HpId,
                        SortNo = mstModel.SortNo,
                        FilterName = mstModel.FilterName,
                        SelectKbn = mstModel.SelectKbn,
                        Shortcut = mstModel.Shortcut,
                        CreateDate = DateTime.UtcNow,
                        CreateId = TempIdentity.UserId,
                        CreateMachine = TempIdentity.ComputerName
                    };
                    // Create RaiinFilterSort entities with temporary FilterId = 0
                    var sorts = mstModel.ColumnSortInfos.Select(sortModel => CreateSortEntity(0, sortModel)).ToList();

                    mstVsSortsToInsert.Add(new(mst, sorts));
                }
            }

            // Insert msts
            var mstsToInsert = mstVsSortsToInsert.Select(x => x.Mst);
            _tenantDataContext.RaiinFilterMsts.AddRange(mstsToInsert);
            _tenantDataContext.SaveChanges();
            // Insert related sorts
            // After SaveChanges called FilterId of RaiinFilterMst is populated
            // so we can set FilterId for the related RaiinFilterSort entities here
            foreach (var item in mstVsSortsToInsert)
            {
                foreach (var sort in item.Sorts)
                {
                    sort.FilterId = item.Mst.FilterId;
                    sortsToInsert.Add(sort);
                }
            }

            _tenantDataContext.RaiinFilterSorts.AddRange(sortsToInsert);

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

            _tenantDataContext.SaveChanges();

            transaction.Commit();
        });

        #region Helper methods

        void UpdateMstIfChanged(RaiinFilterMst entity, RaiinFilterMstModel model)
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
                entity.UpdateDate = DateTime.UtcNow;
                entity.UpdateId = TempIdentity.UserId;
                entity.UpdateMachine = TempIdentity.ComputerName;
            }
        }

        void UpdateSortIfChanged(RaiinFilterSort entity, RaiinFilterSortModel model)
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
                entity.UpdateDate = DateTime.UtcNow;
                entity.UpdateId = TempIdentity.UserId;
                entity.UpdateMachine = TempIdentity.ComputerName;
            }
        }

        RaiinFilterSort CreateSortEntity(int filterId, RaiinFilterSortModel sortModel)
        {
            return new RaiinFilterSort
            {
                HpId = TempIdentity.HpId,
                FilterId = filterId,
                SeqNo = sortModel.SeqNo,
                Priority = sortModel.Priority,
                ColumnName = sortModel.ColumnName,
                KbnCd = sortModel.KbnCd,
                SortKbn = sortModel.SortKbn,
                CreateDate = DateTime.UtcNow,
                CreateId = TempIdentity.UserId,
                CreateMachine = TempIdentity.ComputerName
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
