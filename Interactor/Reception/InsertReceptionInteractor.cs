using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Helper.Extension;
using UseCase.Reception.Insert;

namespace Interactor.Reception;

public class InsertReceptionInteractor : IInsertReceptionInputPort
{
    private readonly IReceptionRepository _receptionRepository;
    private readonly ISystemConfRepository _systemConfRepository;

    public InsertReceptionInteractor(IReceptionRepository receptionRepository, ISystemConfRepository systemConfRepository)
    {
        _receptionRepository = receptionRepository;
        _systemConfRepository = systemConfRepository;
    }

    public InsertReceptionOutputData Handle(InsertReceptionInputData input)
    {
        ReceptionSaveDto dto = input.Dto;

        if (dto!.Insurances.Any(i => !i.IsValidData()))
        {
            return new InsertReceptionOutputData(InsertReceptionStatus.InvalidInsuranceList, 0);
        }

        int uketukeNoMode = _systemConfRepository.GetSettingValue(1008, 0, input.HpId).AsInteger();
        int uketukeNoStart = _systemConfRepository.GetSettingParams(1008, 0, input.HpId, defaultParam: "1").AsInteger();

        // check end set uketukeNo

        int uketukeNo = dto.Reception.UketukeNo < 0 ? _receptionRepository.GetNextUketukeNoBySetting(input.HpId, dto.Reception.SinDate, dto.Reception.UketukeSbt, dto.Reception.KaId, uketukeNoMode, uketukeNoStart) : dto.Reception.UketukeNo;
        dto.ChangeUketukeNo(uketukeNo);

        var raiinNo = _receptionRepository.Insert(dto, input.HpId, input.UserId);
        return new InsertReceptionOutputData(InsertReceptionStatus.Success, raiinNo);
    }
}
