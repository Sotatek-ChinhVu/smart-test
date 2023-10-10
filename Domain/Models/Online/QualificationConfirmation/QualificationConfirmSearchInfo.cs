using System.Xml.Serialization;

namespace Domain.Models.Online.QualificationConfirmation;

[Serializable]
[XmlRoot(ElementName = "QualificationConfirmSearchInfo")]
public class QualificationConfirmSearchInfo
{
    [XmlElement(ElementName = "InsurerNumber")]
    public string InsurerNumber { get; set; } = string.Empty;

    [XmlElement(ElementName = "InsuredCardSymbol")]
    public string InsuredCardSymbol { get; set; } = string.Empty;

    [XmlElement(ElementName = "InsuredIdentificationNumber")]
    public string InsuredIdentificationNumber { get; set; } = string.Empty;

    [XmlElement(ElementName = "InsuredBranchNumber")]
    public string InsuredBranchNumber { get; set; } = string.Empty;

    [XmlElement(ElementName = "Birthdate")]
    public string Birthdate { get; set; } = string.Empty;

    [XmlElement(ElementName = "LimitApplicationCertificateRelatedInfo")]
    public LimitApplicationCertificateRelatedInfo LimitApplicationCertificateRelatedInfo { get; set; } = new();

    [XmlElement(ElementName = "ArbitraryIdentifier")]
    public string ArbitraryIdentifier { get; set; } = string.Empty;

    [XmlElement(ElementName = "LimitApplicationCertificateRelatedConsFlg")]
    public string LimitApplicationCertificateRelatedConsFlg { get; set; } = string.Empty;
}