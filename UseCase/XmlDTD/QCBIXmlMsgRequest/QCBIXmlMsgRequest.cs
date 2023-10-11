using System.Xml.Serialization;
using UseCase.Online.QualificationConfirmation;

namespace UseCase.XmlDTD.QCBIXmlMsgRequest
{
    [Serializable]
    [XmlRoot(ElementName = "XmlMsg")]
    public class QCBIXmlMsgRequest
    {
        [XmlElement(ElementName = "MessageHeader")]
        public MessageHeader MessageHeader { get; set; }
        [XmlElement(ElementName = "MessageBody")]
        public MessageBody MessageBody { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "MessageHeader")]
    public class MessageHeader
    {
        [XmlElement(ElementName = "QualificationConfirmationDate")]
        public string QualificationConfirmationDate { get; set; }

        [XmlElement(ElementName = "MedicalInstitutionCode")]
        public string MedicalInstitutionCode { get; set; }

        [XmlElement(ElementName = "ArbitraryFileIdentifier")]
        public string ArbitraryFileIdentifier { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "MessageBody")]
    public class MessageBody
    {
        [XmlElement(ElementName = "QualificationConfirmSearchInfo")]
        public QualificationConfirmSearchInfo[] QualificationConfirmSearchInfo { get; set; }
    }
}
