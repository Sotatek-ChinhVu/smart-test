namespace Domain.Models.Todo;
using static Helper.Constants.TodoGrpMstConstant;

public class TodoGrpMstModel
{
    public TodoGrpMstModel(int todoGrpNo, string todoGrpName, string grpColor, int sortNo, int isDeleted)
    {
        TodoGrpNo = todoGrpNo;
        TodoGrpName = todoGrpName;
        GrpColor = grpColor;
        SortNo = sortNo;
        IsDeleted = isDeleted;
    }

    public int HpId { get; private set; }

    public int TodoGrpNo { get; private set; }

    public string TodoGrpName { get; private set; }

    public string GrpColor { get; private set; }

    public int SortNo { get; private set; }

    public int IsDeleted { get; private set; }

    public ValidationStatus Validation()
    {
        if (TodoGrpNo <= 0)
        {
            return ValidationStatus.InvalidTodoGrpNo;
        }

        if (TodoGrpName.Length > 20)
        {
            return ValidationStatus.InvalidTodoGrpName;
        }

        if (GrpColor.Length > 8)
        {
            return ValidationStatus.InvalidGrpColor;
        }

        if (SortNo <= 0)
        {
            return ValidationStatus.InvalidSortNo;
        }

        if (IsDeleted < 0)
        {
            return ValidationStatus.InvalidIsDeleted;
        }

        return ValidationStatus.Valid;
    }
}
