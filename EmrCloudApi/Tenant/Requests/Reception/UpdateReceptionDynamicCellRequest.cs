﻿namespace EmrCloudApi.Tenant.Requests.Reception;

public class UpdateReceptionDynamicCellRequest
{
    public int SinDate { get; set; }
    public long RaiinNo { get; set; }
    public long PtId { get; set; }
    public int GrpId { get; set; }
    public int KbnCd { get; set; }
}
