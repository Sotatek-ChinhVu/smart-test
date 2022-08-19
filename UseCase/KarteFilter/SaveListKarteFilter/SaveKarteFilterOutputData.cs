using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.SaveListKarteFilter;

public class SaveKarteFilterOutputData : IOutputData
{
    public SaveKarteFilterStatus Status { get; private set; }

    public SaveKarteFilterOutputData(SaveKarteFilterStatus status)
    {
        Status = status;
    }
}
