using System.Xml.Serialization;
using UseCase.Core.Sync.Core;
using UseCase.XmlDTD.QCBIXmlMsgRequest;
using UseCase.XmlDTD.QCBIXmlMsgResponse;

namespace UseCase.Online.InsertOnlineConfirmation
{
    public class InsertOnlineConfirmationInputData : IInputData<InsertOnlineConfirmationOutputData>
    {
        public InsertOnlineConfirmationInputData(int userId, int sinDate, string qCBIXmlMsgRequest, string qCBIXmlMsgResponse)
        {
            UserId = userId;
            SinDate = sinDate;
            QCBIXmlMsgRequest = qCBIXmlMsgRequest;
            QCBIXmlMsgResponse = qCBIXmlMsgResponse;
        }

        public int UserId { get; private set; }

        public int SinDate { get; private set; }

        public string QCBIXmlMsgRequest { get; private set; }

        public string QCBIXmlMsgResponse { get; private set; }

        public QCBIXmlMsgResponse QCBIResponse
        {
            get => new XmlSerializer(typeof(QCBIXmlMsgResponse)).Deserialize(new StringReader(QCBIXmlMsgResponse)) as QCBIXmlMsgResponse ?? new();
        }

        public QCBIXmlMsgRequest QCBIRequest
        {
            get => new XmlSerializer(typeof(QCBIXmlMsgRequest)).Deserialize(new StringReader(QCBIXmlMsgRequest)) as QCBIXmlMsgRequest ?? new();
        }
    }
}
