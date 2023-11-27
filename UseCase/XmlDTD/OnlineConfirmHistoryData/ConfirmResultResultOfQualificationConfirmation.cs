using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UseCase.XmlDTD.OnlineConfirmHistoryData
{
    [Serializable]
    [XmlRoot(ElementName = "ResultOfQualificationConfirmation")]
    public class ConfirmResultResultOfQualificationConfirmation
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
        public ConfirmResultElderlyRecipientCertificateInfo ElderlyRecipientCertificateInfo { get; set; } = new();

        [XmlElement(ElementName = "LimitApplicationCertificateChanged")]
        public string LimitApplicationCertificateChanged { get; set; } = string.Empty;

        [XmlElement(ElementName = "ReferenceNumber")]
        public string ReferenceNumber { get; set; } = string.Empty;
    }
}
