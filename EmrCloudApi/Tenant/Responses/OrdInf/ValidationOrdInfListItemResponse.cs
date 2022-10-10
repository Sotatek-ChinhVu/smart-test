using Helper.Constants;

namespace EmrCloudApi.Tenant.Responses.OrdInf
{
    public class ValidationOrdInfListItemResponse
    {
        public ValidationOrdInfListItemResponse(TodayOrderConst.TodayOrdValidationStatus status, string orderInfPosition, string orderInfDetailPosition, string validationMessage, string validationField)
        {
            Status = status;
            OrderInfPosition = orderInfPosition;
            OrderInfDetailPosition = orderInfDetailPosition;
            ValidationMessage = validationMessage;
            ValidationField = validationField;
        }

        public TodayOrderConst.TodayOrdValidationStatus Status { get; private set; }
        public string OrderInfPosition { get; private set; }
        public string OrderInfDetailPosition { get; private set; }
        public string ValidationMessage { get; private set; }
        public string ValidationField { get; private set; }

    }
}
