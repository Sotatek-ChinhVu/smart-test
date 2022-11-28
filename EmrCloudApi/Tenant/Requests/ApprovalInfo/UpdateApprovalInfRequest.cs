﻿using Entity.Tenant;
namespace EmrCloudApi.Tenant.Requests.ApprovalInfo
{
    public class UpdateApprovalInfRequest
    {
        public List<UpdateApprovalInfListRequest> ApprovalIfnList { get; set; } = new List<UpdateApprovalInfListRequest>();
    }
    public class UpdateApprovalInfListRequest
    {
        public int HpId { get; set; }
        public int Id { get; set; }
        public long RaiinNo { get; set; }
        public int SeqNo { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public int IsDeleted { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateMachine { get; set; } = string.Empty;   
        public int UpdateId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateMachine { get; set; } = string.Empty;
    }
}
