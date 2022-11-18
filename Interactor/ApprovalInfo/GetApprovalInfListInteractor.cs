using Domain.Models.ApprovalInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.ApprovalInfo.GetApprovalInfList;
using UseCase.Core.Sync.Core;

namespace Interactor.ApprovalInfo;

public class GetApprovalInfListInteractor : IGetApprovalInfListInputPort
{
    private readonly IApprovalInfRepository _approvalInfRepository;
    public GetApprovalInfListInteractor(IApprovalInfRepository approvalInfRepository)
    {
        _approvalInfRepository = approvalInfRepository;
    }
    public GetApprovalInfListOutputData Handle(GetApprovalInfListInputData input)
    {
        try
        {
            if (input.StarDate <= 0)
            {
                return new GetApprovalInfListOutputData(GetApprovalInfListStatus.InvalidStarDate);
            }

            if (input.EndDate <= 0)
            {
                return new GetApprovalInfListOutputData(GetApprovalInfListStatus.InvalidEndDate);
            }

            if (input.KaId <= 0)
            {
                return new GetApprovalInfListOutputData(GetApprovalInfListStatus.InvalidKaId);
            }

            if (input.TantoId <= 0)
            {
                return new GetApprovalInfListOutputData(GetApprovalInfListStatus.InvalidTantoId);
            }

            var ApprovalInf = _approvalInfRepository.GetList(input.HpId, input.StarDate, input.EndDate, input.KaId, input.TantoId);
            return new GetApprovalInfListOutputData(GetApprovalInfListStatus.Success, ApprovalInf);
        }
        catch
        {
            return new GetApprovalInfListOutputData(GetApprovalInfListStatus.Failed);
        }
        
    }

}
