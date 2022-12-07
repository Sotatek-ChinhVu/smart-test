using UseCase.KarteInfs.GetLists;

namespace EmrCloudApi.Responses.KarteInf
{
    public class GetListKarteInfResponse
    {
        public List<KarteInfDto> KarteInfs { get; private set; }

        public List<string> ListKarteFile { get; private set; }

        public GetListKarteInfResponse(List<GetListKarteInfOuputItem> karteInfs, List<string> listKarteFile)
        {
            KarteInfs = karteInfs.Select(item => new KarteInfDto(item)).ToList();
            ListKarteFile = listKarteFile;
        }
    }
}
