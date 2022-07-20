using UseCase.Core.Sync.Core;

namespace UseCase.KarteInfs.GetLists
{
    public class GetListKarteInfOutputData : IOutputData
    {
        public List<GetListKarteInfOuputItem> KarteInfs { get; private set; }

        public GetListKarteInfStatus Status { get; private set; }

        public GetListKarteInfOutputData(List<GetListKarteInfOuputItem> karteInfs, GetListKarteInfStatus status)
        {
            KarteInfs = karteInfs;
            Status = status;
        }
    }
}
