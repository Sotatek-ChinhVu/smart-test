using Domain.Models.TimeZone;
using UseCase.Reception.GetDefaultSelectedTime;

namespace Interactor.Reception;

public class GetDefaultSelectedTimeInteractor : IGetDefaultSelectedTimeInputPort
{
    private readonly ITimeZoneRepository _timeZoneRepository;

    public GetDefaultSelectedTimeInteractor(ITimeZoneRepository timeZoneRepository)
    {
        _timeZoneRepository = timeZoneRepository;
    }

    public GetDefaultSelectedTimeOutputData Handle(GetDefaultSelectedTimeInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.InvalidHpId);
            }
            else if (inputData.SinDate.ToString().Length != 8)
            {
                return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.InvalidSinDate);
            }
            else if (inputData.BirthDay.ToString().Length != 8)
            {
                return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.InvalidBirthDay);
            }
            var result = _timeZoneRepository.GetDefaultSelectedTime(inputData.HpId, inputData.SinDate, inputData.BirthDay);
            return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.Successed, result);
        }
        catch
        {
            return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.Failed);
        }
    }
}
