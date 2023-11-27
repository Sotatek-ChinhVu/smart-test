using Domain.Common;
using Reporting.Yakutai.Model;

namespace Reporting.Yakutai.DB
{
    public interface ICoYakutaiFinder : IRepositoryBase
    {
        CoHpInfModel FindHpInf(int hpId, int sinDate);
        CoPtInfModel FindPtInf(int hpId, long ptId, int sinDate);
        CoRaiinInfModel FindRaiinInfData(int hpId, long ptId, int sinDate, long raiinNo);
        List<CoOdrInfModel> FindOdrInf(int hpId, long ptId, int sinDate, long raiinNo);
        List<CoOdrInfDetailModel> FindOdrInfDetail(int hpId, long ptId, int sinDate, long raiinNo);
        List<CoSingleDoseMstModel> FindSingleDoseMst(int hpId);
    }
}
