using Domain.Common;
using Domain.Types;

namespace Domain.Models.Todo
{
    public interface ITodoInfRepository : IRepositoryBase
    {
        void Upsert(List<TodoInfModel> upsertTodoList, int userId, int hpId);

        bool CheckExistedTodoNo(List<int> todoNos);

        bool CheckExistedTodoEdaNo(List<int> todoEdaNos);

        bool CheckExistedPtId(List<long> ptIds);

        List<TodoInfModel> GetList(int hpId, int todoNo, int todoEdaNo, int ptId, int isDone);
    }
}