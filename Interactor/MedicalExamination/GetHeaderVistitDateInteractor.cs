using Domain.Models.Reception;
using Domain.Models.UserConf;
using UseCase.MedicalExamination.GetHeaderVistitDate;
using static Helper.Common.UserConfCommon;

namespace Interactor.MedicalExamination;

public class GetHeaderVistitDateInteractor : IGetHeaderVistitDateInputPort
{
    private readonly IUserConfRepository _userConfRepository;
    private readonly IReceptionRepository _receptionRepository;

    public GetHeaderVistitDateInteractor(IUserConfRepository userConfRepository, IReceptionRepository receptionRepository)
    {
        _userConfRepository = userConfRepository;
        _receptionRepository = receptionRepository;
    }

    public GetHeaderVistitDateOutputData Handle(GetHeaderVistitDateInputData inputData)
    {
        try
        {
            var dateTimeFormart = GetVisitingDateFormartSetting(inputData.HpId, inputData.UserId);
            int firstTimeDate = _receptionRepository.GetFirstVisitWithSyosin(inputData.HpId, inputData.PtId, inputData.SinDate);
            int lastTimeDate = _receptionRepository.GetLastVisit(inputData.HpId, inputData.PtId, inputData.SinDate, true)?.SinDate ?? 0;
            return new GetHeaderVistitDateOutputData(dateTimeFormart, firstTimeDate, lastTimeDate, GetHeaderVistitDateStatus.Successed);
        }
        finally
        {
            _userConfRepository.ReleaseResource();
        }
    }

    private DateTimeFormart GetVisitingDateFormartSetting(int hpId, int userId)
    {
        int DateTimeFormartSetting = _userConfRepository.GetSettingValue(hpId, userId, 100012, 0);
        switch (DateTimeFormartSetting)
        {
            case 0:
                return DateTimeFormart.JapaneseCalendar;
            case 1:
                return DateTimeFormart.WesternCalendar;
            case 2:
                return DateTimeFormart.JapAndWestCalendar;
            default:
                return DateTimeFormart.JapaneseCalendar;
        }
    }
}
