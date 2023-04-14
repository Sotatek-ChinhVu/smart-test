using Domain.Models.Todo;
using UseCase.Todo.TodoGrpMst;

namespace UseCase.Todo
{
    public class InsertTodoGrpMstDto
    {
        public InsertTodoGrpMstDto(int todoGrpNo, string todoGrpName, string grpColor, int sortNo, int isDeleted)
        {
            TodoGrpNo = todoGrpNo;
            TodoGrpName = todoGrpName;
            GrpColor = grpColor;
            SortNo = sortNo;
            IsDeleted = isDeleted;
        }

        public int TodoGrpNo { get; private set; }

        public string TodoGrpName { get; private set; }

        public string GrpColor { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
