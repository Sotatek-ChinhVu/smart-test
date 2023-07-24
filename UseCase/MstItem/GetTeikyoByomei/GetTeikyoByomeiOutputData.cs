using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTeikyoByomei;

public class GetTeikyoByomeiOutputData : IOutputData
{
    public GetTeikyoByomeiOutputData(List<TeikyoByomeiModel> teikyoByomeis, GetTeikyoByomeiStatus status)
    {
        TeikyoByomeis = teikyoByomeis;
        Status = status;
    }

    public List<TeikyoByomeiModel> TeikyoByomeis { get; private set; }

    public GetTeikyoByomeiStatus Status { get; private set; }
}
