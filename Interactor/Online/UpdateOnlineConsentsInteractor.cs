using Domain.Models.Online;
using System.Xml;
using UseCase.Online.UpdateOnlineConsents;
using Domain.Models.PatientInfor;
using Domain.Models.Online.QualificationConfirmation;
using System.Globalization;
using System.Xml.Serialization;

namespace Interactor.Online;

public class UpdateOnlineConsentsInteractor : IUpdateOnlineConsentsInputPort
{
    private readonly IOnlineRepository _onlineRepository;
    private readonly IPatientInforRepository _patientInforRepository;

    public UpdateOnlineConsentsInteractor(IOnlineRepository onlineRepository, IPatientInforRepository patientInforRepository)
    {
        _onlineRepository = onlineRepository;
        _patientInforRepository = patientInforRepository;
    }

    public UpdateOnlineConsentsOutputData Handle(UpdateOnlineConsentsInputData inputData)
    {
        try
        {
            var validateResult = ValidateData(inputData);
            if (validateResult.status != UpdateOnlineConsentsStatus.ValidateSuccess)
            {
                return new UpdateOnlineConsentsOutputData(validateResult.status);
            }
            if (_onlineRepository.UpdateOnlineConsents(inputData.UserId, inputData.PtId, validateResult.responseList))
            {
                return new UpdateOnlineConsentsOutputData(UpdateOnlineConsentsStatus.Successed);
            }
            return new UpdateOnlineConsentsOutputData(UpdateOnlineConsentsStatus.Failed);
        }
        finally
        {
            _onlineRepository.ReleaseResource();
        }
    }

    private (UpdateOnlineConsentsStatus status, List<QCXmlMsgResponse> responseList) ValidateData(UpdateOnlineConsentsInputData inputData)
    {
        if (!_patientInforRepository.CheckExistIdList(inputData.HpId, new List<long>() { inputData.PtId }))
        {
            return (UpdateOnlineConsentsStatus.InvalidPtId, new());
        }
        List<QCXmlMsgResponse> responseList = new();
        foreach (var xmlString in inputData.ResponseList)
        {
            if (string.IsNullOrEmpty(xmlString))
            {
                return (UpdateOnlineConsentsStatus.InvalidXmlFile, new());
            }
            try
            {
                XmlDocument xmlDoc = new();
                xmlDoc.LoadXml(xmlString);
            }
            catch
            {
                return (UpdateOnlineConsentsStatus.InvalidXmlFile, new());
            }
            var xmlObject = new XmlSerializer(typeof(QCXmlMsgResponse)).Deserialize(new StringReader(xmlString)) as QCXmlMsgResponse;
            if (xmlObject != null)
            {
                var onlineConfirmationDate = xmlObject.MessageHeader.ProcessExecutionTime;
                DateTime confirmDateInsert;
                try
                {
                    confirmDateInsert = TimeZoneInfo.ConvertTimeToUtc(DateTime.ParseExact(onlineConfirmationDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture));
                    responseList.Add(xmlObject);
                }
                catch
                {
                    return (UpdateOnlineConsentsStatus.InvalidOnlineConfirmationDate, new());
                }
            }
        }
        return (UpdateOnlineConsentsStatus.ValidateSuccess, responseList);
    }
}

