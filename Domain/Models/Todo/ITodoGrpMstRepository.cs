using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Todo
{
    public interface ITodoGrpMstRepository : IRepositoryBase
    {
        void Upsert(List<TodoGrpMstModel> upsertTodoGrpMstList, int userId, int hpId);

        bool CheckExistedTodoGrpNo(List<int> todoGrpNos);
    }
}
