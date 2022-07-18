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

        public void Create(OrdInfMst ord)
        {
            throw new NotImplementedException();
        }

        public void Delete(int ordId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrdInfMst> GetAll()
        {
            var result = _tenantDataContext.OdrInfs.AsQueryable().Select(o => new OrdInfMst(
                o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.PtId, o.SinDate, o.HokenPid, o.OdrKouiKbn, o.RpName, o.InoutKbn, o.SikyuKbn, o.SyohoSbt, o.SanteiKbn, o.TosekiKbn, o.DaysCnt, o.SortNo, o.IsDeleted, o.CreateDate, o.CreateId, o.CreateMachine, o.UpdateDate, o.UpdateId, o.UpdateMachine, o.Id));
            return result;
        }
        public IEnumerable<OrdInfMst> GetList(long ptId, long raiinNo, int sinDate)
        {
            var result = _tenantDataContext.OdrInfs.Where(odr => odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate && odr.OdrKouiKbn != 10 && odr.IsDeleted == 0).Select(o => new OrdInfMst(
                o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.PtId, o.SinDate, o.HokenPid, o.OdrKouiKbn, o.RpName, o.InoutKbn, o.SikyuKbn, o.SyohoSbt, o.SanteiKbn, o.TosekiKbn, o.DaysCnt, o.SortNo, o.IsDeleted, o.CreateDate, o.CreateId, o.CreateMachine, o.UpdateDate, o.UpdateId, o.UpdateMachine, o.Id));
            return result;
        }

        public int MaxUserId()
        {
            return 100;
        }

        public OrdInfMst Read(int ordId)
        {
            throw new NotImplementedException();
        }

        public void Update(OrdInfMst ord)
        {
            throw new NotImplementedException();
        }
    }
}
