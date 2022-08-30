namespace EmrCloudApi.Tenant.Messages.Reception;

public class UpdateReceptionDynamicCellMessage
{
    public UpdateReceptionDynamicCellMessage(long raiinNo, int grpId, int kbnCd)
    {
        RaiinNo = raiinNo;
        GrpId = grpId;
        KbnCd = kbnCd;
    }

    public long RaiinNo { get; private set; }
    public int GrpId { get; private set; }
    public int KbnCd { get; private set; }
}
