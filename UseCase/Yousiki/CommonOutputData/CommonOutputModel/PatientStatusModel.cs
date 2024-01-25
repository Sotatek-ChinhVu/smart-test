namespace UseCase.Yousiki.CommonOutputData.CommonOutputModel;

public class PatientStatusModel
{
    public PatientStatusModel(string statusLabel, int statusValue)
    {
        StatusLabel = statusLabel;
        StatusValue = statusValue;
    }

    public string StatusLabel { get; private set; }

    public int StatusValue { get; private set; }
}
