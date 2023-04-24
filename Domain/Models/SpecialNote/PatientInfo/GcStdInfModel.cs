namespace Domain.Models.SpecialNote.PatientInfo
{
    public class GcStdInfModel
    {
        public GcStdInfModel(int hpId, int stdKbn, int sex, double point, double sdM25, double sdM20, double sdM10, double sdAvg, double sdP10, double sdP20, double sdP25, double per03, double per10, double per25, double per50, double per75, double per90, double per97)
        {
            HpId = hpId;
            StdKbn = stdKbn;
            Sex = sex;
            Point = point;
            SdM25 = sdM25;
            SdM20 = sdM20;
            SdM10 = sdM10;
            SdAvg = sdAvg;
            SdP10 = sdP10;
            SdP20 = sdP20;
            SdP25 = sdP25;
            Per03 = per03;
            Per10 = per10;
            Per25 = per25;
            Per50 = per50;
            Per75 = per75;
            Per90 = per90;
            Per97 = per97;
        }

        public GcStdInfModel()
        {
        }

        public int HpId { get; private set; }

        public int StdKbn { get; private set; }

        public int Sex { get; private set; }

        public double Point { get; private set; }

        public double SdM25 { get; private set; }

        public double SdM20 { get; private set; }

        public double SdM10 { get; private set; }

        public double SdAvg { get; private set; }

        public double SdP10 { get; private set; }

        public double SdP20 { get; private set; }

        public double SdP25 { get; private set; }

        public double Per03 { get; private set; }

        public double Per10 { get; private set; }

        public double Per25 { get; private set; }

        public double Per50 { get; private set; }

        public double Per75 { get; private set; }

        public double Per90 { get; private set; }

        public double Per97 { get; private set; }
    }
}
