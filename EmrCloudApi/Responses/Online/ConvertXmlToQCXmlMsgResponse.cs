using Domain.Models.Online.QualificationConfirmation;
using System.Xml.Serialization;

namespace EmrCloudApi.Responses.Online;

public class ConvertXmlToQCXmlMsgResponse
{
    public ConvertXmlToQCXmlMsgResponse(string confirmationResult)
    {
        ConfirmationResult = confirmationResult;
    }

    public string ConfirmationResult { get; private set; }

    public QCXmlMsgResponse QCXmlMsgResponse { get => FormatXml(ConfirmationResult); }

    private QCXmlMsgResponse FormatXml(string confirmationResult)
    {
        var response = new XmlSerializer(typeof(QCXmlMsgResponse)).Deserialize(new StringReader(confirmationResult)) as QCXmlMsgResponse;
        return response ?? new();
    }
}
