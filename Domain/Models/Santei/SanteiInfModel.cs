namespace Domain.Models.Santei;

public class SanteiInfModel
{
    public SanteiInfModel(long id, long ptId, string itemCd, int seqNo, int alertDays, int alertTerm, string itemName, int lastOdrDate, int santeiItemCount, double santeiItemSum, int currentMonthSanteiItemCount, double currentMonthSanteiItemSum)
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
}
