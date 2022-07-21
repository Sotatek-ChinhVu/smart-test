using UseCase.KarteInfs.GetLists;

namespace EmrCloudApi.Tenant.Responses.KarteInfs
{
    public class GetListKarteInfResponse
    {
        public List<GetListKarteInfOuputItem>? KarteInfs { get; set; }
        public GetListKarteInfResponse(List<GetListKarteInfOuputItem>? karteInfs)
        {
            KarteInfs = karteInfs;
        }
    }
}
