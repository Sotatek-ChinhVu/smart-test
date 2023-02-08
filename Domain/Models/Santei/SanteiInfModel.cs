namespace Domain.Models.Santei;

public class SanteiInfModel
{
    public SanteiInfModel(long id, long ptId, string itemCd, int seqNo, int alertDays, int alertTerm, string itemName, int lastOdrDate, int santeiItemCount, double santeiItemSum, int currentMonthSanteiItemCount, double currentMonthSanteiItemSum, int sinDate, List<SanteiInfDetailModel> listSanteiInfDetails)
    {
        Id = id;
        PtId = ptId;
        ItemCd = itemCd;
        SeqNo = seqNo;
        AlertDays = alertDays;
        AlertTerm = alertTerm;
        ItemName = itemName;
        LastOdrDate = lastOdrDate;
        SanteiItemCount = santeiItemCount;
        SanteiItemSum = santeiItemSum;
        CurrentMonthSanteiItemCount = currentMonthSanteiItemCount;
        CurrentMonthSanteiItemSum = currentMonthSanteiItemSum;
        ListSanteiInfDetails = listSanteiInfDetails;
        IsDeleted = false;
        SinDate = sinDate;
    }

    public SanteiInfModel(string itemCd, int santeiItemCount, double santeiItemSum, int currentMonthSanteiItemCount, double currentMonthSanteiItemSum)
    {
        Id = 0;
        SeqNo = 0;
        AlertDays = 0;
        AlertTerm = 0;
        ItemName = string.Empty;
        LastOdrDate = 0;
        PtId = 0;
        ListSanteiInfDetails = new();
        IsDeleted = false;
        ItemCd = itemCd;
        SanteiItemCount = santeiItemCount;
        SanteiItemSum = santeiItemSum;
        CurrentMonthSanteiItemCount = currentMonthSanteiItemCount;
        CurrentMonthSanteiItemSum = currentMonthSanteiItemSum;
    }

    public SanteiInfModel(long id, long ptId, string itemCd, int alertDays, int alertTerm, List<SanteiInfDetailModel> listSanteiInfDetails, bool isDeleted)
    {
        Id = id;
        PtId = ptId;
        ItemCd = itemCd;
        AlertDays = alertDays;
        AlertTerm = alertTerm;
        ListSanteiInfDetails = listSanteiInfDetails;
        IsDeleted = isDeleted;
        SeqNo = 0;
        ItemName = string.Empty;
        LastOdrDate = 0;
        SanteiItemCount = 0;
        SanteiItemSum = 0;
        CurrentMonthSanteiItemCount = 0;
        CurrentMonthSanteiItemSum = 0;
    }

    public SanteiInfModel(long id, long ptId, string itemCd, int alertDays, int alertTerm)
    {
        Id = id;
        PtId = ptId;
        ItemCd = itemCd;
        AlertDays = alertDays;
        AlertTerm = alertTerm;
        SeqNo = 0;
        ItemName = string.Empty;
        LastOdrDate = 0;
        SanteiItemCount = 0;
        SanteiItemSum = 0;
        CurrentMonthSanteiItemCount = 0;
        CurrentMonthSanteiItemSum = 0;
        ListSanteiInfDetails = new();
        IsDeleted = false;
    }

    public long Id { get; private set; }

    public long PtId { get; private set; }

    public string ItemCd { get; private set; }

    public int SeqNo { get; private set; }

    public int AlertDays { get; private set; }

    public int AlertTerm { get; private set; }

    public string ItemName { get; private set; }

    public int LastOdrDate { get; private set; }

    public int SanteiItemCount { get; private set; }

    public double SanteiItemSum { get; private set; }

    public int CurrentMonthSanteiItemCount { get; private set; }

    public double CurrentMonthSanteiItemSum { get; private set; }

    public bool IsDeleted { get; private set; }

    public int SinDate { get; private set; }

    public List<SanteiInfDetailModel> ListSanteiInfDetails { get; private set; }
}
