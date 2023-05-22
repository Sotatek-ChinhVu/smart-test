using Domain.Models.Reception;
using Domain.Models.User;
using UseCase.Reception.Update;

namespace Interactor.Reception;

public class UpdateReceptionInteractor : IUpdateReceptionInputPort
{
    private readonly IReceptionRepository _receptionRepository;
    private readonly IUserRepository _userRepository;

    public UpdateReceptionInteractor(IReceptionRepository receptionRepository, IUserRepository userRepository)
    {
        _receptionRepository = receptionRepository;
        _userRepository = userRepository;
    }

    public UpdateReceptionOutputData Handle(UpdateReceptionInputData input)
    {
        try
        {
            ReceptionSaveDto dto = input.Dto;

            var checkLockMedical = _userRepository.CheckLockMedicalExamination(input.HpId, dto.Reception.PtId, dto.Reception.RaiinNo, dto.Reception.SinDate, input.UserId);
            if (checkLockMedical)
            {
                return new UpdateReceptionOutputData(UpdateReceptionStatus.MedicalScreenLocked);
            }

            else if (dto!.Insurances.Any(i => !i.IsValidData()))
            {
                return new UpdateReceptionOutputData(UpdateReceptionStatus.InvalidInsuranceList);
            }

            var success = _receptionRepository.Update(input.Dto, input.HpId, input.UserId);
            var status = success ? UpdateReceptionStatus.Success : UpdateReceptionStatus.NotFound;
            return new UpdateReceptionOutputData(status);
        }
        finally
        {
            _receptionRepository.ReleaseResource();
        }
    }
}
