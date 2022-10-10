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
    private readonly TenantNoTrackingDataContext _tenantDataContext;

    public PtTagRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<StickyNoteModel> SearchByPtId(int hpId, int ptId, int isDeleted)
    {

        return _tenantDataContext.PtTag.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == isDeleted)
            .Select(x => new StickyNoteModel(x.HpId, x.PtId, x.SeqNo, x.Memo, x.MemoData ?? new byte[0], x.StartDate, x.EndDate, x.IsDspUketuke, x.IsDspKarte, x.IsDspKaikei, x.IsDspRece, x.BackgroundColor, x.TagGrpCd, x.AlphablendVal, x.FontSize, x.IsDeleted, x.Width, x.Height, x.Left, x.Top))
            .ToList();
    }
}
