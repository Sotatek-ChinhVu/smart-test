using Domain.Models.ApprovalInfo;
using Domain.Models.UketukeSbtMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class UketukeSbtMstRepository : RepositoryBase, IUketukeSbtMstRepository
{
    public UketukeSbtMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public UketukeSbtMstModel? GetByKbnId(int kbnId)
    {
        var entity = NoTrackingDataContext.UketukeSbtMsts.Where(u => u.KbnId == kbnId && u.IsDeleted == DeleteTypes.None).FirstOrDefault();
        return entity is null ? null : ToModel(entity);
    }

    public List<UketukeSbtMstModel> GetList()
    {
        return NoTrackingDataContext.UketukeSbtMsts
            .Where(u => u.IsDeleted == DeleteTypes.None)
            .OrderBy(u => u.SortNo).AsEnumerable()
            .Select(u => ToModel(u)).ToList();
    }

    private UketukeSbtMstModel ToModel(UketukeSbtMst u)
    {
        return new UketukeSbtMstModel(
            u.KbnId,
            u.KbnName ?? string.Empty,
            u.SortNo,
            u.IsDeleted);
    }

    public void Upsert(List<UketukeSbtMstModel> upsertUketukeList, int userId, int hpId)
    {
        foreach(var inputData in upsertUketukeList)
        {
            if (inputData.IsDeleted == DeleteTypes.Deleted)
            {
                var uketukeSbtMsts = TrackingDataContext.UketukeSbtMsts.FirstOrDefault(x => x.KbnId == inputData.KbnId);
                if (uketukeSbtMsts != null)
                {
                    uketukeSbtMsts.IsDeleted = DeleteTypes.Deleted;
                }
            }
            else
            {
                var uketukeSbtMst = TrackingDataContext.UketukeSbtMsts.FirstOrDefault(x => x.KbnId == inputData.KbnId);
                if (uketukeSbtMst != null)
                {
                    uketukeSbtMst.KbnId = inputData.KbnId;
                    uketukeSbtMst.KbnName = inputData.KbnName;
                    uketukeSbtMst.SortNo = inputData.SortNo;
                    uketukeSbtMst.IsDeleted = inputData.IsDeleted;
                    uketukeSbtMst.UpdateMachine = string.Empty;
                    uketukeSbtMst.UpdateDate = DateTime.UtcNow;
                    uketukeSbtMst.UpdateId = userId;
                }
                else
                {
                    TrackingDataContext.UketukeSbtMsts.AddRange(CreateUketukeSbtMst(inputData, userId, hpId));
                }
            }
            
        }
        TrackingDataContext.SaveChanges();
    }

    private UketukeSbtMst CreateUketukeSbtMst(UketukeSbtMstModel u, int userId, int hpId)
    {
        return new UketukeSbtMst
        {
            HpId = hpId,
            KbnId = u.KbnId,
            KbnName = u.KbnName,
            SortNo = u.SortNo,
            IsDeleted = u.IsDeleted,
            CreateDate = DateTime.UtcNow,
            CreateId = userId,
            CreateMachine = string.Empty,
            UpdateDate = DateTime.UtcNow,
            UpdateId = userId,
            UpdateMachine = string.Empty
        };
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public bool CheckExistedKbnId(List<int> kbnIds)
    {
        var anyUketukeSbtMsts = NoTrackingDataContext.UketukeSbtMsts.Any(x => kbnIds.Contains(x.KbnId) && x.IsDeleted != 1);
        return anyUketukeSbtMsts ;
    }
}
