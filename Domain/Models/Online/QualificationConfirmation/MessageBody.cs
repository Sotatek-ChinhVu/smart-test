using System.Xml.Serialization;

namespace Domain.Models.Online.QualificationConfirmation;

[Serializable]
[XmlRoot(ElementName = "MessageBody")]
public class MessageBody
{
    [XmlElement(ElementName = "ProcessingResultStatus")]
    public string ProcessingResultStatus { get; set; } = string.Empty;

    [XmlElement(ElementName = "ProcessingResultCode")]
    public string ProcessingResultCode { get; set; } = string.Empty;

    [XmlElement(ElementName = "ProcessingResultMessage")]
    public string ProcessingResultMessage { get; set; } = string.Empty;

    [XmlElement(ElementName = "QualificationValidity")]
    public string QualificationValidity { get; set; } = string.Empty;

    [XmlElement(ElementName = "ResultList")]
    public ResultList ResultList { get; set; } = new();

    [XmlElement(ElementName = "QualificationConfirmSearchInfo")]
    public QualificationConfirmSearchInfo QualificationConfirmSearchInfo { get; set; } = new();
}