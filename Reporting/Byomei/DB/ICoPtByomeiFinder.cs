using Entity.Tenant;
using Reporting.Byomei.Model;

namespace Reporting.Byomei.DB;

public interface ICoPtByomeiFinder
{
    PtInf FindPtInf(int hpId, long ptId);

    List<PtByomei> GetPtByomei(int hpId, long ptId, int fromDay, int toDay,
        bool tenkiIn, List<int> hokenIds);

    List<CoPtHokenInfModel> GetPtHokenInf(int hpId, long ptId, List<int> hokenIds, int sinDate);

    void ReleaseResource();
}
