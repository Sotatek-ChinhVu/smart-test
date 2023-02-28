using Domain.Models.Diseases;
using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.GetSetByomeiTree
{
    public class GetSetByomeiTreeOutputData : IOutputData
    {

        public IEnumerable<ByomeiSetMstModel> Datas { get; private set; }

        public GetSetByomeiTreeStatus Status { get; private set; }

        public GetSetByomeiTreeOutputData(IEnumerable<ByomeiSetMstModel> datas, GetSetByomeiTreeStatus status)
        {
            Datas = datas;
            Status = status;
        }
    }
}
