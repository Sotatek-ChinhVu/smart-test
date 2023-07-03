using Domain.Models.Reception;
using Helper.Constants;
using UseCase.SetMst.GetList;

namespace EmrCloudApi.Messages;

public class CommonMessage
{
    public int SinDate { get; set; }
    public long RaiinNo { get; set; } = CommonConstants.InvalidId;
    public long PtId { get; set; } = CommonConstants.InvalidId;
}

public class LockMessage
{
    public int SinDate { get; set; }
    public long RaiinNo { get; set; } = CommonConstants.InvalidId;
    public long PtId { get; set; } = CommonConstants.InvalidId;
    public byte Type { get; set; }
    public string FunctionCod { get; set; } = String.Empty;
}

public class SuperSetMessage
{
    public List<GetSetMstListOutputItem> ReorderSetMstModels { get; set; } = new();
}

public class ReceptionChangedMessage
{
    public ReceptionChangedMessage(List<ReceptionRowModel> receptionInfos, List<SameVisitModel> sameVisitList)
    {
        ReceptionInfos = receptionInfos;
        SameVisitList = sameVisitList;
    }

    public List<ReceptionRowModel> ReceptionInfos { get; set; }

    public List<SameVisitModel> SameVisitList { get; set; }
}