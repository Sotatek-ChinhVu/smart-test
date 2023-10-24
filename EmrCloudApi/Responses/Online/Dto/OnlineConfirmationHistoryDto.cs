using Domain.Models.Online;
using Domain.Models.Online.QualificationConfirmation;
using System.Xml.Serialization;

namespace EmrCloudApi.Responses.Online.Dto;

public class OnlineConfirmationHistoryDto
{
    public OnlineConfirmationHistoryDto(OnlineConfirmationHistoryModel model)
    {
        Id = model.Id;
        PtId = model.PtId;
        OnlineConfirmationDate = model.OnlineConfirmationDate;
        ConfirmationType = model.ConfirmationType;
        InfoConsFlg = model.InfoConsFlg;
        ConfirmationResult = model.ConfirmationResult;
        PrescriptionIssueType = model.PrescriptionIssueType;
        UketukeStatus = model.UketukeStatus;
    }

    public long Id { get; private set; }

    public long PtId { get; private set; }

    public DateTime OnlineConfirmationDate { get; private set; }

    public int ConfirmationType { get; private set; }

    public string InfoConsFlg { get; private set; }

    public string ConfirmationResult { get; private set; }

    public int PrescriptionIssueType { get; private set; }

    public int UketukeStatus { get; private set; }

    public QCXmlMsgResponse QCXmlMsgResponse { get => FormatXml(ConfirmationResult); }

    private QCXmlMsgResponse FormatXml(string confirmationResult)
    {
        var response = new XmlSerializer(typeof(QCXmlMsgResponse)).Deserialize(new StringReader(confirmationResult)) as QCXmlMsgResponse;
        return response ?? new();
    }
}
