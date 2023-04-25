using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.GetValidGairaiRiha;

namespace Interactor.MedicalExamination
{
    public class GetValidGairaiRihaInteractor : IGetValidGairaiRihaInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;

        public GetValidGairaiRihaInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public GetValidGairaiRihaOutputData Handle(GetValidGairaiRihaInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetValidGairaiRihaOutputData(new(), GetValidGairaiRihaStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new GetValidGairaiRihaOutputData(new(), GetValidGairaiRihaStatus.InvalidPtId);
                }
                if (inputData.RaiinNo <= 0)
                {
                    return new GetValidGairaiRihaOutputData(new(), GetValidGairaiRihaStatus.InvalidRaiinNo);
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetValidGairaiRihaOutputData(new(), GetValidGairaiRihaStatus.InvalidSinDate);
                }
                if (inputData.SyosaiKbn < 0)
                {
                    return new GetValidGairaiRihaOutputData(new(), GetValidGairaiRihaStatus.InvalidSyosaiKbn);
                }

                var check = _todayOdrRepository.GetValidGairaiRiha(
                        inputData.HpId,
                        inputData.PtId,
                        inputData.RaiinNo,
                        inputData.SinDate,
                        inputData.SyosaiKbn,
                        inputData.AllOdrInfItem
                        );

                return new GetValidGairaiRihaOutputData(check.Select(c => new GairaiRihaItem(c.type, c.itemName, c.lastDaySanteiRiha, c.rihaItemName)).ToList(), GetValidGairaiRihaStatus.Successed);
            }
            catch
            {
                return new GetValidGairaiRihaOutputData(new(), GetValidGairaiRihaStatus.Failed);
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
            }
        }
    }
}
