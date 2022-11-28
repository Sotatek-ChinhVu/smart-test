﻿using Domain.Models.NextOrder;

namespace EmrCloudApi.Responses.NextOrder
{
    public class GetNextOrderListResponse
    {
        public GetNextOrderListResponse(List<NextOrderModel> nextOrders)
        {
            NextOrders = nextOrders;
        }

        public List<NextOrderModel> NextOrders { get; private set; }
    }
}
