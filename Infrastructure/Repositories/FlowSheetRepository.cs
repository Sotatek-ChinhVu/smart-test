using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Repositories
{
    public class FlowSheetRepository : IFlowSheetRepository
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        private readonly TenantDataContext _tenantTrackingDataContext;
        private readonly int cmtKbn = 9;

        public FlowSheetRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo, int startIndex, int count, string sort, ref long totalCount)
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
                            raiinInf.RaiinNo,
                            raiinInf.SyosaisinKbn,
                            raiinInf.Status,
                            raiinInf.SinDate,
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
                            //.AsEnumerable<RaiinListInfModel>()
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
                    (r.RaiinNo == raiinNo && r.SinDate == sinDate && r.Status < 3)
                   )
            ).AsEnumerable<FlowSheetModel>();

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

            var groupNextOdr = from rsvkrtOdrInf in rsvkrtOdrInfs.AsEnumerable<RsvkrtOdrInf>()
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
                                                    select new RaiinListInfModel(nextOdr.RsvkrtNo, raiinListInf.GrpId, raiinListInf.KbnCd, raiinListInf.RaiinListKbn, raiinListMst.KbnName ?? string.Empty, raiinListMst.ColorCd ?? string.Empty)
                                            )
                                   //.AsEnumerable<RaiinListInfModel>()
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
                        false
                    ));

            var todayNextOdrs = todayOdr.Union(nextOdrs).ToList();
            totalCount = todayNextOdrs.Count();

            FlowSheetModel? sinDateCurrent = null;
            if (!todayNextOdrs.Any(r => r.SinDate == sinDate && r.RaiinNo == raiinNo))
            {
                sinDateCurrent = new FlowSheetModel(
                        0,
                        0,
                        string.Empty,
                        0,
                        2,
                        string.Empty,
                        0,
                        false,
                        false,
                        new List<RaiinListInfModel>(),
                        0,
                        false
                    );
                totalCount = totalCount + 1;
            }

            if (string.IsNullOrEmpty(sort))
                result = todayNextOdrs.OrderByDescending(o => o.SinDate).Skip(startIndex).Take(count).ToList();
            else
            {
                SortAll(sort, todayNextOdrs);
                result = todayNextOdrs.Skip(startIndex).Take(count).ToList();
            }

            if (sinDateCurrent != null && startIndex == 0)
            {
                result.Insert(0, sinDateCurrent);
            }

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

        public void UpsertTag(List<FlowSheetModel> inputDatas)
        {
            foreach (var inputData in inputDatas)
            {
                var raiinListTag = _tenantTrackingDataContext.RaiinListTags
                           .OrderByDescending(p => p.UpdateDate)
                           .FirstOrDefault(p => p.RaiinNo == inputData.RaiinNo);
                if (raiinListTag is null)
                {
                    _tenantTrackingDataContext.RaiinListTags.Add(new RaiinListTag
                    {
                        HpId = TempIdentity.HpId,
                        PtId = inputData.PtId,
                        SinDate = inputData.SinDate,
                        RaiinNo = inputData.RaiinNo,
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
        public void UpsertCmt(List<FlowSheetModel> inputDatas)
        {
            foreach (var inputData in inputDatas)
            {
                var raiinListCmt = _tenantTrackingDataContext.RaiinListCmts
                               .OrderByDescending(p => p.UpdateDate)
                               .FirstOrDefault(p => p.RaiinNo == inputData.RaiinNo);

                if (raiinListCmt is null)
                {
                    _tenantTrackingDataContext.RaiinListCmts.Add(new RaiinListCmt
                    {
                        HpId = TempIdentity.HpId,
                        PtId = inputData.PtId,
                        SinDate = inputData.SinDate,
                        RaiinNo = inputData.RaiinNo,
                        CmtKbn = cmtKbn,
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
            }
            _tenantTrackingDataContext.SaveChanges();
        }

        private void SortAll(string sort, List<FlowSheetModel> todayNextOdrs)
        {
            try
            {
                var childrenOfSort = sort.Trim().Split(",");
                var order = todayNextOdrs.OrderBy(o => o.PtId);
                foreach (var item in childrenOfSort)
                {
                    var elementDynamics = item.Trim().Split(" ");
                    var checkGroupId = int.TryParse(elementDynamics[0], out int groupId);

                    if (!checkGroupId)
                    {
                        SortStaticColumn(elementDynamics.FirstOrDefault() ?? string.Empty, elementDynamics?.Count() > 1 ? elementDynamics.LastOrDefault() ?? string.Empty : string.Empty, order);
                    }
                    else
                    {
                        if (elementDynamics.Length > 1)
                        {
                            if (childrenOfSort[1].ToLower() == "desc")
                            {
                                order = order.ThenByDescending(o => o.RaiinListInfs.FirstOrDefault(r => r.GrpId == groupId)?.KbnName);
                            }
                            else
                            {
                                order = order.ThenByDescending(o => o.RaiinListInfs.FirstOrDefault(r => r.GrpId == groupId)?.KbnName);
                            }
                        }
                        else
                        {
                            order = order.ThenByDescending(o => o.RaiinListInfs.FirstOrDefault(r => r.GrpId == groupId)?.KbnName);
                        }
                    }
                }
                todayNextOdrs = order.ToList();
            }
            catch
            {
                todayNextOdrs = todayNextOdrs.OrderByDescending(o => o.SinDate).ToList();
            }
        }

        private void SortStaticColumn(string fieldName, string sortType, IOrderedEnumerable<FlowSheetModel> order)
        {
            if (fieldName.ToLower().Equals("sindate"))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.SinDate);

                }
                else
                {
                    order = order.ThenByDescending(o => o.SinDate);
                }
            }
            if (fieldName.ToLower().Equals("tagno"))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.TagNo);

                }
                else
                {
                    order = order.ThenByDescending(o => o.TagNo);
                }
            }
            if (fieldName.ToLower().Equals("fulllineofkarte"))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.FullLineOfKarte);

                }
                else
                {
                    order = order.ThenByDescending(o => o.FullLineOfKarte);
                }
            }
            if (fieldName.ToLower().Equals("syosaisinkbn"))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.SyosaisinKbn);

                }
                else
                {
                    order = order.ThenByDescending(o => o.SyosaisinKbn);
                }
            }
            if (fieldName.ToLower().Equals("comment"))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.Comment);

                }
                else
                {
                    order = order.ThenByDescending(o => o.Comment);
                }
            }
        }
    }
}