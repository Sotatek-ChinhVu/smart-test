namespace EmrCloudApi.Requests.PatientInfor
{
    public class CheckValidSamePatientRequest
    {
        public CheckValidSamePatientRequest(long ptId, string kanjiName, int birthday, int sex)
        {
            PtId = ptId;
            KanjiName = kanjiName;
            Birthday = birthday;
            Sex = sex;
        }

        public long PtId { get; private set; }

        public string KanjiName { get; private set; }

        public int Birthday { get; private set; }

        public int Sex { get; private set; }
    }
}
