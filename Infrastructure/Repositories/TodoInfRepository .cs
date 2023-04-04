using Domain.Models.Todo;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{

    public class TodoInfRepository : RepositoryBase, ITodoInfRepository
    {
        public TodoInfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public void Upsert(List<TodoInfModel> upsertTodoList, int userId, int hpId)
        {
            foreach (var input in upsertTodoList)
            {
                var todoInfs = TrackingDataContext.TodoInfs.FirstOrDefault(x => x.TodoNo == input.TodoNo && x.TodoEdaNo == input.TodoEdaNo && x.PtId == input.PtId && x.IsDeleted == DeleteTypes.None);
                if (todoInfs != null)
                {
                    if (input.IsDeleted == DeleteTypes.Deleted)
                    {
                        todoInfs.IsDeleted = DeleteTypes.Deleted;
                    }
                    else
                    {
                        todoInfs.SinDate = input.SinDate;
                        todoInfs.RaiinNo = input.RaiinNo;
                        todoInfs.TodoKbnNo = input.TodoKbnNo;
                        todoInfs.TodoGrpNo = input.TodoGrpNo;
                        todoInfs.Tanto = input.Tanto;
                        todoInfs.Term = input.Term;
                        todoInfs.Cmt1 = input.Cmt1;
                        todoInfs.Cmt2 = input.Cmt2;
                        todoInfs.IsDone = input.IsDone;
                        todoInfs.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        todoInfs.UpdateId = userId;
                        todoInfs.UpdateMachine = string.Empty;
                    }
                }
                else
                {
                    TrackingDataContext.TodoInfs.AddRange(ConvertTo_TodoGrpMst(input, userId, hpId));
                }
            }
            TrackingDataContext.SaveChanges();
        }

        private TodoInf ConvertTo_TodoGrpMst(TodoInfModel u, int userId, int hpId)
        {
            return new TodoInf
            {
                HpId = hpId,
                TodoNo = u.TodoNo,
                TodoEdaNo = u.TodoEdaNo,
                PtId = u.PtId,
                SinDate = u.SinDate,
                RaiinNo = u.RaiinNo,
                TodoKbnNo = u.TodoKbnNo,
                TodoGrpNo = u.TodoGrpNo,
                Tanto = u.Tanto,
                Term = u.Term,
                Cmt1 = u.Cmt1,
                Cmt2 = u.Cmt2,
                IsDone = u.IsDone,
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

        /// <summary>
        /// item1:TodoNo, item2: TodoEdaNo, item3: PtId
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public bool Check(List<Tuple<int, int, long>> inputs)
        {
            inputs = inputs.Distinct().ToList();
            var countptIds = NoTrackingDataContext.TodoInfs.Count(t => inputs.Any(i => i.Item1 ==  t.TodoNo && i.Item2 == t.TodoEdaNo && i.Item3 == t.PtId));
            return inputs.Count == countptIds;
        }
    }
}