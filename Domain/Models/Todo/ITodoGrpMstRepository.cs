using Domain.Common;

namespace Domain.Models.Todo
{
    public interface ITodoGrpMstRepository : IRepositoryBase
    {
        void Upsert(List<TodoGrpMstModel> upsertTodoGrpMstList, int userId, int hpId);

        bool CheckExistedTodoGrpNo(int hpId, List<int> todoGrpNos);

        List<TodoGrpMstModel> GetTodoGrpMsts(int hpId);
    }
}
