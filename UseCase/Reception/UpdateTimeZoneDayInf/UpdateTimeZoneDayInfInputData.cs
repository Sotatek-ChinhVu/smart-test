using UseCase.Core.Sync.Core;

namespace UseCase.Reception.UpdateTimeZoneDayInf;

public class UpdateTimeZoneDayInfInputData : IInputData<UpdateTimeZoneDayInfOutputData>
{
    public UpdateTimeZoneDayInfInputData(int hpId, int userId, int sinDate, int currentTimeKbn, int beforeTimeKbn, int uketukeTime)
    {
        HpId = hpId;
        UserId = userId;
        SinDate = sinDate;
        CurrentTimeKbn = currentTimeKbn;
        BeforeTimeKbn = beforeTimeKbn;
        UketukeTime = uketukeTime;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int SinDate { get; private set; }

    public int CurrentTimeKbn { get; private set; }

    public int BeforeTimeKbn { get; private set; }

    public int UketukeTime { get; private set; }
}
