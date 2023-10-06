using Domain.Models.MainMenu;
using Helper.Constants;
using System.Xml.Serialization;
using UseCase.Core.Sync.Core;
using UseCase.XmlDTD.QCBIDXmlMsgRequest;
using UseCase.XmlDTD.QCBIDXmlMsgResponse;

namespace UseCase.Online.SaveOnlineConfirmation
{
    public class SaveOnlineConfirmationInputData : IInputData<SaveOnlineConfirmationOutputData>
    {
        public SaveOnlineConfirmationInputData(int userId, string qCBIDXmlMsgRequest, string qCBIDXmlMsgResponse, QualificationInfModel qualificationInf, ModelStatus modelStatus)
        {
            UserId = userId;
            QCBIDXmlMsgRequest = qCBIDXmlMsgRequest;
            QCBIDXmlMsgResponse = qCBIDXmlMsgResponse;
            QualificationInf = qualificationInf;
            ModelStatus = modelStatus;
        }

        public int UserId { get; set; }

        public string QCBIDXmlMsgRequest { get; private set; }

        public string QCBIDXmlMsgResponse { get; private set; }

        public QualificationInfModel QualificationInf { get; private set; }

        public QCBIDXmlMsgResponse QCBIDResponse
        {
            get => new XmlSerializer(typeof(QCBIDXmlMsgResponse)).Deserialize(new StringReader(QCBIDXmlMsgRequest)) as QCBIDXmlMsgResponse ?? new();
        }

        public QCBIDXmlMsgRequest QCBIDRequest
        {
            get => new XmlSerializer(typeof(QCBIDXmlMsgRequest)).Deserialize(new StringReader(QCBIDXmlMsgRequest)) as QCBIDXmlMsgRequest ?? new();
        }

        public ModelStatus ModelStatus { get; private set; }
    }
}
