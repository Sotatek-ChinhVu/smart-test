namespace Domain.Models.Receipt.Recalculation;

public class ReceSinKouiCountModel
{
    public ReceSinKouiCountModel(List<PtHokenPatternModel> ptHokenPatterns, List<SinKouiDetailModel> sinKouiDetailModels, long ptId, int sinDate, long raiinNo, bool isFirstVisit, bool isReVisit, bool existSameFirstVisit)
    {
        PtHokenPatterns = ptHokenPatterns;
        SinKouiDetailModels = sinKouiDetailModels;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        IsFirstVisit = isFirstVisit;
        IsReVisit = isReVisit;
        ExistSameFirstVisit = existSameFirstVisit;
    }

    public List<PtHokenPatternModel> PtHokenPatterns { get; private set; }

    public List<SinKouiDetailModel> SinKouiDetailModels { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public bool IsFirstVisit { get; private set; }

    public bool IsReVisit { get; private set; }

    public bool ExistSameFirstVisit { get; private set; }
}
