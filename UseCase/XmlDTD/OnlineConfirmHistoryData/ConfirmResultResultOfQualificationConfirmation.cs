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
        public ConfirmResultElderlyRecipientCertificateInfo ElderlyRecipientCertificateInfo { get; set; }

        [XmlElement(ElementName = "LimitApplicationCertificateChanged")]
        public string LimitApplicationCertificateChanged { get; set; }

        [XmlElement(ElementName = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }
    }
}
