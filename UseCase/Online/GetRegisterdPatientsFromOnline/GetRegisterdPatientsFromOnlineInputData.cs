using UseCase.Core.Sync.Core;

namespace UseCase.Online.GetRegisterdPatientsFromOnline;

public class GetRegisterdPatientsFromOnlineInputData : IInputData<GetRegisterdPatientsFromOnlineOutputData>
{
    public GetRegisterdPatientsFromOnlineInputData(int sinDate, int confirmType, int id)
    {
        SinDate = sinDate;
        ConfirmType = confirmType;
        Id = id;
    }

    public int SinDate { get; private set; }

    public int ConfirmType { get; private set; }

    public int Id { get; private set; }
}
