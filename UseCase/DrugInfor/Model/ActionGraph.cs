namespace UseCase.DrugInfor.Model;

public class ActionGraph
{
    public ActionGraph(ActionType actionType, int sinDate, int startDate, int endDate, int dayCount, string tooltip)
    {
        ActionType = actionType;
        SinDate = sinDate;
        StartDate = sinDate < startDate ? startDate : sinDate;
        EndDate = endDate;
        DayCount = dayCount;
        Tooltip = tooltip;
    }

    public ActionType ActionType { get; private set; }

    public int SinDate { get; private set; }

    public int StartDate { get; private set; }

    public int EndDate { get; private set; }

    public int DayCount { get; private set; }

    public string Tooltip { get; private set; }
}
