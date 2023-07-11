namespace Reporting.InDrug.DB
{
    public interface ICoInDrugFinder
    {
        CoPtInfModel FindPtInf(int hpId, long ptId, int sinDate);

        CoRaiinInfModel FindRaiinInfData(int hpId, long ptId, int sinDate, long raiinNo);

        List<CoOdrInfModel> FindOdrInf(int hpId, long ptId, int sinDate, long raiinNo);

        List<CoOdrInfDetailModel> FindOdrInfDetail(int hpId, long ptId, int sinDate, long raiinNo);
    }
}
