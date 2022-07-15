using Domain.CommonObject;
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

        public void Create(OrdInfDetailMst ord)
        {
            throw new NotImplementedException();
        }

        public void Delete(HpId ordId, RaiinNo raiinNo, RpNo rpNo, RpEdaNo rpEdaNo, RowNo rowNo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrdInfDetailMst> GetAll()
        {
            var result = _tenantDataContext.OdrInfDetails.AsQueryable().Select(o => new OrdInfDetailMst(
                o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.RowNo, o.PtId, o.SinDate, o.SinKouiKbn, o.ItemCd, o.ItemName, o.Suryo, o.UnitName, o.UnitSBT, o.TermVal, o.KohatuKbn, o.SyohoKbn, o.SyohoLimitKbn, o.DrugKbn, o.YohoKbn, o.Kokuji1, o.Kokiji2, o.IsNodspRece, o.IpnCd, o.IpnName, o.JissiKbn, o.JissiDate, o.JissiId, o.JissiMachine, o.ReqCd, o.Bunkatu, o.CmtName, o.CmtOpt, o.FontColor, o.CommentNewline));
            return result;
        }
        public IEnumerable<OrdInfDetailMst> GetList(PtId ptId, RaiinNo raiinNo, SinDate sinDate)
        {
            var result = _tenantDataContext.OdrInfDetails.Where(o => o.PtId == ptId.Value && o.RaiinNo == raiinNo.Value && o.SinDate == sinDate.Value).Select(o => new OrdInfDetailMst(
                o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.RowNo, o.PtId, o.SinDate, o.SinKouiKbn, o.ItemCd, o.ItemName, o.Suryo, o.UnitName, o.UnitSBT, o.TermVal, o.KohatuKbn, o.SyohoKbn, o.SyohoLimitKbn, o.DrugKbn, o.YohoKbn, o.Kokuji1, o.Kokiji2, o.IsNodspRece, o.IpnCd, o.IpnName, o.JissiKbn, o.JissiDate, o.JissiId, o.JissiMachine, o.ReqCd, o.Bunkatu, o.CmtName, o.CmtOpt, o.FontColor, o.CommentNewline));
            return result;
        }

        public int MaxUserId()
        {
            return 100;
        }

        public OrdInfDetailMst Read(HpId ordId, RaiinNo raiinNo, RpNo rpNo, RpEdaNo rpEdaNo, RowNo rowNo)
        {
            throw new NotImplementedException();
        }

        public void Update(OrdInfDetailMst ordDetail)
        {
            throw new NotImplementedException();
        }
    }
}
