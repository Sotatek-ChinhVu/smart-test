using System.Xml.Serialization;
using UseCase.Core.Sync.Core;
using UseCase.XmlDTD.QCBIDXmlMsgRequest;
using UseCase.XmlDTD.QCBIDXmlMsgResponse;

namespace UseCase.Online.SaveOnlineConfirmation
{
    public class UpdateOnlineConfirmationInputData : IInputData<UpdateOnlineConfirmationOutputData>
    {
        public UpdateOnlineConfirmationInputData(int hpId, int userId, long raiinNo, string qCBIDXmlMsgRequest, string qCBIDXmlMsgResponse)
        {
            HpId = hpId;
            UserId = userId;
            RaiinNo = raiinNo;
            QCBIDXmlMsgRequest = qCBIDXmlMsgRequest;
            QCBIDXmlMsgResponse = qCBIDXmlMsgResponse;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public long RaiinNo { get; private set; }

        public string QCBIDXmlMsgRequest { get; private set; }

        public string QCBIDXmlMsgResponse { get; private set; }

        public QCBIDXmlMsgResponse QCBIDResponse
        {
            get => new XmlSerializer(typeof(QCBIDXmlMsgResponse)).Deserialize(new StringReader(QCBIDXmlMsgRequest)) as QCBIDXmlMsgResponse ?? new();
        }

        public QCBIDXmlMsgRequest QCBIDRequest
        {
            get => new XmlSerializer(typeof(QCBIDXmlMsgRequest)).Deserialize(new StringReader(QCBIDXmlMsgRequest)) as QCBIDXmlMsgRequest ?? new();
        }
    }
}
