using Domain.Models.NextOrder;
using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.Get
{
    public class GetNextOrderOutputData : IOutputData
    {
        public GetNextOrderOutputData(GetNextOrderStatus status)
        {
            GroupHokenItems = new();
            KarteInf = new();
            ByomeiItems = new();
            NextOrderFiles = new();
            Status = status;
        }

        public GetNextOrderOutputData(List<GroupHokenItem> groupHokenItems, RsvkrtKarteInfModel karteInf, List<RsvKrtByomeiItem> byomeiItems, List<NextOrderFileItem> nextOrderFiles, GetNextOrderStatus status)
        {
            GroupHokenItems = groupHokenItems;
            KarteInf = karteInf;
            ByomeiItems = byomeiItems;
            NextOrderFiles = nextOrderFiles;
            Status = status;
        }

        public List<GroupHokenItem> GroupHokenItems { get; private set; }

        public RsvkrtKarteInfModel KarteInf { get; private set; }

        public List<RsvKrtByomeiItem> ByomeiItems { get; private set; }

        public List<NextOrderFileItem> NextOrderFiles { get; private set; }

        public GetNextOrderStatus Status { get; private set; }

    }
}
