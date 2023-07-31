using UseCase.NextOrder.Check;

namespace EmrCloudApi.Responses.NextOrder
{
    public class CheckUpsertNextOrderResponse
    {
        public CheckUpsertNextOrderResponse(CheckUpsertNextOrderStatus status)
        {
            Status = status;
        }

        public CheckUpsertNextOrderStatus Status { get; private set; }
    }
}
