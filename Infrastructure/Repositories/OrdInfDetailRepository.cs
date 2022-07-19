using Domain.Models.OrdInfDetails;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class OrdInfDetailRepository : IOrdInfDetailRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public OrdInfDetailRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public void Create(OrdInfDetailModel ord)
        {
            throw new NotImplementedException();
        }

        public void Delete(int ordId, long raiinNo, long rpNo, long rpEdaNo, int rowNo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrdInfDetailModel> GetAll()
        {
            var result = _tenantDataContext.OdrInfDetails.AsQueryable().Select(o => new OrdInfDetailModel(
                o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.RowNo, o.PtId, o.SinDate, o.SinKouiKbn, o.ItemCd, o.ItemName, o.Suryo, o.UnitName, o.UnitSBT, o.TermVal, o.KohatuKbn, o.SyohoKbn, o.SyohoLimitKbn, o.DrugKbn, o.YohoKbn, o.Kokuji1, o.Kokiji2, o.IsNodspRece, o.IpnCd, o.IpnName, o.JissiKbn, o.JissiDate, o.JissiId, o.JissiMachine, o.ReqCd, o.Bunkatu, o.CmtName, o.CmtOpt, o.FontColor, o.CommentNewline));
            return result;
        }
        public IEnumerable<OrdInfDetailModel> GetList(long ptId, long raiinNo, int sinDate)
        {
            var result = _tenantDataContext.OdrInfDetails.Where(o => o.PtId == ptId && o.RaiinNo == raiinNo && o.SinDate == sinDate).Select(o => new OrdInfDetailModel(
                o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.RowNo, o.PtId, o.SinDate, o.SinKouiKbn, o.ItemCd, o.ItemName, o.Suryo, o.UnitName, o.UnitSBT, o.TermVal, o.KohatuKbn, o.SyohoKbn, o.SyohoLimitKbn, o.DrugKbn, o.YohoKbn, o.Kokuji1, o.Kokiji2, o.IsNodspRece, o.IpnCd, o.IpnName, o.JissiKbn, o.JissiDate, o.JissiId, o.JissiMachine, o.ReqCd, o.Bunkatu, o.CmtName, o.CmtOpt, o.FontColor, o.CommentNewline));
            return result;
        }

        public int MaxUserId()
        {
            return 100;
        }

        public OrdInfDetailModel Read(int ordId, long raiinNo, long rpNo, long rpEdaNo, int rowNo)
        {
            throw new NotImplementedException();
        }

        public void Update(OrdInfDetailModel ordDetail)
        {
            throw new NotImplementedException();
        }
    }
}
