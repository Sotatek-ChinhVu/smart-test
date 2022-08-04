using Domain.Constant;
using Domain.Models.PatientRaiinKubun;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PatientRaiinKubunReponsitory: IPatientRaiinKubunReponsitory
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public PatientRaiinKubunReponsitory(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public IEnumerable<PatientRaiinKubunModel> GetPatientRaiinKubun(int hpId, long ptId, int raiinNo, int sinDate)
        {
            var raiinKbnMst = _tenantDataContext.RaiinKbnMsts.Where(x => x.IsDeleted == DeleteStatus.None && x.HpId == hpId).ToList();

            var raiinKbnInf = _tenantDataContext.RaiinKbnInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RaiinNo == raiinNo && x.SinDate == sinDate && x.IsDelete == DeleteStatus.None ).ToList();

            var joinQuery =    from rkbInf in raiinKbnInf
                               join rknMst in raiinKbnMst on rkbInf.GrpId  equals rknMst.GrpCd 
                               select new
                               {
                                   HpId = rkbInf.HpId,
                                   GrpId = rkbInf.GrpId,
                                   KbnCd = rkbInf.KbnCd,
                                   GrpName = rknMst.GrpName,
                                   SortNo = rknMst.SortNo
                               };
            var dataListItem = joinQuery.AsEnumerable().Select(x => new PatientRaiinKubunModel(
                                                         x.HpId,
                                                         x.GrpId,
                                                         x.GrpName,
                                                         x.KbnCd,
                                                         x.SortNo
                                                         )).OrderBy(x => x.SortNo).ToList();
            return dataListItem;
        }
    }
}
