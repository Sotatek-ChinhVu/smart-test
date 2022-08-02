using Domain.Models.UketukeSbtDayInf;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class UketukeSbtDayInfRepository : IUketukeSbtDayInfRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public UketukeSbtDayInfRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<UketukeSbtDayInfModel> GetListBySinDate(int sinDate)
    {
        return _tenantDataContext.UketukeSbtDayInfs
            .Where(u => u.SinDate == sinDate).ToList()
            .Select(u => ToModel(u)).ToList();
    }

    public void Upsert(int sinDate, int uketukeSbt, int seqNo)
    {
        var dayInf = _tenantDataContext.UketukeSbtDayInfs.AsTracking().Where(d => d.SinDate == sinDate).FirstOrDefault();
        if (dayInf is null)
        {
            _tenantDataContext.UketukeSbtDayInfs.Add(new UketukeSbtDayInf
            {
                HpId = 1,
                SinDate = sinDate,
                UketukeSbt = uketukeSbt,
                SeqNo = seqNo,
                CreateId = CommonConstants.InvalidId,
                CreateDate = DateTime.UtcNow,
                CreateMachine = CIUtil.GetComputerName()
            });
        }
        else
        {
            dayInf.UketukeSbt = uketukeSbt;
        }

        _tenantDataContext.SaveChanges();
    }

    private UketukeSbtDayInfModel ToModel(UketukeSbtDayInf u)
    {
        return new UketukeSbtDayInfModel(u.SinDate, u.SeqNo, u.UketukeSbt);
    }
}
