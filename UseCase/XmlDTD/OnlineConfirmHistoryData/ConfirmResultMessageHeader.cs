using System.Xml.Serialization;

namespace UseCase.XmlDTD.OnlineConfirmHistoryData;

[Serializable]
[XmlRoot(ElementName = "MessageHeader")]
public class ConfirmResultMessageHeader
{
    [XmlElement(ElementName = "ProcessExecutionTime")]
    public string ProcessExecutionTime { get; set; } = string.Empty;

    [XmlElement(ElementName = "QualificationConfirmationDate")]
    public string QualificationConfirmationDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "MedicalInstitutionCode")]
    public string MedicalInstitutionCode { get; set; } = string.Empty;

    [XmlElement(ElementName = "ArbitraryFileIdentifier")]
    public string ArbitraryFileIdentifier { get; set; } = string.Empty;

    [XmlElement(ElementName = "ReceptionNumber")]
    public string ReceptionNumber { get; set; } = string.Empty;

    [XmlElement(ElementName = "SegmentOfResult")]
    public string SegmentOfResult { get; set; } = string.Empty;

    [XmlElement(ElementName = "ErrorCode")]
    public string ErrorCode { get; set; } = string.Empty;

    [XmlElement(ElementName = "ErrorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;

    [XmlElement(ElementName = "NumberOfProcessingResult")]
    public string NumberOfProcessingResult { get; set; } = string.Empty;

    [XmlElement(ElementName = "NumberOfNormalProcessing")]
    public string NumberOfNormalProcessing { get; set; } = string.Empty;

    [XmlElement(ElementName = "NumberOfError")]
    public string NumberOfError { get; set; } = string.Empty;

    [XmlElement(ElementName = "CharacterCodeIdentifier")]
    public string CharacterCodeIdentifier { get; set; } = string.Empty;
}
