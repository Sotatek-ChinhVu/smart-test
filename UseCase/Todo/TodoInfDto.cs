using Domain.Models.Todo;

namespace UseCase.Todo
{
    public class TodoInfDto
    {
        public TodoInfDto(int todoNo, int todoEdaNo, long ptId, int sinDate, long raiinNo, int todoKbnNo, int todoGrpNo, int tanto, int term, string cmt1, string cmt2, int isDone, int isDeleted)
        {
            TodoNo = todoNo;
            TodoEdaNo = todoEdaNo;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            RaiinNo = raiinNo;
            TodoKbnNo = todoKbnNo;
            TodoGrpNo = todoGrpNo;
            Tanto = tanto;
            Term = term;
            Cmt1 = cmt1;
            Cmt2 = cmt2;
            IsDone = isDone;
            IsDeleted = isDeleted;
        }

        public TodoInfDto(TodoInfModel model)
        {
            TodoNo = model.TodoNo;
            TodoEdaNo = model.TodoEdaNo;
            PtId = model.PtId;
            SinDate = model.SinDate;
            RaiinNo = model.RaiinNo;
            TodoKbnNo = model.TodoKbnNo;
            TodoGrpNo = model.TodoGrpNo;
            Tanto = model.Tanto;
            Term = model.Term;
            Cmt1 = model.Cmt1;
            Cmt2 = model.Cmt2;
            IsDone = model.IsDone;
            IsDeleted = model.IsDeleted;
        }

        public int TodoNo { get; private set; }

        public int TodoEdaNo { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int TodoKbnNo { get; private set; }

        public int TodoGrpNo { get; private set; }

        public int Tanto { get; private set; }

        public int Term { get; private set; }

        public string Cmt1 { get; private set; }

        public string Cmt2 { get; private set; }

        public int IsDone { get; private set; }

        public int IsDeleted { get; private set; }
    }
}