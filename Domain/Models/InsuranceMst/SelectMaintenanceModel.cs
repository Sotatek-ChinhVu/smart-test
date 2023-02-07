namespace Domain.Models.InsuranceMst
{
    public class SelectMaintenanceModel
    {
        public SelectMaintenanceModel(HokenMstModel insurance)
        {
            Insurance = insurance;
        }

        public HokenMstModel Insurance { get; private set; }

        public int StartDate { get => Insurance.StartDate; }

        public string StartDateDisplay
        {
            get
            {
                switch (StartDate)
                {
                    case 0:
                        return "00000000~";
                    default:
                        return StartDate + "~";
                }
            }
        }

        public int EndDate { get => Insurance.EndDate; }

        public int HokenNo { get => Insurance.HokenNo; }

        public int HokenEdaNo { get => Insurance.HokenEdaNo; }
    }
}
