using Domain.Models.UketukeSbtMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class UketukeSbtMstRepository : RepositoryBase, IUketukeSbtMstRepository
{
    public UketukeSbtMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public UketukeSbtMstModel GetByKbnId(int kbnId)
    {
        var entity = NoTrackingDataContext.UketukeSbtMsts.Where(u => u.KbnId == kbnId && u.IsDeleted == DeleteTypes.None).FirstOrDefault();
        return entity is null ? new() : ToModel(entity);
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
        foreach (var inputData in upsertUketukeList)
        {
            var uketukeSbtMsts = TrackingDataContext.UketukeSbtMsts.FirstOrDefault(x => x.KbnId == inputData.KbnId && x.IsDeleted == DeleteTypes.None);
            if (inputData.IsDeleted == DeleteTypes.Deleted)
            {
                if (uketukeSbtMsts != null)
                {
                    uketukeSbtMsts.IsDeleted = DeleteTypes.Deleted;
                }
            }
            else
            {
                if (uketukeSbtMsts != null)
                {
                    uketukeSbtMsts.KbnName = inputData.KbnName;
                    uketukeSbtMsts.SortNo = inputData.SortNo;
                    uketukeSbtMsts.UpdateMachine = string.Empty;
                    uketukeSbtMsts.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    uketukeSbtMsts.UpdateId = userId;
                }
                else
                {
                    TrackingDataContext.UketukeSbtMsts.AddRange(ConvertToUketukeSbtMst(inputData, userId, hpId));
                }
            }

        }
        TrackingDataContext.SaveChanges();
    }

    private UketukeSbtMst ConvertToUketukeSbtMst(UketukeSbtMstModel u, int userId, int hpId)
    {
        return new UketukeSbtMst
        {
            HpId = hpId,
            KbnId = u.KbnId,
            KbnName = u.KbnName,
            SortNo = u.SortNo,
            IsDeleted = u.IsDeleted,
            CreateDate = CIUtil.GetJapanDateTimeNow(),
            CreateId = userId,
            CreateMachine = string.Empty,
            UpdateDate = CIUtil.GetJapanDateTimeNow(),
            UpdateId = userId,
            UpdateMachine = string.Empty
        };
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
