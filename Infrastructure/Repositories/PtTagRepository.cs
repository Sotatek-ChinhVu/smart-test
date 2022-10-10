using Domain.Models.PtTag;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using PostgreDataContext;
using System;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositories;

public class PtTagRepository : IPtTagRepository
{
    private readonly TenantNoTrackingDataContext _tenantDataContextNoTracking;
    private readonly TenantDataContext _tenantDataContextTracking;


    public PtTagRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContextNoTracking = tenantProvider.GetNoTrackingDataContext();
        _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<StickyNoteModel> SearchByPtId(int hpId, int ptId)
    {
        return _tenantDataContextNoTracking.PtTag.Where(x => x.HpId == hpId && x.PtId == ptId && (x.IsDeleted == 0 || x.IsDeleted == 1))
            .Select(x => new StickyNoteModel(x.HpId, x.PtId, x.SeqNo, x.Memo, x.MemoData ?? Array.Empty<byte>(), x.StartDate, x.EndDate, x.IsDspUketuke, x.IsDspKarte, x.IsDspKaikei, x.IsDspRece, x.BackgroundColor, x.TagGrpCd, x.AlphablendVal, x.FontSize, x.IsDeleted, x.Width, x.Height, x.Left, x.Top))
            .ToList();
    }
    public bool UpdateIsDeleted(int hpId, int ptId, int seqNo, int isDeleted)
    {
        try
        {
            var ptTag = _tenantDataContextNoTracking.PtTag.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId && x.SeqNo == seqNo);

            if (ptTag == null) return false;

            if (ptTag.IsDeleted == 2) return false;

            ptTag.IsDeleted = isDeleted;

            ptTag.UpdateDate = DateTime.UtcNow;
            ptTag.UpdateId = TempIdentity.UserId;
            ptTag.UpdateMachine = TempIdentity.ComputerName;
            ptTag.CreateDate = DateTime.SpecifyKind(ptTag.CreateDate, DateTimeKind.Utc);

            _tenantDataContextTracking.PtTag.Update(ptTag);
            _tenantDataContextTracking.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
