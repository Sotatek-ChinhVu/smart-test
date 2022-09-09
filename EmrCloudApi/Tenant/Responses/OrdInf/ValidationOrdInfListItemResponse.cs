using Helper.Constants;

namespace EmrCloudApi.Tenant.Responses.OrdInf
{
    public class ValidationOrdInfListItemResponse
    {
        public ValidationOrdInfListItemResponse(TodayOrderConst.TodayOrdValidationStatus status, int orderInfPosition, int orderInfDetailPosition, string validationMessage)
        {
            Status = status;
            OrderInfPosition = orderInfPosition;
            OrderInfDetailPosition = orderInfDetailPosition;
            ValidationMessage = validationMessage;
        }

        public TodayOrderConst.TodayOrdValidationStatus Status { get; private set; }
        public int OrderInfPosition { get; private set; }
        public int OrderInfDetailPosition { get; private set; }
        public string ValidationMessage { get; private set; } = string.Empty;
    }
}
