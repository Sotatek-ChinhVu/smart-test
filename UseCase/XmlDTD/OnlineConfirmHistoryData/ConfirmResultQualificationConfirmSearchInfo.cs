using System.Xml.Serialization;

namespace UseCase.XmlDTD.OnlineConfirmHistoryData
{
    [Serializable]
    [XmlRoot(ElementName = "QualificationConfirmSearchInfo")]
    public class ConfirmResultQualificationConfirmSearchInfo
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
        public ConfirmResultLimitApplicationCertificateRelatedInfo LimitApplicationCertificateRelatedInfo { get; set; } = new();

        [XmlElement(ElementName = "ArbitraryIdentifier")]
        public string ArbitraryIdentifier { get; set; } = string.Empty;
    }
}
