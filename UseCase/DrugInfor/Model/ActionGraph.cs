namespace UseCase.DrugInfor.Model;

public class ActionGraph
{
    public ActionGraph(ActionType actionType, int sinDate, int endDate, int dayCount, string tooltip)
    {
        ActionType = actionType;
        SinDate = sinDate;
        EndDate = endDate;
        DayCount = dayCount;
        Tooltip = tooltip;
    }

    public ActionType ActionType { get; private set; }

    public int SinDate { get; private set; }

    public int EndDate { get; private set; }

    public int DayCount { get; private set; }

    public string Tooltip { get; private set; }
}
