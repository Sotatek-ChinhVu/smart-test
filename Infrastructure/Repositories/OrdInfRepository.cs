using Domain.Models.User;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.OrdInfs;
using Domain.CommonObject;

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

        public void Delete(OrderId ordId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrdInfMst> GetAll()
        {
            var result = _tenantDataContext.OdrInfs.AsQueryable().Select(o => new OrdInfMst(
                o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.PtId, o.SinDate, o.HokenPid, o.OdrKouiKbn, o.RpName, o.InoutKbn, o.SikyuKbn, o.SyohoSbt, o.SanteiKbn, o.TosekiKbn, o.DaysCnt, o.SortNo, o.IsDeleted, o.CreateDate, o.CreateId, o.CreateMachine, o.UpdateDate, o.UpdateId, o.UpdateMachine, o.Id));
            return result;
        }
        public IEnumerable<OrdInfMst> GetList(PtId ptId, RaiinNo raiinNo, SinDate sinDate)
        {
            var result = _tenantDataContext.OdrInfs.Where(odr => odr.PtId == ptId.Value && odr.RaiinNo == raiinNo.Value && odr.SinDate == sinDate.Value && odr.OdrKouiKbn != 10 && odr.IsDeleted == 0).Select(o => new OrdInfMst(
                o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.PtId, o.SinDate, o.HokenPid, o.OdrKouiKbn, o.RpName, o.InoutKbn, o.SikyuKbn, o.SyohoSbt, o.SanteiKbn, o.TosekiKbn, o.DaysCnt, o.SortNo, o.IsDeleted, o.CreateDate, o.CreateId, o.CreateMachine, o.UpdateDate, o.UpdateId, o.UpdateMachine, o.Id));
            return result;
        }

        public int MaxUserId()
        {
            return 100;
        }

        public OrdInfMst Read(OrderId ordId)
        {
            throw new NotImplementedException();
        }

        public void Update(OrdInfMst ord)
        {
            throw new NotImplementedException();
        }
    }
}
