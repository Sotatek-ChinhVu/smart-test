using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.GetListKarteFilter;

public class GetKarteFilterInputData : IInputData<GetKarteFilterOutputData>
{
    public GetKarteFilterInputData(int sinDate)
    {
        this.sinDate = sinDate;
    }

    public int sinDate { get; private set; }

}
