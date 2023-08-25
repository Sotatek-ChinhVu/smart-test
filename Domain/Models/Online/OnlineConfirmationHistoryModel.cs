namespace Domain.Models.Online;

public class OnlineConfirmationHistoryModel
{
    public OnlineConfirmationHistoryModel(long id, long ptId, DateTime onlineConfirmationDate, int confirmationType, string infoConsFlg, string confirmationResult, int prescriptionIssueType, int uketukeStatus)
    {
        Id = id;
        PtId = ptId;
        OnlineConfirmationDate = onlineConfirmationDate;
        ConfirmationType = confirmationType;
        InfoConsFlg = infoConsFlg;
        ConfirmationResult = confirmationResult;
        PrescriptionIssueType = prescriptionIssueType;
        UketukeStatus = uketukeStatus;
    }

    public long Id { get; private set; }

    public long PtId { get; private set; }

    public DateTime OnlineConfirmationDate { get; private set; }

    public int ConfirmationType { get; private set; }

    public string InfoConsFlg { get; private set; }

    public string ConfirmationResult { get; private set; }

    public int PrescriptionIssueType { get; private set; }

    public int UketukeStatus { get; private set; }
}
