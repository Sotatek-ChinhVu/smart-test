using System.Xml.Serialization;

namespace Domain.Models.Online.QualificationConfirmation;

[Serializable]
[XmlRoot(ElementName = "LimitApplicationCertificateRelatedInfo")]
public class LimitApplicationCertificateRelatedInfo
{
    [XmlElement(ElementName = "LimitApplicationCertificateClassification")]
    public string LimitApplicationCertificateClassification { get; set; } = string.Empty;

    [XmlElement(ElementName = "LimitApplicationCertificateClassificationFlag")]
    public string LimitApplicationCertificateClassificationFlag { get; set; } = string.Empty;

    [XmlElement(ElementName = "LimitApplicationCertificateDate")]
    public string LimitApplicationCertificateDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "LimitApplicationCertificateValidStartDate")]
    public string LimitApplicationCertificateValidStartDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "LimitApplicationCertificateValidEndDate")]
    public string LimitApplicationCertificateValidEndDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "LimitApplicationCertificateLongTermDate")]
    public string LimitApplicationCertificateLongTermDate { get; set; } = string.Empty;
}