using static Helper.Constants.NextOrderConst;

namespace EmrCloudApi.Tenant.Responses.NextOrder
{
    public class UpsertNextOrderItemResponse
    {
        public UpsertNextOrderItemResponse(int nextOrderPosition, UpsertNextOrderValidationItemResponse validationNextOrders)
        {
            NextOrderPosition = nextOrderPosition;
            ValidationNextOrders = validationNextOrders;
        }

        public int NextOrderPosition { get; private set; }
        public UpsertNextOrderValidationItemResponse ValidationNextOrders { get; private set; }
    }

    public class UpsertNextOrderValidationItemResponse
    {
        public UpsertNextOrderValidationItemResponse(NextOrderStatus status, string validationMessage)
        {
            Status = status;
            ValidationMessage = validationMessage;
        }

        public NextOrderStatus Status { get; private set; }
        public string ValidationMessage { get; private set; }
    }
}
