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

        public int TotalPoint { get; set; }
        public int KanFutan { get; set; }
        public int TotalSelfExpense { get; set; }
        public int Tax { get; set; }
        public int AdjustFutan { get; set; }
        public int SumAdjust { get; set; }
    }
}
