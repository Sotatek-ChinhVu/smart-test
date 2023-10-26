using System.Xml.Serialization;

namespace Domain.Models.Online
{
    [Serializable]
    [XmlRoot(ElementName = "ElderlyRecipientCertificateInfo")]
    public class ElderlyRecipientCertificateInfo
    {
        [XmlElement(ElementName = "ElderlyRecipientCertificateDate")]
        public string ElderlyRecipientCertificateDate { get; set; }

        [XmlElement(ElementName = "ElderlyRecipientValidStartDate")]
        public string ElderlyRecipientValidStartDate { get; set; }

        [XmlElement(ElementName = "ElderlyRecipientValidEndDate")]
        public string ElderlyRecipientValidEndDate { get; set; }

        [XmlElement(ElementName = "ElderlyRecipientContributionRatio")]
        public string ElderlyRecipientContributionRatio { get; set; }
    }
}
