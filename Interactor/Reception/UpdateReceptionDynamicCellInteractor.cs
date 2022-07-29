using Domain.Models.RaiinKbnInf;
using Helper.Constants;
using UseCase.Reception.UpdateDynamicCell;

namespace Interactor.Reception;

public class UpdateReceptionDynamicCellInteractor : IUpdateReceptionDynamicCellInputPort
{
    private readonly IRaiinKbnInfRepository _raiinKbnInfRepository;

    public UpdateReceptionDynamicCellInteractor(IRaiinKbnInfRepository raiinKbnInfRepository)
    {
        _raiinKbnInfRepository = raiinKbnInfRepository;
    }

    public UpdateReceptionDynamicCellOutputData Handle(UpdateReceptionDynamicCellInputData input)
    {
        if (input.HpId <= 0)
        {
            return new UpdateReceptionDynamicCellOutputData($"{nameof(input.HpId)} must be greater than 0.");
        }
        if (input.SinDate <= 0)
        {
            return new UpdateReceptionDynamicCellOutputData($"{nameof(input.SinDate)} must be greater than 0.");
        }
        if (input.RaiinNo <= 0)
        {
            return new UpdateReceptionDynamicCellOutputData($"{nameof(input.RaiinNo)} must be greater than 0.");
        }
        if (input.PtId <= 0)
        {
            return new UpdateReceptionDynamicCellOutputData($"{nameof(input.PtId)} must be greater than 0.");
        }
        if (input.GrpId < 0)
        {
            return new UpdateReceptionDynamicCellOutputData($"{nameof(input.GrpId)} cannot be negative.");
        }

        UpdateDynamicCell(input);
        return new UpdateReceptionDynamicCellOutputData(true);
    }

    private void UpdateDynamicCell(UpdateReceptionDynamicCellInputData input)
    {
        if (input.KbnCd == CommonConstants.KbnCdDeleteFlag)
        {
            _raiinKbnInfRepository.SoftDelete(input.HpId, input.PtId, input.SinDate, input.RaiinNo, input.GrpId);
            return;
        }

        _raiinKbnInfRepository.Upsert(input.HpId, input.PtId, input.SinDate, input.RaiinNo, input.GrpId, input.KbnCd);
    }
}
