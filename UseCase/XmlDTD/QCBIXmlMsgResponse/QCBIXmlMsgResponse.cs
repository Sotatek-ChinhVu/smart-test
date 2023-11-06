using System.Xml.Serialization;

namespace UseCase.XmlDTD.QCBIXmlMsgResponse
{
    [Serializable]
    [XmlRoot(ElementName = "XmlMsg")]
    public class QCBIXmlMsgResponse
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
        [XmlElement(ElementName = "ProcessExecutionTime")]
        public string ProcessExecutionTime { get; set; }

        [XmlElement(ElementName = "MedicalInstitutionCode")]
        public string MedicalInstitutionCode { get; set; }

        [XmlElement(ElementName = "ArbitraryFileIdentifier")]
        public string ArbitraryFileIdentifier { get; set; }

        [XmlElement(ElementName = "CharacterCodeIdentifier")]
        public string CharacterCodeIdentifier { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "MessageBody")]
    public class MessageBody
    {
        [XmlElement(ElementName = "ReceptionNumber")]
        public string ReceptionNumber { get; set; }

        [XmlElement(ElementName = "ReceptionDateTime")]
        public string ReceptionDateTime { get; set; }
    }
}
