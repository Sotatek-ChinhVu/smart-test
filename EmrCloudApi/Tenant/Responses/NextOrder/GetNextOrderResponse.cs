using Domain.Models.KarteInfs;
using Domain.Models.OrdInfs;
using UseCase.NextOrder.Get;

namespace EmrCloudApi.Tenant.Responses.NextOrder
{
    public class GetNextOrderResponse
    {
        public GetNextOrderResponse(List<GroupHokenItem> groupHokenItems, KarteInfModel karteInfModel, List<RsvKrtByomeiItem> byomeiItems)
        {
            GroupHokenItems = groupHokenItems;
            KarteInfModel = karteInfModel;
            ByomeiItems = byomeiItems;
        }

        public List<GroupHokenItem> GroupHokenItems { get; private set; }

        public KarteInfModel KarteInfModel { get; private set; }

        public List<RsvKrtByomeiItem> ByomeiItems { get; private set; }
    }
}
