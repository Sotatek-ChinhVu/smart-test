using PostgreDataContext;
using EmrCalculateApi.Extensions;
using EmrCalculateApi.Constants;
using EmrCalculateApi.Interface;

namespace EmrCalculateApi.ReceFutan.DB.CommandHandler
{
    public class ClearCommandHandler
    {
        private readonly TenantDataContext _tenantDataContext;
        private readonly IEmrLogger _emrLogger;
        public ClearCommandHandler(TenantDataContext tenantDataContext, IEmrLogger emrLogger)
        {
            _tenantDataContext = tenantDataContext;
            _emrLogger = emrLogger;
        }

        public void ClearCalculate(long hpId, List<long> ptIds, int seikyuYm)
        {
            const string conFncName = nameof(ClearCalculate);
            try
            {
                if (ptIds?.Count >= 1)
                {
                    _tenantDataContext.ReceInfs.RemoveRange(p =>
                        p.HpId == hpId &&
                        ptIds.Contains(p.PtId) &&
                        p.SeikyuYm == seikyuYm
                    );
                    _tenantDataContext.ReceInfPreEdits.RemoveRange(p =>
                        p.HpId == hpId &&
                        ptIds.Contains(p.PtId) &&
                        p.SeikyuYm == seikyuYm
                    );
                    _tenantDataContext.ReceFutanKbns.RemoveRange(p =>
                        p.HpId == hpId &&
                        ptIds.Contains(p.PtId) &&
                        p.SeikyuYm == seikyuYm
                    );
                    _tenantDataContext.ReceInfJds.RemoveRange(p =>
                        p.HpId == hpId &&
                        ptIds.Contains(p.PtId) &&
                        p.SeikyuYm == seikyuYm
                    );
                }
                else
                {
                    _tenantDataContext.ReceInfs.RemoveRange(p =>
                        p.HpId == hpId &&
                        p.SeikyuYm == seikyuYm
                    );
                    _tenantDataContext.ReceInfPreEdits.RemoveRange(p =>
                        p.HpId == hpId &&
                        p.SeikyuYm == seikyuYm
                    );
                    _tenantDataContext.ReceFutanKbns.RemoveRange(p =>
                        p.HpId == hpId &&
                        p.SeikyuYm == seikyuYm
                    );
                    _tenantDataContext.ReceInfJds.RemoveRange(p =>
                        p.HpId == hpId &&
                        p.SeikyuYm == seikyuYm
                    );
                }                
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }
    }
}
