using Domain.Models.TodayOdr;
using UseCase.LastDayInformation.GetLastDayInfoList;

namespace Interactor.LastDayInformation;

public class GetLastDayInfoListInteractor : IGetLastDayInfoListInputPort
{
    private readonly ITodayOdrRepository _todayOdrRepository;

    public GetLastDayInfoListInteractor(ITodayOdrRepository todayOdrRepository)
    {
        _todayOdrRepository = todayOdrRepository;
    }

    public GetLastDayInfoListOutputData Handle(GetLastDayInfoListInputData inputData)
    {
        try
        {
            var result = _todayOdrRepository.GetLastDayInfoList(inputData.HpId, inputData.PtId, inputData.SinDate);
            return new GetLastDayInfoListOutputData(result, GetLastDayInfoListStatus.Successed);
        }
        finally
        {
            _todayOdrRepository.ReleaseResource();
        }
    }
}
