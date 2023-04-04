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
                if (input.IsDeleted == DeleteTypes.Deleted)
                {
                    if (todoInfs != null)
                    {
                        todoInfs.IsDeleted = DeleteTypes.Deleted;
                    }
                }
                else
                {
                    if (todoInfs != null)
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
                    else
                    {
                        TrackingDataContext.TodoInfs.AddRange(ConvertTo_TodoGrpMst(input, userId, hpId));
                    }
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

        public bool CheckExistedTodoNo(List<int> todoNos)
        {
            var countTodoNos = NoTrackingDataContext.TodoInfs.Where(x => x.TodoNo > 0).Distinct().Count(x => todoNos.Contains(x.TodoNo));
            return todoNos.Count == countTodoNos;
        }

        public bool CheckExistedTodoEdaNo(List<int> todoEdaNos)
        {
            var countTodoEdaNos = NoTrackingDataContext.TodoInfs.Where(x => x.TodoEdaNo > 0).Distinct().Count(x => todoEdaNos.Contains(x.TodoEdaNo));
            return todoEdaNos.Count == countTodoEdaNos;
        }

        public bool CheckExistedPtId(List<long> ptIds)
        {
            var countptIds = NoTrackingDataContext.TodoInfs.Where(x => x.PtId > 0).Distinct().Count(x => ptIds.Contains(x.PtId));
            return ptIds.Count == countptIds;
        }
    }
}