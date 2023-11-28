using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Calculate.Extensions;
using Reporting.KensaLabel.Model;

namespace Reporting.KensaLabel.DB
{
    public class KensaLabelFinder : RepositoryBase, IKensaLabelFinder
    {
        public KensaLabelFinder(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public PtInfModel GetPtInfModel(int hpId, long ptId)
        {
            int printDate = CIUtil.DateTimeToInt(DateTime.Now);
            return NoTrackingDataContext.PtInfs.FindListQueryable(pt => pt.HpId == hpId && pt.PtId == ptId && pt.IsDelete != 1)
                .Select(pt => new PtInfModel()
                {
                    PtNum = pt.PtNum,
                    KanaName = pt.KanaName,
                    Name = pt.Name,
                    PrintDate = printDate,
                    Sex = pt.Sex,
                    BirthDay = pt.Birthday,
                }).FirstOrDefault();
        }
    }
}
