using Domain.Models.HpInf;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Santei;
using Domain.Models.User;
using Helper.Common;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Santei.SaveListSanteiInf;

namespace Interactor.Santei;

public class SaveListSanteiInfInteractor : ISaveListSanteiInfInputPort
{
    private readonly ISanteiInfRepository _santeiInfRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMstItemRepository _mstItemRepository;
    private readonly ILoggingHandler? _loggingHandler;

    public SaveListSanteiInfInteractor(ITenantProvider tenantProvider, ISanteiInfRepository santeiInfRepository, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository, IUserRepository userRepository, IMstItemRepository mstItemRepository)
    {
        _santeiInfRepository = santeiInfRepository;
        _hpInfRepository = hpInfRepository;
        _patientInforRepository = patientInforRepository;
        _userRepository = userRepository;
        _mstItemRepository = mstItemRepository;
        var _tenantProvider = tenantProvider;
        if (_tenantProvider.CreateNewTrackingAdminDbContextOption() != null)
        {
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }
    }

    public SaveListSanteiInfOutputData Handle(SaveListSanteiInfInputData inputData)
    {
        try
        {
            var resultValidate = ValidateInput(inputData);
            if (resultValidate != SaveListSanteiInfStatus.ValidateSuccess)
            {
                return new SaveListSanteiInfOutputData(resultValidate);
            }
            var listSanteiInfs = ConvertToSanteiInfModel(inputData.PtId, inputData.ListSanteiInfs);
            if (_santeiInfRepository.SaveSantei(inputData.HpId, inputData.UserId, inputData.PtId, listSanteiInfs))
            {
                return new SaveListSanteiInfOutputData(SaveListSanteiInfStatus.Successed);
            }
            return new SaveListSanteiInfOutputData(SaveListSanteiInfStatus.Failed);
        }
        catch (Exception ex)
        {
            if (_loggingHandler != null)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
            }
            throw;
        }
        finally
        {
            _santeiInfRepository.ReleaseResource();
            _hpInfRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _userRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
            if (_loggingHandler != null)
            {
                _loggingHandler.Dispose();
            }
        }
    }

    public SaveListSanteiInfStatus ValidateInput(SaveListSanteiInfInputData input)
    {
        // validate simple param
        if (input.HpId <= 0 || !_hpInfRepository.CheckHpId(input.HpId))
        {
            return SaveListSanteiInfStatus.InvalidHpId;
        }
        else if (input.PtId <= 0 || !_patientInforRepository.CheckExistIdList(input.HpId, new List<long>() { input.PtId }))
        {
            return SaveListSanteiInfStatus.InvalidPtId;
        }
        else if (input.UserId <= 0 || !_userRepository.CheckExistedUserId(input.UserId))
        {
            return SaveListSanteiInfStatus.InvalidUserId;
        }
        // Check itemCd is exist in TenMst
        else if (!_santeiInfRepository.CheckExistItemCd(input.HpId, input.ListSanteiInfs.Select(item => item.ItemCd).ToList()))
        {
            return SaveListSanteiInfStatus.InvalidItemCd;
        }

        // get list of SanteiInfs that doesn't contain deleted item in DB to validate SanteiInf
        var listSanteiInfs = _santeiInfRepository.GetOnlyListSanteiInf(input.HpId, input.PtId);
        var listSanteiIdDeletes = input.ListSanteiInfs.Where(item => item.IsDeleted).Select(item => item.Id).ToList();
        listSanteiInfs = listSanteiInfs.Where(item => !listSanteiIdDeletes.Contains(item.Id)).ToList();

        // get list of SanteiInfDetails to validate SanteiInfDetail
        var listSanteiInfDetails = _santeiInfRepository.GetListSanteiInfDetails(input.HpId, input.PtId);

        foreach (var santeiInf in input.ListSanteiInfs)
        {
            // validate itemCd, itemCd is not duplicate
            if (santeiInf.Id <= 0 && listSanteiInfs.Any(item => item.ItemCd == santeiInf.ItemCd))
            {
                return SaveListSanteiInfStatus.InvalidItemCd;
            }
            else if (santeiInf.Id > 0 && !santeiInf.IsDeleted && !listSanteiInfs.Any(item => item.Id == santeiInf.Id && item.ItemCd == santeiInf.ItemCd))
            {
                return SaveListSanteiInfStatus.InvalidItemCd;
            }

            // validate AlertTerm, AlertTerm must exist in dictionary
            //    { 2, "日" },
            //    { 3, "暦週" },
            //    { 4, "暦月" },
            //    { 5, "週" },
            //    { 6, "月" },
            else if (santeiInf.AlertTerm < 2 || santeiInf.AlertTerm > 6)
            {
                return SaveListSanteiInfStatus.InvalidAlertTerm;
            }

            // validate AlertDays
            else if (santeiInf.AlertDays < 0 || santeiInf.AlertDays > int.MaxValue)
            {
                return SaveListSanteiInfStatus.InvalidAlertDays;
            }

            // if itemCd of santeiInf have any SanteiInfDetail, not allow
            else if ((santeiInf.ItemCd.StartsWith("KN") || santeiInf.ItemCd.StartsWith("IGE")) && santeiInf.ListSanteInfDetails.Any())
            {
                return SaveListSanteiInfStatus.ThisSanteiInfDoesNotAllowSanteiInfDetail;
            }

            // validate list SanteiInfDetails
            foreach (var santeiInfDetail in santeiInf.ListSanteInfDetails)
            {
                var resultValidateDetail = ValidateSanteiInfDetail(santeiInf.ItemCd, santeiInfDetail, listSanteiInfDetails);
                if (resultValidateDetail != SaveListSanteiInfStatus.ValidateSuccess)
                {
                    return resultValidateDetail;
                }
            }
        }
        return SaveListSanteiInfStatus.ValidateSuccess;
    }

    public SaveListSanteiInfStatus ValidateSanteiInfDetail(string itemCd, SanteiInfDetailInputItem santeiInfDetail, List<SanteiInfDetailModel> listSanteiInfDetails)
    {
        // validate KisanSbt, KisanSbt must exist in dictionary
        //{ 0, string.Empty },
        //{ 1, "初回算定" },
        //{ 2, "発症" },
        //{ 3, "急性憎悪" },
        //{ 4, "治療開始" },
        //{ 5, "手術" },
        //{ 6, "初回診断" }
        if (santeiInfDetail.KisanSbt < 1 || santeiInfDetail.KisanSbt > 6)
        {
            return SaveListSanteiInfStatus.InvalidKisanSbt;
        }

        // check santeiInfDetail is exist in SanteiInf
        else if (santeiInfDetail.Id > 0 && listSanteiInfDetails.FirstOrDefault(item => item.Id == santeiInfDetail.Id)?.ItemCd != itemCd)
        {
            return SaveListSanteiInfStatus.InvalidSanteiInfDetail;
        }

        // validate EndDate
        else if (santeiInfDetail.EndDate != 0 && santeiInfDetail.EndDate != 99999999 && CIUtil.SDateToShowSDate(santeiInfDetail.EndDate) == string.Empty)
        {
            return SaveListSanteiInfStatus.InvalidEndDate;
        }

        // validate KisanDate
        else if (CIUtil.SDateToShowSDate(santeiInfDetail.KisanDate) == string.Empty)
        {
            return SaveListSanteiInfStatus.InvalidKisanDate;
        }

        // validate HosokuComment
        else if (santeiInfDetail.HosokuComment.Length > 0 && santeiInfDetail.HosokuComment.Length > 80)
        {
            return SaveListSanteiInfStatus.InvalidHosokuComment;
        }
        return SaveListSanteiInfStatus.ValidateSuccess;
    }

    public List<SanteiInfModel> ConvertToSanteiInfModel(long ptId, List<SanteiInfInputItem> listSanteiInfInputs)
    {
        return listSanteiInfInputs.Select(santaiInf => new SanteiInfModel(
                                                                        santaiInf.Id,
                                                                        santaiInf.PtId <= 0 ? 0 : ptId,
                                                                        santaiInf.ItemCd,
                                                                        santaiInf.AlertDays,
                                                                        santaiInf.AlertTerm,
                                                                        santaiInf.SortNo,
                                                                        santaiInf.ListSanteInfDetails.Select(santaiInfDetail => new SanteiInfDetailModel(
                                                                                                                santaiInfDetail.Id,
                                                                                                                santaiInf.PtId <= 0 ? 0 : ptId,
                                                                                                                santaiInf.ItemCd,
                                                                                                                santaiInfDetail.EndDate,
                                                                                                                santaiInfDetail.KisanSbt,
                                                                                                                santaiInfDetail.KisanDate,
                                                                                                                santaiInfDetail.Byomei,
                                                                                                                santaiInfDetail.HosokuComment,
                                                                                                                santaiInfDetail.Comment,
                                                                                                                santaiInf.IsDeleted ? santaiInf.IsDeleted : santaiInfDetail.IsDeleted
                                                                                                            )).ToList(),
                                                                        santaiInf.IsDeleted
                                                                    )).ToList();
    }
}
