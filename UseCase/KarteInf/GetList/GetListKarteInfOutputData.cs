using UseCase.Core.Sync.Core;

namespace UseCase.KarteInf.GetList;

public class GetListKarteInfOutputData : IOutputData
{
    public GetListKarteInfOutputData(List<GetListKarteInfOuputItem> karteInfs, List<KarteFileOutputItem> listKarteFile, bool isKarteExisted, GetListKarteInfStatus status)
    {
        KarteInfs = karteInfs;
        ListKarteFile = listKarteFile;
        IsKarteExisted = isKarteExisted;
        Status = status;
    }

    public GetListKarteInfOutputData(GetListKarteInfStatus status)
    {
        KarteInfs = new();
        ListKarteFile = new();
        IsKarteExisted = new();
        Status = status;
    }

    public List<GetListKarteInfOuputItem> KarteInfs { get; private set; }

    public List<KarteFileOutputItem> ListKarteFile { get; private set; }

    public bool IsKarteExisted { get; private set; }

    public GetListKarteInfStatus Status { get; private set; }


}
