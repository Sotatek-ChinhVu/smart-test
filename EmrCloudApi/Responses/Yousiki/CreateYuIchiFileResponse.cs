namespace EmrCloudApi.Responses.Yousiki;

public class CreateYuIchiFileResponse
{
    public CreateYuIchiFileResponse(string messageType, string confirmMessage)
    {
        MessageType = messageType;
        ConfirmMessage = confirmMessage;
    }

    public string MessageType { get; private set; }

    public string ConfirmMessage { get; private set; }
}
