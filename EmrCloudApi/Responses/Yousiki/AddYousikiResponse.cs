namespace EmrCloudApi.Responses.Yousiki;

public class AddYousikiResponse
{
    public AddYousikiResponse(string messageType)
    {
        MessageType = messageType;
    }

    public string MessageType { get; private set; }
}
