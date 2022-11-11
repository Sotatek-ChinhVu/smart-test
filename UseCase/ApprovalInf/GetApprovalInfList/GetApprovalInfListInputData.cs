using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Async.Core;
using UseCase.Core.Sync.Core;

namespace UseCase.ApprovalInf.GetApprovalInfList;

public class GetApprovalInfListInputData : IInputData<GetApprovalInfListOutputData>
{
    public GetApprovalInfListInputData(int starDate, int endDate, string kaName, string drName)
    {
        StarDate = starDate;
        EndDate = endDate;
        KaName = kaName;
        DrName = drName;
    }

    public int StarDate { get; private set; }
    public int EndDate { get; private set; }
    public string DrName { get; private set; }
    public string KaName { get; private set; }
}
