namespace EmrCloudApi.Tenant.Responses.NextOrder
{
    public class UpsertNextOrderListResponse
    {
        public UpsertNextOrderListResponse(List<UpsertNextOrderItemResponse> validationNextOrders, List<UpsertOrderInfItemResponse> validationOdrs, List<UpsertKarteInfItemResponse> validationKarte, List<UpsertByomeiItemResponse> validationByomeis)
        {
            ValidationNextOrders = validationNextOrders;
            ValidationOdrs = validationOdrs;
            ValidationKarte = validationKarte;
            ValidationByomeis = validationByomeis;
        }

        public List<UpsertNextOrderItemResponse> ValidationNextOrders { get; private set; }

        public List<UpsertOrderInfItemResponse> ValidationOdrs { get; private set; }

        public List<UpsertKarteInfItemResponse> ValidationKarte { get; private set; }

        public List<UpsertByomeiItemResponse> ValidationByomeis { get; private set; }
    }
}
