using Domain.Models.Diseases;
using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.GetTreeSetByomei
{
    public class GetTreeSetByomeiOutputData : IOutputData
    {

        public IEnumerable<ByomeiSetMstModel> Datas { get; private set; }

        public GetTreeSetByomeiStatus Status { get; private set; }

        public GetTreeSetByomeiOutputData(IEnumerable<ByomeiSetMstModel> datas, GetTreeSetByomeiStatus status)
        {
            Datas = datas;
            Status = status;
        }
    }
}
