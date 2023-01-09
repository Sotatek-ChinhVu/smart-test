using Domain.Models.RaiinKubunMst;
using Helper.Constants;
using UseCase.Reception.UpdateDynamicCell;

namespace Interactor.Reception;

public class UpdateReceptionDynamicCellInteractor : IUpdateReceptionDynamicCellInputPort
{
    private readonly IRaiinKubunMstRepository _raiinKbnInfRepository;

    public UpdateReceptionDynamicCellInteractor(IRaiinKubunMstRepository raiinKbnInfRepository)
    {
        _raiinKbnInfRepository = raiinKbnInfRepository;
    }

    public UpdateReceptionDynamicCellOutputData Handle(UpdateReceptionDynamicCellInputData input)
    {
        if (input.HpId <= 0)
        {
            return new UpdateReceptionDynamicCellOutputData(UpdateReceptionDynamicCellStatus.InvalidHpId);
        }
        if (input.SinDate <= 0)
        {
            return new UpdateReceptionDynamicCellOutputData(UpdateReceptionDynamicCellStatus.InvalidSinDate);
        }
        if (input.RaiinNo <= 0)
        {
            return new UpdateReceptionDynamicCellOutputData(UpdateReceptionDynamicCellStatus.InvalidRaiinNo);
        }
        if (input.PtId <= 0)
        {
            return new UpdateReceptionDynamicCellOutputData(UpdateReceptionDynamicCellStatus.InvalidPtId);
        }
        if (input.GrpId < 0)
        {
            return new UpdateReceptionDynamicCellOutputData(UpdateReceptionDynamicCellStatus.InvalidGrpId);
        }

        try
        {
            UpdateDynamicCell(input);
            return new UpdateReceptionDynamicCellOutputData(UpdateReceptionDynamicCellStatus.Success);
        }
        finally
        {
            _raiinKbnInfRepository.ReleaseResource();
        }
    }

    private void UpdateDynamicCell(UpdateReceptionDynamicCellInputData input)
    {
        if (input.KbnCd == CommonConstants.KbnCdDeleteFlag)
        {
            _raiinKbnInfRepository.SoftDelete(input.HpId, input.PtId, input.SinDate, input.RaiinNo, input.GrpId);
            return;
        }

        _raiinKbnInfRepository.Upsert(input.HpId, input.PtId, input.SinDate, input.RaiinNo, input.GrpId, input.KbnCd, input.UserId);
    }
}
