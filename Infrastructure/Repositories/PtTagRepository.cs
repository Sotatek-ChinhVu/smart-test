using Domain.Models.PtTag;
using Entity.Tenant;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

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
            .Select(x => new StickyNoteModel(x.HpId, x.PtId, x.SeqNo, x.Memo ?? string.Empty, x.StartDate, x.EndDate, x.IsDspUketuke, x.IsDspKarte, x.IsDspKaikei, x.IsDspRece, x.BackgroundColor ?? string.Empty, x.TagGrpCd, x.AlphablendVal, x.FontSize, x.IsDeleted, x.Width, x.Height, x.Left, x.Top))
            .ToList();
    }
    public bool UpdateIsDeleted(int hpId, int ptId, int seqNo, int isDeleted, int userId)
    {
        try
        {
            var ptTag = _tenantDataContextNoTracking.PtTag.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId && x.SeqNo == seqNo);

            if (ptTag == null) return false;

            if (ptTag.IsDeleted == 2) return false;

            ptTag.IsDeleted = isDeleted;

            ptTag.UpdateDate = DateTime.UtcNow;
            ptTag.UpdateId = userId;
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
    public bool SaveStickyNote(List<StickyNoteModel> stickyNoteModels, int userId)
    {
        var executionStrategy = _tenantDataContextTracking.Database.CreateExecutionStrategy();

        var result = executionStrategy.Execute(
                () =>
                {
                    // execute your logic here
                    using (var transaction = _tenantDataContextTracking.Database.BeginTransaction())
                    {
                        try
                        {
                            var ptTags = stickyNoteModels.Select(x => new PtTag()
                            {
                                HpId = x.HpId,
                                PtId = x.PtId,
                                SeqNo = x.SeqNo,
                                Memo = x.Memo,
                                StartDate = x.StartDate,
                                EndDate = x.EndDate,
                                IsDspUketuke = x.IsDspUketuke,
                                IsDspKarte = x.IsDspKarte,
                                IsDspKaikei = x.IsDspKaikei,
                                IsDspRece = x.IsDspRece,
                                BackgroundColor = x.BackgroundColor,
                                TagGrpCd = x.TagGrpCd,
                                AlphablendVal = x.AlphablendVal,
                                FontSize = x.FontSize,
                                IsDeleted = x.IsDeleted,
                                Width = x.Width,
                                Height = x.Height,
                                Left = x.Left,
                                Top = x.Top,
                            }).ToList();

                            var updateList = ptTags.Where(x => x.SeqNo != 0).ToList();
                            var addList = ptTags.Where(x => x.SeqNo == 0).ToList();

                            updateList.ForEach(ptTag =>
                            {
                                ptTag.UpdateDate = DateTime.UtcNow;
                                ptTag.UpdateId = userId;
                                ptTag.CreateDate = DateTime.SpecifyKind(ptTag.CreateDate, DateTimeKind.Utc);

                            });

                            addList.ForEach(ptTag =>
                            {
                                ptTag.CreateDate = DateTime.UtcNow;
                                ptTag.CreateId = userId;
                                ptTag.UpdateDate = DateTime.UtcNow;
                                ptTag.UpdateId = userId;
                            });

                            _tenantDataContextTracking.PtTag.UpdateRange(updateList);
                            _tenantDataContextTracking.PtTag.AddRange(addList);
                            _tenantDataContextTracking.SaveChanges();
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                });
        return result;
    }

    public StickyNoteModel GetStickyNoteModel(int hpId, long ptId, long seqNo)
    {
        var result = _tenantDataContextNoTracking.PtTag.Where(x => x.HpId == hpId && x.PtId == ptId && x.SeqNo == seqNo)
                                                .Select(x => new StickyNoteModel(x.HpId, x.PtId, x.SeqNo, x.Memo ?? string.Empty, x.StartDate, x.EndDate, x.IsDspUketuke, x.IsDspKarte, x.IsDspKaikei, x.IsDspRece, x.BackgroundColor ?? string.Empty, x.TagGrpCd, x.AlphablendVal, x.FontSize, x.IsDeleted, x.Width, x.Height, x.Left, x.Top))
                                                .FirstOrDefault();
        return result ?? new StickyNoteModel();
    }
}
