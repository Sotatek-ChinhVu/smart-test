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
        try
        {
            ReceptionSaveDto dto = input.Dto;

            if (dto!.Insurances.Any(i => !i.IsValidData()))
            {
                return new InsertReceptionOutputData(InsertReceptionStatus.InvalidInsuranceList, 0);
            }

            // check end set uketukeNo
            int uketukeNo = GetUketukeNo(input.HpId, dto.Reception.SinDate, dto.Reception.UketukeSbt, dto.Reception.KaId, dto.Reception.UketukeNo);
            dto.ChangeUketukeNo(uketukeNo);

            var raiinNo = _receptionRepository.Insert(dto, input.HpId, input.UserId);
            return new InsertReceptionOutputData(InsertReceptionStatus.Success, raiinNo);
        }
        finally
        {
            _receptionRepository.ReleaseResource();
            _systemConfRepository.ReleaseResource();
        }
    }
    private int GetUketukeNo(int hpId, int sindate, int uketukeSbt, int kaId, int inputUketukeNo)
    {
        int uketukeNoMode = _systemConfRepository.GetSettingValue(1008, 0, hpId).AsInteger();
        int uketukeNoStart = _systemConfRepository.GetSettingParams(1008, 0, hpId, defaultParam: "1").AsInteger();

        int maxUketukeNo = _receptionRepository.GetMaxUketukeNo(hpId, sindate, uketukeSbt, kaId, uketukeNoMode);
        int nextUketukeNo = maxUketukeNo + 1 < uketukeNoStart ? uketukeNoStart : maxUketukeNo + 1;

        return inputUketukeNo < 0 ? nextUketukeNo : inputUketukeNo;
    }
}
