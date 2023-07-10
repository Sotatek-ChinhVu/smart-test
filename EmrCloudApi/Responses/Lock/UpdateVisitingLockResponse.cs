namespace EmrCloudApi.Responses.Lock;

public class UpdateVisitingLockResponse
{
    public UpdateVisitingLockResponse(List<ResponseLockDto> data)
    {
        Data = data;
    }

    public List<ResponseLockDto> Data { get; private set; }
}
