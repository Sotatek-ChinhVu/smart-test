namespace UseCase.PatientInfor.GetPatientInfoBetweenTimesList;

public class PatientInfoOutputItem
{
    public PatientInfoOutputItem(long ptId, long ptNum, string kanaName, string name, int sex)
    {
        PtId = ptId;
        PtNum = ptNum;
        KanaName = kanaName;
        Name = name;
        Sex = sex;
    }

    public long PtId { get; private set; }

    public long PtNum { get; private set; }

    public string KanaName { get; private set; }

    public string Name { get; private set; }

    public int Sex { get; private set; }
}
