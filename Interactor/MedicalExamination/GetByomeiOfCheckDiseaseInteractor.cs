using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.GetByomeiFollowItemCd;

namespace Interactor.MedicalExamination
{
    public class GetByomeiOfCheckDiseaseInteractor : IGetByomeiFollowItemCdInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;
        public GetByomeiOfCheckDiseaseInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public GetByomeiFollowItemCdOutputData Handle(GetByomeiFollowItemCdInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetByomeiFollowItemCdOutputData(new(), GetByomeiFollowItemCdStatus.InvalidHpId);
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetByomeiFollowItemCdOutputData(new(), GetByomeiFollowItemCdStatus.InvalidSinDate);
                }
                if (inputData.ItemCd.Length > 10)
                {
                    return new GetByomeiFollowItemCdOutputData(new(), GetByomeiFollowItemCdStatus.InvalidItemCd);
                }
                if (inputData.TodayByomeis.Count == 0)
                {
                    return new GetByomeiFollowItemCdOutputData(new(), GetByomeiFollowItemCdStatus.InvalidByomeis);
                }


                var result = _todayOdrRepository.GetByomeisOfCheckDiseases(inputData.IsGridStyle, inputData.HpId, inputData.ItemCd, inputData.SinDate, inputData.TodayByomeis);

                if (!(result?.Count > 0))
                {
                    return new GetByomeiFollowItemCdOutputData(new(), GetByomeiFollowItemCdStatus.NoData);
                }

                return new GetByomeiFollowItemCdOutputData(result, GetByomeiFollowItemCdStatus.Successed);
            }
            catch
            {
                return new GetByomeiFollowItemCdOutputData(new(), GetByomeiFollowItemCdStatus.Failed);
            }
        }
    }
}
