using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Domain.Models.Online;
using Helper.Extension;
using System.Xml.Serialization;
using UseCase.Online;
using UseCase.Online.GetRegisterdPatientsFromOnline;
using UseCase.Online.QualificationConfirmation;

namespace Interactor.Online;

public class GetRegisterdPatientsFromOnlineInteractor : IGetRegisterdPatientsFromOnlineInputPort
{
    private readonly IOnlineRepository _onlineRepository;

    public GetRegisterdPatientsFromOnlineInteractor(IOnlineRepository onlineRepository)
    {
        _onlineRepository = onlineRepository;
    }

    public GetRegisterdPatientsFromOnlineOutputData Handle(GetRegisterdPatientsFromOnlineInputData inputData)
    {
        try
        {
            var onlineList = _onlineRepository.GetRegisterdPatientsFromOnline(inputData.SinDate, inputData.Id, inputData.ConfirmType);
            var result = onlineList.Select(item => FormatXml(inputData.SinDate, item)).ToList();
            return new GetRegisterdPatientsFromOnlineOutputData(result, GetRegisterdPatientsFromOnlineStatus.Successed);
        }
        finally
        {
            _onlineRepository.ReleaseResource();
        }
    }

    private PatientInfoItem FormatXml(int sinDate, OnlineConfirmationHistoryModel model)
    {
        var confirmationResult = model.ConfirmationResult;
        var response = new XmlSerializer(typeof(QCXmlMsgResponse)).Deserialize(new StringReader(confirmationResult)) as QCXmlMsgResponse;
        if (response == null)
        {
            return new();
        }
        var oqPtInf = response.MessageBody.ResultList.ResultOfQualificationConfirmation[0];
        if (oqPtInf == null)
        {
            return new();
        }
        return new PatientInfoItem(
                   sinDate,
                   model.Id,
                   0,
                   0,
                   0,
                   oqPtInf.NameKana,
                   oqPtInf.Name,
                   oqPtInf.Birthdate.AsInteger(),
                   oqPtInf.Sex1.AsInteger(),
                   oqPtInf.Sex2.AsInteger(),
                   oqPtInf.Address,
                   oqPtInf.PostNumber,
                   string.Empty,
                   model.OnlineConfirmationDate.ToString("HH:mm:ss"),
                   oqPtInf.InsurerNumber,
                   oqPtInf.InsuredBranchNumber,
                   oqPtInf.InsuredCardSymbol + "・" + oqPtInf.InsuredIdentificationNumber,
                   response.MessageBody.QualificationValidity,
                   model.UketukeStatus,
                   model.ConfirmationResult,
                   response.MessageHeader.SegmentOfResult.AsInteger(),
                   response.MessageBody.ProcessingResultStatus.AsInteger());
    }
}
