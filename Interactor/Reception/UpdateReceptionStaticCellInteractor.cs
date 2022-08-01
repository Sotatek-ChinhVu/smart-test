using Domain.Models.KaMst;
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

    public UpdateReceptionStaticCellInteractor(IReceptionRepository receptionRepository,
        IRaiinCmtInfRepository raiinCmtInfRepository,
        IUserRepository userRepository,
        IUketukeSbtMstRepository uketukeSbtMstRepository,
        IKaMstRepository kaMstRepository)
    {
        _receptionRepository = receptionRepository;
        _raiinCmtInfRepository = raiinCmtInfRepository;
        _userRepository = userRepository;
        _uketukeSbtMstRepository = uketukeSbtMstRepository;
        _kaMstRepository = kaMstRepository;
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

        var status = UpdateStaticCell(input) ? UpdateReceptionStaticCellStatus.Success : UpdateReceptionStaticCellStatus.UnknownError;
        return new UpdateReceptionStaticCellOutputData(status);
    }

    private bool UpdateStaticCell(UpdateReceptionStaticCellInputData input)
    {
        string pascalCaseCellName = FirstCharToUpper(input.CellName);
        switch (pascalCaseCellName)
        {
            // Update RaiinInf
            case nameof(ReceptionRowModel.Status):
                return _receptionRepository.UpdateStatus(input.HpId, input.RaiinNo, int.Parse(input.CellValue));
            case nameof(ReceptionRowModel.UketukeNo):
                return _receptionRepository.UpdateUketukeNo(input.HpId, input.RaiinNo, int.Parse(input.CellValue));
            case nameof(ReceptionRowModel.UketukeTime):
                return _receptionRepository.UpdateUketukeTime(input.HpId, input.RaiinNo, input.CellValue);
            case nameof(ReceptionRowModel.SinStartTime):
                return _receptionRepository.UpdateSinStartTime(input.HpId, input.RaiinNo, input.CellValue);
            case nameof(ReceptionRowModel.UketukeSbtId):
                var uketukeSbt = _uketukeSbtMstRepository.GetByKbnId(int.Parse(input.CellValue));
                if (uketukeSbt is null)
                {
                    return false;
                }
                return _receptionRepository.UpdateUketukeSbt(input.HpId, input.RaiinNo, uketukeSbt.KbnId);
            case nameof(ReceptionRowModel.TantoId):
                var tanto = _userRepository.GetByUserId(int.Parse(input.CellValue));
                if (tanto is null)
                {
                    return false;
                }
                return _receptionRepository.UpdateTantoId(input.HpId, input.RaiinNo, tanto.UserId);
            case nameof(ReceptionRowModel.KaId):
                var ka = _kaMstRepository.GetByKaId(int.Parse(input.CellValue));
                if (ka is null)
                {
                    return false;
                }
                return _receptionRepository.UpdateKaId(input.HpId, input.RaiinNo, ka.KaId);
            // Update or insert RaiinCmtInf
            case nameof(ReceptionRowModel.RaiinCmt):
                _raiinCmtInfRepository.Upsert(input.HpId, input.PtId, input.SinDate, input.RaiinNo, CmtKbns.Comment, input.CellValue);
                return true;
            case nameof(ReceptionRowModel.RaiinRemark):
                _raiinCmtInfRepository.Upsert(input.HpId, input.PtId, input.SinDate, input.RaiinNo, CmtKbns.Remark, input.CellValue);
                return true;
            default:
                return false;
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
