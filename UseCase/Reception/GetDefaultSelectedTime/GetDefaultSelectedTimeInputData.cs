using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetDefaultSelectedTime;

public class GetDefaultSelectedTimeInputData : IInputData<GetDefaultSelectedTimeOutputData>
{
    public GetDefaultSelectedTimeInputData(int hpId, int uketukeTime, int sinDate, int birthDay)
    {
        HpId = hpId;
        UketukeTime = uketukeTime;
        SinDate = sinDate;
        BirthDay = birthDay;
    }

    public int HpId { get; private set; }

    public int UketukeTime { get; private set; }

    public int SinDate { get; private set; }

    public int BirthDay { get; private set; }
}
