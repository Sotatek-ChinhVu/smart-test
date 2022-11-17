using Domain.Models.NextOrder;
using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.Get
{
    public class GetNextOrderOutputData : IOutputData
    {
        public GetNextOrderOutputData(List<GroupHokenItem> groupHokenItems, RsvkrtKarteInfModel karteInf, List<RsvKrtByomeiItem> byomeiItems, GetNextOrderStatus status)
        {
            GroupHokenItems = groupHokenItems;
            KarteInf = karteInf;
            ByomeiItems = byomeiItems;
            Status = status;
        }

        public List<GroupHokenItem> GroupHokenItems { get; private set; }

        public RsvkrtKarteInfModel KarteInf { get; private set; }

        public List<RsvKrtByomeiItem> ByomeiItems { get; private set; }

        public GetNextOrderStatus Status { get; private set; }

    }
}
