using Domain.Models.KensaIrai;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using UseCase.MainMenu.CreateDataKensaIraiRenkei;

namespace Interactor.MainMenu;

public class CreateDataKensaIraiRenkeiInteractor : ICreateDataKensaIraiRenkeiInputPort
{
    private readonly IKensaIraiRepository _kensaIraiRepository;
    private readonly IReceptionRepository _receptionRepository;
    private readonly IPatientInforRepository _patientInforRepository;

    public CreateDataKensaIraiRenkeiInteractor(IKensaIraiRepository kensaIraiRepository, IReceptionRepository receptionRepository, IPatientInforRepository patientInforRepository)
    {
        _kensaIraiRepository = kensaIraiRepository;
        _receptionRepository = receptionRepository;
        _patientInforRepository = patientInforRepository;
    }

    public CreateDataKensaIraiRenkeiOutputData Handle(CreateDataKensaIraiRenkeiInputData inputData)
    {
        try
        {
            var validateResult = ValidateInput(inputData);
            if (validateResult != CreateDataKensaIraiRenkeiStatus.ValidateSuccess)
            {
                return new CreateDataKensaIraiRenkeiOutputData(validateResult, new());
            }
            if (!inputData.ReCreateDataKensaIraiRenkei)
            {
                var result = _kensaIraiRepository.CreateDataKensaIraiRenkei(inputData.HpId, inputData.UserId, inputData.KensaIraiList, inputData.CenterCd, inputData.SystemDate);
                return new CreateDataKensaIraiRenkeiOutputData(CreateDataKensaIraiRenkeiStatus.Successed, result);
            }
            else
            {
                var result = _kensaIraiRepository.ReCreateDataKensaIraiRenkei(inputData.HpId, inputData.UserId, inputData.KensaIraiList, inputData.SystemDate);
                return new CreateDataKensaIraiRenkeiOutputData(CreateDataKensaIraiRenkeiStatus.Successed, result);
            }
        }
        finally
        {
            _kensaIraiRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _receptionRepository.ReleaseResource();
        }
    }

    private CreateDataKensaIraiRenkeiStatus ValidateInput(CreateDataKensaIraiRenkeiInputData inputData)
    {
        if (!_patientInforRepository.CheckExistIdList(inputData.KensaIraiList.Select(item => item.PtId).Distinct().ToList()))
        {
            return CreateDataKensaIraiRenkeiStatus.InvalidPtId;
        }
        else if (!inputData.ReCreateDataKensaIraiRenkei && !_kensaIraiRepository.CheckExistCenterCd(inputData.HpId, inputData.CenterCd))
        {
            return CreateDataKensaIraiRenkeiStatus.InvalidCenterCd;
        }
        foreach (var kensaIrai in inputData.KensaIraiList)
        {
            if (!_receptionRepository.CheckExistRaiinNo(inputData.HpId, kensaIrai.PtId, kensaIrai.RaiinNo))
            {
                return CreateDataKensaIraiRenkeiStatus.InvalidRaiinNo;
            }
        }
        return CreateDataKensaIraiRenkeiStatus.ValidateSuccess;
    }
}
