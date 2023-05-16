namespace Domain.Models.Todo;

public class TodoKbnMstModel
{
    public TodoKbnMstModel(int hpId, int todoKbnNo, string todoKbnName, int actCd)
    {
        HpId = hpId;
        TodoKbnNo = todoKbnNo;
        TodoKbnName = todoKbnName;
        ActCd = actCd;
    }

    public int HpId { get; private set; }

    public int TodoKbnNo { get; private set; }

    public string TodoKbnName { get; private set; }

    public int ActCd { get; private set; }
}
