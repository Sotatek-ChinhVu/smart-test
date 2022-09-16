using Helper.Constants;

namespace EmrCloudApi.Tenant.Responses.SpecialNote
{
    public class AddAlrgyDrugListItemResponse
    {
        public AddAlrgyDrugListItemResponse(TodayOrderConst.TodayOrdValidationStatus status, int position, string validationMessage)
        {
            Status = status;
            Position = position;
            ValidationMessage = validationMessage;
        }

        public TodayOrderConst.TodayOrdValidationStatus Status { get; private set; }
        public int Position { get; private set; }
        public string ValidationMessage { get; private set; }
    }
}
