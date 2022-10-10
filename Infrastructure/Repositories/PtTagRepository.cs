using Domain.Models.PtTag;
using Entity.Tenant;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using PostgreDataContext;
using System.Linq;
using System.Linq.Expressions;

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

    public List<StickyNoteModel> SearchByPtId(int hpId, int ptId, int isDeleted)
    {

        return _tenantDataContextNoTracking.PtTag.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == isDeleted)
            .Select(x => new StickyNoteModel(x.HpId, x.PtId, x.SeqNo, x.Memo, x.MemoData ?? new byte[0], x.StartDate, x.EndDate, x.IsDspUketuke, x.IsDspKarte, x.IsDspKaikei, x.IsDspRece, x.BackgroundColor, x.TagGrpCd, x.AlphablendVal, x.FontSize, x.IsDeleted, x.Width, x.Height, x.Left, x.Top))
            .ToList();
    }
    public bool UpdateIsDeleted(int hpId, int ptId, int seqNo, int isDeleted)
    {
        try
        {
            var ptTag = _tenantDataContextNoTracking.PtTag.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId && x.SeqNo == seqNo);

            if (ptTag == null) return false;

            if (ptTag.IsDeleted == 0) return true;

            ptTag.IsDeleted = isDeleted;

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
