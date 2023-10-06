using System.Xml.Serialization;

namespace Domain.Models.Online.QualificationConfirmation;

[Serializable]
[XmlRoot(ElementName = "ElderlyRecipientCertificateInfo")]
public class ElderlyRecipientCertificateInfo
{
    [XmlElement(ElementName = "ElderlyRecipientCertificateDate")]
    public string ElderlyRecipientCertificateDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "ElderlyRecipientValidStartDate")]
    public string ElderlyRecipientValidStartDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "ElderlyRecipientValidEndDate")]
    public string ElderlyRecipientValidEndDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "ElderlyRecipientContributionRatio")]
    public string ElderlyRecipientContributionRatio { get; set; } = string.Empty;
}