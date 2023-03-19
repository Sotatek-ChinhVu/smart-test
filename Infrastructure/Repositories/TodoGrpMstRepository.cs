using Domain.Models.Todo;
using Domain.Models.UketukeSbtMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TodoGrpMstRepository : RepositoryBase, ITodoGrpMstRepository
    {
        public TodoGrpMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
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
                    if(todoGrpMsts != null)
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
                        TrackingDataContext.TodoGrpMsts.AddRange(ConvertTo_TodoGrpMst(input, userId, hpId));
                    }
                }
            }
            TrackingDataContext.SaveChanges();
        }

        private TodoGrpMst ConvertTo_TodoGrpMst(TodoGrpMstModel u, int userId, int hpId)
        {
            return new TodoGrpMst
            {
                HpId = hpId,
                TodoGrpNo = u.TodoGrpNo,
                TodoGrpName = u.TodoGrpName,
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
