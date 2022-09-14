using PostgreDataContext;
using EmrCalculateApi.Extensions;
using EmrCalculateApi.Constants;
using EmrCalculateApi.Interface;

namespace EmrCalculateApi.Futan.DB.CommandHandler
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

        public void ClearCalculate(long hpId, long ptId, int sinday, List<long> raiinNos)
        {
            const string conFncName = nameof(ClearCalculate);
            try
            {
                _tenantDataContext.KaikeiInfs.RemoveRange(
                    p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinDate == sinday);

                _tenantDataContext.KaikeiDetails.RemoveRange(
                    p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinDate == sinday
                );
                _tenantDataContext.LimitListInfs.RemoveRange(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinDate == sinday &&
                    p.RaiinNo != 0 &&
                    p.IsDeleted == DeleteStatus.None
                );
                _tenantDataContext.LimitCntListInfs.RemoveRange(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinDate == sinday &&
                    p.OyaRaiinNo != 0 &&
                    p.IsDeleted == DeleteStatus.None
                );
                _tenantDataContext.SyunoSeikyus.RemoveRange(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinDate == sinday &&
                    p.NyukinKbn == 0 &&
                    !raiinNos.Contains(p.RaiinNo)
                );
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }
    }
}
