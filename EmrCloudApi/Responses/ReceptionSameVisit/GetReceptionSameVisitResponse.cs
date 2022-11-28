using Domain.Models.ReceptionSameVisit;

namespace EmrCloudApi.Responses.ReceptionSameVisit
{
    public class GetReceptionSameVisitResponse
    {
        public GetReceptionSameVisitResponse(List<ReceptionSameVisitModel> listSameVisit)
        {
            ListSameVisit = listSameVisit;
        }

        public List<ReceptionSameVisitModel> ListSameVisit { get; private set; }
    }
}
