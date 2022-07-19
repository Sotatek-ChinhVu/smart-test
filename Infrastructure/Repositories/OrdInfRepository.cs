using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class OrdInfRepository : IOrdInfRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public OrdInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public void Create(OrdInf ord)
        {
            throw new NotImplementedException();
        }

        public void Delete(int ordId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrdInf> GetAll()
        {
            var result = _tenantDataContext.OdrInfs.AsQueryable().Select(o => new OrdInf(
                o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.PtId, o.SinDate, o.HokenPid, o.OdrKouiKbn, o.RpName, o.InoutKbn, o.SikyuKbn, o.SyohoSbt, o.SanteiKbn, o.TosekiKbn, o.DaysCnt, o.SortNo, o.IsDeleted, o.Id, new List<OrdInfDetail>()));
            return result;
        }
        public IEnumerable<OrdInf> GetList(long ptId, long raiinNo, int sinDate)
        {
            var result = new List<OrdInf>();
            var allOdrInfDetails = _tenantDataContext.OdrInfDetails.Where(o => o.PtId == ptId && o.RaiinNo == raiinNo && o.SinDate == sinDate)?.ToList();
            var allOdrInf = _tenantDataContext.OdrInfs.Where(odr => odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate && odr.OdrKouiKbn != 10 && odr.IsDeleted == 0).Select(o => new OrdInf(
                o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.PtId, o.SinDate, o.HokenPid, o.OdrKouiKbn, o.RpName, o.InoutKbn, o.SikyuKbn, o.SyohoSbt, o.SanteiKbn, o.TosekiKbn, o.DaysCnt, o.SortNo, o.IsDeleted, o.Id, new List<OrdInfDetail>()
                  )).ToList();

            foreach (OrdInf rpOdrInf in allOdrInf)
            {
                // Find OdrInfDetail
                var odrInfDetails = allOdrInfDetails?.Where(detail => detail.RpNo == rpOdrInf.RpNo && detail.RpEdaNo == rpOdrInf.RpEdaNo)
                    .ToList();
                var ordInf = new OrdInf(rpOdrInf.HpId, rpOdrInf.RaiinNo, rpOdrInf.RpNo, rpOdrInf.RpEdaNo, rpOdrInf.PtId, rpOdrInf.SinDate, rpOdrInf.HokenPid, rpOdrInf.OdrKouiKbn, rpOdrInf.RpName, rpOdrInf.InoutKbn, rpOdrInf.SikyuKbn, rpOdrInf.SyohoSbt, rpOdrInf.SanteiKbn, rpOdrInf.TosekiKbn, rpOdrInf.DaysCnt, rpOdrInf.SortNo, rpOdrInf.IsDeleted, rpOdrInf.Id,
                  odrInfDetails == null ? new List<OrdInfDetail>() : odrInfDetails.Select(od => new OrdInfDetail(od.HpId, od.RaiinNo, od.RpNo, od.RpEdaNo, od.RowNo, od.PtId, od.SinDate, od.SinKouiKbn, od.ItemCd, od.ItemName, od.Suryo, od.UnitName, od.UnitSBT, od.TermVal, od.KohatuKbn, od.SyohoKbn, od.SyohoLimitKbn, od.DrugKbn, od.YohoKbn, od.Kokuji1, od.Kokiji2, od.IsNodspRece, od.IpnCd, od.IpnName, od.JissiKbn, od.JissiDate, od.JissiId, od.JissiMachine, od.ReqCd, od.Bunkatu, od.CmtName, od.CmtOpt, od.FontColor, od.CommentNewline
                  )).ToList()
                    );
                result.Add(ordInf);
            }
            return result;
        }

        public int MaxUserId()
        {
            return 100;
        }

        public OrdInf Read(int ordId)
        {
            throw new NotImplementedException();
        }

        public void Update(OrdInf ord)
        {
            throw new NotImplementedException();
        }
    }
}
