namespace UseCase.MedicalExamination.TrailAccounting
{
    public class TrialAccountingInfDto
    {
        public TrialAccountingInfDto(int totalPoint, int kanFutan, int totalSelfExpense, int tax, int adjustFutan, int sumAdjust)
        {
            TotalPoint = totalPoint;
            KanFutan = kanFutan;
            TotalSelfExpense = totalSelfExpense;
            Tax = tax;
            AdjustFutan = adjustFutan;
            SumAdjust = sumAdjust;
        }

        public int TotalPoint { get; private set; }
        public int KanFutan { get; private set; }
        public int TotalSelfExpense { get; private set; }
        public int Tax { get; private set; }
        public int AdjustFutan { get; private set; }
        public int SumAdjust { get; private set; }
    }
}
