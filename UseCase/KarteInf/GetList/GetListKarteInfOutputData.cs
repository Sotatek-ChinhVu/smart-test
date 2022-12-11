using UseCase.Core.Sync.Core;

namespace UseCase.KarteInf.GetList;

public class GetListKarteInfOutputData : IOutputData
{
    public GetListKarteInfOutputData(List<GetListKarteInfOuputItem> karteInfs, List<string> listKarteFile, GetListKarteInfStatus status)
    {
        KarteInfs = karteInfs;
        ListKarteFile = listKarteFile;
        Status = status;
    }

    public GetListKarteInfOutputData(GetListKarteInfStatus status)
    {
        KarteInfs = new();
        ListKarteFile = new();
        Status = status;
    }

    public List<GetListKarteInfOuputItem> KarteInfs { get; private set; }

    public List<string> ListKarteFile { get; private set; }

    public GetListKarteInfStatus Status { get; private set; }


}
