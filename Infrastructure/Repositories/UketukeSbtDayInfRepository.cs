using Domain.Models.UketukeSbtDayInf;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UketukeSbtDayInfRepository : RepositoryBase, IUketukeSbtDayInfRepository
{
    public UketukeSbtDayInfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<UketukeSbtDayInfModel> GetListBySinDate(int hpId, int sinDate)
    {
        return NoTrackingDataContext.UketukeSbtDayInfs
            .Where(u => u.HpId == hpId && u.SinDate == sinDate).AsEnumerable()
            .Select(u => ToModel(u)).ToList();
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public void Upsert(int hpId, int sinDate, int uketukeSbt, int seqNo, int userId)
    {
        var dayInf = TrackingDataContext.UketukeSbtDayInfs.AsTracking().Where(d => d.HpId == hpId && d.SinDate == sinDate).FirstOrDefault();
        if (dayInf is null)
        {
            TrackingDataContext.UketukeSbtDayInfs.Add(new UketukeSbtDayInf
            {
                HpId = 1,
                SinDate = sinDate,
                UketukeSbt = uketukeSbt,
                SeqNo = seqNo,
                CreateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow()
            });
        }
        else
        {
            dayInf.UketukeSbt = uketukeSbt;
        }

        TrackingDataContext.SaveChanges();
    }

    private UketukeSbtDayInfModel ToModel(UketukeSbtDayInf u)
    {
        return new UketukeSbtDayInfModel(u.SinDate, u.SeqNo, u.UketukeSbt);
    }
}
