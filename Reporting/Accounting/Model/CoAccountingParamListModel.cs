namespace Reporting.Accounting.Model;

public class CoAccountingParamListModel
{
    public CoAccountingParamListModel(int startDate, int endDate, List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions)
    {
        StartDate = startDate;
        EndDate = endDate;
        PtConditions = ptConditions;
        GrpConditions = grpConditions;
    }

    public int StartDate { get; private set; }

    public int EndDate { get; private set; }

    public List<(long ptId, int hokenId)> PtConditions { get; private set; }

    public List<(int grpId, string grpCd)> GrpConditions { get; private set; }
}
