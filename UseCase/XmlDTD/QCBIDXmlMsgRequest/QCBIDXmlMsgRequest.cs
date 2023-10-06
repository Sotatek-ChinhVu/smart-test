using System.Xml.Serialization;

namespace UseCase.XmlDTD.QCBIDXmlMsgRequest
{
    /// <summary>
    /// 資格確認一括照会ダウンロード要求
    /// OQSmuquc02req_xxxxxxxxxxxx.xml
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "XmlMsg")]
    public class QCBIDXmlMsgRequest
    {
        [XmlElement(ElementName = "MessageHeader")]
        public MessageHeader MessageHeader { get; set; } = new();
        [XmlElement(ElementName = "MessageBody")]
        public MessageBody MessageBody { get; set; } = new();
    }

    [Serializable]
    [XmlRoot(ElementName = "MessageHeader")]
    public class MessageHeader
    {
        [XmlElement(ElementName = "MedicalInstitutionCode")]
        public string MedicalInstitutionCode { get; set; } = string.Empty;
    }

    [Serializable]
    [XmlRoot(ElementName = "MessageBody")]
    public class MessageBody
    {
        [XmlElement(ElementName = "ReceptionNumber")]
        public string ReceptionNumber { get; set; } = string.Empty;
    }
}
