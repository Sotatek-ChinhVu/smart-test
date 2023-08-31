namespace EmrCloudApi.Responses.MedicalExamination;

public class SaveKensaIraiResponse
{
    public SaveKensaIraiResponse(string message)
    {
        Message = message;
    }

    public string Message { get; private set; }
}
