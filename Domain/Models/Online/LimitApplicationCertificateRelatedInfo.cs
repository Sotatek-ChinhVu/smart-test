using System.Xml.Serialization;

namespace Domain.Models.Online
{
    [Serializable]
    [XmlRoot(ElementName = "LimitApplicationCertificateRelatedInfo")]
    public class LimitApplicationCertificateRelatedInfo
    {
        [XmlElement(ElementName = "LimitApplicationCertificateClassification")]
        public string LimitApplicationCertificateClassification { get; set; }

        [XmlElement(ElementName = "LimitApplicationCertificateClassificationFlag")]
        public string LimitApplicationCertificateClassificationFlag { get; set; }

        [XmlElement(ElementName = "LimitApplicationCertificateDate")]
        public string LimitApplicationCertificateDate { get; set; }

        [XmlElement(ElementName = "LimitApplicationCertificateValidStartDate")]
        public string LimitApplicationCertificateValidStartDate { get; set; }

        [XmlElement(ElementName = "LimitApplicationCertificateValidEndDate")]
        public string LimitApplicationCertificateValidEndDate { get; set; }

        [XmlElement(ElementName = "LimitApplicationCertificateLongTermDate")]
        public string LimitApplicationCertificateLongTermDate { get; set; }
    }
}
