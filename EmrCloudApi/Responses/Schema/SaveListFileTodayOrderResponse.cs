namespace EmrCloudApi.Responses.Schema;

public class SaveListFileTodayOrderResponse
{
    public SaveListFileTodayOrderResponse(long seqNo)
    {
        SeqNo = seqNo;
    }

    public long SeqNo { get; private set; }
}
