
using UseCase.MedicalExamination.KarteInfs.GetLists;

namespace EmrCloudApi.Tenant.Responses.MedicalExamination.KarteInfs
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
