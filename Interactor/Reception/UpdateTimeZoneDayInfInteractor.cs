using Domain.Models.TimeZone;
using UseCase.Reception.UpdateTimeZoneDayInf;

namespace Interactor.Reception;

public class UpdateTimeZoneDayInfInteractor : IUpdateTimeZoneDayInfInputPort
{
    private readonly ITimeZoneRepository _timeZoneRepository;

    public UpdateTimeZoneDayInfInteractor(ITimeZoneRepository timeZoneRepository)
    {
        _timeZoneRepository = timeZoneRepository;
    }
    public UpdateTimeZoneDayInfOutputData Handle(UpdateTimeZoneDayInfInputData inputData)
    {
        if (inputData.HpId <= 0)
        {
            return new UpdateTimeZoneDayInfOutputData(UpdateTimeZoneDayInfStatus.InvalidHpId);
        }
        else if (inputData.UserId <= 0)
        {
            return new UpdateTimeZoneDayInfOutputData(UpdateTimeZoneDayInfStatus.InvalidUserId);
        }
        else if (inputData.CurrentTimeKbn <= 0)
        {
            return new UpdateTimeZoneDayInfOutputData(UpdateTimeZoneDayInfStatus.InvalidCurrentTimeKbn);
        }
        else if (inputData.BeforeTimeKbn <= 0)
        {
            return new UpdateTimeZoneDayInfOutputData(UpdateTimeZoneDayInfStatus.InvalidBeforeTimeKbn);
        }
        else if (inputData.UketukeTime <= 0)
        {
            return new UpdateTimeZoneDayInfOutputData(UpdateTimeZoneDayInfStatus.InvalidUketukeTime);
        }
        else if (inputData.SinDate.ToString().Length != 8)
        {
            return new UpdateTimeZoneDayInfOutputData(UpdateTimeZoneDayInfStatus.InvalidSinDate);
        }
        else if (inputData.CurrentTimeKbn == inputData.BeforeTimeKbn)
        {
            return new UpdateTimeZoneDayInfOutputData(UpdateTimeZoneDayInfStatus.CanNotUpdateTimeZoneInf);
        }
        else if (_timeZoneRepository.UpdateTimeZoneDayInf(inputData.HpId, inputData.UserId, inputData.SinDate, inputData.CurrentTimeKbn, inputData.UketukeTime))
        {
            return new UpdateTimeZoneDayInfOutputData(UpdateTimeZoneDayInfStatus.Successed);
        }
        return new UpdateTimeZoneDayInfOutputData(UpdateTimeZoneDayInfStatus.Failed);
    }
}
