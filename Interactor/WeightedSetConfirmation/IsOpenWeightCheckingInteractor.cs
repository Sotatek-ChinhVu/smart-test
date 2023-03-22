using Domain.Models.SystemConf;
using Helper.Common;
using Helper.Extension;
using UseCase.WeightedSetConfirmation.CheckOpen;

namespace Interactor.WeightedSetConfirmation
{
    public class IsOpenWeightCheckingInteractor : IIsOpenWeightCheckingInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;

        public IsOpenWeightCheckingInteractor(ISystemConfRepository systemConfRepository) => _systemConfRepository = systemConfRepository;

        public IsOpenWeightCheckingOutputData Handle(IsOpenWeightCheckingInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new IsOpenWeightCheckingOutputData(IsOpenWeightCheckingStatus.InvalidHpId, false);

                int weightConfirmDays = _systemConfRepository.GetSettingParams(2010, 0, inputData.HpId).AsInteger();
                if (inputData.LastWeight == 0 || inputData.LastDate == 0) return new IsOpenWeightCheckingOutputData(IsOpenWeightCheckingStatus.Successful, true);
                if (weightConfirmDays == 0) return new IsOpenWeightCheckingOutputData(IsOpenWeightCheckingStatus.Successful, false);

                int weightCheckType = _systemConfRepository.GetSettingValue(2010, 0, inputData.HpId).AsInteger();

                if (weightCheckType == 1)
                {
                    if (inputData.SinDate >= CIUtil.DateTimeToInt(CIUtil.IntToDate(inputData.LastDate).AddDays(weightConfirmDays)))
                    {
                        return new IsOpenWeightCheckingOutputData(IsOpenWeightCheckingStatus.Successful, true);
                    }
                }
                else if (weightCheckType == 2)
                {
                    if (inputData.SinDate / 100 >= CIUtil.DateTimeToInt(CIUtil.IntToDate(inputData.LastDate).AddMonths(weightConfirmDays)) / 100)
                    {
                        return new IsOpenWeightCheckingOutputData(IsOpenWeightCheckingStatus.Successful, true);
                    }
                }
                return new IsOpenWeightCheckingOutputData(IsOpenWeightCheckingStatus.Successful, false);
            }
            finally
            {
                _systemConfRepository.ReleaseResource();
            }
        }
    }
}
