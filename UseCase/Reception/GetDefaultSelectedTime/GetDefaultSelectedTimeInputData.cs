using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetDefaultSelectedTime;

public class GetDefaultSelectedTimeInputData : IInputData<GetDefaultSelectedTimeOutputData>
{
    public GetDefaultSelectedTimeInputData(int hpId, int sinDate, int birthDay)
    {
        HpId = hpId;
        SinDate = sinDate;
        BirthDay = birthDay;
    }

    public int HpId { get; private set; }

    public int SinDate { get; private set; }

    public int BirthDay { get; private set; }
}
