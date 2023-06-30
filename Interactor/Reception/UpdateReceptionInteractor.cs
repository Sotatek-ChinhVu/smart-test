using Domain.Models.Reception;
using Domain.Models.User;
using Entity.Tenant;
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
            List<ReceptionRowModel> receptionInfos = new();

            var notAllowSave = _userRepository.NotAllowSaveMedicalExamination(input.HpId, dto.Reception.PtId, dto.Reception.RaiinNo, dto.Reception.SinDate, input.UserId);
            if (notAllowSave)
            {
                return new UpdateReceptionOutputData(UpdateReceptionStatus.MedicalScreenLocked, receptionInfos);
            }

            else if (dto!.Insurances.Any(i => !i.IsValidData()))
            {
                return new UpdateReceptionOutputData(UpdateReceptionStatus.InvalidInsuranceList, receptionInfos);
            }

            var success = _receptionRepository.Update(input.Dto, input.HpId, input.UserId);
            var status = success ? UpdateReceptionStatus.Success : UpdateReceptionStatus.NotFound;
            if (success)
            {
                receptionInfos = _receptionRepository.GetList(input.HpId, dto.Reception.SinDate, dto.Reception.RaiinNo, dto.Reception.PtId);
            }
            return new UpdateReceptionOutputData(status, receptionInfos);
        }
        finally
        {
            _receptionRepository.ReleaseResource();
        }
    }
}
