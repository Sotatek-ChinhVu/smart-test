using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem;

public class GetTeikyoByomeiResponse
{
    public GetTeikyoByomeiResponse(List<TeikyoByomeiModel> teikyoByomeis)
    {
        TeikyoByomeis = teikyoByomeis;
    }

    public List<TeikyoByomeiModel> TeikyoByomeis { get; private set; }
}
