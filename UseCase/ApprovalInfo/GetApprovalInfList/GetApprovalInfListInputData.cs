using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Async.Core;
using UseCase.Core.Sync.Core;

namespace UseCase.ApprovalInfo.GetApprovalInfList;

public class GetApprovalInfListInputData : IInputData<GetApprovalInfListOutputData>
{
    public GetApprovalInfListInputData(int hpId, int starDate, int endDate, int kaId, int tantoId)
    {
        HpId = hpId;
        StarDate = starDate;
        EndDate = endDate;
        KaId = kaId;
        TantoId = tantoId;
    }
    public int HpId { get; private set; }
    public int StarDate { get; private set; }
    public int EndDate { get; private set; }
    public int KaId { get; private set; }
    public int TantoId { get; private set; }
}
