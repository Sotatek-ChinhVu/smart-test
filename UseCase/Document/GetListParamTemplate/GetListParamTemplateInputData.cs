using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetListParamTemplate;

public class GetListParamTemplateInputData : IInputData<GetListParamTemplateOutputData>
{
    public GetListParamTemplateInputData(int hpId, int userId, long ptId, int sinDate, long raiinNo, int hokenPId)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        HokenPId = hokenPId;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int HokenPId { get; private set; }
}
