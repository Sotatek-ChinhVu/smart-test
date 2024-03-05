namespace Domain.Models.Yousiki;

public class VisitingInfModel
{
    public VisitingInfModel(int sinDate, string uketukeTime, long raiinNo, int status, string kaName, string doctorName, int syosaisinKbn, string uketukeSbtName, string ptComment, string yousikiKaCd, List<RaiinListInfModel> raiinListInfList)
    {
        SinDate = sinDate;
        UketukeTime = uketukeTime;
        RaiinNo = raiinNo;
        Status = status;
        KaName = kaName;
        DoctorName = doctorName;
        SyosaisinKbn = syosaisinKbn;
        UketukeSbtName = uketukeSbtName;
        PtComment = ptComment.Replace(Environment.NewLine, " ・ ");
        YousikiKaCd = yousikiKaCd;
        RaiinListInfList = raiinListInfList;
    }

    public VisitingInfModel UpdateRaiinListInfList(List<RaiinListInfModel> raiinListInfList)
    {
        RaiinListInfList = raiinListInfList;
        return this;
    }

    public int SinDate { get; private set; }

    public string UketukeTime { get; private set; }

    public long RaiinNo { get; private set; }

    public int Status { get; private set; }

    public string KaName { get; private set; }

    public string DoctorName { get; private set; }

    public int SyosaisinKbn { get; private set; }

    public string UketukeSbtName { get; private set; }

    public string PtComment { get; private set; }

    public string YousikiKaCd { get; private set; }

    public List<RaiinListInfModel> RaiinListInfList { get; private set; }
}
