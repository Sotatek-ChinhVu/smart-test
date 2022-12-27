using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinFilterMst.GetListRaiinInf;

public class GetListRaiinInfFilterInputData : IInputData<GetListRaiinInfFilterOutputData>
{
    public GetListRaiinInfFilterInputData(int hpId, long ptId)
    {
        HpId = hpId;
        PtId = ptId;
    }
    public int HpId { get; private set; }
    public long PtId { get; private set; }
}
