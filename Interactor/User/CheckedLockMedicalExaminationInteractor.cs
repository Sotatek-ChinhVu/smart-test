using Domain.Models.User;
using UseCase.User.CheckedLockMedicalExamination;

namespace Interactor.User;

public class CheckedLockMedicalExaminationInteractor : ICheckedLockMedicalExaminationInputPort
{
    private readonly IUserRepository _userRepository;

    public CheckedLockMedicalExaminationInteractor(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public CheckedLockMedicalExaminationOutputData Handle(CheckedLockMedicalExaminationInputData input)
    {
        try
        {
            if (input.HpId <= 0)
            {
                return new CheckedLockMedicalExaminationOutputData(CheckedLockMedicalExaminationStatus.InvalidHpId, true);
            }
            if (input.PtId <= 0)
            {
                return new CheckedLockMedicalExaminationOutputData(CheckedLockMedicalExaminationStatus.InvalidPtId, true);
            }
            if (input.SinDate <= 0)
            {
                return new CheckedLockMedicalExaminationOutputData(CheckedLockMedicalExaminationStatus.InvalidSinDate, true);
            }
            if (input.RaiinNo <= 0)
            {
                return new CheckedLockMedicalExaminationOutputData(CheckedLockMedicalExaminationStatus.InvalidRaiinNo, true);
            }
            if (input.UserId <= 0)
            {
                return new CheckedLockMedicalExaminationOutputData(CheckedLockMedicalExaminationStatus.InvalidUserId, true);
            }
            if (string.IsNullOrEmpty(input.Token.Trim()))
            {
                return new CheckedLockMedicalExaminationOutputData(CheckedLockMedicalExaminationStatus.InvalidToken, true);
            }

            var check = _userRepository.CheckLockMedicalExamination(input.HpId, input.PtId, input.RaiinNo, input.SinDate, input.Token, input.UserId);

            return new CheckedLockMedicalExaminationOutputData(CheckedLockMedicalExaminationStatus.Successed, !check);
        }
        catch
        {
            return new CheckedLockMedicalExaminationOutputData(CheckedLockMedicalExaminationStatus.Failed, true);
        }
    }
}
