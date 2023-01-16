using Domain.Models.Insurance;
using UseCase.Insurance.HokenPatternUsed;

namespace Interactor.Insurance
{
    public class HokenPatternUsedInteractor : IHokenPatternUsedInputPort
    {

        private readonly IInsuranceRepository _insuranceRepository;

        public HokenPatternUsedInteractor(IInsuranceRepository insuranceRepository)
        {
            _insuranceRepository = insuranceRepository;
        }

        public HokenPatternUsedOutputData Handle(HokenPatternUsedInputData inputData)
        {
            try
            {
                if(inputData.HpId <= 0)
                    return new HokenPatternUsedOutputData(false, HokenPatternUsedStatus.InvalidHpId);

                if (inputData.PtId <= 0)
                    return new HokenPatternUsedOutputData(false, HokenPatternUsedStatus.InvalidPtId);

                if (inputData.HokenPid <= 0)
                    return new HokenPatternUsedOutputData(false, HokenPatternUsedStatus.InvalidHokenPid);

                return new HokenPatternUsedOutputData(_insuranceRepository.CheckHokenPatternUsed(inputData.HpId, inputData.PtId, inputData.HokenPid), HokenPatternUsedStatus.Successful);
            }
            catch
            {
                return new HokenPatternUsedOutputData(false, HokenPatternUsedStatus.Exception);
            }
            finally
            {
                _insuranceRepository.ReleaseResource();
            }
        }
    }
}
