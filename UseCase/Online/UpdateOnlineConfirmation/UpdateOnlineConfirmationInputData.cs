using System.Xml.Serialization;
using UseCase.Core.Sync.Core;
using UseCase.XmlDTD.QCBIDXmlMsgResponse;

namespace UseCase.Online.SaveOnlineConfirmation
{
    public class UpdateOnlineConfirmationInputData : IInputData<UpdateOnlineConfirmationOutputData>
    {
        public UpdateOnlineConfirmationInputData(int hpId, int userId, string receptionNumber, int yokakuDate, string qCBIDXmlMsgResponse)
        {
            HpId = hpId;
            UserId = userId;
            ReceptionNumber = receptionNumber;
            YokakuDate = yokakuDate;
            QCBIDXmlMsgResponse = qCBIDXmlMsgResponse;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public string ReceptionNumber { get; private set; }

        public int YokakuDate { get; private set; }

        public string QCBIDXmlMsgResponse { get; private set; }

        public QCBIDXmlMsgResponse QCBIDResponse
        {
            get => new XmlSerializer(typeof(QCBIDXmlMsgResponse)).Deserialize(new StringReader(QCBIDXmlMsgResponse)) as QCBIDXmlMsgResponse ?? new();
        }
    }
}
