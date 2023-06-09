using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.CheckValidSamePatient
{
    public class CheckValidSamePatientInputData : IInputData<CheckValidSamePatientOutputData>
    {
        public CheckValidSamePatientInputData(int hpId, long ptId, string kanjiName, int birthday, int sex)
        {
            HpId = hpId;
            PtId = ptId;
            KanjiName = kanjiName;
            Birthday = birthday;
            Sex = sex;
        }

        public int HpId { get;private set; }

        public long PtId { get; private set; }

        public string KanjiName { get; private set; }

        public int Birthday { get; private set; }

        public int Sex { get; private set; }
    }
}
