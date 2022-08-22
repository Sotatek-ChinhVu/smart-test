using Domain.Constant;
using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extendsions;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Collections.ObjectModel;

namespace Infrastructure.Repositories
{
    public class FlowSheetRepository : IFlowSheetRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public FlowSheetRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo)
        {
            var raiinInfs = _tenantDataContext.RaiinInfs.Where(r => r.HpId == hpId && r.PtId == ptId && r.IsDeleted == 0 && r.Status >= 3);
            var karteInfs = _tenantDataContext.KarteInfs.Where(k => k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0);
            var query = from raiinInf in raiinInfs
                        join karteInf in karteInfs on raiinInf.RaiinNo equals karteInf.RaiinNo into gj
                        from karteInf in gj.DefaultIfEmpty()
                        select new
                        {
                            raiinInf,
                            karteInf
                        };

            var dataList = query.ToList();

            var tags = _tenantDataContext.RaiinListTags.Where(tag => tag.HpId == hpId && tag.PtId == ptId).ToList();
            var comments = _tenantDataContext.RaiinListCmts.Where(comment => comment.HpId == hpId && comment.PtId == ptId).ToList();
            var raiinListInfs =
                from raiinListInf in _tenantDataContext.RaiinListInfs.Where(raiinInf => raiinInf.HpId == hpId && raiinInf.PtId == ptId)
                join raiinListMst in _tenantDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None)
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
                if (tags.Any(c => c.RaiinNo == raiinInf.RaiinNo))
                {
                    tag = tags.First(c => c.RaiinNo == raiinInf.RaiinNo).TagNo;
                }

                string comment = string.Empty;
                if (comments.Any(c => c.RaiinNo == raiinInf.RaiinNo))
                {
                    var commentInf = comments.First(c => c.RaiinNo == raiinInf.RaiinNo);
                    comment = commentInf.Text ?? String.Empty;
                }

                var raiinListInfoModelList = raiinListInfs
                    .Where(r => r.RaiinNo == raiinInf.RaiinNo)
                    .Select(r => new RaiinListInfModel(r.RaiinNo, r.GrpId, r.KbnCd, r.RaiinListKbn, r.KbnName, r.ColorCd ?? string.Empty))
                    .ToList();

                bool isContainsFile = raiinListInfoModelList.Any(r => r.RaiinListKbn == 4);

                result.Add(new FlowSheetModel(raiinInf.SinDate, text, raiinInf.RaiinNo, raiinInf.SyosaisinKbn, raiinInf.Status, isContainsFile, tag, comment, raiinListInfoModelList, false, raiinInf.RaiinNo == raiinNo));
            }
            return result;
        }

        public List<RaiinListMstModel> GetRaiinListMsts(int hpId)
        {
            var raiinListMst = _tenantDataContext.RaiinListMsts.Where(m => m.HpId == hpId && m.IsDeleted == DeleteTypes.None).ToList();
            var raiinListDetail = _tenantDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None).ToList();
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
            var holidayCollection = _tenantDataContext.HolidayMsts.Where(h => h.HpId == hpId && h.IsDeleted == DeleteTypes.None && holidayFrom <= h.SinDate && h.SinDate <= holidayTo);
            return holidayCollection.Select(h => new HolidayModel(h.SinDate, h.HolidayKbn, h.KyusinKbn, h.HolidayName)).ToList();
        }
    }
}
