using System.Xml.Serialization;

namespace Domain.Models.Online
{
    [Serializable]
    [XmlRoot(ElementName = "ResultOfQualificationConfirmation")]
    public class ResultOfQualificationConfirmation
    {
        [XmlElement(ElementName = "InsuredCardClassification")]
        public string InsuredCardClassification { get; set; }

        [XmlElement(ElementName = "InsurerNumber")]
        public string InsurerNumber { get; set; }

        [XmlElement(ElementName = "InsuredCardSymbol")]
        public string InsuredCardSymbol { get; set; }

        [XmlElement(ElementName = "InsuredIdentificationNumber")]
        public string InsuredIdentificationNumber { get; set; }

        [XmlElement(ElementName = "InsuredBranchNumber")]
        public string InsuredBranchNumber { get; set; }

        [XmlElement(ElementName = "PersonalFamilyClassification")]
        public string PersonalFamilyClassification { get; set; }

        [XmlElement(ElementName = "InsuredName")]
        public string InsuredName { get; set; }

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "NameOfOther")]
        public string NameOfOther { get; set; }

        [XmlElement(ElementName = "NameKana")]
        public string NameKana { get; set; }

        [XmlElement(ElementName = "NameOfOtherKana")]
        public string NameOfOtherKana { get; set; }

        [XmlElement(ElementName = "Sex1")]
        public string Sex1 { get; set; }

        [XmlElement(ElementName = "Sex2")]
        public string Sex2 { get; set; }

        [XmlElement(ElementName = "Birthdate")]
        public string Birthdate { get; set; }

        [XmlElement(ElementName = "Address")]
        public string Address { get; set; }

        [XmlElement(ElementName = "PostNumber")]
        public string PostNumber { get; set; }

        [XmlElement(ElementName = "InsuredCertificateIssuanceDate")]
        public string InsuredCertificateIssuanceDate { get; set; }

        [XmlElement(ElementName = "InsuredCardValidDate")]
        public string InsuredCardValidDate { get; set; }

        [XmlElement(ElementName = "InsuredCardExpirationDate")]
        public string InsuredCardExpirationDate { get; set; }

        [XmlElement(ElementName = "InsuredPartialContributionRatio")]
        public string InsuredPartialContributionRatio { get; set; }

        [XmlElement(ElementName = "PreschoolClassification")]
        public string PreschoolClassification { get; set; }

        [XmlElement(ElementName = "ReasonOfLoss")]
        public string ReasonOfLoss { get; set; }

        [XmlElement(ElementName = "InsurerName")]
        public string InsurerName { get; set; }

        [XmlElement(ElementName = "ElderlyRecipientCertificateInfo")]
        public ElderlyRecipientCertificateInfo ElderlyRecipientCertificateInfo { get; set; }

        [XmlElement(ElementName = "LimitApplicationCertificateRelatedConsFlg")]
        public string LimitApplicationCertificateRelatedConsFlg { get; set; }

        [XmlElement(ElementName = "LimitApplicationCertificateRelatedConsTime")]
        public string LimitApplicationCertificateRelatedConsTime { get; set; }

        [XmlElement(ElementName = "LimitApplicationCertificateRelatedInfo")]
        public LimitApplicationCertificateRelatedInfo LimitApplicationCertificateRelatedInfo { get; set; }

        [XmlElement(ElementName = "SpecificDiseasesCertificateRelatedConsFlg")]
        public string SpecificDiseasesCertificateRelatedConsFlg { get; set; }

        [XmlElement(ElementName = "SpecificDiseasesCertificateRelatedConsTime")]
        public string SpecificDiseasesCertificateRelatedConsTime { get; set; }

        [XmlElement(ElementName = "SpecificDiseasesCertificateList")]
        public SpecificDiseasesCertificateInfo[] SpecificDiseasesCertificateList { get; set; }

        [XmlElement(ElementName = "SpecificHealthCheckupsInfoConsFlg")]
        public string SpecificHealthCheckupsInfoConsFlg { get; set; }

        [XmlElement(ElementName = "SpecificHealthCheckupsInfoConsTime")]
        public string SpecificHealthCheckupsInfoConsTime { get; set; }

        [XmlElement(ElementName = "SpecificHealthCheckupsInfoAvailableTime")]
        public string SpecificHealthCheckupsInfoAvailableTime { get; set; }

        [XmlElement(ElementName = "PharmacistsInfoConsFlg")]
        public string PharmacistsInfoConsFlg { get; set; }

        [XmlElement(ElementName = "PharmacistsInfoConsTime")]
        public string PharmacistsInfoConsTime { get; set; }

        [XmlElement(ElementName = "PharmacistsInfoAvailableTime")]
        public string PharmacistsInfoAvailableTime { get; set; }

        [XmlElement(ElementName = "ArbitraryIdentifier")]
        public string ArbitraryIdentifier { get; set; }

        [XmlElement(ElementName = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [XmlElement(ElementName = "DiagnosisInfoConsFlg")]
        public string DiagnosisInfoConsFlg { get; set; }

        [XmlElement(ElementName = "DiagnosisInfoConsTime")]
        public string DiagnosisInfoConsTime { get; set; }

        [XmlElement(ElementName = "DiagnosisInfoAvailableTime")]
        public string DiagnosisInfoAvailableTime { get; set; }

        [XmlElement(ElementName = "OperationInfoConsFlg")]
        public string OperationInfoConsFlg { get; set; }

        [XmlElement(ElementName = "OperationInfoConsTime")]
        public string OperationInfoConsTime { get; set; }

        [XmlElement(ElementName = "OperationInfoAvailableTime")]
        public string OperationInfoAvailableTime { get; set; }

        [XmlElement(ElementName = "PrescriptionIssueSelect")]
        public string PrescriptionIssueSelect { get; set; }
    }
}
