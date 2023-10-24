namespace EmrCloudApi.Responses.Lock;

public class UpdateVisitingLockResponse
{
    public UpdateVisitingLockResponse(List<ResponseLockDto> data, int removedCount)
    {
        Data = data;
        RemovedCount = removedCount;
    }

    public List<ResponseLockDto> Data { get; private set; }

    public int RemovedCount { get; private set; }
}
