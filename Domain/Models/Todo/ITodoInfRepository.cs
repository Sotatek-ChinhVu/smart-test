using Domain.Common;
using Domain.Types;

namespace Domain.Models.Todo
{
    public interface ITodoInfRepository : IRepositoryBase
    {
        void Upsert(List<TodoInfModel> upsertTodoList, int userId, int hpId);
        
        //Item1: TodoNo, Item2: TodoEdaNo, Item3: PtId
        bool CheckExist(List<Tuple<int, int, long>> inputs);
    }
}