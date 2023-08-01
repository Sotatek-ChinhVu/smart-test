using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetListKaikeiInf;

public class GetListKaikeiInfOutputData : IOutputData
{
    public GetListKaikeiInfOutputData(List<PtHokenInfKaikeiModel> ptHokenInfKaikeiList, GetListKaikeiInfStatus status)
    {
        PtHokenInfKaikeiList = ptHokenInfKaikeiList.Select(item => new PtHokenInfKaikeiItem(item)).ToList();
        Status = status;
    }

    public List<PtHokenInfKaikeiItem> PtHokenInfKaikeiList { get; private set; }

    public GetListKaikeiInfStatus Status { get; private set; }
}
