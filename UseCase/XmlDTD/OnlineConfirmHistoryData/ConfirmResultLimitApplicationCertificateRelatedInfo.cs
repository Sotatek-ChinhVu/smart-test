using System.Xml.Serialization;

namespace UseCase.XmlDTD.OnlineConfirmHistoryData
{
    [Serializable]
    [XmlRoot(ElementName = "LimitApplicationCertificateRelatedInfo")]
    public class ConfirmResultLimitApplicationCertificateRelatedInfo
    {
        [XmlElement(ElementName = "LimitApplicationCertificateClassification")]
        public string LimitApplicationCertificateClassification { get; set; }

        [XmlElement(ElementName = "LimitApplicationCertificateClassificationFlag")]
        public string LimitApplicationCertificateClassificationFlag { get; set; }
    }
}
