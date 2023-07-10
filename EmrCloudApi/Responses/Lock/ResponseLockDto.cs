using Domain.Models.Lock;

namespace EmrCloudApi.Responses.Lock;

public class ResponseLockDto
{
    public ResponseLockDto(ResponseLockModel model)
    {
        SinDate = model.SinDate;
        PtId = model.PtId;
        RaiinNo = model.RaiinNo;
        Status = model.Status;
    }

    public int SinDate { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public int Status { get; private set; }
}
