namespace UseCase.Online;

public class OnlineConfirmationHistoryItem
{
    public OnlineConfirmationHistoryItem(long ptId, DateTime onlineConfirmationDate, int confirmationType, string infoConsFlg, string confirmationResult, int prescriptionIssueType, int uketukeStatus)
    {
        PtId = ptId;
        OnlineConfirmationDate = onlineConfirmationDate;
        ConfirmationType = confirmationType;
        InfoConsFlg = infoConsFlg;
        ConfirmationResult = confirmationResult;
        PrescriptionIssueType = prescriptionIssueType;
        UketukeStatus = uketukeStatus;
    }

    public long PtId { get; private set; }

    public DateTime OnlineConfirmationDate { get; private set; }

    public int ConfirmationType { get; private set; }

    public string InfoConsFlg { get; private set; }

    public string ConfirmationResult { get; private set; }

    public int PrescriptionIssueType { get; private set; }

    public int UketukeStatus { get; private set; }
}
