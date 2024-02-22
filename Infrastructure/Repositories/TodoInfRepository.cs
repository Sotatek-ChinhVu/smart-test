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

        public List<TodoInfModel> Upsert(List<TodoInfModel> upsertTodoList, int userId, int hpId)
        {
            var todoInfs = new List<TodoInf>();
            foreach (var input in upsertTodoList)
            {
                var todoInf = TrackingDataContext.TodoInfs.FirstOrDefault(x => x.HpId == hpId && x.TodoNo == input.TodoNo && x.TodoEdaNo == input.TodoEdaNo && x.PtId == input.PtId && x.IsDeleted == DeleteTypes.None);
                if (todoInf != null)
                {
                    if (input.IsDeleted == DeleteTypes.Deleted)
                    {
                        todoInf.IsDeleted = DeleteTypes.Deleted;
                    }
                    else
                    {
                        todoInf.SinDate = input.SinDate;
                        todoInf.RaiinNo = input.RaiinNo;
                        todoInf.TodoKbnNo = input.TodoKbnNo;
                        todoInf.TodoGrpNo = input.TodoGrpNo;
                        todoInf.Tanto = input.Tanto;
                        todoInf.Term = input.Term;
                        todoInf.Cmt1 = input.Cmt1;
                        todoInf.Cmt2 = input.Cmt2;
                        todoInf.IsDone = input.IsDone;
                        todoInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        todoInf.UpdateId = userId;
                        todoInf.UpdateMachine = string.Empty;
                    }
                    todoInfs.Add(todoInf);
                }
                else
                {
                    var newTodoInf = ConvertTo_TodoGrpMst(input, userId, hpId);
                    TrackingDataContext.TodoInfs.AddRange(newTodoInf);
                    todoInfs.Add(newTodoInf);
                }
            }

            TrackingDataContext.SaveChanges();

            if (todoInfs.Count <= 1)
            {
                var firtTodo = todoInfs.FirstOrDefault() ?? new();
                return GetList(hpId, firtTodo.TodoNo, firtTodo.TodoEdaNo, true, true);
            }

            List<TodoInfModel> result = GetList(hpId, 0, 0, true, true);
            return result;
        }

        private TodoInf ConvertTo_TodoGrpMst(TodoInfModel u, int userId, int hpId)
        {
            return new TodoInf
            {
                HpId = hpId,
                TodoNo = u.TodoNo,
                TodoEdaNo = hpId,
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
        public bool CheckExist(int hpId, List<Tuple<int, int, long>> inputs)
        {
            inputs = inputs.Distinct().ToList();
            var countptIds = NoTrackingDataContext.TodoInfs.AsEnumerable().Count(t => t.HpId == hpId && inputs.Any(i => i.Item1 == t.TodoNo && i.Item2 == t.TodoEdaNo && i.Item3 == t.PtId));
            return inputs.Count == countptIds;
        }

        public List<TodoInfModel> GetList(int hpId, int todoNo, int todoEdaNo, bool incDone, bool isDeleted = false, bool sortByPtNum = false)
        {
            List<TodoInfModel> result;
            var todoInfRes = NoTrackingDataContext.TodoInfs.Where(inf => inf.HpId == hpId && (isDeleted || inf.IsDeleted == 0));
            var raiinInfRes = NoTrackingDataContext.RaiinInfs.Where(inf => inf.HpId == hpId);
            var patientInfRes = NoTrackingDataContext.PtInfs.Where(inf => inf.HpId == hpId);
            var userMstRes = NoTrackingDataContext.UserMsts.Where(mst => mst.HpId == hpId);
            var kaMstRes = NoTrackingDataContext.KaMsts.Where(mst => mst.HpId == hpId);
            var todoKbnMstRes = NoTrackingDataContext.TodoKbnMsts.Where(mst => mst.HpId == hpId);
            var todoGrpMstRes = NoTrackingDataContext.TodoGrpMsts.Where(mst => mst.HpId == hpId);
            var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(
                (p) => p.HpId == hpId &&
                       p.IsDeleted == 0
            );
            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(
                (p) => p.HpId == hpId &&
                       p.IsDeleted == 0
            );

            var todoInfRes2 =
                from ti1 in todoInfRes
                join ri1 in raiinInfRes on
                    new { ti1.HpId, ti1.PtId, ti1.SinDate, ti1.RaiinNo } equals
                    new { ri1.HpId, ri1.PtId, ri1.SinDate, ri1.RaiinNo } into raiinInfs
                from ri2 in raiinInfs.DefaultIfEmpty()
                where
                    (todoNo <= 0 || todoEdaNo <= 0 || (ti1.TodoNo == todoNo && ti1.TodoEdaNo == todoEdaNo)) &&
                    (incDone || (ti1.IsDone == 0))
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
                    IsVisitDeleted = ri2 != null && (ri2.IsDeleted == DeleteTypes.Deleted || ri2.IsDeleted == DeleteTypes.Confirm),
                };

            var query =
                from ti2 in todoInfRes2
                join pi1 in patientInfRes on
                    new { ti2.TodoInf.HpId, ti2.TodoInf.PtId } equals
                    new { pi1.HpId, pi1.PtId }
                join um1 in userMstRes on
                    new { ti2.TodoInf.HpId, ti2.TodoInf.Tanto } equals
                    new { um1.HpId, Tanto = um1.UserId } into tantoList
                from tantoInf in tantoList.DefaultIfEmpty()
                join um2 in userMstRes on
                    new { ti2.TodoInf.HpId, ti2.TodoInf.CreateId } equals
                    new { um2.HpId, CreateId = um2.UserId } into createrList
                from createrInf in createrList.DefaultIfEmpty()
                join um3 in userMstRes on
                    new { ti2.TodoInf.HpId, ti2.TodoInf.UpdateId } equals
                    new { um3.HpId, UpdateId = um3.UserId } into updaterList
                from updaterInf in updaterList.DefaultIfEmpty()
                join km1 in kaMstRes on
                    new { ti2.TodoInf.HpId, KaId = ti2.KaId } equals
                    new { km1.HpId, km1.KaId } into kaList
                from kaInf in kaList.DefaultIfEmpty()
                join um4 in userMstRes on
                    new { ti2.TodoInf.HpId, PrimaryDoctor = ti2.TantoId } equals
                    new { um4.HpId, PrimaryDoctor = um4.UserId } into primaryDoctorList
                from primaryDoctor in primaryDoctorList.DefaultIfEmpty()
                join tkm1 in todoKbnMstRes on
                    new { ti2.TodoInf.HpId, ti2.TodoInf.TodoKbnNo } equals
                    new { tkm1.HpId, tkm1.TodoKbnNo } into todoKbnMstList
                from todoKbnMstInf in todoKbnMstList.DefaultIfEmpty()
                join tgm1 in todoGrpMstRes on
                    new { ti2.TodoInf.HpId, ti2.TodoInf.TodoGrpNo } equals
                    new { tgm1.HpId, tgm1.TodoGrpNo } into todoGrpMstList
                from todoGrpMstInf in todoGrpMstList.DefaultIfEmpty()
                join ptHokenPattern in ptHokenPatterns on
                    new { ti2.TodoInf.HpId, ti2.PtId, ti2.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid } into ptHokenPatternList
                from ptHokenPatternItem in ptHokenPatternList.DefaultIfEmpty()
                join ptHokenInf in ptHokenInfs on
                    new { ptHokenPatternItem.HpId, ptHokenPatternItem.PtId, ptHokenPatternItem.HokenId } equals
                    new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenInfList
                from ptHokenInfItem in ptHokenInfList.DefaultIfEmpty()
                select new
                {
                    ti2.TodoInf,
                    ti2.IsYoyaku,
                    ti2.OyaRaiinNo,
                    ti2.SyosaisinKbn,
                    ti2.JikanKbn,
                    ti2.Status,
                    ti2.IsVisitDeleted,
                    PtNum = pi1 == null ? 0 : pi1.PtNum,
                    PatientName = pi1 == null ? string.Empty : pi1.Name,
                    PrimaryDoctorName = primaryDoctor == null ? string.Empty : primaryDoctor.Sname,
                    KaSname = kaInf == null ? string.Empty : kaInf.KaSname,
                    TodoKbnName = todoKbnMstInf == null ? string.Empty : todoKbnMstInf.TodoKbnName,
                    TodoKbnNo = todoKbnMstInf == null ? 0 : todoKbnMstInf.TodoKbnNo,
                    CreaterName = createrInf == null ? string.Empty : createrInf.Sname,
                    TantoName = tantoInf == null ? string.Empty : tantoInf.Sname,
                    UpdaterName = updaterInf == null ? string.Empty : updaterInf.Sname,
                    TodoGrpNo = todoGrpMstInf == null ? 0 : todoGrpMstInf.TodoGrpNo,
                    TodoGrpName = todoGrpMstInf == null ? string.Empty : todoGrpMstInf.TodoGrpName,
                    TodoGrpColor = todoGrpMstInf == null ? string.Empty : todoGrpMstInf.GrpColor,
                    Gender = pi1 == null ? 0 : pi1.Sex,
                    HokenPattern = ptHokenPatternItem,
                    PtHokenInf = ptHokenInfItem
                };

            result = query.AsEnumerable().Select(
                            x => new TodoInfModel(
                                hpId,
                                x.TodoInf.PtId,
                                x.PtNum,
                                x.PatientName,
                                x.TodoInf.SinDate,
                                x.PrimaryDoctorName,
                                x.KaSname,
                                x.TodoKbnName,
                                x.TodoInf.Cmt1 ?? string.Empty,
                                x.TodoInf.CreateDate,
                                x.CreaterName,
                                x.TantoName,
                                x.TodoInf.Cmt2 ?? string.Empty,
                                x.TodoInf.UpdateDate,
                                x.UpdaterName,
                                x.TodoGrpName,
                                x.TodoInf.Term,
                                x.HokenPattern?.HokenPid ?? 0,
                                x.PtHokenInf?.Houbetu ?? string.Empty,
                                x.HokenPattern?.HokenKbn ?? 0,
                                x.PtHokenInf?.HokensyaNo ?? string.Empty,
                                x.PtHokenInf?.HokenId ?? 0,
                                x.TodoInf.Tanto,
                                x.TodoInf.TodoNo,
                                x.TodoInf.TodoEdaNo,
                                x.TodoInf.RaiinNo,
                                x.TodoKbnNo,
                                x.TodoGrpNo,
                                x.TodoInf.IsDone,
                                x.Status,
                                x.Gender,
                                x.TodoGrpColor,
                                x.TodoInf.CreateId
                                ))
                                .ToList();
            result = sortByPtNum ? result.OrderBy(item => item.PtNum)
                                         .ThenByDescending(model => model.UpdateDate)
                                         .ToList()
                                 : result.OrderByDescending(model => model.UpdateDate)
                                       .ThenBy(model => model.PtId)
                                       .ToList();
            return result;
        }
    }
}