using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class FlowSheetRepository : IFlowSheetRepository
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        private readonly TenantDataContext _tenantTrackingDataContext;
        public FlowSheetRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo, int startIndex, int count)
        {
            List<FlowSheetModel> result;

            var raiinInfsQueryable = _tenantNoTrackingDataContext.RaiinInfs.Where(r => r.HpId == hpId && r.PtId == ptId && r.IsDeleted == 0);
            var karteInfsQueryable = _tenantNoTrackingDataContext.KarteInfs.Where(k => k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0);
            var tagsQueryable = _tenantNoTrackingDataContext.RaiinListTags.Where(tag => tag.HpId == hpId && tag.PtId == ptId);
            var commentsQueryable = _tenantNoTrackingDataContext.RaiinListCmts.Where(comment => comment.HpId == hpId && comment.PtId == ptId);
            var query = from raiinInf in raiinInfsQueryable
                        join karteInf in karteInfsQueryable on raiinInf.RaiinNo equals karteInf.RaiinNo into gj
                        from karteInf in gj.DefaultIfEmpty()
                        join tagInf in tagsQueryable on raiinInf.RaiinNo equals tagInf.RaiinNo into gjTag
                        from tagInf in gjTag.DefaultIfEmpty()
                        join commentInf in commentsQueryable on raiinInf.RaiinNo equals commentInf.RaiinNo into gjComment
                        from commentInf in gjComment.DefaultIfEmpty()
                        select new
                        {
                            RaiinNo = raiinInf.RaiinNo,
                            SyosaisinKbn = raiinInf.SyosaisinKbn,
                            Status = raiinInf.Status,
                            SinDate = raiinInf.SinDate,
                            Text = karteInf == null ? string.Empty : karteInf.Text,
                            TagNo = tagInf == null ? 0 : tagInf.TagNo,
                            TagSeqNo = tagInf == null ? 0 : tagInf.SeqNo,
                            CommentContent = commentInf == null ? string.Empty : commentInf.Text,
                            CommentSeqNo = commentInf == null ? 0 : commentInf.SeqNo,
                            CommentKbn = commentInf == null ? 9 : commentInf.CmtKbn,
                            RaiinListInfs = (from raiinListInf in _tenantNoTrackingDataContext.RaiinListInfs.Where(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == raiinInf.RaiinNo)
                                            join raiinListMst in _tenantNoTrackingDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None)
                                            on raiinListInf.KbnCd equals raiinListMst.KbnCd
                                            select new RaiinListInfModel(raiinInf.RaiinNo, raiinListInf.GrpId, raiinListInf.KbnCd, raiinListInf.RaiinListKbn, raiinListMst.KbnName ?? string.Empty, raiinListMst.ColorCd ?? string.Empty)
                                            )
                                            .AsEnumerable()
                        };


            var todayOdr = query.Select(r =>
                new FlowSheetModel(
                    r.SinDate,
                    r.TagNo,
                    r.Text,
                    r.RaiinNo,
                    r.SyosaisinKbn,
                    r.CommentContent,
                    r.Status,
                    false,
                    r.RaiinNo == raiinNo,
                    r.RaiinListInfs.ToList(),
                    ptId,
                    r.CommentKbn,
                    r.CommentSeqNo,
                    r.TagSeqNo)
            ).AsEnumerable();

            // Add NextOrder Information
            // Get next order information
            var rsvkrtOdrInfs = _tenantNoTrackingDataContext.RsvkrtOdrInfs.Where(r => r.HpId == hpId
                                                                                        && r.PtId == ptId
                                                                                        && r.IsDeleted == DeleteTypes.None);
            var rsvkrtMsts = _tenantNoTrackingDataContext.RsvkrtMsts.Where(r => r.HpId == hpId
                                                                                        && r.PtId == ptId
                                                                                        && r.IsDeleted == DeleteTypes.None
                                                                                        && r.RsvkrtKbn == 0);
            var nextOdrKarteInfs = _tenantNoTrackingDataContext.RsvkrtKarteInfs.Where(karte => karte.HpId == hpId
                                   && karte.PtId == ptId
                                   && karte.IsDeleted == 0
                                   && !string.IsNullOrEmpty(karte.Text.Trim()))
                                   .OrderBy(karte => karte.RsvDate)
                                   .ThenBy(karte => karte.KarteKbn);

            var groupNextOdr = from rsvkrtOdrInf in rsvkrtOdrInfs.AsEnumerable()
                               join rsvkrtMst in rsvkrtMsts on new { rsvkrtOdrInf.HpId, rsvkrtOdrInf.PtId, rsvkrtOdrInf.RsvkrtNo }
                                                equals new { rsvkrtMst.HpId, rsvkrtMst.PtId, rsvkrtMst.RsvkrtNo }
                               join karte in nextOdrKarteInfs on new { rsvkrtOdrInf.HpId, rsvkrtOdrInf.PtId, rsvkrtOdrInf.RsvkrtNo }
                                                equals new { karte.HpId, karte.PtId, karte.RsvkrtNo } into odrKarteLeft
                               group rsvkrtOdrInf by new { rsvkrtOdrInf.HpId, rsvkrtOdrInf.PtId, rsvkrtOdrInf.RsvDate, rsvkrtOdrInf.RsvkrtNo } into g
                               select new
                               {
                                   g.Key.HpId,
                                   g.Key.PtId,
                                   g.Key.RsvDate,
                                   g.Key.RsvkrtNo
                               };

            var queryNextOdr = from nextOdr in groupNextOdr
                               join karte in nextOdrKarteInfs on new { nextOdr.HpId, nextOdr.PtId, nextOdr.RsvkrtNo }
                                                equals new { karte.HpId, karte.PtId, karte.RsvkrtNo } into odrKarteLeft
                               join tagInf in tagsQueryable on nextOdr.RsvkrtNo equals tagInf.RaiinNo into gjTag
                               from tagInf in gjTag.DefaultIfEmpty()
                               select new
                               {
                                   NextOdr = nextOdr,
                                   TagInf = tagInf,
                                   Karte = odrKarteLeft.FirstOrDefault(),
                                   RaiinListInfs = (from raiinListInf in _tenantNoTrackingDataContext.RaiinListInfs.Where(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == nextOdr.RsvkrtNo)
                                                    join raiinListMst in _tenantNoTrackingDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None)
                                                    on raiinListInf.KbnCd equals raiinListMst.KbnCd
                                                    select new RaiinListInfModel(nextOdr.RsvkrtNo, raiinListInf.GrpId, raiinListInf.KbnCd, raiinListInf.RaiinListKbn, raiinListMst.KbnName, raiinListMst.ColorCd ?? string.Empty)
                                            )
                                            .AsEnumerable()
                               };
            var nextOdrs = queryNextOdr.Select(
                    data => new FlowSheetModel(
                        data.NextOdr?.RsvDate ?? 0,
                        data.TagInf?.TagNo ?? 0,
                        data.Karte?.Text ?? string.Empty,
                        data.NextOdr?.RsvkrtNo ?? 0,
                        -1,
                        string.Empty,
                        0,
                        true,
                        false,
                        data.RaiinListInfs.ToList(),
                        data.NextOdr?.PtId ?? 0,
                        0,
                        0,
                        data.TagInf?.SeqNo ?? 0
                    ));

            result = todayOdr.Union(nextOdrs).OrderByDescending(o => o.SinDate).Skip(startIndex).Take(count).ToList();

            return result;
        }

        public List<RaiinListMstModel> GetRaiinListMsts(int hpId)
        {
            var raiinListMst = _tenantNoTrackingDataContext.RaiinListMsts.Where(m => m.HpId == hpId && m.IsDeleted == DeleteTypes.None).ToList();
            var raiinListDetail = _tenantNoTrackingDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None).ToList();
            var query = from mst in raiinListMst
                        select new
                        {
                            Mst = mst,
                            Detail = raiinListDetail.Where(c => c.HpId == mst.HpId && c.GrpId == mst.GrpId).ToList()
                        };
            var output = query.Select(
                data => new RaiinListMstModel(data.Mst.GrpId, data.Mst.GrpName ?? string.Empty, data.Mst.SortNo, data.Detail.Select(d => new RaiinListDetailModel(d.GrpId, d.KbnCd, d.SortNo, d.KbnName ?? string.Empty, d.ColorCd ?? String.Empty, d.IsDeleted)).ToList()));
            return output.ToList();
        }

        public List<HolidayModel> GetHolidayMst(int hpId, int holidayFrom, int holidayTo)
        {
            var holidayCollection = _tenantNoTrackingDataContext.HolidayMsts.Where(h => h.HpId == hpId && h.IsDeleted == DeleteTypes.None && holidayFrom <= h.SinDate && h.SinDate <= holidayTo);
            return holidayCollection.Select(h => new HolidayModel(h.SinDate, h.HolidayKbn, h.KyusinKbn, h.HolidayName)).ToList();
        }

        public void Upsert(List<FlowSheetModel> inputDatas)
        {
            foreach (var inputData in inputDatas)
            {
                var raiinListCmt = _tenantTrackingDataContext.RaiinListCmts
                            .OrderByDescending(p => p.UpdateDate)
                            .FirstOrDefault(p => p.RaiinNo == inputData.RaiinNo && p.CmtKbn == inputData.CmtKbn);

                if (raiinListCmt is null)
                {
                    _tenantTrackingDataContext.RaiinListCmts.Add(new RaiinListCmt
                    {
                        HpId = 1,
                        PtId = inputData.PtId,
                        SinDate = inputData.SinDate,
                        RaiinNo = inputData.RaiinNo,
                        CmtKbn = inputData.CmtKbn,
                        SeqNo = inputData.RainListCmtSeqNo,
                        Text = inputData.Comment,
                        CreateDate = DateTime.UtcNow,
                        CreateId = TempIdentity.UserId,
                        CreateMachine = TempIdentity.ComputerName
                    });
                }
                else
                {
                    raiinListCmt.Text = inputData.Comment;
                    raiinListCmt.UpdateDate = DateTime.UtcNow;
                    raiinListCmt.UpdateId = TempIdentity.UserId;
                    raiinListCmt.UpdateMachine = TempIdentity.ComputerName;
                }

                var raiinListTag = _tenantTrackingDataContext.RaiinListTags
                           .OrderByDescending(p => p.UpdateDate)
                           .FirstOrDefault(p => p.RaiinNo == inputData.RaiinNo);

                if (raiinListTag is null)
                {
                    _tenantTrackingDataContext.RaiinListTags.Add(new RaiinListTag
                    {
                        HpId = 1,
                        PtId = inputData.PtId,
                        SinDate = inputData.SinDate,
                        RaiinNo = inputData.RaiinNo,
                        SeqNo = inputData.RainListTagSeqNo,
                        TagNo = inputData.TagNo,
                        CreateDate = DateTime.UtcNow,
                        CreateId = TempIdentity.UserId,
                        CreateMachine = TempIdentity.ComputerName
                    });
                }
                else
                {
                    raiinListTag.TagNo = inputData.TagNo;
                    raiinListTag.UpdateDate = DateTime.UtcNow;
                    raiinListTag.UpdateId = TempIdentity.UserId;
                    raiinListTag.UpdateMachine = TempIdentity.ComputerName;
                }
            }
            _tenantTrackingDataContext.SaveChanges();
        }
    }
}
