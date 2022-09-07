using UseCase.Core.Sync.Core;

namespace UseCase.Byomei.DiseaseSearch;

public class DiseaseSearchInputData : IInputData<DiseaseSearchOutputData>
{
    public DiseaseSearchInputData(bool isSyusyoku, string keyword, int pageIndex, int pageCount)
    {
        IsSyusyoku = isSyusyoku;
        Keyword = keyword;
        PageIndex = pageIndex;
        PageCount = pageCount;
    }

    public bool IsByomei { get; private set; }

    public bool IsSyusyoku { get; private set; }

    public bool IsMisaiyou { get; private set; }

    public string Keyword { get; private set; }

    public int PageIndex { get; private set; }

    public int PageCount { get; private set; }

}
