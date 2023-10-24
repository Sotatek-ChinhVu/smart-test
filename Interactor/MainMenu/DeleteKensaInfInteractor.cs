using Domain.Models.KensaIrai;
using Domain.Models.PatientInfor;
using UseCase.MainMenu.DeleteKensaInf;

namespace Interactor.MainMenu;

public class DeleteKensaInfInteractor : IDeleteKensaInfInputPort
{
    private readonly IKensaIraiRepository _kensaIraiRepository;
    private readonly IPatientInforRepository _patientInforRepository;

    public DeleteKensaInfInteractor(IKensaIraiRepository kensaIraiRepository, IPatientInforRepository patientInforRepository)
    {
        _kensaIraiRepository = kensaIraiRepository;
        _patientInforRepository = patientInforRepository;
    }

    public DeleteKensaInfOutputData Handle(DeleteKensaInfInputData inputData)
    {
        try
        {
            var resultValidate = ValidateData(inputData);
            if (resultValidate != DeleteKensaInfStatus.ValidateSuccess)
            {
                return new DeleteKensaInfOutputData(resultValidate);
            }
            if (_kensaIraiRepository.DeleteKensaInfModel(inputData.HpId, inputData.UserId, inputData.KensaInfList))
            {
                return new DeleteKensaInfOutputData(DeleteKensaInfStatus.Successed);
            }
            return new DeleteKensaInfOutputData(DeleteKensaInfStatus.Failed);
        }
        finally
        {
            _kensaIraiRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
        }
    }

    private DeleteKensaInfStatus ValidateData(DeleteKensaInfInputData inputData)
    {
        if (!_patientInforRepository.CheckExistIdList(inputData.KensaInfList.Select(item => item.PtId).Distinct().ToList()))
        {
            return DeleteKensaInfStatus.InvalidPtId;
        }
        else if (!_kensaIraiRepository.CheckExistIraiCdList(inputData.HpId, inputData.KensaInfList.Select(item => item.IraiCd).Distinct().ToList()))
        {
            return DeleteKensaInfStatus.InvalidIraiCd;
        }
        return DeleteKensaInfStatus.ValidateSuccess;
    }
}
