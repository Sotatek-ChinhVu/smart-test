using System.Xml.Serialization;

namespace UseCase.XmlDTD.OnlineConfirmHistoryData
{
    [Serializable]
    [XmlRoot(ElementName = "ElderlyRecipientCertificateInfo")]
    public class ConfirmResultElderlyRecipientCertificateInfo
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
