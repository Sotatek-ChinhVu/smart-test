using UseCase.KarteInfs.GetLists;

namespace EmrCloudApi.Tenant.Responses.KarteInfs
{
    public class GetListKarteInfResponse
    {
        public List<GetListKarteInfOuputItem>? KarteInfs { get; private set; }
        public GetListKarteInfResponse(List<GetListKarteInfOuputItem>? karteInfs)
        {
            KarteInfs = karteInfs;
        }
    }
}
