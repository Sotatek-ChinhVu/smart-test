namespace Domain.Models.PatientInfor
{
    public class PtInfSanteiConfModel
    {
        public PtInfSanteiConfModel(int kbnNo, int edaNo, int kbnVal, int sortNo, int startDate, int endDate)
        {
            KbnNo = kbnNo;
            EdaNo = edaNo;
            KbnVal = kbnVal;
            SortNo = sortNo;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int KbnNo { get; private set; }
        public int EdaNo { get; private set; }
        public int KbnVal { get; private set; }
        public int SortNo { get; private set; }
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
    }
}
