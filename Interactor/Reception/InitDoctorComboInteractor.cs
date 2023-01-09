using Domain.Models.Reception;
using UseCase.Reception.InitDoctorCombo;

namespace Interactor.Reception;

public class InitDoctorComboInteractor : IInitDoctorComboInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public InitDoctorComboInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public InitDoctorComboOutputData Handle(InitDoctorComboInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new InitDoctorComboOutputData(InitDoctorComboStatus.InvalidHpId, 0);
            }
            else if (inputData.PtId <= 0)
            {
                return new InitDoctorComboOutputData(InitDoctorComboStatus.InvalidPtId, 0);
            }
            else if (inputData.SinDate <= 10000101 || inputData.SinDate > 99999999)
            {
                return new InitDoctorComboOutputData(InitDoctorComboStatus.InvalidSinDate, 0);
            }
            else if (inputData.UserId <= 0)
            {
                return new InitDoctorComboOutputData(InitDoctorComboStatus.InvalidUserId, 0);
            }

            var data = _receptionRepository.InitDoctorCombobox(inputData.UserId, inputData.TantoId, inputData.PtId, inputData.HpId, inputData.SinDate);

            return new InitDoctorComboOutputData(InitDoctorComboStatus.Successed, data);
        }
        catch (Exception)
        {
            return new InitDoctorComboOutputData(InitDoctorComboStatus.Failed, 0);
        }
        finally
        {
            _receptionRepository.ReleaseResource();
        }
    }
}
