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
        private readonly TenantDataContext _tenantNoTrackingDataContext;
        private readonly TenantDataContext _tenantTrackingDataContext;
        public FlowSheetRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo)
        {
            var raiinInfs = _tenantNoTrackingDataContext.RaiinInfs.Where(r => r.HpId == hpId && r.PtId == ptId && r.IsDeleted == 0 && r.Status >= 3);
            var karteInfs = _tenantNoTrackingDataContext.KarteInfs.Where(k => k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0);
            var query = from raiinInf in raiinInfs
                        join karteInf in karteInfs on raiinInf.RaiinNo equals karteInf.RaiinNo into gj
                        from karteInf in gj.DefaultIfEmpty()
                        select new
                        {
                            raiinInf,
                            karteInf
                        };

            var dataList = query.ToList();

            var tags = _tenantNoTrackingDataContext.RaiinListTags.Where(tag => tag.HpId == hpId && tag.PtId == ptId).ToList();
            var comments = _tenantNoTrackingDataContext.RaiinListCmts.Where(comment => comment.HpId == hpId && comment.PtId == ptId).ToList();
            var raiinListInfs =
                from raiinListInf in _tenantNoTrackingDataContext.RaiinListInfs.Where(raiinInf => raiinInf.HpId == hpId && raiinInf.PtId == ptId)
                join raiinListMst in _tenantNoTrackingDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None)
                on raiinListInf.KbnCd equals raiinListMst.KbnCd
                select new
                {
                    raiinListInf.RaiinNo,
                    raiinListInf.GrpId,
                    raiinListInf.KbnCd,
                    raiinListInf.RaiinListKbn,
                    raiinListMst.KbnName,
                    raiinListMst.ColorCd
                };

            List<FlowSheetModel> result = new();
            foreach (var data in dataList)
            {
                var raiinInf = data.raiinInf;
                var text = data.karteInf == null ? string.Empty : data.karteInf.Text;

                int tag = 0;
                int raiinListTagSeqNo = 0;
                if (tags.Any(c => c.RaiinNo == raiinInf.RaiinNo))
                {
                    var tagObj = tags.First(c => c.RaiinNo == raiinInf.RaiinNo);
                    tag = tagObj?.TagNo ?? 0;
                    raiinListTagSeqNo = tagObj?.SeqNo ?? 0;
                }

                string comment = string.Empty;
                long raiinListCmtSeqNo = 0;
                int cmtKbn = 0;
                if (comments.Any(c => c.RaiinNo == raiinInf.RaiinNo))
                {
                    var commentInf = comments.First(c => c.RaiinNo == raiinInf.RaiinNo);
                    comment = commentInf.Text ?? String.Empty;
                    raiinListCmtSeqNo = commentInf == null ? 0 : commentInf.SeqNo;
                    cmtKbn = commentInf?.CmtKbn ?? 0;
                }

                var raiinListInfoModelList = raiinListInfs
                    .Where(r => r.RaiinNo == raiinInf.RaiinNo)
                    .Select(r => new RaiinListInfModel(r.RaiinNo, r.GrpId, r.KbnCd, r.RaiinListKbn, r.KbnName, r.ColorCd ?? string.Empty))
                    .ToList();

                bool isContainsFile = raiinListInfoModelList.Any(r => r.RaiinListKbn == 4);

                result.Add(new FlowSheetModel(raiinInf.PtId, raiinInf.SinDate, tag, text, raiinInf.RaiinNo, raiinInf.SyosaisinKbn, comment, raiinInf.Status, isContainsFile, false, raiinInf.RaiinNo == raiinNo, raiinListTagSeqNo, raiinListCmtSeqNo, cmtKbn, raiinListInfoModelList));
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
                        SeqNo = inputData.RaiinListCmtSeqNo,
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
                        SeqNo = inputData.RaiinListTagSeqNo,
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
