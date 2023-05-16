using Domain.Common;

namespace Domain.Models.Todo
{
    public interface ITodoKbnMstRepository : IRepositoryBase
    {
        List<TodoKbnMstModel> GetTodoKbnMsts(int hpId);
    }
}
