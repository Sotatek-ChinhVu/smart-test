using System.Xml.Serialization;

namespace UseCase.XmlDTD.OnlineConfirmHistoryData
{
    [Serializable]
    [XmlRoot(ElementName = "ElderlyRecipientCertificateInfo")]
    public class ConfirmResultElderlyRecipientCertificateInfo
    {
        [XmlElement(ElementName = "ElderlyRecipientCertificateDate")]
        public string ElderlyRecipientCertificateDate { get; set; } = string.Empty;

        [XmlElement(ElementName = "ElderlyRecipientValidStartDate")]
        public string ElderlyRecipientValidStartDate { get; set; } = string.Empty;

        [XmlElement(ElementName = "ElderlyRecipientValidEndDate")]
        public string ElderlyRecipientValidEndDate { get; set; } = string.Empty;

        [XmlElement(ElementName = "ElderlyRecipientContributionRatio")]
        public string ElderlyRecipientContributionRatio { get; set; } = string.Empty;
    }
}
