﻿using System.Xml.Serialization;

namespace UseCase.XmlDTD.OnlineConfirmHistoryData
{
    [Serializable]
    [XmlRoot(ElementName = "XmlMsg")]
    public class ConfirmResultResponse
    {
        [XmlElement(ElementName = "MessageHeader")]
        public ConfirmResultMessageHeader MessageHeader { get; set; }

        [XmlElement(ElementName = "MessageBody")]
        public ConfirmResultMessageBody MessageBody { get; set; }
    }
}