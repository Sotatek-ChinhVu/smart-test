using EmrCloudApi.Tenant.Responses.OrdInf;
using UseCase.OrdInfs.GetListTrees;
using static Helper.Constants.OrderInfConst;

namespace EmrCloudApi.Tenant.Responses.SpecialNote
{
    public class AddAlrgyDrugListResponse
    {
        public AddAlrgyDrugListResponse(List<AddAlrgyDrugListItemResponse> validations)
        {
            Validations = validations;
        }

        public List<AddAlrgyDrugListItemResponse> Validations { get; private set; }
    }
}
