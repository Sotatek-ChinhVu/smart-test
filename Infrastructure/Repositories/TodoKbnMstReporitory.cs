using Domain.Models.Todo;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Repositories
{
    public class TodoKbnMstReporitory : RepositoryBase, ITodoKbnMstRepository
    {
        public TodoKbnMstReporitory(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<TodoKbnMstModel> GetTodoKbnMsts(int hpId)
        {
            List<TodoKbnMstModel> result = NoTrackingDataContext.TodoKbnMsts.Where(x => x.HpId == hpId)
                                                    .AsEnumerable()
                                                    .OrderBy(x => x.TodoKbnNo)
                                                    .Select((x) => new TodoKbnMstModel(x.TodoKbnNo,
                                                                                       x.TodoKbnName ?? string.Empty,
                                                                                       x.ActCd))
                                                    .ToList();
            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
