using Domain.Common;
using Domain.Types;

namespace Domain.Models.Todo
{
    public interface ITodoInfRepository : IRepositoryBase
    {
        void Upsert(List<TodoInfModel> upsertTodoList, int userId, int hpId);

        bool Check(List<Tuple<int, int, long>> inputs);
    }
}