﻿using Domain.Models.Reception;
using UseCase.Reception.Insert;
using UseCase.Reception.Update;

namespace Interactor.Reception;

public class UpdateReceptionInteractor : IUpdateReceptionInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public UpdateReceptionInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public UpdateReceptionOutputData Handle(UpdateReceptionInputData input)
    {
        try
        {
            ReceptionSaveDto dto = input.Dto;

            if (dto!.Insurances.Any(i => !i.IsValidData()))
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
