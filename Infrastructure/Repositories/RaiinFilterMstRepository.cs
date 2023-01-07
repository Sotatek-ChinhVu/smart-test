using Domain.Models.RaiinFilterMst;
using Domain.Models.ReceptionSameVisit;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using System.Linq;

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

    public List<RaiinFilterMstModel> GetListRaiinInf(int hpId, long ptId)
    {
        var result = new List<RaiinFilterMstModel>();
        var usermsts = _tenantDataContext.UserMsts.Where(x => 
                    x.HpId == hpId &&
                    x.Id == ptId &&
                    ptId.Equals(x.Id) &&
                    x.IsDeleted ==0
                    );
        var raiinInfs = _tenantDataContext.RaiinInfs.Where(x =>
                    x.HpId == hpId &&
                    x.IsDeleted == 0 &&
                    x.PtId == ptId
                    );
        var kaMsts = _tenantDataContext.KaMsts.Where(x =>
                    x.HpId == hpId &&
                    x.IsDeleted == 0
                    );
        var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.Where(x =>
                    x.HpId == hpId &&
                    x.IsDeleted ==0 &&
                    x.PtId == ptId
                    );
        var ptHokenInfs = _tenantDataContext.PtHokenInfs.Where(x =>
                    x.HpId == hpId &&
                    x.IsDeleted == 0 &&
                    x.PtId == ptId
                    );
        var ptKohis = _tenantDataContext.PtKohis.Where(x =>
                    x.HpId == hpId &&
                   x.IsDeleted == 0 &&
                   x.PtId == ptId
                    );

        var query = from raiinInf in raiinInfs.AsEnumerable()
                    join KaMst in kaMsts on
                       new { raiinInf.HpId, raiinInf.KaId } equals
                       new { KaMst.HpId, KaMst.KaId } into listKaMst
                    join usermst in usermsts on
                        new { raiinInf.HpId, raiinInf.TantoId } equals
                        new { usermst.HpId, TantoId = usermst.UserId } into listUserMst
                    join ptHokenPattern in ptHokenPatterns on
                         new { raiinInf.HpId, raiinInf.PtId, raiinInf.HokenPid } equals
                         new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid } into raiinPtHokenPatterns
                    from raiinPtHokenPattern in raiinPtHokenPatterns.DefaultIfEmpty()
                    join ptKohi1 in ptKohis on
                         new { raiinPtHokenPattern.HpId, raiinPtHokenPattern.PtId, HokenId = raiinPtHokenPattern.Kohi1Id } equals
                         new { ptKohi1.HpId, ptKohi1.PtId, ptKohi1.HokenId } into ptKoHi1Infs
                    from ptKoHi1Inf in ptKoHi1Infs.DefaultIfEmpty()
                    join ptKohi2 in ptKohis on
                         new { raiinPtHokenPattern.HpId, raiinPtHokenPattern.PtId, HokenId = raiinPtHokenPattern.Kohi2Id } equals
                         new { ptKohi2.HpId, ptKohi2.PtId, ptKohi2.HokenId } into ptKoHi2Infs
                    from ptKoHi2Inf in ptKoHi2Infs.DefaultIfEmpty()
                    join ptKohi3 in ptKohis on
                         new { raiinPtHokenPattern.HpId, raiinPtHokenPattern.PtId, HokenId = raiinPtHokenPattern.Kohi3Id } equals
                         new { ptKohi3.HpId, ptKohi3.PtId, ptKohi3.HokenId } into ptKoHi3Infs
                    from ptKoHi3Inf in ptKoHi3Infs.DefaultIfEmpty()
                    join ptKohi4 in ptKohis on
                         new { raiinPtHokenPattern.HpId, raiinPtHokenPattern.PtId, HokenId = raiinPtHokenPattern.Kohi4Id } equals
                         new { ptKohi4.HpId, ptKohi4.PtId, ptKohi4.HokenId } into ptKoHi4Infs
                    from ptKoHi4Inf in ptKoHi4Infs.DefaultIfEmpty()
                    join ptHokenInf in ptHokenInfs on
                         new { raiinPtHokenPattern.HpId, raiinPtHokenPattern.PtId, raiinPtHokenPattern.HokenId } equals
                         new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } into raiinPtHokenInfs
                    from raiinPtHokenInf in raiinPtHokenInfs.DefaultIfEmpty()
                    select new
                    {
                        RaiinInf = raiinInf,
                        HokenPattern = raiinPtHokenPattern,
                        PtHokenInf = raiinPtHokenInf,
                        PtKoHi1 = ptKoHi1Inf,
                        PtKoHi2 = ptKoHi2Inf,
                        PtKoHi3 = ptKoHi3Inf,
                        PtKoHi4 = ptKoHi4Inf,
                        UserMst = listUserMst.FirstOrDefault(),
                        KaMst = listKaMst.FirstOrDefault()
                    };

        result = query.Select((x) => new RaiinFilterMstModel(
                        x.RaiinInf,
                        new HokenPatternModel(x.HokenPattern, x.PtHokenInf, x.PtKoHi1, x.PtKoHi2, x.PtKoHi3, x.PtKoHi4),
                        x.UserMst?.Sname ?? string.Empty,
                        x.KaMst?.KaSname ?? string.Empty))
                        .OrderByDescending(x => x.RaiinInf.SinDate)
                        .ToList();
        return result;
    }

    public void SaveList(List<RaiinFilterMstModel> mstModels, int hpId, int userId)
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
                        UpdateDate = DateTime.UtcNow,
                        UpdateId = userId,
                        CreateDate = DateTime.UtcNow,
                        CreateId = userId
                    };
                    // Create RaiinFilterSort entities with temporary FilterId = 0
                    var sorts = mstModel.ColumnSortInfos.Select(sortModel => CreateSortEntity(tempFilterId, sortModel, hpId, userId)).ToList();

                    mstWithSortsToInsert.Add(new(mst, sorts));
                }
            }

            // Insert msts
            var mstsToInsert = mstWithSortsToInsert.Select(x => x.Mst);
            _tenantDataContext.RaiinFilterMsts.AddRange(mstsToInsert);
            _tenantDataContext.SaveChanges();
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
                entity.UpdateDate = DateTime.UtcNow;
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
                entity.UpdateDate = DateTime.UtcNow;
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
                UpdateDate = DateTime.UtcNow,
                UpdateId = userId,
                SortKbn = sortModel.SortKbn,
                CreateDate = DateTime.UtcNow,
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
