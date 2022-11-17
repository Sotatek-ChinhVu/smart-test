using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.DiseaseSearch;

public class DiseaseSearchInputData : IInputData<DiseaseSearchOutputData>
{
    public DiseaseSearchInputData(bool isPrefix, bool isByomei, bool isSuffix, bool isMisaiyou, int sindate, string keyword, int pageIndex, int pageSize)
    {
        IsPrefix = isPrefix;
        IsByomei = isByomei;
        IsSuffix = isSuffix;
        IsMisaiyou = isMisaiyou;
        Sindate = sindate;
        Keyword = keyword;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public bool IsPrefix { get; private set; }

    public bool IsByomei { get; private set; }

    public bool IsSuffix { get; private set; }

    public bool IsMisaiyou { get; private set; }

    public int Sindate { get; private set; }

    public string Keyword { get; private set; }

    public int PageIndex { get; private set; }

    public int PageSize { get; private set; }
}