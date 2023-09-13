using Domain.Models.Diseases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.Diseases.GetSetByomeiTree;

namespace UseCase.Diseases.GetTreeByomeiSet
{
    public sealed class GetTreeByomeiSetOutputData : IOutputData
    {
        public IEnumerable<ByomeiSetMstItem> Datas { get; private set; }

        public GetTreeByomeiSetStatus Status { get; private set; }

        public GetTreeByomeiSetOutputData(IEnumerable<ByomeiSetMstItem> datas, GetTreeByomeiSetStatus status)
        {
            Datas = datas;
            Status = status;
        }
    }
}
