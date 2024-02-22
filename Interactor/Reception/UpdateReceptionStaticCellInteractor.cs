using Domain.Models.Ka;
using Domain.Models.PatientInfor;
using Domain.Models.PtCmtInf;
using Domain.Models.RaiinCmtInf;
using Domain.Models.Reception;
using Domain.Models.UketukeSbtMst;
using Domain.Models.User;
using Helper.Constants;
using Interactor.CalculateService;
using UseCase.Accounting.Recaculate;
using UseCase.Reception.UpdateStaticCell;

namespace Interactor.Reception;

public class UpdateReceptionStaticCellInteractor : IUpdateReceptionStaticCellInputPort
{
    private readonly IReceptionRepository _receptionRepository;
    private readonly IRaiinCmtInfRepository _raiinCmtInfRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUketukeSbtMstRepository _uketukeSbtMstRepository;
    private readonly IKaRepository _kaMstRepository;
    private readonly IPtCmtInfRepository _ptCmtInfRepository;
    private readonly ICalculateService _calculateRepository;
    private readonly IPatientInforRepository _patientInforRepository;

    public UpdateReceptionStaticCellInteractor(IReceptionRepository receptionRepository,
        IRaiinCmtInfRepository raiinCmtInfRepository,
        IUserRepository userRepository,
        IUketukeSbtMstRepository uketukeSbtMstRepository,
        IKaRepository kaMstRepository,
        IPtCmtInfRepository ptCmtInfRepository,
        ICalculateService calculateRepository,
        IPatientInforRepository patientInforRepository
        )
    {
        _receptionRepository = receptionRepository;
        _raiinCmtInfRepository = raiinCmtInfRepository;
        _userRepository = userRepository;
        _uketukeSbtMstRepository = uketukeSbtMstRepository;
        _kaMstRepository = kaMstRepository;
        _ptCmtInfRepository = ptCmtInfRepository;
        _calculateRepository = calculateRepository;
        _patientInforRepository = patientInforRepository;
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

        try
        {
            var status = UpdateStaticCell(input);
            List<ReceptionRowModel> receptionInfos = new();
            List<SameVisitModel> sameVisitList = new();
            PatientInforModel patientInforModel = new();

            if (status == UpdateReceptionStaticCellStatus.RaiinInfUpdated && input.CellName.ToLower() == "status")
            {
                Task.Run(() =>
                {
                    _calculateRepository.RunCalculate(new RecaculationInputDto(input.HpId, input.PtId, input.SinDate, 0, ""));
                });
            }
            if (status == UpdateReceptionStaticCellStatus.RaiinInfUpdated || status == UpdateReceptionStaticCellStatus.RaiinCmtUpdated)
            {
                receptionInfos = _receptionRepository.GetList(input.HpId, input.SinDate, input.RaiinNo, input.PtId, isDeleted: 0);
            }
            if (status == UpdateReceptionStaticCellStatus.PatientCmtUpdated)
            {
                patientInforModel = _patientInforRepository.GetById(input.HpId, input.PtId, input.SinDate, 0) ?? new();
            }
            return new UpdateReceptionStaticCellOutputData(status, receptionInfos, sameVisitList, patientInforModel);
        }
        finally
        {
            _receptionRepository.ReleaseResource();
            _raiinCmtInfRepository.ReleaseResource();
            _userRepository.ReleaseResource();
            _uketukeSbtMstRepository.ReleaseResource();
            _kaMstRepository.ReleaseResource();
            _ptCmtInfRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
        }
    }

    private UpdateReceptionStaticCellStatus UpdateStaticCell(UpdateReceptionStaticCellInputData input)
    {
        string pascalCaseCellName = FirstCharToUpper(input.CellName);
        switch (pascalCaseCellName)
        {
            // Update RaiinInf
            case nameof(ReceptionRowModel.Status):
                var success = _receptionRepository.UpdateStatus(input.HpId, input.RaiinNo, int.Parse(input.CellValue), input.UserId);
                return GetRaiinInfUpdateStatus(success);
            case nameof(ReceptionRowModel.UketukeNo):
                success = _receptionRepository.UpdateUketukeNo(input.HpId, input.RaiinNo, int.Parse(input.CellValue), input.UserId);
                return GetRaiinInfUpdateStatus(success);
            case nameof(ReceptionRowModel.UketukeTime):
                success = _receptionRepository.UpdateUketukeTime(input.HpId, input.RaiinNo, input.CellValue, input.UserId);
                return GetRaiinInfUpdateStatus(success);
            case nameof(ReceptionRowModel.SinStartTime):
                success = _receptionRepository.UpdateSinStartTime(input.HpId, input.RaiinNo, input.CellValue, input.UserId);
                return GetRaiinInfUpdateStatus(success);
            case nameof(ReceptionRowModel.UketukeSbtId):
                var uketukeSbt = _uketukeSbtMstRepository.GetByKbnId(input.HpId, int.Parse(input.CellValue));
                if (uketukeSbt is null)
                {
                    return UpdateReceptionStaticCellStatus.InvalidUketukeSbtId;
                }
                success = _receptionRepository.UpdateUketukeSbt(input.HpId, input.RaiinNo, uketukeSbt.KbnId, input.UserId);
                return GetRaiinInfUpdateStatus(success);
            case nameof(ReceptionRowModel.TantoId):
                var tanto = _userRepository.GetByUserId(input.HpId, int.Parse(input.CellValue));
                if (tanto is null)
                {
                    return UpdateReceptionStaticCellStatus.InvalidTantoId;
                }
                success = _receptionRepository.UpdateTantoId(input.HpId, input.RaiinNo, tanto.UserId, input.UserId);
                return GetRaiinInfUpdateStatus(success);
            case nameof(ReceptionRowModel.KaId):
                var ka = _kaMstRepository.GetByKaId(input.HpId, int.Parse(input.CellValue));
                if (ka is null)
                {
                    return UpdateReceptionStaticCellStatus.InvalidKaId;
                }
                success = _receptionRepository.UpdateKaId(input.HpId, input.RaiinNo, ka.KaId, input.UserId);
                return GetRaiinInfUpdateStatus(success);
            // Update or insert RaiinCmtInf
            case nameof(ReceptionRowModel.RaiinCmt):
                _raiinCmtInfRepository.Upsert(input.HpId, input.PtId, input.SinDate, input.RaiinNo, CmtKbns.Comment, input.CellValue, input.UserId);
                return UpdateReceptionStaticCellStatus.RaiinCmtUpdated;
            case nameof(ReceptionRowModel.RaiinRemark):
                _raiinCmtInfRepository.Upsert(input.HpId, input.PtId, input.SinDate, input.RaiinNo, CmtKbns.Remark, input.CellValue, input.UserId);
                return UpdateReceptionStaticCellStatus.RaiinCmtUpdated;
            // Update or insert PtCmtInf
            case nameof(ReceptionRowModel.PtComment):
                _ptCmtInfRepository.Upsert(input.HpId, input.PtId, input.CellValue, input.UserId);
                return UpdateReceptionStaticCellStatus.PatientCmtUpdated;
            default:
                return UpdateReceptionStaticCellStatus.InvalidCellName;
        }

        UpdateReceptionStaticCellStatus GetRaiinInfUpdateStatus(bool success)
        {
            return success ? UpdateReceptionStaticCellStatus.RaiinInfUpdated : UpdateReceptionStaticCellStatus.RaiinInfNotFound;
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
