using UseCase.KarteInf.GetList;

namespace EmrCloudApi.Responses.KarteInf
{
    public class GetListKarteInfResponse
    {
        public List<KarteInfDto> KarteInfs { get; private set; }

        public List<KarteFileOutputItem> ListKarteFile { get; private set; }

        public GetListKarteInfResponse(List<GetListKarteInfOuputItem> karteInfs, List<KarteFileOutputItem> listKarteFile)
        {
            KarteInfs = karteInfs.Select(item => new KarteInfDto(item)).ToList();
            ListKarteFile = listKarteFile;
        }
    }
}
