using Helper.Constants;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class ValidationTodayOrdItemResponse
    {
        public ValidationTodayOrdItemResponse(OrderInfConst.OrdInfValidationStatus status, string orderInfPosition, string orderInfDetailPosition, string validationMessage, string validationField)
        {
            Status = status;
            OrderInfPosition = orderInfPosition;
            OrderInfDetailPosition = orderInfDetailPosition;
            ValidationMessage = validationMessage;
            ValidationField = validationField;
        }

        public OrderInfConst.OrdInfValidationStatus Status { get; private set; }
        public string OrderInfPosition { get; private set; }
        public string OrderInfDetailPosition { get; private set; }
        public string ValidationMessage { get; private set; }
        public string ValidationField { get; private set; }

    }
}
