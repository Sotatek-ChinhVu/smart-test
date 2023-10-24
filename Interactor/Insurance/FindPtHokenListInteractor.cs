using Domain.Models.Insurance;
using UseCase.Insurance.FindPtHokenList;

namespace Interactor.Insurance
{
    public class FindPtHokenListInteractor : IFindPtHokenListInputPort
    {
        private readonly IInsuranceRepository _insuranceResponsitory;
        public FindPtHokenListInteractor(IInsuranceRepository insuranceResponsitory)
        {
            _insuranceResponsitory = insuranceResponsitory;
        }

        public FindPtHokenListOutputData Handle(FindPtHokenListInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                {
                    return new FindPtHokenListOutputData(new(), FindPtHokenListStatus.InvalidHpId);
                }
                if (inputData.PtId < 0)
                {
                    return new FindPtHokenListOutputData(new(), FindPtHokenListStatus.InvalidPtId);
                }
                if (inputData.SinDate < 0)
                {
                    return new FindPtHokenListOutputData(new(), FindPtHokenListStatus.InvalidSinDate);
                }

                var data = _insuranceResponsitory.FindPtHokenList(inputData.HpId, inputData.PtId, inputData.SinDate);
                return new FindPtHokenListOutputData(data, FindPtHokenListStatus.Successed);
            }
            finally
            {
                _insuranceResponsitory.ReleaseResource();
            }
        }
    }
}