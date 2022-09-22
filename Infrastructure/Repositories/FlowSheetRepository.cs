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
            var raiinInfsQueryable = _tenantNoTrackingDataContext.RaiinInfs
                .Where(r => r.HpId == hpId && r.PtId == ptId && r.IsDeleted == 0 && r.Status >= 3)
                .Skip(startIndex)
                .Take(count);
            var karteInfsQueryable = _tenantNoTrackingDataContext.KarteInfs.Where(k => k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0);
            var tagsQueryable = _tenantNoTrackingDataContext.RaiinListTags.Where(tag => tag.HpId == hpId && tag.PtId == ptId);
            var commentsQueryable = _tenantNoTrackingDataContext.RaiinListCmts.Where(comment => comment.HpId == hpId && comment.PtId == ptId);
            var query = from raiinInf in raiinInfsQueryable
                        join karteInf in karteInfsQueryable on raiinInf.RaiinNo equals karteInf.RaiinNo into gj from karteInf in gj.DefaultIfEmpty()
                        join tagInf in tagsQueryable on raiinInf.RaiinNo equals tagInf.RaiinNo into gjTag from tagInf in gjTag.DefaultIfEmpty()
                        join commentInf in commentsQueryable on raiinInf.RaiinNo equals commentInf.RaiinNo into gjComment from commentInf in gjComment.DefaultIfEmpty()
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
                                            select new RaiinListInfModel(raiinInf.RaiinNo, raiinListInf.GrpId, raiinListInf.KbnCd, raiinListInf.RaiinListKbn, raiinListMst.KbnName, raiinListMst.ColorCd ?? string.Empty)
                                            )
                                            .AsEnumerable()
                        };

            var rawDataList = query.ToList();

            return rawDataList.Select(r => 
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
            ).ToList();
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
                data => new RaiinListMstModel(data.Mst.GrpId, data.Mst.GrpName, data.Mst.SortNo, data.Detail.Select(d => new RaiinListDetailModel(d.GrpId, d.KbnCd, d.SortNo, d.KbnName, d.ColorCd ?? String.Empty, d.IsDeleted)).ToList()));
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
