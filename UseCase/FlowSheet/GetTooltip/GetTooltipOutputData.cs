using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.GetTooltip
{
    public class GetTooltipOutputData :IOutputData
    {
        public GetTooltipOutputData(List<(int, string)> values)
        {
            Values = values;
        }

        public List<(int, string)> Values { get; private set; }
    }
}
