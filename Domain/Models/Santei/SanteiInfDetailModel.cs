namespace Domain.Models.Santei;

public class SanteiInfDetailModel
{
    public SanteiInfDetailModel(long id, long ptId, string itemCd, int endDate, int kisanSbt, int kisanDate, string byomei, string hosokuComment, string comment)
    {
        Id = id;
        PtId = ptId;
        ItemCd = itemCd;
        EndDate = endDate;
        KisanSbt = kisanSbt;
        KisanDate = kisanDate;
        Byomei = byomei;
        HosokuComment = hosokuComment;
        Comment = comment;
        IsDeleted = false;
    }

    public SanteiInfDetailModel(long id, long ptId, string itemCd, int endDate, int kisanSbt, int kisanDate, string byomei, string hosokuComment, string comment, bool isDeleted)
    {
        Id = id;
        PtId = ptId;
        ItemCd = itemCd;
        EndDate = endDate;
        KisanSbt = kisanSbt;
        KisanDate = kisanDate;
        Byomei = byomei;
        HosokuComment = hosokuComment;
        Comment = comment;
        IsDeleted = isDeleted;
    }

    public long Id { get; private set; }

    public long PtId { get; private set; }

    public string ItemCd { get; private set; }

    public int EndDate { get; private set; }

    public int KisanSbt { get; private set; }

    public int KisanDate { get; private set; }

    public string Byomei { get; private set; }

    public string HosokuComment { get; private set; }

    public string Comment { get; private set; }

    public bool IsDeleted { get; private set; }
}
