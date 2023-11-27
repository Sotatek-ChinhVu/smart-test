using System.Xml.Serialization;

namespace Domain.Models.Online.QualificationConfirmation;

[Serializable]
[XmlRoot(ElementName = "ResultOfQualificationConfirmation")]
public class ResultOfQualificationConfirmation
{
    [XmlElement(ElementName = "InsuredCardClassification")]
    public string InsuredCardClassification { get; set; } = string.Empty;

    [XmlElement(ElementName = "InsurerNumber")]
    public string InsurerNumber { get; set; } = string.Empty;

    [XmlElement(ElementName = "InsuredCardSymbol")]
    public string InsuredCardSymbol { get; set; } = string.Empty;

    [XmlElement(ElementName = "InsuredIdentificationNumber")]
    public string InsuredIdentificationNumber { get; set; } = string.Empty;

    [XmlElement(ElementName = "InsuredBranchNumber")]
    public string InsuredBranchNumber { get; set; } = string.Empty;

    [XmlElement(ElementName = "PersonalFamilyClassification")]
    public string PersonalFamilyClassification { get; set; } = string.Empty;

    [XmlElement(ElementName = "InsuredName")]
    public string InsuredName { get; set; } = string.Empty;

    [XmlElement(ElementName = "Name")]
    public string Name { get; set; } = string.Empty;

    [XmlElement(ElementName = "NameOfOther")]
    public string NameOfOther { get; set; } = string.Empty;

    [XmlElement(ElementName = "NameKana")]
    public string NameKana { get; set; } = string.Empty;

    [XmlElement(ElementName = "NameOfOtherKana")]
    public string NameOfOtherKana { get; set; } = string.Empty;

    [XmlElement(ElementName = "Sex1")]
    public string Sex1 { get; set; } = string.Empty;

    [XmlElement(ElementName = "Sex2")]
    public string Sex2 { get; set; } = string.Empty;

    [XmlElement(ElementName = "Birthdate")]
    public string Birthdate { get; set; } = string.Empty;

    [XmlElement(ElementName = "Address")]
    public string Address { get; set; } = string.Empty;

    [XmlElement(ElementName = "PostNumber")]
    public string PostNumber { get; set; } = string.Empty;

    [XmlElement(ElementName = "InsuredCertificateIssuanceDate")]
    public string InsuredCertificateIssuanceDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "InsuredCardValidDate")]
    public string InsuredCardValidDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "InsuredCardExpirationDate")]
    public string InsuredCardExpirationDate { get; set; } = string.Empty;

    [XmlElement(ElementName = "InsuredPartialContributionRatio")]
    public string InsuredPartialContributionRatio { get; set; } = string.Empty;

    [XmlElement(ElementName = "PreschoolClassification")]
    public string PreschoolClassification { get; set; } = string.Empty;

    [XmlElement(ElementName = "ReasonOfLoss")]
    public string ReasonOfLoss { get; set; } = string.Empty;

    [XmlElement(ElementName = "InsurerName")]
    public string InsurerName { get; set; } = string.Empty;

    [XmlElement(ElementName = "ElderlyRecipientCertificateInfo")]
    public ElderlyRecipientCertificateInfo ElderlyRecipientCertificateInfo { get; set; } = new();

    [XmlElement(ElementName = "LimitApplicationCertificateRelatedConsFlg")]
    public string LimitApplicationCertificateRelatedConsFlg { get; set; } = string.Empty;

    [XmlElement(ElementName = "LimitApplicationCertificateRelatedConsTime")]
    public string LimitApplicationCertificateRelatedConsTime { get; set; } = string.Empty;

    [XmlElement(ElementName = "LimitApplicationCertificateRelatedInfo")]
    public LimitApplicationCertificateRelatedInfo LimitApplicationCertificateRelatedInfo { get; set; } = new();

    [XmlElement(ElementName = "SpecificDiseasesCertificateRelatedConsFlg")]
    public string SpecificDiseasesCertificateRelatedConsFlg { get; set; } = string.Empty;

    [XmlElement(ElementName = "SpecificDiseasesCertificateRelatedConsTime")]
    public string SpecificDiseasesCertificateRelatedConsTime { get; set; } = string.Empty;

    [XmlElement(ElementName = "SpecificDiseasesCertificateList")]
    public SpecificDiseasesCertificateInfo[] SpecificDiseasesCertificateList { get; set; } = new SpecificDiseasesCertificateInfo[0];

    [XmlElement(ElementName = "SpecificHealthCheckupsInfoConsFlg")]
    public string SpecificHealthCheckupsInfoConsFlg { get; set; } = string.Empty;

    [XmlElement(ElementName = "SpecificHealthCheckupsInfoConsTime")]
    public string SpecificHealthCheckupsInfoConsTime { get; set; } = string.Empty;

    [XmlElement(ElementName = "SpecificHealthCheckupsInfoAvailableTime")]
    public string SpecificHealthCheckupsInfoAvailableTime { get; set; } = string.Empty;

    [XmlElement(ElementName = "PharmacistsInfoConsFlg")]
    public string PharmacistsInfoConsFlg { get; set; } = string.Empty;

    [XmlElement(ElementName = "PharmacistsInfoConsTime")]
    public string PharmacistsInfoConsTime { get; set; } = string.Empty;

    [XmlElement(ElementName = "PharmacistsInfoAvailableTime")]
    public string PharmacistsInfoAvailableTime { get; set; } = string.Empty;

    [XmlElement(ElementName = "ArbitraryIdentifier")]
    public string ArbitraryIdentifier { get; set; } = string.Empty;

    [XmlElement(ElementName = "ReferenceNumber")]
    public string ReferenceNumber { get; set; } = string.Empty;

    [XmlElement(ElementName = "DiagnosisInfoConsFlg")]
    public string DiagnosisInfoConsFlg { get; set; } = string.Empty;

    [XmlElement(ElementName = "DiagnosisInfoConsTime")]
    public string DiagnosisInfoConsTime { get; set; } = string.Empty;

    [XmlElement(ElementName = "DiagnosisInfoAvailableTime")]
    public string DiagnosisInfoAvailableTime { get; set; } = string.Empty;

    [XmlElement(ElementName = "OperationInfoConsFlg")]
    public string OperationInfoConsFlg { get; set; } = string.Empty;

    [XmlElement(ElementName = "OperationInfoConsTime")]
    public string OperationInfoConsTime { get; set; } = string.Empty;

    [XmlElement(ElementName = "OperationInfoAvailableTime")]
    public string OperationInfoAvailableTime { get; set; } = string.Empty;

    [XmlElement(ElementName = "PrescriptionIssueSelect")]
    public string PrescriptionIssueSelect { get; set; } = string.Empty;
}
