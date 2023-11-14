using System.Xml.Serialization;

namespace UseCase.XmlDTD.OnlineConfirmHistoryData;

[Serializable]
[XmlRoot(ElementName = "MessageBody")]
public class ConfirmResultMessageBody
{
    [XmlElement(ElementName = "ResultList")]
    public ConfirmResultResultList ResultList { get; set; } = new();

    [XmlElement(ElementName = "QualificationConfirmSearchInfo")]
    public ConfirmResultQualificationConfirmSearchInfo QualificationConfirmSearchInfo { get; set; } = new();

    [XmlElement(ElementName = "ProcessingResultStatus")]
    public string ProcessingResultStatus { get; set; } = string.Empty;

    [XmlElement(ElementName = "ProcessingResultCode")]
    public string ProcessingResultCode { get; set; } = string.Empty;

    [XmlElement(ElementName = "ProcessingResultMessage")]
    public string ProcessingResultMessage { get; set; } = string.Empty;

    [XmlElement(ElementName = "QualificationValidity")]
    public string QualificationValidity { get; set; } = string.Empty;
}
