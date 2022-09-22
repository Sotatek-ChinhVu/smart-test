using Domain.Models.LockInf;
using Domain.Models.Reception;

namespace EmrCloudApi.Tenant.Responses.VisitingList
{
    public class GetReceptionLockRespone
    {
        public GetReceptionLockRespone(List<ReceptionLockModel> receptionLockModels)
        {
            ReceptionLockModels = receptionLockModels;
        }

        public List<ReceptionLockModel> ReceptionLockModels { get; private set; }
    }
}
