using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.GetValidJihiYobo;

namespace Interactor.MedicalExamination
{
    public class GetValidJihiYoboInteractor : IGetValidJihiYoboInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;

        public GetValidJihiYoboInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public GetValidJihiYoboOutputData Handle(GetValidJihiYoboInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetValidJihiYoboOutputData(0, false, GetValidJihiYoboStatus.InvalidHpId);
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetValidJihiYoboOutputData(0, false, GetValidJihiYoboStatus.InvalidSinDate);
                }
                if (inputData.SyosaiKbn < 0)
                {
                    return new GetValidJihiYoboOutputData(0, false, GetValidJihiYoboStatus.InvalidSyosaiKbn);
                }

                var check = _todayOdrRepository.GetValidJihiYobo(inputData.HpId, inputData.SinDate, inputData.SyosaiKbn, inputData.ItemCds);

                return new GetValidJihiYoboOutputData(check.systemSetting, check.isExistYoboItemOnly, GetValidJihiYoboStatus.Successed);
            }
            catch
            {
                return new GetValidJihiYoboOutputData(0, false, GetValidJihiYoboStatus.Failed);
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
            }
        }
    }
}
