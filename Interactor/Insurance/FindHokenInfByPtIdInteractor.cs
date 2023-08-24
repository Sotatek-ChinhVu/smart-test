using Domain.Models.Insurance;
using UseCase.Insurance.FindHokenInfByPtId;

namespace Interactor.Insurance;

public class FindHokenInfByPtIdInteractor : IFindHokenInfByPtIdInputPort
{
    private readonly IInsuranceRepository _insuranceResponsitory;

    public FindHokenInfByPtIdInteractor(IInsuranceRepository insuranceResponsitory)
    {
        _insuranceResponsitory = insuranceResponsitory;
    }

    public FindHokenInfByPtIdOutputData Handle(FindHokenInfByPtIdInputData inputData)
    {
        try
        {
            var result = _insuranceResponsitory.FindHokenInfByPtId(inputData.HpId, inputData.PtId);
            return new FindHokenInfByPtIdOutputData(result, FindHokenInfByPtIdStatus.Successed);
        }
        finally
        {
            _insuranceResponsitory.ReleaseResource();
        }
    }
}
