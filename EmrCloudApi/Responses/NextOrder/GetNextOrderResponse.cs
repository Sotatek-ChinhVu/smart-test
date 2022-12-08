using Domain.Models.NextOrder;
using UseCase.NextOrder.Get;

namespace EmrCloudApi.Responses.NextOrder;

public class GetNextOrderResponse
{
    public GetNextOrderResponse(List<GroupHokenItem> groupHokenItems, RsvkrtKarteInfModel karteInfModel, List<RsvKrtByomeiItem> byomeiItems, List<string> nextOrderFileItems)
    {
        GroupHokenItems = groupHokenItems;
        KarteInfModel = karteInfModel;
        ByomeiItems = byomeiItems;
        NextOrderFileItems = nextOrderFileItems;
    }

    public List<GroupHokenItem> GroupHokenItems { get; private set; }

    public RsvkrtKarteInfModel KarteInfModel { get; private set; }

    public List<RsvKrtByomeiItem> ByomeiItems { get; private set; }

    public List<string> NextOrderFileItems { get; private set; }
}
