using Domain.Models.KaMst;
using Domain.Models.PtCmtInf;
using Domain.Models.RaiinCmtInf;
using Domain.Models.Reception;
using Domain.Models.UketukeSbtMst;
using Domain.Models.User;
using Helper.Constants;
using UseCase.Reception.UpdateStaticCell;

namespace Interactor.Reception;

public class UpdateReceptionStaticCellInteractor : IUpdateReceptionStaticCellInputPort
{
    private readonly IReceptionRepository _receptionRepository;
    private readonly IRaiinCmtInfRepository _raiinCmtInfRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUketukeSbtMstRepository _uketukeSbtMstRepository;
    private readonly IKaMstRepository _kaMstRepository;
    private readonly IPtCmtInfRepository _ptCmtInfRepository;

    public UpdateReceptionStaticCellInteractor(IReceptionRepository receptionRepository,
        IRaiinCmtInfRepository raiinCmtInfRepository,
        IUserRepository userRepository,
        IUketukeSbtMstRepository uketukeSbtMstRepository,
        IKaMstRepository kaMstRepository,
        IPtCmtInfRepository ptCmtInfRepository)
    {
        _receptionRepository = receptionRepository;
        _raiinCmtInfRepository = raiinCmtInfRepository;
        _userRepository = userRepository;
        _uketukeSbtMstRepository = uketukeSbtMstRepository;
        _kaMstRepository = kaMstRepository;
        _ptCmtInfRepository = ptCmtInfRepository;
    }

    public UpdateReceptionStaticCellOutputData Handle(UpdateReceptionStaticCellInputData input)
    {
        if (input.HpId <= 0)
        {
            return new UpdateReceptionStaticCellOutputData(UpdateReceptionStaticCellStatus.InvalidHpId);
        }
        if (input.SinDate <= 0)
        {
            return new UpdateReceptionStaticCellOutputData(UpdateReceptionStaticCellStatus.InvalidSinDate);
        }
        if (input.RaiinNo <= 0)
        {
            return new UpdateReceptionStaticCellOutputData(UpdateReceptionStaticCellStatus.InvalidRaiinNo);
        }
        if (input.PtId <= 0)
        {
            return new UpdateReceptionStaticCellOutputData(UpdateReceptionStaticCellStatus.InvalidPtId);
        }

        var status = UpdateStaticCell(input);
        return new UpdateReceptionStaticCellOutputData(status);
    }

    private UpdateReceptionStaticCellStatus UpdateStaticCell(UpdateReceptionStaticCellInputData input)
    {
        string pascalCaseCellName = FirstCharToUpper(input.CellName);
        switch (pascalCaseCellName)
        {
            // Update RaiinInf
            case nameof(ReceptionRowModel.Status):
                var success = _receptionRepository.UpdateStatus(input.HpId, input.RaiinNo, int.Parse(input.CellValue));
                return GetReceptionInfoUpdateStatus(success);
            case nameof(ReceptionRowModel.UketukeNo):
                success = _receptionRepository.UpdateUketukeNo(input.HpId, input.RaiinNo, int.Parse(input.CellValue));
                return GetReceptionInfoUpdateStatus(success);
            case nameof(ReceptionRowModel.UketukeTime):
                success = _receptionRepository.UpdateUketukeTime(input.HpId, input.RaiinNo, input.CellValue);
                return GetReceptionInfoUpdateStatus(success);
            case nameof(ReceptionRowModel.SinStartTime):
                success = _receptionRepository.UpdateSinStartTime(input.HpId, input.RaiinNo, input.CellValue);
                return GetReceptionInfoUpdateStatus(success);
            case nameof(ReceptionRowModel.UketukeSbtId):
                var uketukeSbt = _uketukeSbtMstRepository.GetByKbnId(int.Parse(input.CellValue));
                if (uketukeSbt is null)
                {
                    return UpdateReceptionStaticCellStatus.InvalidUketukeSbtId;
                }
                success = _receptionRepository.UpdateUketukeSbt(input.HpId, input.RaiinNo, uketukeSbt.KbnId);
                return GetReceptionInfoUpdateStatus(success);
            case nameof(ReceptionRowModel.TantoId):
                var tanto = _userRepository.GetByUserId(int.Parse(input.CellValue));
                if (tanto is null)
                {
                    return UpdateReceptionStaticCellStatus.InvalidTantoId;
                }
                success = _receptionRepository.UpdateTantoId(input.HpId, input.RaiinNo, tanto.UserId);
                return GetReceptionInfoUpdateStatus(success);
            case nameof(ReceptionRowModel.KaId):
                var ka = _kaMstRepository.GetByKaId(int.Parse(input.CellValue));
                if (ka is null)
                {
                    return UpdateReceptionStaticCellStatus.InvalidKaId;
                }
                success = _receptionRepository.UpdateKaId(input.HpId, input.RaiinNo, ka.KaId);
                return GetReceptionInfoUpdateStatus(success);
            // Update or insert RaiinCmtInf
            case nameof(ReceptionRowModel.RaiinCmt):
                _raiinCmtInfRepository.Upsert(input.HpId, input.PtId, input.SinDate, input.RaiinNo, CmtKbns.Comment, input.CellValue);
                return UpdateReceptionStaticCellStatus.ReceptionCmtUpdated;
            case nameof(ReceptionRowModel.RaiinRemark):
                _raiinCmtInfRepository.Upsert(input.HpId, input.PtId, input.SinDate, input.RaiinNo, CmtKbns.Remark, input.CellValue);
                return UpdateReceptionStaticCellStatus.ReceptionCmtUpdated;
            // Update or insert PtCmtInf
            case nameof(ReceptionRowModel.PtComment):
                _ptCmtInfRepository.Upsert(input.PtId, input.CellValue);
                return UpdateReceptionStaticCellStatus.PatientCmtUpdated;
            default:
                return UpdateReceptionStaticCellStatus.InvalidCellName;
        }

        UpdateReceptionStaticCellStatus GetReceptionInfoUpdateStatus(bool success)
        {
            return success ? UpdateReceptionStaticCellStatus.ReceptionUpdated : UpdateReceptionStaticCellStatus.ReceptionNotFound;
        }
    }

    private string FirstCharToUpper(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        return char.ToUpper(s[0]) + s.Substring(1);
    }
}
