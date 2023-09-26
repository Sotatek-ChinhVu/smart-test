﻿using Domain.Models.KensaSet;
using Domain.Models.KensaSetDetail;

namespace EmrCloudApi.Requests.MstItem
{
    public class UpdateKensaSetRequest
    {
        public int SetId { get; set; }
        public string SetName { get; set; }
        public int SortNo { get; set; }
        public int IsDeleted { get; set; }
        public List<KensaSetDetailModel> KensaSetDetails { get; set; }
    }
}
