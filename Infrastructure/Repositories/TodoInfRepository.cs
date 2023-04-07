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
        public bool CheckExist(List<Tuple<int, int, long>> inputs)
        {
            inputs = inputs.Distinct().ToList();
            var countptIds = NoTrackingDataContext.TodoInfs.Count(t => inputs.Any(i => i.Item1 ==  t.TodoNo && i.Item2 == t.TodoEdaNo && i.Item3 == t.PtId));
            return inputs.Count == countptIds;
        }

        public List<TodoInfModel> GetList(int hpId, int todoNo, int todoEdaNo, int ptId, int isDone)
        {
            List<TodoInfModel> result;
            var todoInfs = NoTrackingDataContext.TodoInfs.Where(x => x.HpId == hpId && x.IsDeleted == 0);

            var todoGrpMsts = NoTrackingDataContext.TodoGrpMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0);

            var userMsts = NoTrackingDataContext.UserMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0);

            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId);

            var patientInfs = NoTrackingDataContext.PtInfs.Where(x => x.HpId == hpId && x.IsDelete == 0);

            var kaMsts = NoTrackingDataContext.KaMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0);

            var todoKbnMsts = NoTrackingDataContext.TodoKbnMsts.Where(x => x.HpId == hpId);

            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.IsDeleted == 0);

            var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(x => x.HpId == hpId && x.IsDeleted == 0);

            var todoInfRes2 =
                    from ti1 in todoInfs
                    join ri1 in raiinInfs on
                        new { ti1.HpId, ti1.PtId, ti1.SinDate, ti1.RaiinNo } equals
                        new { ri1.HpId, ri1.PtId, ri1.SinDate, ri1.RaiinNo } into listraiinInf
                    from ri2 in listraiinInf.DefaultIfEmpty()
                    where
                        (todoNo <= 0 || todoEdaNo <= 0 || (ti1.TodoNo == todoNo && ti1.TodoEdaNo == todoEdaNo)) &&
                        (ti1.IsDone == 0)
                    select new
                    {
                        TodoInf = ti1,
                        Status = ri2 == null ? 0 : ri2.Status,
                        KaId = ri2 == null ? 0 : ri2.KaId,
                        TantoId = ri2 == null ? 0 : ri2.TantoId,
                        PtId = ri2 == null ? 0 : ri2.PtId,
                        HokenPid = ri2 == null ? 0 : ri2.HokenPid,
                        IsYoyaku = ri2 == null ? 0 : ri2.IsYoyaku,
                        OyaRaiinNo = ri2 == null ? 0 : ri2.OyaRaiinNo,
                        SyosaisinKbn = ri2 == null ? 0 : ri2.SyosaisinKbn,
                        JikanKbn = ri2 == null ? 0 : ri2.JikanKbn,
                        IsVisitDeleted = ri2 == null ? false : (ri2.IsDeleted == DeleteTypes.Deleted || ri2.IsDeleted == DeleteTypes.Confirm),
                    };

            var query = from todoInfRes in todoInfRes2.AsEnumerable()
                        join ptInf in patientInfs on
                            new { todoInfRes.TodoInf.HpId, todoInfRes.PtId } equals
                            new { ptInf.HpId, ptInf.PtId } into listPtInfs
                        join todoGrpMst in todoGrpMsts on
                            new { todoInfRes.TodoInf.HpId, todoInfRes.TodoInf.TodoGrpNo } equals
                            new { todoGrpMst.HpId, todoGrpMst.TodoGrpNo } into listTodoGrpMsts
                        join todoKbnMst in todoKbnMsts on
                             new { todoInfRes.TodoInf.HpId, todoInfRes.TodoInf.TodoKbnNo } equals
                             new { todoKbnMst.HpId, todoKbnMst.TodoKbnNo } into listTodoKbnMsts
                        join raiinInf in raiinInfs on
                            new { todoInfRes.TodoInf.HpId, todoInfRes.TodoInf.PtId } equals
                            new { raiinInf.HpId, raiinInf.PtId } into listRaiinInfs
                        from listRaiinInf in listRaiinInfs.DefaultIfEmpty()
                        join kaMst in kaMsts on
                             new { listRaiinInf.HpId, listRaiinInf.KaId } equals
                             new { kaMst.HpId, kaMst.KaId } into listKaMsts
                        join userMst1 in userMsts on
                             new { listRaiinInf.HpId, listRaiinInf.TantoId } equals
                             new { userMst1.HpId, TantoId = userMst1.UserId } into listUserMsts1
                        join userMst2 in userMsts on
                             new { listRaiinInf.HpId, listRaiinInf.CreateId } equals
                             new { userMst2.HpId, CreateId = userMst2.UserId } into listUserMsts2
                        join userMst3 in userMsts on
                             new { listRaiinInf.HpId, listRaiinInf.UpdateId } equals
                             new { userMst3.HpId, UpdateId = userMst3.UserId } into listUserMsts3
                        join userMst4 in userMsts on
                            new {listRaiinInf.HpId, PrimaryDoctor = listRaiinInf.TantoId} equals
                            new {userMst4.HpId, PrimaryDoctor = userMst4.UserId} into listUserMsts4
                        join ptHokenPattern in ptHokenPatterns on
                             new { todoInfRes.TodoInf.HpId, todoInfRes.TodoInf.PtId, todoInfRes.HokenPid } equals
                             new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid } into listHokenPatterns
                        from listHokenPattern in listHokenPatterns.DefaultIfEmpty()
                        join ptHokenInf in ptHokenInfs on
                             new { listHokenPattern.HpId, listHokenPattern.PtId, listHokenPattern.HokenId } equals
                             new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } into listHokenInfs
                        from listHokenInf in listHokenInfs.DefaultIfEmpty()
                        select new
                        {
                            TodoInf = todoInfRes,
                            HokenInf = listHokenInf,
                            TodoGrpMsts = listTodoGrpMsts.FirstOrDefault(),
                            PtInfs = listPtInfs.FirstOrDefault(),
                            RaiinInfs = listRaiinInfs.FirstOrDefault(),
                            TodoKbnMsts = listTodoKbnMsts.FirstOrDefault(),
                            KaMsts = listKaMsts.FirstOrDefault(),
                            UserMsts1 = listUserMsts1.FirstOrDefault(),
                            UserMsts2 = listUserMsts2.FirstOrDefault(),
                            UserMsts3 = listUserMsts3.FirstOrDefault(),
                            UserMsts4 = listUserMsts4.FirstOrDefault(),
                            HokenPatterns = listHokenPatterns.FirstOrDefault()
                        };

            var count1 = query.Count();

            var count0 = todoInfRes2.Count();

            result = query.AsEnumerable().Select((x) => new TodoInfModel(
                            x.PtInfs.PtNum,
                            x.RaiinInfs.SinDate,
                            x.PtInfs.Name ?? string.Empty,
                            x.UserMsts4.Sname ?? string.Empty,
                            x.KaMsts.KaName ?? string.Empty,
                            x.TodoKbnMsts.TodoKbnName ?? string.Empty,
                            x.UserMsts1.Sname ?? string.Empty,
                            x.TodoInf.TodoInf.Cmt2 ?? string.Empty,
                            x.TodoInf.TodoInf.CreateDate,
                            x.UserMsts3.Sname ?? string.Empty,
                            x.TodoInf.TodoInf.Cmt1 ?? string.Empty,
                            x.UserMsts2.Sname ?? string.Empty,
                            x.TodoGrpMsts.TodoGrpName ?? string.Empty,
                            x.TodoInf.TodoInf.Term,
                            x.HokenPatterns.HokenPid,
                            x.HokenInf.Houbetu ?? string.Empty,
                            x.HokenPatterns.HokenKbn,
                            x.HokenInf.HokensyaNo ?? string.Empty,
                            x.HokenInf.HokenId
                            ))
                    .ToList();
            return result;
        }
    }
}