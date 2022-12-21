using Domain.Models.Insurance;
using UseCase.Insurance.GetDefaultSelectPattern;

namespace Interactor.Insurance
{
    public class GetDefaultSelectPatternInteractor : IGetDefaultSelectPatternInputPort
    {
        private readonly IInsuranceRepository _insuranceResponsitory;
        public GetDefaultSelectPatternInteractor(IInsuranceRepository insuranceResponsitory)
        {
            _insuranceResponsitory = insuranceResponsitory;
        }

        public GetDefaultSelectPatternOutputData Handle(GetDefaultSelectPatternInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                {
                    return new GetDefaultSelectPatternOutputData(new(), GetDefaultSelectPatternStatus.InvalidHpId);
                }


                if (inputData.PtId < 0)
                {
                    return new GetDefaultSelectPatternOutputData(new(), GetDefaultSelectPatternStatus.InvalidPtId);
                }


                if (inputData.SinDate < 0)
                {
                    return new GetDefaultSelectPatternOutputData(new(), GetDefaultSelectPatternStatus.InvalidSinDate);
                }

                if (inputData.HistoryPid < 0)
                {
                    return new GetDefaultSelectPatternOutputData(new(), GetDefaultSelectPatternStatus.InvalidHistoryPid);
                }

                if (inputData.SelectedHokenPid < 0)
                {
                    return new GetDefaultSelectPatternOutputData(new(), GetDefaultSelectPatternStatus.InvalidSelectedHokenPid);
                }

                var data = _insuranceResponsitory.GetDefaultSelectPattern(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.HistoryPid, inputData.SelectedHokenPid);

                return new GetDefaultSelectPatternOutputData(data, GetDefaultSelectPatternStatus.Successed);
            }
            catch
            {
                return new GetDefaultSelectPatternOutputData(new(), GetDefaultSelectPatternStatus.Failed);
            }
            finally
            {
                _insuranceResponsitory.ReleaseResource();
            }
        }
    }
}