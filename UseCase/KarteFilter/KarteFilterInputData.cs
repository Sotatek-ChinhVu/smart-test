using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter;

public class KarteFilterInputData : IInputData<KarteFilterOutputData>
{
    public KarteFilterInputData(int sinDate)
    {
        this.sinDate = sinDate;
    }

    public int sinDate { get; private set; }

}
