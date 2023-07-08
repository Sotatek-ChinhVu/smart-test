using Domain.Models.RaiinKubunMst;
using Domain.Models.Reception;
using Helper.Constants;
using UseCase.Reception.UpdateDynamicCell;

namespace Interactor.Reception;

public class UpdateReceptionDynamicCellInteractor : IUpdateReceptionDynamicCellInputPort
{
    private readonly IRaiinKubunMstRepository _raiinKbnInfRepository;
    private readonly IReceptionRepository _receptionRepository;

    public UpdateReceptionDynamicCellInteractor(IRaiinKubunMstRepository raiinKbnInfRepository, IReceptionRepository receptionRepository)
    {
        _raiinKbnInfRepository = raiinKbnInfRepository;
        _receptionRepository = receptionRepository;
    }

    public UpdateReceptionDynamicCellOutputData Handle(UpdateReceptionDynamicCellInputData input)
    {
        List<ReceptionRowModel> receptionInfos = new();
        List<SameVisitModel> sameVisitList = new();
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
            receptionInfos = _receptionRepository.GetList(input.HpId, input.SinDate, input.RaiinNo, input.PtId);
            return new UpdateReceptionDynamicCellOutputData(UpdateReceptionDynamicCellStatus.Success, receptionInfos, sameVisitList);
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
