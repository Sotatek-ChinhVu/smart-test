using Newtonsoft.Json;

namespace UseCase.MedicalExamination.SaveMedical
{
    public class MedicalStateChanged
    {
        public MedicalStateChanged() { }

        [JsonConstructor]
        public MedicalStateChanged(bool fromRece, bool odrDrugInChanged, bool odrOrSyosaisinChanged, bool todayKarteChanged, bool nextOdrChanged, bool periodicOdrChanged)
        {
            FromRece = fromRece;
            OdrDrugInChanged = odrDrugInChanged;
            OdrOrSyosaisinChanged = odrOrSyosaisinChanged;
            TodayKarteChanged = todayKarteChanged;
            NextOdrChanged = nextOdrChanged;
            PeriodicOdrChanged = periodicOdrChanged;
        }

        public bool FromRece { get; private set; }
        public bool OdrDrugInChanged { get; private set; }
        public bool OdrOrSyosaisinChanged { get; private set; }
        public bool TodayKarteChanged { get; private set; }
        public bool NextOdrChanged { get; private set; }
        public bool PeriodicOdrChanged { get; private set; }
    }
}
