namespace Domain.Models.Todo;

public class TodoKbnMstModel
{
    public TodoKbnMstModel(int todoKbnNo, string todoKbnName, int actCd)
    {
        TodoKbnNo = todoKbnNo;
        TodoKbnName = todoKbnName;
        ActCd = actCd;
    }

    public int TodoKbnNo { get; private set; }

    public string TodoKbnName { get; private set; }

    public int ActCd { get; private set; }
}
