using static Helper.Constants.NextOrderConst;

namespace EmrCloudApi.Responses.NextOrder
{
    public class NextOrderItemResponse
    {
        public NextOrderItemResponse(int nextOrderPosition, NextOrderValidationItemResponse validationNextOrders)
        {
            NextOrderPosition = nextOrderPosition;
            ValidationNextOrders = validationNextOrders;
        }

        public int NextOrderPosition { get; private set; }
        public NextOrderValidationItemResponse ValidationNextOrders { get; private set; }
    }

    public class NextOrderValidationItemResponse
    {
        public NextOrderValidationItemResponse(NextOrderStatus status, string validationMessage)
        {
            Status = status;
            ValidationMessage = validationMessage;
        }

        public NextOrderStatus Status { get; private set; }
        public string ValidationMessage { get; private set; }
    }
}
