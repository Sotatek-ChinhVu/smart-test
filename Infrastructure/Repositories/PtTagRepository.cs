using Domain.Models.PtTag;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtTagRepository : RepositoryBase, IPtTagRepository
{
    public PtTagRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<StickyNoteModel> SearchByPtId(int hpId, int ptId)
    {
        return NoTrackingDataContext.PtTag.Where(x => x.HpId == hpId && x.PtId == ptId && (x.IsDeleted == 0 || x.IsDeleted == 1))
            .Select(x => new StickyNoteModel(x.HpId, x.PtId, x.SeqNo, x.Memo ?? string.Empty, x.StartDate, x.EndDate, x.IsDspUketuke, x.IsDspKarte, x.IsDspKaikei, x.IsDspRece, x.BackgroundColor ?? string.Empty, x.TagGrpCd, x.AlphablendVal, x.FontSize, x.IsDeleted, x.Width, x.Height, x.Left, x.Top))
            .ToList();
    }
    public bool UpdateIsDeleted(int hpId, int ptId, int seqNo, int isDeleted, int userId)
    {
        try
        {
            var ptTag = NoTrackingDataContext.PtTag.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId && x.SeqNo == seqNo);

            if (ptTag == null) return false;

            if (ptTag.IsDeleted == 2) return false;

            ptTag.IsDeleted = isDeleted;

            ptTag.UpdateDate = DateTime.UtcNow;
            ptTag.UpdateId = userId;
            ptTag.CreateDate = DateTime.SpecifyKind(ptTag.CreateDate, DateTimeKind.Utc);

            TrackingDataContext.PtTag.Update(ptTag);
            TrackingDataContext.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public bool SaveStickyNote(List<StickyNoteModel> stickyNoteModels, int userId)
    {
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();

        var result = executionStrategy.Execute(
                () =>
                {
                    // execute your logic here
                    using (var transaction = TrackingDataContext.Database.BeginTransaction())
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

                            TrackingDataContext.PtTag.UpdateRange(updateList);
                            TrackingDataContext.PtTag.AddRange(addList);
                            TrackingDataContext.SaveChanges();
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
        var result = NoTrackingDataContext.PtTag.Where(x => x.HpId == hpId && x.PtId == ptId && x.SeqNo == seqNo)
                                                .Select(x => new StickyNoteModel(x.HpId, x.PtId, x.SeqNo, x.Memo ?? string.Empty, x.StartDate, x.EndDate, x.IsDspUketuke, x.IsDspKarte, x.IsDspKaikei, x.IsDspRece, x.BackgroundColor ?? string.Empty, x.TagGrpCd, x.AlphablendVal, x.FontSize, x.IsDeleted, x.Width, x.Height, x.Left, x.Top))
                                                .FirstOrDefault();
        return result ?? new StickyNoteModel();
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
