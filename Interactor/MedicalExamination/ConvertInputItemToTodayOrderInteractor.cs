using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.ConvertInputItemToTodayOdr;

namespace Interactor.MedicalExamination
{
    public class ConvertInputItemToTodayOrderInteractor : IConvertInputItemToTodayOrdInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;

        public ConvertInputItemToTodayOrderInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public ConvertInputItemToTodayOrdOutputData Handle(ConvertInputItemToTodayOrdInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new ConvertInputItemToTodayOrdOutputData(ConvertInputItemToTodayOrdStatus.InvalidHpId, new());
                }
                if (inputData.SinDate <= 0)
                {
                    return new ConvertInputItemToTodayOrdOutputData(ConvertInputItemToTodayOrdStatus.InvalidSinDate, new());
                }
                if (inputData.DetailInfs.Count == 0)
                {
                    return new ConvertInputItemToTodayOrdOutputData(ConvertInputItemToTodayOrdStatus.InvalidDetailInfs, new());
                }

                var result = _todayOdrRepository.ConvertInputItemToTodayOdr(inputData.HpId, inputData.SinDate, inputData.DetailInfs);

                return new ConvertInputItemToTodayOrdOutputData(ConvertInputItemToTodayOrdStatus.Successed, result);
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
            }
        }
    }
}
