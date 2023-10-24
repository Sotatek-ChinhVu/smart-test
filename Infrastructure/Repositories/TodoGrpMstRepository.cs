using Domain.Models.Todo;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class TodoGrpMstRepository : RepositoryBase, ITodoGrpMstRepository
    {
        public TodoGrpMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public bool CheckExistedTodoGrpNo(List<int> todoGrpNos)
        {
            var countTodoGrpNos = NoTrackingDataContext.TodoGrpMsts.Where(x => todoGrpNos.Distinct().Contains(x.TodoGrpNo)).ToList();
            var countTodoGrpNoIsDeleteds = countTodoGrpNos.Where(x => x.IsDeleted == 1);
            return countTodoGrpNoIsDeleteds.Count() != 0;
        }

        public void Upsert(List<TodoGrpMstModel> upsertTodoGrpMstList, int userId, int hpId)
        {
            foreach (var input in upsertTodoGrpMstList)
            {
                var todoGrpMsts = TrackingDataContext.TodoGrpMsts.FirstOrDefault(x => x.TodoGrpNo == input.TodoGrpNo && x.IsDeleted == DeleteTypes.None);
                if (input.IsDeleted == DeleteTypes.Deleted)
                {
                    if (todoGrpMsts != null)
                    {
                        todoGrpMsts.IsDeleted = DeleteTypes.Deleted;
                    }
                }
                else
                {
                    if (todoGrpMsts != null)
                    {
                        todoGrpMsts.TodoGrpName = input.TodoGrpName;
                        todoGrpMsts.GrpColor = input.GrpColor;
                        todoGrpMsts.SortNo = input.SortNo;
                        todoGrpMsts.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        todoGrpMsts.UpdateId = userId;
                        todoGrpMsts.UpdateMachine = string.Empty;
                    }
                    else
                    {
                        TrackingDataContext.TodoGrpMsts.AddRange(ConvertTodoGrpMst(input, userId, hpId));
                    }
                }
            }
            TrackingDataContext.SaveChanges();
        }

        public List<TodoGrpMstModel> GetTodoGrpMsts(int hpId)
        {
            List<TodoGrpMstModel> result = NoTrackingDataContext.TodoGrpMsts.Where(p => p.HpId == hpId && p.IsDeleted == 0)
                        .OrderBy(t => t.SortNo).AsEnumerable()
                        .Select(t => new TodoGrpMstModel(t.TodoGrpNo, t.TodoGrpName ?? string.Empty, t.GrpColor ?? string.Empty, t.SortNo, t.IsDeleted)).ToList();
            return result;
        }

        private TodoGrpMst ConvertTodoGrpMst(TodoGrpMstModel u, int userId, int hpId)
        {
            return new TodoGrpMst
            {
                HpId = hpId,
                TodoGrpNo = 0,
                TodoGrpName = u.TodoGrpName,
                GrpColor = u.GrpColor,
                SortNo = u.SortNo,
                IsDeleted = u.IsDeleted,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                CreateId = userId,
                CreateMachine = string.Empty,
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId,
                UpdateMachine = string.Empty
            };
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
