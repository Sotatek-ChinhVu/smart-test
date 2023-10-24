using System.Xml.Serialization;

namespace Domain.Models.Online.QualificationConfirmation;

[Serializable]
[XmlRoot(ElementName = "MessageHeader")]
public class MessageHeader
{
    [XmlElement(ElementName = "ProcessExecutionTime")]
    public string ProcessExecutionTime { get; set; } = string.Empty;

    [XmlElement(ElementName = "QualificationConfirmationDate")]
    public string QualificationConfirmationDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "MedicalInstitutionCode")]
    public string MedicalInstitutionCode { get; set; } = string.Empty;

    [XmlElement(ElementName = "ArbitraryFileIdentifier")]
    public string ArbitraryFileIdentifier { get; set; } = string.Empty;

    [XmlElement(ElementName = "ReferenceClassification")]
    public string ReferenceClassification { get; set; } = string.Empty;

    [XmlElement(ElementName = "SegmentOfResult")]
    public string SegmentOfResult { get; set; } = string.Empty;

    [XmlElement(ElementName = "ErrorCode")]
    public string ErrorCode { get; set; } = string.Empty;

    [XmlElement(ElementName = "ErrorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;

    [XmlElement(ElementName = "CharacterCodeIdentifier")]
    public string CharacterCodeIdentifier { get; set; } = string.Empty;
}
