namespace EmrCloudApi.Responses.Receipt;

public class GetReceHenReasonResponse
{
    public GetReceHenReasonResponse(string receReasonCmt)
    {
        ReceReasonCmt = receReasonCmt;
    }

    public string ReceReasonCmt { get; private set; }
}
