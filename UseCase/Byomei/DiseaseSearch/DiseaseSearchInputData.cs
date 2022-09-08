using UseCase.Core.Sync.Core;

namespace UseCase.Byomei.DiseaseSearch;

public class DiseaseSearchInputData : IInputData<DiseaseSearchOutputData>
{
    public DiseaseSearchInputData(bool isPrefix, bool isByomei, bool isSuffix, string keyword, int pageIndex, int pageCount)
    {
        IsPrefix = isPrefix;
        IsByomei = isByomei;
        IsSuffix = isSuffix;
        Keyword = keyword;
        PageIndex = pageIndex;
        PageCount = pageCount;
    }

    public bool IsPrefix { get; private set; }

    public bool IsByomei { get; private set; }

    public bool IsSuffix { get; private set; }

    public string Keyword { get; private set; }

    public int PageIndex { get; private set; }

    public int PageCount { get; private set; }
}