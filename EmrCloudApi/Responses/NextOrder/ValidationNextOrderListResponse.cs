namespace EmrCloudApi.Responses.NextOrder
{
    public class ValidationNextOrderListResponse
    {
        public ValidationNextOrderListResponse(List<NextOrderItemResponse> validationNextOrders, List<OrderInfItemResponse> validationOdrs, List<KarteInfItemResponse> validationKarte, List<ByomeiItemResponse> validationByomeis)
        {
            ValidationNextOrders = validationNextOrders;
            ValidationOdrs = validationOdrs;
            ValidationKarte = validationKarte;
            ValidationByomeis = validationByomeis;
        }

        public List<NextOrderItemResponse> ValidationNextOrders { get; private set; }

        public List<OrderInfItemResponse> ValidationOdrs { get; private set; }

        public List<KarteInfItemResponse> ValidationKarte { get; private set; }

        public List<ByomeiItemResponse> ValidationByomeis { get; private set; }
    }
}
