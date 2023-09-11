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

    public CreateDataKensaIraiRenkeiInteractor(IKensaIraiRepository kensaIraiRepository)
    {
        _kensaIraiRepository = kensaIraiRepository;
    }

    public CreateDataKensaIraiRenkeiOutputData Handle(CreateDataKensaIraiRenkeiInputData inputData)
    {
        try
        {
            var validateResult = ValidateInput(inputData);
            if (validateResult != CreateDataKensaIraiRenkeiStatus.ValidateSuccess)
            {
                return new CreateDataKensaIraiRenkeiOutputData(validateResult);
            }
            if (_kensaIraiRepository.CreateDataKensaIraiRenkei(inputData.HpId, inputData.UserId, inputData.KensaIraiList, inputData.CenterCd, inputData.SystemDate))
            {
                return new CreateDataKensaIraiRenkeiOutputData(CreateDataKensaIraiRenkeiStatus.Successed);
            }
            return new CreateDataKensaIraiRenkeiOutputData(CreateDataKensaIraiRenkeiStatus.Failed);
        }
        finally
        {
            _kensaIraiRepository.ReleaseResource();
        }
    }

    private CreateDataKensaIraiRenkeiStatus ValidateInput(CreateDataKensaIraiRenkeiInputData inputData)
    {

        return CreateDataKensaIraiRenkeiStatus.ValidateSuccess;
    }
}
