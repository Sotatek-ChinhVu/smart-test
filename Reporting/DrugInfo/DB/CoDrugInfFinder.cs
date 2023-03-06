using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using PostgreDataContext;
using Reporting.DrugInfo.Model;

namespace Reporting.DrugInfo.DB
{
    public class CoDrugInfFinder
    {
        private readonly int hpId = Session.HospitalID;

        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        public CoDrugInfFinder(TenantNoTrackingDataContext tenantNoTrackingDataContext)
        {
            _tenantNoTrackingDataContext = tenantNoTrackingDataContext;
        }
        public PathConf GetPathConf(int grpCode)
        {
            var pathConfs = _tenantNoTrackingDataContext.PathConfs.FirstOrDefault(p => p.GrpCd == grpCode);
            return pathConfs;
        }

        public DrugInfoModel GetBasicInfo(long ptId, int orderDate = 0)
        {
            DrugInfoModel info = new DrugInfoModel();
            info.OrderDate = orderDate == 0 ? CIUtil.DateTimeToInt(DateTime.Now) : orderDate;

            var hpInfo = _tenantNoTrackingDataContext.HpInfs.Where(p => p.HpId == hpId && p.StartDate <= info.OrderDate).OrderByDescending(p => p.StartDate).FirstOrDefault();
            if (hpInfo != null)
            {
                info.HpName = hpInfo.HpName ?? string.Empty;
                info.Address1 = hpInfo.Address1 ?? string.Empty;
                info.Address2 = hpInfo.Address2 ?? string.Empty;
                info.Phone = hpInfo.Tel ?? string.Empty;
            }

            var ptInfo = _tenantNoTrackingDataContext.PtInfs.FirstOrDefault(pt => pt.HpId == hpId && pt.PtId == ptId);
            if (ptInfo != null)
            {
                info.PtNo = ptInfo.PtNum;
                info.PtName = ptInfo.Name;
                info.Sex = ptInfo.Sex == 1 ? "M" : "F";
                info.IntAge = (int)(info.OrderDate - ptInfo.Birthday) / 10000;
            }

            return info;
        }
    }
}
