using Domain.Common;

namespace Domain.Models.Todo
{
    public interface ITodoGrpMstRepository : IRepositoryBase
    {
        void Upsert(List<TodoGrpMstModel> upsertTodoGrpMstList, int userId, int hpId);

        bool CheckExistedTodoGrpNo(List<int> todoGrpNos);
    }
}
