﻿using Domain.Models.NextOrder;
using UseCase.NextOrder.Get;

namespace EmrCloudApi.Responses.NextOrder
{
    public class GetNextOrderResponse
    {
        public GetNextOrderResponse(List<GroupHokenItem> groupHokenItems, RsvkrtKarteInfModel karteInfModel, List<RsvKrtByomeiItem> byomeiItems)
        {
            GroupHokenItems = groupHokenItems;
            KarteInfModel = karteInfModel;
            ByomeiItems = byomeiItems;
        }

    public List<GroupHokenItem> GroupHokenItems { get; private set; }

    public RsvkrtKarteInfModel KarteInfModel { get; private set; }

    public List<RsvKrtByomeiItem> ByomeiItems { get; private set; }

    public List<NextOrderFileInfItem> NextOrderFileItems { get; private set; }
}
