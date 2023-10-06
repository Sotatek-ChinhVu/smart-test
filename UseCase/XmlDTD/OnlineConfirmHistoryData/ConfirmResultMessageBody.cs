using System.Xml.Serialization;

namespace UseCase.XmlDTD.OnlineConfirmHistoryData
{
    [Serializable]
    [XmlRoot(ElementName = "MessageBody")]
    public class ConfirmResultMessageBody
    {
        [XmlElement(ElementName = "ResultList")]
        public ConfirmResultResultList ResultList { get; set; }

        [XmlElement(ElementName = "QualificationConfirmSearchInfo")]
        public ConfirmResultQualificationConfirmSearchInfo QualificationConfirmSearchInfo { get; set; }

        [XmlElement(ElementName = "ProcessingResultStatus")]
        public string ProcessingResultStatus { get; set; }

        [XmlElement(ElementName = "ProcessingResultCode")]
        public string ProcessingResultCode { get; set; }

        [XmlElement(ElementName = "ProcessingResultMessage")]
        public string ProcessingResultMessage { get; set; }

        [XmlElement(ElementName = "QualificationValidity")]
        public string QualificationValidity { get; set; }
    }
}
