using Domain.Models.Online;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using UseCase.Online.UpdatePtInfOnlineQualify;

namespace Interactor.Online;

public class UpdatePtInfOnlineQualifyInteractor : IUpdatePtInfOnlineQualifyInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IOnlineRepository _onlineRepository;
    private readonly ISystemConfRepository _systemConfig;

    public UpdatePtInfOnlineQualifyInteractor(IOnlineRepository onlineRepository, IPatientInforRepository patientInforRepository, ISystemConfRepository systemConfig)
    {
        _onlineRepository = onlineRepository;
        _patientInforRepository = patientInforRepository;
        _systemConfig = systemConfig;
    }

    public UpdatePtInfOnlineQualifyOutputData Handle(UpdatePtInfOnlineQualifyInputData inputData)
    {
        List<PtInfConfirmationModel> resultList = new();
        if (!_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
        {
            return new UpdatePtInfOnlineQualifyOutputData(UpdatePtInfOnlineQualifyStatus.InvalidPtId);
        }
        var systemConfigList = _systemConfig.GetList(inputData.HpId, new List<int> { 100029 });

        int nameBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 1)?.Val ?? 1);
        int kanaNameBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 2)?.Val ?? 1);
        int genderBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 3)?.Val ?? 1);
        int birthDayBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 4)?.Val ?? 1);
        int addressBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 5)?.Val ?? 1);
        int postcodeBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 6)?.Val ?? 1);
        int seitaiNushiBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 7)?.Val ?? 1);

        foreach (var item in inputData.ResultList)
        {
            resultList.Add(new PtInfConfirmationModel(item.AttributeName,
                                                      item.CurrentValue,
                                                      item.XmlValue,
                                                      nameBasicInfoCheck,
                                                      kanaNameBasicInfoCheck,
                                                      genderBasicInfoCheck,
                                                      birthDayBasicInfoCheck,
                                                      addressBasicInfoCheck,
                                                      postcodeBasicInfoCheck,
                                                      seitaiNushiBasicInfoCheck));
        }

        if (_onlineRepository.UpdatePtInfOnlineQualify(inputData.HpId, inputData.UserId, inputData.PtId, resultList))
        {
            return new UpdatePtInfOnlineQualifyOutputData(UpdatePtInfOnlineQualifyStatus.Successed);
        }
        return new UpdatePtInfOnlineQualifyOutputData(UpdatePtInfOnlineQualifyStatus.Failed);
    }
}
