using System.Xml.Serialization;

namespace UseCase.XmlDTD.OnlineConfirmHistoryData
{
    [Serializable]
    [XmlRoot(ElementName = "QualificationConfirmSearchInfo")]
    public class ConfirmResultQualificationConfirmSearchInfo
    {
        [XmlElement(ElementName = "InsurerNumber")]
        public string InsurerNumber { get; set; }

        [XmlElement(ElementName = "InsuredCardSymbol")]
        public string InsuredCardSymbol { get; set; }

        [XmlElement(ElementName = "InsuredIdentificationNumber")]
        public string InsuredIdentificationNumber { get; set; }

        [XmlElement(ElementName = "InsuredBranchNumber")]
        public string InsuredBranchNumber { get; set; }

        [XmlElement(ElementName = "Birthdate")]
        public string Birthdate { get; set; }

        [XmlElement(ElementName = "LimitApplicationCertificateRelatedInfo")]
        public ConfirmResultLimitApplicationCertificateRelatedInfo LimitApplicationCertificateRelatedInfo { get; set; }

        [XmlElement(ElementName = "ArbitraryIdentifier")]
        public string ArbitraryIdentifier { get; set; }
    }
}
