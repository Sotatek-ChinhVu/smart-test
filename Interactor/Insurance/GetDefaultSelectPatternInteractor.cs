using Domain.Models.Insurance;
using Infrastructure.Repositories;
using UseCase.Insurance.GetDefaultSelectPattern;
using UseCase.User.UpsertList;

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

                if(inputData.HistoryPids.Any(h => h< 0))
                {
                    return new GetDefaultSelectPatternOutputData(new(), GetDefaultSelectPatternStatus.InvalidHistoryPid);
                }

                if (!_insuranceResponsitory.CheckExistHokenPids(inputData.HpId, inputData.HistoryPids))
                {
                    return new GetDefaultSelectPatternOutputData(new(), GetDefaultSelectPatternStatus.HokenPidInvalidNoExisted);
                }

                if (inputData.SelectedHokenPid < 0)
                {
                    return new GetDefaultSelectPatternOutputData(new(), GetDefaultSelectPatternStatus.InvalidSelectedHokenPid);
                }

                var data = _insuranceResponsitory.GetListHistoryPid(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.HistoryPids, inputData.SelectedHokenPid);

                return new GetDefaultSelectPatternOutputData(data.Select(d => new GetDefaultSelectPatternItem(d.Item1, d.Item2)).ToList(), GetDefaultSelectPatternStatus.Successed);
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