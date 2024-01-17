using Domain.Common;
using Entity.Tenant;
using Reporting.GrowthCurve.Model;

namespace Reporting.GrowthCurve.DB;

public interface ISpecialNoteFinder : IRepositoryBase
{
    List<GcStdInfModel> GetStdPoint(int hpId);

    List<KensaInfDetail> GetKensaInf(long ptId, int fromDate, int toDate, string itemCD);
}
